// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Nuke.Common.Tooling;

namespace Nuke.Common.Tools.Source.Tooling
{
	/// <summary>
	/// https://developer.valvesoftware.com/wiki/VPK
	/// </summary>
	[PublicAPI]
	[ExcludeFromCodeCoverage]
	[Serializable]
	public class VPK : Tools
	{

		public VPK() : base("vpk.exe")
		{

		}

		/// <summary>
		/// Keep (don't delete) input files
		/// </summary>
		/// <param name="arguments"></param>
		/// <returns></returns>
		protected override Arguments ConfigureProcessArguments(Arguments arguments)
		{
			arguments
				.Add("-v", Verbose)
				.Add("-M", MultiChunk)
				.Add("-P", SteamPipe)
				.Add("-c {value}", ChunkSize)
				.Add("-a {value}", Align)
				.Add("-K {value}", PrivateKey)
				.Add("-k {value}", PublicKey)
				.Add("{value}", Input);
			return base.ConfigureProcessArguments(arguments);
		}

		/// <summary>
		/// Produce a multi-chunk VPK.
		/// Note: Required if creating a VPK with key values.
		/// Each chunk is a file limited to around 200MB.
		/// To reduce patch sizes, chunks are never overwritten. New/modified files are instead written to a brand new chunk every time you run the tool.
		/// Note: Multi-chunk generations only works when creating a VPK from a response file.
		/// Tip: To inspect a multi-chunk VPK open the '_dir' file.
		/// </summary>
		public bool? MultiChunk { get; internal set; }

		/// <summary>
		/// Use SteamPipe-friendly incremental build algorithm. Use with 'k' command. For optimal incremental build performance, the control file used for the previous build should exist and be named the same as theinput control file, with '.bak' appended, and each file entryshould have an 'md5' value. The 'md5' field need not be theactual MD5 of the file contents, it is just a unique identifierthat will be compared to determine if the file contents has changedbetween builds.
		/// </summary>
		public bool? SteamPipe { get; internal set; }

		/// <summary>
		/// Use specified chunk size (in MB). Default is 200.
		/// </summary>
		public int? ChunkSize { get; internal set; }

		/// <summary>
		/// Align files within chunk on n-byte boundary. Default is 1.
		/// </summary>
		public byte? Align { get; set; }

		/// <summary>
		/// With commands 'a' or 'k': Sign VPK with specified private key.
		/// </summary>
		public string PrivateKey { get; set; }

		/// <summary>
		/// With commands 'a' or 'k': Public key that will be distributed and used by third parties to verify signatures.
		/// With command 'checksig': Check signature using specified key file.
		/// </summary>
		public string PublicKey { get; set; }

	}

	public static partial class Extensions
	{
		#region MultiChunk

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="multichunk"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetMultiChunk<T>(this T toolSettings, bool? multichunk) where T : VPK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MultiChunk = multichunk;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetMultiChunk<T>(this T toolSettings) where T : VPK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MultiChunk = null;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T EnableMultiChunk<T>(this T toolSettings) where T : VPK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MultiChunk = true;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T DisableMultiChunk<T>(this T toolSettings) where T : VPK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MultiChunk = false;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ToggleMultiChunk<T>(this T toolSettings) where T : VPK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MultiChunk = !toolSettings.MultiChunk;
			return toolSettings;
		}

		#endregion

		#region SteamPipe

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="steampipe"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetSteamPipe<T>(this T toolSettings, bool? steampipe) where T : VPK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.SteamPipe = steampipe;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetSteamPipe<T>(this T toolSettings) where T : VPK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.SteamPipe = null;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T EnableSteamPipe<T>(this T toolSettings) where T : VPK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.SteamPipe = true;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T DisableSteamPipe<T>(this T toolSettings) where T : VPK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.SteamPipe = false;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ToggleSteamPipe<T>(this T toolSettings) where T : VPK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.SteamPipe = !toolSettings.SteamPipe;
			return toolSettings;
		}

		#endregion

		#region ChunkSize

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="chunksize"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetChunkSize<T>(this T toolSettings, int chunksize) where T : VPK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.ChunkSize = chunksize;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetChunkSize<T>(this T toolSettings) where T : VPK
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.ChunkSize = null;
			return toolSettings;
		}

		#endregion

	}
}
