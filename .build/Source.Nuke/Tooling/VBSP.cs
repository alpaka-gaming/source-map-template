// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Nuke.Common.Tooling;

namespace Nuke.Common.Tools.Source.Tooling
{
    /// <summary>
    /// https://developer.valvesoftware.com/wiki/VBSP
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class VBSP : Tools
    {

	    public VBSP() : base("vbsp.exe")
	    {

	    }

	    public VBSP(string executable) : base(executable)
	    {
	    }


        /// <summary>
        /// Only import the entities from the VMF. Brushes and internal entities are not modified. Conserves existing lighting data.
        /// </summary>
        public virtual bool? OnlyEnts { get; internal set; }

        /// <summary>
        /// Only update static and detail props (i.e. the internal entities). Does not generate a .prt file, making VVIS fail!
        /// </summary>
        public virtual bool? OnlyProps { get; internal set; }

        /// <summary>
        /// Writes glview data to the VMF's directory. -tmpout will cause the files will be written to \tmp instead.
        /// </summary>
        public virtual bool? GLView { get; internal set; }

        /// <summary>
        /// Removes func_detail brushes. The geometry left over is what affects visibility.
        /// </summary>
        public virtual bool? NoDetail { get; internal set; }

        /// <summary>
        /// Get rid of water brushes.
        /// </summary>
        public virtual bool? NoWater { get; internal set; }

        /// <summary>
        /// Includes an extra base directory for mounting additional content (like Gameinfo.txt entries). Useful if you want to separate some assets from the mod for whatever reason.
        /// </summary>
        public virtual string InsertSearchPath { get; internal set; }

        /// <summary>
        /// Embed the contents of directory in the packfile.
        /// </summary>
        public virtual string Embed { get; internal set; }

        /// <summary>
        /// Overrides InstancePath key in gameinfo.
        /// (only in Portal 2: Community Edition)
        /// </summary>
        public virtual string InstancePath { get; internal set; }

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
		        .Add("-onlyents", OnlyEnts)
		        .Add("-onlyprops", OnlyProps)
		        .Add("-glview", GLView)
		        .Add("-nowater", NoWater)
		        .Add("-insert_search_path {value}", InsertSearchPath)
		        .Add("-embed {value}", Embed)
		        .Add("-instancepath {value}", InstancePath)
		        //
		        .Add("{value}", Input);
            return base.ConfigureProcessArguments(arguments);
        }

    }
}
