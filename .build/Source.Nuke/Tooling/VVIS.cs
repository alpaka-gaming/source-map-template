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
    public class VVIS : Tools, IFastable
    {

	    public VVIS() : base("vvis.exe")
	    {

	    }

	    public VVIS(string executable) : base(executable)
	    {
	    }

        /// <summary>
        /// Only do a quick first pass. Does not actually test visibility.
        /// </summary>
        public virtual bool? Fast { get; set; }

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

    }
}
