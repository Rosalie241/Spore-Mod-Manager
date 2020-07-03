﻿using SporeMods.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using static SporeMods.Core.Injection.SporeLauncher;

namespace SporeMods.DragServant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        FileSystemWatcher _launcherWatcher = new FileSystemWatcher(Settings.TempFolderPath)
        {
            EnableRaisingEvents = true,
            IncludeSubdirectories = false
        };


        public App()
        {
            var win = new MainWindow();
            win.Show();
            MainWindow = win;
            _launcherWatcher.Created += (sneder, args) =>
            {
                if (Path.GetFileName(args.FullPath) == "LaunchGame")
                {
                    StartLauncher();
                    //Process.Start(Path.Combine(Settings.ManagerInstallLocationPath, "Spore ModAPI Launcher.exe"));
                    File.Delete(args.FullPath);
                }
            };
        }
    }
}
