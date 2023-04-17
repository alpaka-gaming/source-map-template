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
	public class DEPOTDOWNLOADER : Tools, IDownloadable
	{

		public DEPOTDOWNLOADER() : base("DepotDownloader.exe")
		{

		}

		public override string ProcessToolPath => Path.Combine(InstallDir, Executable);

		/// <summary>
		/// Overwrite existing output files
		/// </summary>
		public virtual bool? Force { get; internal set; }

		public virtual string Username { get; internal set; }
		public virtual string Password { get; internal set; }
		public virtual string InstallDir { get; set; }

		/// <summary>
		/// Keep (don't delete) input files
		/// </summary>
		/// <param name="arguments"></param>
		/// <returns></returns>
		protected override Arguments ConfigureProcessArguments(Arguments arguments)
		{
			arguments
				.Add("-username {value}", Username)
				.Add("-password {value}", Password)
				.Add("-app {value}", AppId)
				.Add("-dir {value}", Path.Combine(InstallDir, AppId.ToString()));
			return base.ConfigureProcessArguments(arguments);
		}

		public string Url => "https://github.com/SteamRE/DepotDownloader/releases/download/DepotDownloader_2.4.7/depotdownloader-2.4.7.zip";
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
					localDir = toolPath; //Path.Combine(toolPath, GetType().Name.ToLower());
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

		#region Username

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="username"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetUsername<T>(this T toolSettings, string username) where T : DEPOTDOWNLOADER
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Username = username;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetUsername<T>(this T toolSettings) where T : DEPOTDOWNLOADER
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Username = null;
			return toolSettings;
		}

		#endregion

		#region Password

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="password"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetPassword<T>(this T toolSettings, string password) where T : DEPOTDOWNLOADER
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Password = password;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetPassword<T>(this T toolSettings) where T : DEPOTDOWNLOADER
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Username = null;
			return toolSettings;
		}

		#endregion


	}
}
