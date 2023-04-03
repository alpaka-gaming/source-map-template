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
	/// https://developer.valvesoftware.com/wiki/Cubemaps
	/// </summary>
	[PublicAPI]
	[ExcludeFromCodeCoverage]
	[Serializable]
	public class CUBEMAP : Tools
	{

		public CUBEMAP() : base("hl2.exe")
		{

		}

		public override string ProcessToolPath => Path.Combine(Game, "..", Executable);

		public int MatSpecular { get; internal set; } = 0;
		public int MatHdrLevel { get; internal set; } = 2;


		/// <summary>
		///
		/// </summary>
		/// <param name="arguments"></param>
		/// <returns></returns>
		protected override Arguments ConfigureProcessArguments(Arguments arguments)
		{
			arguments
				.Add("-steam")
				.Add("-windowed")
				.Add("-novid")
				.Add("-nosound")
				.Add("+mat_specular {value}", MatSpecular)
				.Add("+mat_hdr_level {value}", MatHdrLevel)
				.Add("+map {value}", Input)
				.Add("-game {value}", Game)
				.Add("-buildcubemaps");
			return base.ConfigureProcessArguments(arguments);
		}

	}

	public static partial class Extensions
	{

		#region MatSpecular

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="matspecular"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetMatSpecular<T>(this T toolSettings, int matspecular) where T : CUBEMAP
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MatSpecular = matspecular;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetMatSpecular<T>(this T toolSettings) where T : CUBEMAP
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MatSpecular = 0;
			return toolSettings;
		}

		#endregion

		#region MatSpecular

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="mathdrlevel"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetMatHDRLevel<T>(this T toolSettings, int mathdrlevel) where T : CUBEMAP
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MatHdrLevel = mathdrlevel;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetMatHDRLevel<T>(this T toolSettings) where T : CUBEMAP
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MatHdrLevel = 2;
			return toolSettings;
		}

		#endregion

	}
}
