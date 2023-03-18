// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Nuke.Common.Tooling;

namespace Nuke.Common.Tools.Source.Tooling
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class BZIP : Tools
    {

	    public BZIP() : base("bzip2.exe")
	    {

	    }

	    public BZIP(string executable) : base(executable)
	    {
	    }

        /// <summary>
        /// Overwrite existing output files
        /// </summary>
        public virtual bool? Force { get; internal set; }

        public virtual bool Keep { get; internal set; } = true;

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

    }
}
