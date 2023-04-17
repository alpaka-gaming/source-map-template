// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using JetBrains.Annotations;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Source.Interfaces;

namespace Nuke.Common.Tools.Source.Tooling
{
	/// <summary>
	///
	/// </summary>
	[PublicAPI]
	[ExcludeFromCodeCoverage]
	[Serializable]
	public class BZIP : Tools, IDownloadable
	{

		public BZIP() : base("bzip2.exe")
		{

		}

		/// <summary>
		/// Overwrite existing output files
		/// </summary>
		public virtual bool? Force { get; internal set; }

		public virtual bool Keep { get; internal set; } = true;

		public virtual string InstallDir { get; set; }

		/// <summary>
		/// Keep (don't delete) input files
		/// </summary>
		/// <param name="arguments"></param>
		/// <returns></returns>
		protected override Arguments ConfigureProcessArguments(Arguments arguments)
		{
			arguments
				.Add("--verbose", Verbose)
				//
				.Add("--force", Force)
				.Add("--keep", Keep)
				//
				.Add("{value}", Input);
			return base.ConfigureProcessArguments(arguments);
		}

		public string Url => "https://github.com/philr/bzip2-windows/releases/download/v1.0.8.0/bzip2-1.0.8.0-win-x64.zip";
		public bool Download()
		{
			var localFile = string.Empty;
			var localDir = string.Empty;
			var fileName = Path.GetFileName(Url);
			if (fileName != null)
			{
				var toolPath = Path.GetDirectoryName(ProcessToolPath);
				if (string.IsNullOrWhiteSpace(toolPath)) return false;
				if (!string.IsNullOrWhiteSpace(toolPath))
				{
					localFile = Path.Combine(toolPath, fileName);
					localDir = toolPath;
				}
				if (string.IsNullOrWhiteSpace(localFile)) return false;
				if (!File.Exists(localFile))
				{
					using (var client = new HttpClient())
					{
						var response = client.Send(new HttpRequestMessage(HttpMethod.Get, Url));
						using var resultStream = response.Content.ReadAsStream();
						using var fileStream = File.OpenWrite(localFile);
						resultStream.CopyTo(fileStream);
					}
				}
				if (File.Exists(localFile) && !File.Exists(Path.Combine(localDir, Executable)))
					ZipFile.ExtractToDirectory(localFile, localDir, true);
			}
			return !string.IsNullOrWhiteSpace(localFile) && File.Exists(localFile) &&
			       !string.IsNullOrWhiteSpace(localDir) && Directory.Exists(localDir) &&
			       File.Exists(Path.Combine(localDir, Executable));
		}
	}

	public static partial class Extensions
	{
		#region Force

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <param name="force"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T SetForce<T>(this T toolSettings, bool? force) where T : BZIP
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Force = force;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ResetForce<T>(this T toolSettings) where T : BZIP
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Force = null;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T EnableForce<T>(this T toolSettings) where T : BZIP
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Force = true;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T DisableForce<T>(this T toolSettings) where T : BZIP
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Force = false;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ToggleForce<T>(this T toolSettings) where T : BZIP
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Force = !toolSettings.Force;
	        return toolSettings;
        }

        #endregion

        #region Keep

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <param name="keep"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T SetKeep<T>(this T toolSettings, bool keep) where T : BZIP
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Keep = keep;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ResetKeep<T>(this T toolSettings) where T : BZIP
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Keep = true;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T EnableKeep<T>(this T toolSettings) where T : BZIP
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Keep = true;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T DisableKeep<T>(this T toolSettings) where T : BZIP
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Keep = false;
	        return toolSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toolSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        public static T ToggleKeep<T>(this T toolSettings) where T : BZIP
        {
	        toolSettings = toolSettings.NewInstance();
	        toolSettings.Keep = !toolSettings.Keep;
	        return toolSettings;
        }

        #endregion

	}
}
