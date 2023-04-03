// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using JetBrains.Annotations;
using Nuke.Common.Tooling;

namespace Nuke.Common.Tools.Source.Tooling
{
	/// <summary>
	/// https://developer.valvesoftware.com/wiki/BSPZIP
	/// </summary>
	[PublicAPI]
	[ExcludeFromCodeCoverage]
	[Serializable]
	public class PACK : Tools
	{

		public PACK() : base("bspzip.exe")
		{

		}

		public string FileList { get; internal set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="arguments"></param>
		/// <returns></returns>
		protected override Arguments ConfigureProcessArguments(Arguments arguments)
		{
			if (string.IsNullOrWhiteSpace(FileList))
			{
				arguments
					//.Add("-verbose", Verbose)
					.Add("-dir {value}", Input);
			}
			else
			{
				arguments
					.Add("-addlist {value}", Input)
					.Add("{value}", FileList)
					.Add("{value}", Path.ChangeExtension(Input, "bzp"));
			}
			return base.ConfigureProcessArguments(arguments);
		}

	}

	public static partial class Extensions
	{
		#region FileList

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="fileList"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetFileList<T>(this T toolSettings, string fileList) where T : PACK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.FileList = fileList;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetFileList<T>(this T toolSettings) where T : PACK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.FileList = null;
			return toolSettings;
		}

		#endregion
	}
}
