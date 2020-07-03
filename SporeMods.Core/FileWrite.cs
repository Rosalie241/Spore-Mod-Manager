﻿using SporeMods.Core.InstalledMods;
using SporeMods.Core.ModIdentity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SporeMods.Core
{
    public static class FileWrite
    {
        public static readonly string[] ProtectedFileNames =
        {
            "sporemodapi-disk",
            "sporemodapi-steam",
            "sporemodapi-steam_patched",

            "sporemodapi.lib",
            "sporemodapi.disk.dll",
            "sporemodapi.march2017.dll",

            "spore_audio1",
            "spore_audio2",
            "spore_content",
            "spore_game",
            "spore_graphics",

            "patchdata",

            "spore_pack_03",

            "boosterpack_01",
            "spore_ep1_content_01",
            "spore_ep1_content_02",
            "spore_ep1_data",
            "spore_ep1_locale_01",
            "spore_ep1_locale_02",

            "ep1_patchdata",

            "bp2_data"
        };

        public static bool IsUnprotectedFile(string path, bool isFullPath)
        {
            bool canCopy = true;
            string name = path;
            if (isFullPath)
                name = Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
            foreach (string s in ProtectedFileNames)
            {
                if (name.Equals(s, StringComparison.OrdinalIgnoreCase))
                {
                    canCopy = false;
                    break;
                }
            }

            return canCopy;
        }

        public static bool IsUnprotectedFile(string path)
        {
            return IsUnprotectedFile(path, true);
        }

        public static void SafeCopyFile(string sourcePath, string destPath)
        {
            try
            {
                if (IsUnprotectedFile(destPath))
                {
                    if (File.Exists(destPath))
                        File.Delete(destPath);


                    File.Copy(sourcePath, destPath);
                    if (!File.Exists(destPath))
                        throw new FileNotFoundException("destination missing: " + destPath);
                }
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("source missing: " + sourcePath, ex);
            }
        }

        public static void SafeDeleteFile(string targetPath)
        {
            if (File.Exists(targetPath) && IsUnprotectedFile(targetPath))
                File.Delete(targetPath);
        }



        public static string GetFileOutputPath(string dir, string fileName, bool isLegacy)
        {
            string safeFileName = Path.GetFileName(fileName);
            if (dir.ToLowerInvariant() == ComponentGameDir.galacticadventures.ToString()) //"galacticadventures")
                return Path.Combine(GameInfo.GalacticAdventuresData, safeFileName);
            else if (dir.ToLowerInvariant() == ComponentGameDir.spore.ToString()) //"spore")
                return Path.Combine(GameInfo.CoreSporeData, safeFileName);
            else if (dir.ToLowerInvariant() == ComponentGameDir.modapi.ToString())
            {
                if (isLegacy)
                    return Path.Combine(Settings.LegacyLibsPath, safeFileName);
                else
                    return Path.Combine(Settings.ModLibsPath, safeFileName);
            }
            else
                return null;
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }
    }
}
