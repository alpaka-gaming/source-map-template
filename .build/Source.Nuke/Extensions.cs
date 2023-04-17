// ReSharper disable IdentifierTypo

using System;
using JetBrains.Annotations;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Source.Interfaces;

namespace Nuke.Common.Tools.Source
{
	public static class Extensions
	{
		#region Verbose

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="verbose"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetVerbose<T>(this T toolSettings, bool? verbose) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Verbose = verbose;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetVerbose<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Verbose = null;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T EnableVerbose<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Verbose = true;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T DisableVerbose<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Verbose = false;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ToggleVerbose<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Verbose = !toolSettings.Verbose;
			return toolSettings;
		}

		#endregion

		#region Skip

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="skip"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetSkip<T>(this T toolSettings, bool skip) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Skip = skip;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetSkip<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Skip = false;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T EnableSkip<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Skip = true;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T DisableSkip<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Skip = false;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ToggleSkip<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Skip = !toolSettings.Skip;
			return toolSettings;
		}

		#endregion

		#region Game

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="game"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetGamePath<T>(this T toolSettings, string game) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Game = game;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetGame<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Game = null;
			return toolSettings;
		}

		#endregion

		#region Game

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="appId"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetAppId<T>(this T toolSettings, long appId) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.AppId = appId;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetAppId<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.AppId = 0;
			return toolSettings;
		}

		#endregion

		#region Input

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="input"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetInput<T>(this T toolSettings, string input) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Input = input;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetInput<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Input = null;
			return toolSettings;
		}

		#endregion

		#region Output

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="output"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetOutput<T>(this T toolSettings, string output) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Output = output;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetOutput<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Output = null;
			return toolSettings;
		}

		#endregion

		#region Callback

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="callback"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetCallback<T>(this T toolSettings, Action callback) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Callback = callback;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetCallback<T>(this T toolSettings) where T : Tools
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Callback = null;
			return toolSettings;
		}

		#endregion

		#region Low

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <param name="low"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T SetLow<T>(this T toolSettings, bool? low) where T : Tools
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Low = low;
            return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ResetLow<T>(this T toolSettings) where T : Tools
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Low = null;
            return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T EnableLow<T>(this T toolSettings) where T : Tools
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Low = true;
            return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T DisableLow<T>(this T toolSettings) where T : Tools
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Low = false;
            return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ToggleLow<T>(this T toolSettings) where T : Tools
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Low = !toolSettings.Low;
            return toolSettings;
        }

        #endregion

        #region Threads

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <param name="threads"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T SetThreads<T>(this T toolSettings, ushort threads) where T : Tools
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Threads = threads;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ResetThreads<T>(this T toolSettings) where T : Tools
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Threads = null;
	        return toolSettings;
        }

        #endregion

        #region VProject

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <param name="vproject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T SetVProject<T>(this T toolSettings, string vproject) where T : Tools
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.VProject = vproject;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ResetVProject<T>(this T toolSettings) where T : Tools
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.VProject = null;
	        return toolSettings;
        }

        #endregion

        #region Fast

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <param name="fast"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T SetFast<T>(this T toolSettings, bool? fast) where T : Tools
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Fast = fast;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ResetFast<T>(this T toolSettings) where T : Tools
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Fast = null;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T EnableFast<T>(this T toolSettings) where T : Tools
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Fast = true;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T DisableFast<T>(this T toolSettings) where T : Tools
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Fast = false;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ToggleFast<T>(this T toolSettings) where T : Tools
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Fast = !toolSettings.Fast;
	        return toolSettings;
        }

        #endregion

        #region SlamminTools

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <param name="usingSlammin"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T SetUsingSlammin<T>(this T toolSettings, bool? usingSlammin) where T : Tools, ISlammin
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.UsingSlammin = usingSlammin;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ResetUsingSlammin<T>(this T toolSettings) where T : Tools, ISlammin
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.UsingSlammin = null;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T EnableUsingSlammin<T>(this T toolSettings) where T : Tools, ISlammin
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.UsingSlammin = true;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T DisableUsingSlammin<T>(this T toolSettings) where T : Tools, ISlammin
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.UsingSlammin = false;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ToggleUsingSlammin<T>(this T toolSettings) where T : Tools, ISlammin
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.UsingSlammin = !toolSettings.UsingSlammin;
	        return toolSettings;
        }

        #endregion

        #region InstallDir

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <param name="installDir"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T SetInstallDir<T>(this T toolSettings, string installDir) where T :  Tools, IDownloadable
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.InstallDir = installDir;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ResetInstallDir<T>(this T toolSettings) where T :  Tools, IDownloadable
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.InstallDir = null;
	        return toolSettings;
        }

        #endregion

	}
}
