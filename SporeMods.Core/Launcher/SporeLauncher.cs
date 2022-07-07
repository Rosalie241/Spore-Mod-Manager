using SporeMods.Core.Mods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using static SporeMods.Core.GameInfo;

namespace SporeMods.Core.Launcher
{
	public static class SporeLauncher
	{
		public static Func<string, string> GetLocalizedString = null;

		public const string EXTRACT_ORIGIN_PREREQ = "--originFirstRun";

		public static int CaptionHeight = -1;
		public static IntPtr _processHandle = IntPtr.Zero;
		private static bool _debugMode = File.Exists(Path.Combine(Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).ToString(), "debug.txt"));

		static string ModApiHelpThreadURL = "http://davoonline.com/phpBB3/viewtopic.php?f=108&t=6300";
		static string DarkInjectionPageURL = "http://davoonline.com/sporemodder/rob55rod/DarkInjection/";

		public static void OpenDarkInjectionPage()
		{
			Process.Start(new ProcessStartInfo(DarkInjectionPageURL)
			{
				UseShellExecute = true
			});
		}

		//private string SporebinPath;
		private static string _executablePath;
		//private string SteamPath;
		private static GameExecutableType _executableType = GameExecutableType.None;

		public static int CurrentError = 0;

		static void DeleteFolder(string path)
		{
			foreach (string s in Directory.EnumerateFiles(path))
				File.Delete(s);
			foreach (string s in Directory.EnumerateDirectories(path))
				DeleteFolder(s);
			Directory.Delete(path);
		}

		public static Version Spore__1_5_1 = new Version(3, 0, 0, 2818);
		public static Version Spore__March2017 = new Version(3, 1, 0, 22);
		public static void LaunchGame()
		{
			try
			{
				_executablePath = Path.Combine(SporebinEP1, "SporeApp.exe");
				if (File.Exists(_executablePath))
				{
					bool exeSizeRecognized = ExecutableFileGameTypes.Keys.Contains(new FileInfo(_executablePath).Length);
					if (IsValidExe())
					{
						try
						{
							_executableType = GetExecutableType();
						}
						catch (Exception ex)
						{
							MessageDisplay.RaiseError(new ErrorEventArgs(ex)); //ex.Message + "\n\n" + ex.StackTrace
							return;
						}

						// copy mod loader if needed
						if (!File.Exists(Path.Combine(SporebinEP1, "dinput8.dll")))
						{
							Settings.ExtractResource(SporebinEP1, "ModLoaderDLLs", "dinput8.dll");
						}

						// Steam users need to do something different
						if (!SporeIsInstalledOnSteam())
						{

							MessageDisplay.DebugShowMessageBox("2. Executable type: " + _executableType);

							if (_executableType == GameExecutableType.None)
							{
								// don't execute the game if the user closed the dialog
								return;
							}

							// get the correct executable path
							_executablePath = Path.Combine(SporebinEP1, ExecutableFileNames[_executableType]);

							bool isOriginExe = (_executableType == GameExecutableType.Origin__1_5_1) ||
								(_executableType == GameExecutableType.Origin__March2017);
							bool exeExists = File.Exists(_executablePath);
							//MessageDisplay.ShowMessageBox($"NOT LOCALIZED:\nIs Origin EXE: {isOriginExe}\nEXE exists: {exeExists}");
							if (isOriginExe && (!exeExists))
							{
								try
								{
									CrossProcess.StartLauncher(EXTRACT_ORIGIN_PREREQ, true).WaitForExit();
								}
								catch (Exception ex)
								{
									MessageDisplay.ShowMessageBox($"NOT LOCALIZED: couldn't start laucher:\n\n{ex}");
								}
							}

							string dllEnding = GetExecutableDllSuffix(_executableType);

							MessageDisplay.DebugShowMessageBox("4. DLL suffix: " + dllEnding);

							if (dllEnding == null)
							{
								MessageDisplay.DebugShowMessageBox(GetLocalizedString("LauncherError!GameVersion!NullDllSuffix")); //MessageBox.Show(Strings.VersionNotDetected, CommonStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}

							LaunchNormalSporeProcess();

						}
						else
						{
							LaunchSteamSporeProcess();
						}
					}
				}

				int lastError = System.Runtime.InteropServices.Marshal.GetLastWin32Error();

				if ((CurrentError != 0) && (CurrentError != 18) && (CurrentError != 87) && (CurrentError != lastError))
				{
					try
					{
						ThrowWin32Exception("Something went wrong", CurrentError);
					}
					catch (Exception ex)
					{
						MessageDisplay.RaiseError(new ErrorEventArgs(ex));
					}
				}

				if ((lastError != 0) && (lastError != 18))
					ThrowWin32Exception("Something went wrong", lastError);
			}
			catch (Exception ex)
			{
				MessageDisplay.RaiseError(new ErrorEventArgs(ex));
				return;
			}
		}

		/*public static void ShowError(Exception ex)
		{
			//MessageBox.Show(Strings.GalacticAdventuresNotExecuted + "\n" + ModApiHelpThreadURL + "\n\n" + ex.GetType() + "\n\n" + ex.Message + "\n\n" + ex.StackTrace, CommonStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
			if (ex is System.ComponentModel.Win32Exception)
			{
				var exc = ex as System.ComponentModel.Win32Exception;
				//"NativeErrorCode: " + exc.NativeErrorCode.ToString() + "\n" + 
				MessageBox.Show("ErrorCode: " + exc.ErrorCode.ToString() + "\n" +
					"NativeErrorCode: " + exc.NativeErrorCode.ToString() + "\n" +
					exc.StackTrace
					/* +
					"HResult: " + exc.HResult.ToString() + "\n"/, "Additional Win32Exception Error Info");

				if (exc.InnerException != null)
				{
					MessageBox.Show("ErrorCode: " + exc.InnerException.GetType() + "\n\n" + exc.InnerException.Message + "\n\n" + exc.InnerException.StackTrace, "Win32Exception InnerException Error Info");
				}
			}
			Process.Start(ModApiHelpThreadURL);
		}*/

		static void LaunchNormalSporeProcess()
		{
			MessageDisplay.DebugShowMessageBox("SporebinPath: " + GameInfo.SporebinEP1 + "\nExecutablePath: " + _executablePath + "\nCommand Line Options: " + GetGameCommandLineOptions());

			var processStartInfo = new ProcessStartInfo()
			{
				FileName = _executablePath,
				Arguments = GetGameCommandLineOptions(),
				UseShellExecute = true
			};

			Process.Start(processStartInfo);
		}

		// Steam spore needs special treatment: the game will clsoe if not executed through Steam
		static void LaunchSteamSporeProcess()
		{
			string steamPath = SteamInfo.SteamPath;
			steamPath = Path.Combine(steamPath, "Steam.exe");

			var processStartInfo = new ProcessStartInfo()
			{
				FileName = steamPath,
				Arguments = "-applaunch " + SteamInfo.GalacticAdventuresSteamID.ToString() + " " + GetGameCommandLineOptions(),
				UseShellExecute = true
			};

			Process.Start(processStartInfo);
		}

		static bool ShouldGenerateCommandLineOptions
			=> !Environment.GetCommandLineArgs().Any(x => x.Trim('"').Equals(Settings.LaunchSporeWithoutManagerOptions, StringComparison.OrdinalIgnoreCase));
			//!((Environment.GetCommandLineArgs().Length > 1) && (Environment.GetCommandLineArgs()[1] == Settings.LaunchSporeWithoutManagerOptions)); //(!Environment.GetCommandLineArgs().Contains(UpdaterService.IgnoreUpdatesArg)) && (Environment.GetCommandLineArgs().Length > 1) && (Environment.GetCommandLineArgs()[1] == Settings.LaunchSporeWithoutManagerOptions))

		static NativeMethods.MonitorInfoEx _monitor = new NativeMethods.MonitorInfoEx();
		static bool _monitorSelected = false;
		static string GetGameCommandLineOptions()
		{
			var sb = new StringBuilder();
			if (ShouldGenerateCommandLineOptions)
			{
				if (Settings.ForceGameWindowingMode)
				{
					if (Settings.ForceWindowedMode == 1)
						sb.Append("-f");
					else// if (Settings.ForceWindowedMode == 0)
						sb.Append("-w");

					sb.Append(" ");

					string size = "-r:";

					var monitors = NativeMethods.AllMonitors;
					_monitor = NativeMethods.AllMonitors[0];
					if (Settings.ForceWindowedMode == 2)
					{
						string prM = Settings.PreferredBorderlessMonitor;
						if (!prM.IsNullOrEmptyOrWhiteSpace())
						{
							string[] prefMon = prM.Replace(" ", string.Empty).Split(',');
							if (prefMon.Length == 4)
							{
								if (
										int.TryParse(prefMon[0], out int left) &&
										int.TryParse(prefMon[1], out int top) &&
										int.TryParse(prefMon[2], out int right) &&
										int.TryParse(prefMon[3], out int bottom)
									)
								{

									foreach (var monInfo in monitors)
									{
										var bounds = monInfo.rcMonitor;
										if (
												(bounds.Left == left) &&
												(bounds.Top == top) &&
												(bounds.Right == right) &&
												(bounds.Bottom == bottom)
											)
										{
											_monitor = monInfo;
											_monitorSelected = true;
											//MessageDisplay.ShowMessageBox(prM, monitors.IndexOf(_monitor).ToString());
											break;
										}
									}

									if (!_monitorSelected)
                                    {
										int width = right - left;
										int height = bottom - top;
										foreach (var monInfo in monitors)
										{
											var bounds = monInfo.rcMonitor;
											if (
												((bounds.Right - bounds.Left) == width) &&
												((bounds.Bottom - bounds.Top) == height)
												)
											{
												_monitor = monInfo;
												_monitorSelected = true;

												break;
											}
										}

									}
								}
							}
						}
					}

					if (Settings.ForceWindowedMode == 2)
					{
						size += (_monitor.rcMonitor.Right - _monitor.rcMonitor.Left).ToString() + "x" + (_monitor.rcMonitor.Bottom - _monitor.rcMonitor.Top).ToString();
						//MessageDisplay.ShowMessageBox("_monitorSelected: " + _monitorSelected.ToString() + ", size: " + size);
					}
					else if (Settings.ForceGameWindowBounds)
					{
						//MessageDisplay.DebugShowMessageBox("MONITOR: " + monitor.rcMonitor.Left + ", " + monitor.rcMonitor.Top + ", " + monitor.rcMonitor.Right + ", " + monitor.rcMonitor.Bottom + "\n" + +monitor.rcWork.Left + ", " + monitor.rcWork.Top + ", " + monitor.rcWork.Right + ", " + monitor.rcWork.Bottom);

						if (Settings.AutoGameWindowBounds)
						{
							MessageDisplay.DebugShowMessageBox("Settings.AutoGameWindowBounds is true");
							if (Settings.ForceGameWindowingMode)
							{
								MessageDisplay.DebugShowMessageBox("Settings.ForceGameWindowingMode is true, Settings.ForceWindowedMode is " + Settings.ForceWindowedMode);
								if (Settings.ForceWindowedMode == 0)
								{
									size += (_monitor.rcWork.Right - _monitor.rcWork.Left);
								}
								else// if (Settings.ForceWindowedMode == 1)
									size += (_monitor.rcMonitor.Right - _monitor.rcMonitor.Left);
								/*else
									size += Settings.ForcedGameWindowWidth;*/
							}
							else
								size += Settings.ForcedGameWindowWidth;
						}
						else
							size += Settings.ForcedGameWindowWidth;

						size += "x";

						if (Settings.AutoGameWindowBounds)
						{
							if (Settings.ForceGameWindowingMode)
							{
								int maximizedTitlebarHeight = CaptionHeight;
								if (Settings.ForceWindowedMode == 0)
									size += ((_monitor.rcWork.Bottom - _monitor.rcWork.Top) - maximizedTitlebarHeight);
								else if (Settings.ForceWindowedMode == 1)
									size += ((_monitor.rcMonitor.Bottom - _monitor.rcMonitor.Top) - maximizedTitlebarHeight);
								else
									size += Settings.ForcedGameWindowHeight;
							}
							else
								size += Settings.ForcedGameWindowHeight;
						}
						else
							size += Settings.ForcedGameWindowHeight;
					}
					else
						MessageDisplay.DebugShowMessageBox("Settings.ForceGameWindowBounds is false!");

					sb.Append(size);

					sb.Append(" ");
				}

				if (Settings.ForceGameLocale && (!string.IsNullOrWhiteSpace(Settings.ForcedGameLocale)))
				{
					string option = "-locale:";
					if (Settings.ForcedGameLocale.StartsWith("-"))
						option += Settings.ForcedGameLocale.Substring(1);
					else
						option += Settings.ForcedGameLocale;

					sb.Append(option);
					sb.Append(" ");
				}

				if (Settings.UseCustomGameState && (!string.IsNullOrWhiteSpace(Settings.GameState)))
				{
					sb.Append("-state:" + Settings.GameState);
					sb.Append(" ");
				}

				if (!string.IsNullOrWhiteSpace(Settings.CommandLineOptions))
				{
					string[] additionalOptions;
					if (Settings.CommandLineOptions.Contains(" "))
						additionalOptions = Settings.CommandLineOptions.Split(" ".ToCharArray());
					else
						additionalOptions = new string[] { Settings.CommandLineOptions };

					foreach (string arg in additionalOptions)
					{
						sb.Append(arg);
						sb.Append(" ");
					}
				}
			}
			else
			{
				int i = 0;
				foreach (string arg in Environment.GetCommandLineArgs())
				{
					if ((i != 0) && (arg.ToLowerInvariant() != Settings.LaunchSporeWithoutManagerOptions.ToLowerInvariant()))
					{
						string arg2 = $"\"{arg}\"";
						sb.Append(arg2);
						sb.Append(" ");
					}
					i++;
				}
			}

			return sb.ToString();
		}

		static void EnableBorderless(int pid, NativeMethods.MonitorInfoEx monitor)
		{
			if (ShouldGenerateCommandLineOptions && Settings.ForceGameWindowingMode && (Settings.ForceWindowedMode == 2))
			{
				var monBounds = _monitor.rcMonitor;
				//MessageDisplay.ShowMessageBox("_monitorSelected: " + _monitorSelected.ToString() + "\n_monitor.rcMonitor: " + $"{monBounds.Left},{monBounds.Top},{monBounds.Right},{monBounds.Bottom},,,{monBounds.Right - monBounds.Left},{monBounds.Bottom - monBounds.Top}");
				Process process = Process.GetProcessById(pid);
				Debug.WriteLine("Before loop");

				IntPtr spore = IntPtr.Zero;
				while (!NativeMethods.IsWindow(spore)) //(spore == IntPtr.Zero) ||  //(!process.HasExited) && ((process.MainWindowHandle == IntPtr.Zero) || (!NativeMethods.IsWindow(process.MainWindowHandle))))
				{
					if (process.HasExited)
						break;

					spore = GetSporeMainWindow(pid);
				}
				Debug.WriteLine("After loop");
				if (NativeMethods.IsWindow(spore)) //(!process.HasExited) && (process.MainWindowHandle != IntPtr.Zero) && NativeMethods.IsWindow(process.MainWindowHandle))
				{
					Debug.WriteLine("process.MainWindowTitle: " + process.MainWindowTitle);
					//var monitor = NativeMethods.AllMonitors[0];
					//NativeMethods.SetWindowLong(win.Handle, NativeMethods.GwlExstyle, NativeMethods.GetWindowLong(win.Handle, NativeMethods.GwlExstyle).ToInt32() & ~NativeMethods.WsExNoActivate);
					int winStyle = (Int32)NativeMethods.GetWindowLong(spore, NativeMethods.GwlStyle);
					winStyle &= ~NativeMethods.WsBorder;
					winStyle &= ~NativeMethods.WsCaption;
					NativeMethods.SetWindowLong(spore, NativeMethods.GwlStyle, winStyle);

					int x = monBounds.Left;
					int y = monBounds.Top;
					int cx = monBounds.Right - monBounds.Left;
					int cy = monBounds.Bottom - monBounds.Top;
					uint uFlags = 0x0010 | 0x0004 | 0x0020;

					/*KWin or something on Kubuntu decides that having Spore fill one monitor means it was 
					 * actually supposed to fill the other, and unhelpfully "fixes" it for us, so now we
					 * have to "break" this "feature" in order to prevent that "fix" from "fixing" our
					 * "mistake" that was actually the intended behaviour by intentionally being a pixel
					 * or two off. Fun. Note to self: Figure out how */
					if (false && Settings.NonEssentialIsRunningUnderWine)
					{
						cx--;
						/*var furthestRight = monitor;
						foreach (var mon in NativeMethods.AllMonitors)
						{
							if (mon.rcMonitor.Right > furthestRight.rcMonitor.Right)
								furthestRight = mon;
						}

						if (furthestRight.Equals(monitor))
						{
							x++;
							cx--;
						}
						else
						{
							var furthestLeft = monitor;
							foreach (var mon in NativeMethods.AllMonitors)
							{
								if (mon.rcMonitor.Left > furthestLeft.rcMonitor.Left)
									furthestLeft = mon;
							}

							if (furthestLeft.Equals(monitor))
							{
								cx--;
							}
							else
							{
								x++;
								cx -= 2;
							}
						}*/
					}

					NativeMethods.SetWindowPos(spore, IntPtr.Zero, x, y, cx, cy, uFlags);
				}
			}
		}

		public static Func<int, IntPtr> GetSporeMainWindow = null;

		/*string ProcessSporebinPath()
		{
			string path = PathDialogs.ProcessGalacticAdventures();

			if (path == null || !Directory.Exists(path))
			{

				throw new InjectException(CommonStrings.GalacticAdventuresNotFound);
			}

			this.SporebinPath = path;

			return path;
		}

		string ProcessSteamPath()
		{
			string path = PathDialogs.ProcessSteam();

			if (path == null || !Directory.Exists(path))
			{

				throw new InjectException(CommonStrings.SteamNotFound);
			}

			this.SteamPath = path;

			return path;
		}

		GameVersionType ProcessExecutableType()
		{
			GameVersionType executableType = GameVersion.DetectVersion(this.ExecutablePath);

			// for debugging purposes
			//executableType = GameVersionType.None;

			if (executableType == GameVersionType.None)
			{
				if (LauncherSettings.GameVersion != GameVersionType.None)
				{
					executableType = LauncherSettings.GameVersion;
				}
				else
				{
					executableType = ShowVersionSelectorDialog();

					// The detection should work fine unless you have the wrong version, so tell the user                        
					MessageBox.Show(Strings.MightNotWork, Strings.MightNotWorkTitle,
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				}
			}

			this.ExecutableType = executableType;

			return executableType;
		}
		*/

		public static void ThrowWin32Exception(string info, int error)
		{
			if ((error != 0) && (error != 18))
			{
				var exception = new System.ComponentModel.Win32Exception(error);
				//System.Windows.Forms.MessageBox.Show("Error: " + error.ToString() + "\n" + info);
				MessageDisplay.RaiseError(new ErrorEventArgs(exception));
				throw exception;
			}
		}

		public static bool IsInstalledDarkInjectionCompatible()
		{
			string di230 = "di_9_r_beta2-3-0".ToLowerInvariant();
			try
			{
				bool returnValue = true;
				//InstalledMods mods = new InstalledMods();
				//mods.Load();
				MessageDisplay.DebugShowMessageBox("BEGIN MOD CONFIGURATION NAMES");
				foreach (IInstalledMod configuration in ModsManager.InstalledMods)
				{
					MessageDisplay.DebugShowMessageBox(configuration.RealName);
					if (configuration.Unique.ToLowerInvariant().Contains(di230))
					{
						returnValue = false;
						break;
					}
				}
				MessageDisplay.DebugShowMessageBox("END MOD CONFIGURATION NAMES");
				if (!returnValue)
				{
					//TODO: Show error message
					SporeLauncher.OpenDarkInjectionPage();
				}
				return returnValue;
			}
			catch (Exception ex)
			{
				MessageDisplay.DebugShowMessageBox("FAILED TO INSPECT MOD CONFIGURATION NAMES\n" + ex);
				return true;
			}
		}

		public static GameExecutableType GetExecutableType()
		{
			if (Settings.ForcedGameExeType.IsNullOrEmptyOrWhiteSpace())
				return ExecutableFileGameTypes[new FileInfo(_executablePath).Length];
			else if (Enum.TryParse(Settings.ForcedGameExeType, out GameExecutableType exeType))
				return exeType;
			else
				throw new InvalidOperationException("No Spore executable type available. Last-minute UI to specify NYI.");
		}

		public static bool IsValidExe()
		{
			//string errorBase = GetLocalizedString("LauncherError!GameVersion!NotRecognized");
			if (!(Settings.ForcedGameExeType.IsNullOrEmptyOrWhiteSpace()))
				return true;
			else if (TryGetExeVersion(_executablePath, out Version exeVersion))
			{
				if ((exeVersion < Spore__March2017) && (exeVersion != Spore__1_5_1))
				{
					MessageDisplay.RaiseError(new ErrorEventArgs(new InvalidOperationException(GetLocalizedString("LauncherError!GameVersion!TooOld"))));
					return false;
				}
				else if (exeVersion > Spore__March2017)
				{
					MessageDisplay.RaiseError(new ErrorEventArgs(new InvalidOperationException(GetLocalizedString("LauncherError!GameVersion!WaitDidTheyActuallyUpdateSpore"))));
					return false;
				}
				else
					return true;
			}
			else
			{
				MessageDisplay.RaiseError(new ErrorEventArgs(new InvalidOperationException(GetLocalizedString("LauncherError!GameVersion!ReadFailed"))));
				return false;
			}
		}

		public static bool TryGetExeVersion(string executablePath, out Version exeVersion)
		{
			bool retVal = Version.TryParse(FileVersionInfo.GetVersionInfo(executablePath).FileVersion, out exeVersion);
			return retVal;
		}

		static List<Process> GetSporeProcesses()
		{
			List<Process> processes = Process.GetProcessesByName("SporeApp").ToList();
			processes.AddRange(Process.GetProcessesByName("SporeApp_ModAPIFix"));
			return processes;
		}

		public static bool IsSporeRunning()
		{
			return GetSporeProcesses().Count() > 0;
		}

		public static void KillSporeProcesses()
		{
			try
			{
				List<Process> sporeProcesses = GetSporeProcesses();

				for (int i = 0; i < sporeProcesses.Count; i++)
					sporeProcesses[0].Kill();
			}
			catch (Exception ex)
			{
				MessageDisplay.ShowMessageBox(GetLocalizedString("KillSporeError") + "\n\n" + ex.GetType() + ": " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
			}
		}	
	}
}