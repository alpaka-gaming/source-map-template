// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Source.Interfaces;

namespace Nuke.Common.Tools.Source.Tooling
{
    /// <summary>
    /// https://developer.valvesoftware.com/wiki/VVIS
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class VVIS : Tools, ISlammin
    {

	    public VVIS() : base("vvis.exe")
	    {

	    }


        /// <summary>
        /// Only do a quick first pass. Does not actually test visibility.
        /// </summary>
        public override bool? Fast { get; internal set; }

        /// <summary>
        /// Force a maximum vis radius, in units, regardless of whether an env_fog_controller specifies one.
        /// </summary>
        public virtual int? RadiusOverride  { get; internal set; }

        /// <summary>
        /// Don't sort (an optimization) portals.
        /// </summary>
        public virtual bool? NoSort { get; internal set; }

        /// <summary>
        /// Read portals from \tmp\mapname.
        /// </summary>
        public virtual bool? TmpIn { get; internal set; }

        /// <summary>
        /// Write portals to \tmp\mapname.
        /// </summary>
        public virtual bool? TmpOut { get; internal set; }


        /// <summary>
        ///
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        protected override Arguments ConfigureProcessArguments(Arguments arguments)
        {
	        arguments
		        .Add("-verbose", Verbose)
		        .Add("-threads", Threads)
		        .Add("-low", Low)
		        .Add("-vproject {value}", VProject)
		        .Add("-game {value}", Game)
		        //
		        .Add("-fast", Fast)
		        .Add("-radius_override {value}", RadiusOverride)
		        .Add("-nosort", NoSort)
		        .Add("-tmpin", TmpIn)
		        .Add("-tmpout", TmpOut)
		        //
		        .Add("{value}", Input);
            return base.ConfigureProcessArguments(arguments);
        }

        public bool? UsingSlammin { get; set; }
    }

    public static partial class Extensions
    {

        #region NoSort

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <param name="nosort"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T SetNoSort<T>(this T toolSettings, bool? nosort) where T : VVIS
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.NoSort = nosort;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ResetNoSort<T>(this T toolSettings) where T : VVIS
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.NoSort = null;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T EnableNoSort<T>(this T toolSettings) where T : VVIS
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.NoSort = true;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T DisableNoSort<T>(this T toolSettings) where T : VVIS
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.NoSort = false;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ToggleNoSort<T>(this T toolSettings) where T : VVIS
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.NoSort = !toolSettings.NoSort;
	        return toolSettings;
        }

        #endregion

    }

}
