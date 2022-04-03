﻿using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

using SporeMods.Core.Mods;
using SporeMods.Core.Transactions;

namespace SporeMods.Core.Mods
{
    public interface ISporeMod
    {
#region Metadata

        /// <summary>
        /// The name of the mod, to show the user.
        /// </summary>
        IModText DisplayName
        {
            get;
        }


        /// <summary>
        /// Whether or not the mod explicitly provided a unique identifier for itself.
        /// If false, the mod's identity and unique identifier were generated by the SMM.
        /// </summary>
        bool HasExplicitUnique
        {
            get;
        }

        /// <summary>
        /// The mod's unique identifier.
        /// </summary>
        string Unique
        {
            get;
        }

        /// <summary>
        /// Whether or not the mod has an inline description to show in the installed mods list.
        /// </summary>
        bool HasInlineDescription
        {
            get;
        }

        /// <summary>
        /// The mod's inline description. Shown in the installed mods list.
        /// </summary>
        IModText InlineDescription
        {
            get;
        }

        /// <summary>
        /// Whether or not the mod explicitly provided a version.
        /// </summary>
        bool HasExplicitVersion
        {
            get;
        }


        /// <summary>
        /// The version of this mod which is currently installed.
        /// </summary>
        Version ModVersion
        {
            get;
        }


        /// <summary>
        /// Information about other mods which this mod requires in order to function.
        /// </summary>
        List<ModDependency> Dependencies
        {
            get;
        }


        /// <summary>
        /// Contains implicit unique identifiers for pre-mod identity version of this mod, so they can be upgraded to a version with a mod identity.
        /// </summary>
        List<string> UpgradeTargets
        {
            get;
        }

#endregion




#region Warnings

        /// <summary>
        /// Whether or not this is an experimental mod release (may have unforeseen pitfalls, bugs, etc.)
        /// </summary>
        bool IsExperimental
        {
            get;
        }

        /// <summary>
        /// Whether or not playing with this mod will cause save data corruption or other undesirable effects if it is subsequently uninstalled
        /// </summary>
        bool CausesSaveDataDependency
        {
            get;
        }

        /// <summary>
        /// Whether or not this mod requires a Galaxy reset in order to work correctly
        /// </summary>
        bool RequiresGalaxyReset
        {
            get;
        }

        /// <summary>
        /// Whether or not this mod requires custom code injection in order to work correctly
        /// </summary>
        bool UsesCodeInjection
        {
            get;
        }

        /// <summary>
        /// True only if use of this mod will NEVER result in player creations which require the mod to be installed in order to work correctly
        /// </summary>
        bool GuaranteedVanillaCompatible
        {
            get;
        }

        /// <summary>
        /// Whether or not this mod is known to frequently cause problems, yet fails to disclose them adequately via standard warnings. The value of this property is dictated not by the mod, but by the SMM itself, to warn users of problems that weren't known until it was too late.
        /// It is not our place to unconditionally forbid the user from installing these mods. No matter how heavily the SMM ends up having to discourage them from doing so, the user must always be able to override our judgement if they truly see fit.
        /// The SMM is not a babysitter to the user, but an advisor and assistant - that is the baseline of respect we, as developers, owe our users.
        /// </summary>
        bool KnownHazardousMod
        {
            get;
        }

#endregion






#region Utils

        bool IsUpgradeTo(ISporeMod mod);
        bool DependsOn(ISporeMod mod);

        #endregion


        #region Management functions
        
        
        Task<ModJobBatchEntryBase> EnsureCanInstall(ModJobBatchModEntry entry, List<ModJobBatchModEntry> otherEntries);

        /// <summary>
        /// Extract all of this mod's files from the incoming archive to its <see cref="SporeMods.Core.Settings.ModConfigsPath"/>
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        IAsyncOperation GetExtractRecordFilesAsyncOp(ModTransaction transaction, string inPath, ZipArchive archive = null);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        IAsyncOperation GetApplyAsyncOp(ModTransaction transaction);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        IAsyncOperation GetPurgeAsyncOp(ModTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        IAsyncOperation GetRemoveRecordFilesAsyncOp(ModTransaction transaction, bool removeConfig);

#endregion
    }
}