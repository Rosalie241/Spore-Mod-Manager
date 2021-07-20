﻿using SporeMods;
using SporeMods.CommonUI;
using SporeMods.CommonUI.Localization;
using SporeMods.Context;
using SporeMods.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace SporeMods.Manager
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static Process UacMessengerProcess = null;

		public App()
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			DispatcherUnhandledException += App_DispatcherUnhandledException;
		}

		private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			App_Exit(this, null);
			MessageDisplayUI.ShowException(e.Exception);
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			App_Exit(this, null);
			if (e.ExceptionObject is Exception exc)
				MessageDisplayUI.ShowException(exc);
		}

		public static readonly string UacMessengerIdArg = "-UacMessengerId:";
		protected override void OnStartup(StartupEventArgs e)
		{
			if (Settings.ForceSoftwareRendering)
				RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;

			MessageDisplay.ErrorOccurred += (sender, args) =>
			{
				App_Exit(this, null);
				MessageDisplayUI.ShowException(args.Exception);
			};
			MessageDisplay.MessageBoxShown += (sneder, args) => Dispatcher.BeginInvoke(new Action(() => MessageDisplayUI.ShowMessageBox(args.Content, args.Title)));
			MessageDisplay.DebugMessageSent += (sneder, args) => Dispatcher.BeginInvoke(new Action(() => MessageDisplayUI.ShowMessageBox(args.Content, args.Title)));

			Settings.EnsureDllsAreExtracted();
			Updater.CheckForUpdates();
			Settings.ManagerInstallLocationPath = Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).ToString();

			Exit += App_Exit;

			if (VersionValidation.IsConfigVersionCompatible(true, out Version previousModMgrVersion))
			{
				if (ServantCommands.RunLkImporter() == null)
				{
					if (Permissions.IsAtleastWindowsVista() && (!Permissions.IsAdministrator()))
					{
						if (MgrProcesses.AreAnyOtherModManagersRunning)
						{
							if (Environment.GetCommandLineArgs().Length > 1)
							{
								List<string> files = new List<string>();
								foreach (string s in Environment.GetCommandLineArgs().Skip(1))
								{
									string path = s.Trim('\"', ' ');
									if (File.Exists(path))
										files.Add(path);
								}
								string draggedFilesPath = Path.Combine(Settings.TempFolderPath, "draggedFiles");
								File.WriteAllLines(draggedFilesPath, files);
								Permissions.GrantAccessFile(draggedFilesPath);
							}
							else
								MessageBox.Show(LanguageManager.Instance.GetLocalizedText("OneInstanceOnly"));
							Process.GetCurrentProcess().Kill();
						}

						/*foreach (Process proc in Process.GetProcessesByName("SporeMods.UacMessenger").ToList())
							proc.Kill();*/
						string parentDirectoryPath = Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).ToString();
						/*var UacMessengerStartInfo = new ProcessStartInfo(Path.Combine(parentDirectoryPath, "SporeMods.UacMessenger.exe"), Process.GetCurrentProcess().Id.ToString())
						{
							UseShellExecute = true
						};
						//Permissions.ForwardDotnetEnvironmentVariables(ref UacMessengerStartInfo);
						Process p = Process.Start(UacMessengerStartInfo);*/
						Process p = MgrProcesses.StartUacMessenger(/*Process.GetCurrentProcess().Id.ToString()*/);
						string args = Permissions.GetProcessCommandLineArgs();
						args += " " + UacMessengerIdArg + p.Id;
						if (!Environment.GetCommandLineArgs().Contains(UpdaterService.IgnoreUpdatesArg)) args += " " + UpdaterService.IgnoreUpdatesArg;
						try
						{
							//while (p.MainWindowHandle == IntPtr.Zero) { }

							//Permissions.RerunAsAdministrator(args);
							MgrProcesses.RestartModManagerAsAdministrator(args);
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.GetType().FullName + "\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Fatal error");
							/*foreach (Process proc in Process.GetProcessesByName("SporeMods.UacMessenger").ToList())
								proc.Kill();*/
							Application.Current.Shutdown();
						}
					}
					else// if (Permissions.IsAdministrator())
					{
						string[] clArgs = Environment.GetCommandLineArgs();
						foreach (string arg in clArgs)
						{
							string targ = arg.Trim(" ".ToCharArray());
							if (targ.StartsWith(UacMessengerIdArg))
							{
								UacMessengerProcess = Process.GetProcessById(int.Parse(targ.Replace(UacMessengerIdArg, string.Empty)));
								break;
							}
						}

						bool proceed = true;

						if (UacMessengerProcess == null)
						{
							var firstUacMessenger = Process.GetProcessesByName("SporeMods.UacMessenger").FirstOrDefault();
							if (firstUacMessenger != null)
								UacMessengerProcess = firstUacMessenger;
						}
						else if (Permissions.IsAtleastWindowsVista() && (UacMessengerProcess == null))
						{
							proceed = false;
							if (Settings.NonEssentialIsRunningUnderWine)
								proceed = true;
							else if (MessageBox.Show(LanguageManager.Instance.GetLocalizedText("DontRunAsAdmin").Replace("%APPNAME%", "Spore Mod Manager"), String.Empty, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
								proceed = true;
						}
						else if ((!Permissions.IsAtleastWindowsVista()) && (UacMessengerProcess == null))
						{
							/*var UacMessengerStartInfo = new ProcessStartInfo(Path.Combine(Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).ToString(), "SporeMods.UacMessenger.exe"))
							{
								UseShellExecute = true
							};
							//Permissions.ForwardDotnetEnvironmentVariables(ref UacMessengerStartInfo);
							UacMessengerProcess = Process.Start(UacMessengerStartInfo);*/
							UacMessengerProcess = MgrProcesses.StartUacMessenger();
						}

						if (proceed)
						{
							base.OnStartup(e);

							//ModInstallation.DoFirstRunVerification();
							VersionValidation.WarnIfMissingOriginPrerequisites(); //Path.Combine(new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Directory.FullName, "Launch Spore.dll"); //);
							Window window;
							ManagerContent content = new ManagerContent();
							if (Settings.UseCustomWindowDecorations)
							{
								window = new Mechanism.Wpf.Core.Windows.DecoratableWindow()
								{
									Content = content,
									TitlebarHeight = 61,
									ShowFullscreenButton = true,
									AutohideTitlebarWhenFullscreen = false
								};
							}
							else
							{
								window = new Window()
								{
									Content = content
								};
							}

							window.MinWidth = 700;
							window.MinHeight = 400;
							window.Width = 800;
							window.Height = 450;
							MainWindow = window;
							window.ContentRendered += (sneder, args) => content.MainWindow_OnContentRendered(args);
							window.Activated += content.MainWindow_IsActiveChanged;
							window.Deactivated += content.MainWindow_IsActiveChanged;
							window.SizeChanged += content.MainWindow_SizeChanged;
							window.PreviewKeyDown += content.MainWindow_PreviewKeyDown;
							window.Closing += content.MainWindow_Closing;
							window.Show();
						}
						else
						{
							Application.Current.Shutdown();
						}
					}
				}
			}
		}

		private void App_Exit(object sender, ExitEventArgs e)
		{
			if ((UacMessengerProcess != null) && (!UacMessengerProcess.HasExited))
				UacMessengerProcess.Kill();
		}
	}
}
