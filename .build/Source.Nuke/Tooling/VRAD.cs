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
    /// https://developer.valvesoftware.com/wiki/VRAD
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class VRAD : Tools, IFastable
    {

	    public VRAD() : base("vrad.exe")
	    {

	    }

	    public VRAD(string executable) : base(executable)
	    {
	    }

        /// <summary>
        /// Compiles quick low quality lighting. Used for quick previewing.
        /// Note: -fast will cause random and miscolored splotching to appear in darker areas. As well as shadowed edges around Displacements. It is advised to not ship your map using -fast.
        /// </summary>
        public virtual bool? Fast { get; set; }

        /// <summary>
        /// Set the maximum number of light ray bounces. (default: 100).
        /// </summary>
        public virtual ushort? Bounce { get; internal set; }

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
		        .Add("-bounce {value}", Bounce)
		        //
		        .Add("{value}", Input);
            return base.ConfigureProcessArguments(arguments);
        }

    }
}
