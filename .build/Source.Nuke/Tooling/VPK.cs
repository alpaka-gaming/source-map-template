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
				.Add("--verbose", Verbose)
				.Add("{value}", Input);
			return base.ConfigureProcessArguments(arguments);
		}

	}

	public static partial class Extensions
	{

	}
}
