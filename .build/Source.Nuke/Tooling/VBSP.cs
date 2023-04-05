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
	/// https://developer.valvesoftware.com/wiki/VBSP
	/// </summary>
	[PublicAPI]
	[ExcludeFromCodeCoverage]
	[Serializable]
	public class VBSP : Tools, ISlammin
	{

		public VBSP() : base("vbsp.exe")
		{

		}

		/// <summary>
		///
		/// </summary>
		/// <param name="arguments"></param>
		/// <returns></returns>
		protected override Arguments ConfigureProcessArguments(Arguments arguments)
		{
			arguments
				// Common options
				.Add("-verbose", Verbose)
				.Add("-onlyents", OnlyEnts)
				.Add("-onlyprops", OnlyProps)
				.Add("-glview", GLView)
				.Add("-nodetails", NoDetail)
				.Add("-nowater", NoWater)
				.Add("-low", Low)
				.Add("-vproject {value}", VProject)
				.Add("-game {value}", Game)
				.Add("-insert_search_path {value}", InsertSearchPath)
				.Add("-embed {value}", Embed)
				.Add("-instancepath {value}", InstancePath) // Portal_Community_Edition
				// Advanced options
				.Add("-autoviscluster", AutoVisCluster) // CS_Global_Offensive
				.Add("-novconfig", NoVConfig)
				.Add("-threads", Threads)
				.Add("-noweld", NoWeld)
				.Add("-verboseentities", VerboseEntities)
				.Add("-nocsg", NoCsg)
				.Add("-noshare", NoShare)
				.Add("-notjunc", NotJunc)
				.Add("-allowdetailcracks", AllowDetailCracks)
				.Add("-nodrawtriggers", NoDrawTriggers)
				.Add("-noopt", NoOpt)
				.Add("-noprune", NoPrune)
				.Add("-nomerge", NoMerge)
				.Add("-nomergewater", NoMergeWater)
				.Add("-nosubdiv", NoSubDiv)
				.Add("-micro {value}", Micro)
				.Add("-fulldetail", FullDetail)
				.Add("-alldetail ", AllDetail) // Alien_Swarm, Dota_2, _Reactive_Drop
				.Add("-nosubdiv", NoSubDiv)
				.Add("-leaktest", LeakTest)
				.Add("-bumpall", BumpAll)
				.Add("-snapaxial", SnapAxial)
				//.Add("-block", Block)
				.Add("-blocks {value}", Blocks != null ? string.Join(" ", Blocks) : null)
				.Add("-blocksize {value}", BlockSize)
				.Add("-dumpstaticprops", DumpStaticProps)
				.Add("-dumpcollide", DumpCollide)
				.Add("-forceskyvis", ForceSkyVis)
				.Add("-luxelscale {value}", LuxelScale)
				.Add("-lightifmissing", LightIfMissing)
				//.Add("-localphysx", LocalPhysx)
				.Add("-keepstalezip", KeepStaleZip)
				.Add("-visgranularity {value}", VisGranularity != null ? string.Join(" ", VisGranularity) : null)
				.Add("-replacematerials", ReplaceMaterials)
				.Add("-FullMinidumps", FullMinidumps)
				.Add("-novirtualmesh", NoVirtualMesh)
				.Add("-allowdynamicpropsasstatic ", AllowDynamicPropsAsStatic) // Garrys_Mod
				// Static Prop Combine
				.Add("-StaticPropCombine", StaticPropCombine)
				.Add("-StaticPropCombine_AutoCombine", StaticPropCombineAutoCombine)
				.Add("-StaticPropCombine_ConsiderVis", StaticPropCombineConsiderVis)
				.Add("-StaticPropCombine_SuggestRules", StaticPropCombineSuggestRules)
				.Add("-StaticPropCombine_MinInstances {value}", StaticPropCombineMinInstances)
				.Add("-StaticPropCombine_PrintCombineRules", StaticPropCombinePrintCombineRules)
				.Add("-StaticPropCombine_ColorInstances", StaticPropCombineColorInstances)
				.Add("-KeepSources", KeepSources)
				.Add("-CombineIgnore_FastReflection", CombineIgnoreFastReflection)
				.Add("-CombineIgnore_Normals", CombineIgnoreNormals)
				.Add("-CombineIgnore_NoShadow", CombineIgnoreNoShadow)
				.Add("-CombineIgnore_NoVertexLighting", CombineIgnoreNoVertexLighting)
				.Add("-CombineIgnore_NoFlashlight", CombineIgnoreNoFlashlight)
				.Add("-CombineIgnore_NoSelfShadowing", CombineIgnoreNoSelfShadowing)
				.Add("-CombineIgnore_DisableShadowDepth", CombineIgnoreDisableShadowDepth)
				// Nonfunctional Options
				.Add("-linuxdata", LinuxData)
				.Add("-nolinuxdata", NoLinuxData)
				.Add("-virtualdispphysics", VirtualDispPhysics)
				.Add("-xbox", Xbox)
				//
				.Add("{value}", Input);
			return base.ConfigureProcessArguments(arguments);
		}

		#region Common options

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

		#endregion

		#region Advanced Options

		/// <summary>
		/// Used on dz_sirocco, found in scripts/mapautocompile.txt.
		/// </summary>
		public bool? AutoVisCluster { get; set; }

		/// <summary>
		/// Don't bring up graphical UI on vproject errors.
		/// </summary>
		public bool? NoVConfig { get; set; }

		/// <summary>
		/// If -v is on, this disables verbose output for submodels.
		/// </summary>
		public bool? VerboseEntities { get; set; }

		/// <summary>
		/// Don't join face vertexes together.
		/// </summary>
		public bool? NoWeld { get; set; }

		/// <summary>
		/// Don't chop out intersecting brush areas.
		/// </summary>
		public bool? NoCsg { get; set; }

		/// <summary>
		/// Emit unique face edges instead of sharing them.
		/// </summary>
		public bool? NoShare { get; set; }

		/// <summary>
		/// Don't fixup any t-junctions.
		/// </summary>
		public bool? NotJunc { get; set; }

		/// <summary>
		/// Don't fixup t-junctions on func_detail.
		/// </summary>
		public bool? AllowDetailCracks { get; set; }

		/// <summary>
		/// Compiles all triggers as if they were using Nodraw texture, only affects triggers using a texture with %CompileTrigger.
		/// </summary>
		public bool? NoDrawTriggers { get; set; }

		/// <summary>
		/// By default, vbsp removes the 'outer shell' of the map, which are all the faces you can't see because you can never get outside the map. -noopt disables this behavior.
		/// </summary>
		public bool? NoOpt { get; set; }

		/// <summary>
		/// Don't prune neighboring solid nodes.
		/// </summary>
		public bool? NoPrune { get; set; }

		/// <summary>
		/// Don't merge together chopped faces on nodes.
		/// </summary>
		public bool? NoMerge { get; set; }

		/// <summary>
		/// Don't merge together chopped faces on water.
		/// </summary>
		public bool? NoMergeWater { get; set; }

		/// <summary>
		/// Don't subdivide faces for lightmapping.
		/// </summary>
		public bool? NoSubDiv { get; set; }

		/// <summary>
		/// vbsp will warn when brushes are output with a volume less than this number (default: 1.0).
		/// </summary>
		public float? Micro { get; set; }

		/// <summary>
		/// Mark all detail geometry as normal geometry (so all detail geometry will affect visibility).
		/// </summary>
		public bool? FullDetail { get; set; }

		/// <summary>
		/// Convert all structural brushes to detail brushes, except func_brush entities whose names begin with structure_.
		/// </summary>
		public bool? AllDetail { get; set; }

		/// <summary>
		/// Stop processing the map if a leak is detected. Whether or not this flag is set, a leak file will be written out at vmf filename.lin, and it can be imported into  Hammer.
		/// </summary>
		public bool? LeakTest { get; set; }

		/// <summary>
		/// Force all surfaces to be bump mapped.
		/// </summary>
		public bool? BumpAll { get; set; }

		/// <summary>
		/// Snap axial planes to integer coordinates.
		/// </summary>
		public bool? SnapAxial { get; set; }

		/// <summary>
		/// Enter the mins and maxs for the grid size vbsp uses.
		/// </summary>
		public int?[] Blocks { get; set; }

		/// <summary>
		/// Control the size of each grid square that vbsp chops the level on. Default is 1024.
		/// </summary>
		public int? BlockSize { get; set; }

		/// <summary>
		/// Dump static props to staticprop*.txt.
		/// </summary>
		public bool? DumpStaticProps { get; set; }

		/// <summary>
		/// Write files with collision info.
		/// </summary>
		public bool? DumpCollide { get; set; }

		/// <summary>
		/// Enable vis calculations in 3D Skybox leaves.
		/// </summary>
		public bool? ForceSkyVis { get; set; }

		/// <summary>
		/// Scale all lightmaps by this amount (default: 1.0).
		/// </summary>
		public float? LuxelScale { get; set; }

		/// <summary>
		/// Force lightmaps to be generated for all surfaces even if they don't need lightmaps.
		/// </summary>
		public bool? LightIfMissing { get; set; }

		/// <summary>
		/// Keep the BSP's zip files intact but regenerate everything else.
		/// </summary>
		public bool? KeepStaleZip { get; set; }

		/// <summary>
		/// Force visibility splits # of units along X, Y, Z.
		/// </summary>
		public int?[] VisGranularity { get; set; }

		/// <summary>
		/// Substitute materials according to materialsub.txt in content\maps.
		/// </summary>
		public bool? ReplaceMaterials { get; set; }

		/// <summary>
		/// Write large minidumps on crash.
		/// </summary>
		public bool? FullMinidumps { get; set; }

		/// <summary>
		/// Fix Physics entities fall through displacements at cost of bigger bsp size.
		/// </summary>
		public bool? NoVirtualMesh { get; set; }

		/// <summary>
		/// Allow all models with the 'static' flag in the model viewer to be used on prop_static, even when their propdata doesn't contain 'allowstatic'.
		/// </summary>
		public bool? AllowDynamicPropsAsStatic { get; set; }

		#endregion

		#region Static Prop Combine

		/// <summary>
		/// Merges static props together according to the rules defined in scripts/hammer/spcombinerules/spcombinerules.txt. This lowers the number of draw calls, increasing performance. It can also be used to lower the number of static props present in a map. See Static Prop Combine.
		/// </summary>
		public bool? StaticPropCombine { get; set; }

		/// <summary>
		/// Automatically generate static prop combine rules for props that VBSP deems should be combined.
		/// Note: This does not write to spcombinerules.txt.
		/// </summary>
		public bool? StaticPropCombineAutoCombine { get; set; }

		/// <summary>
		/// Instead of using the distance limit, combine all props in the group that share visclusters.
		/// </summary>
		public bool? StaticPropCombineConsiderVis { get; set; }

		/// <summary>
		/// Lists models sharing the same material that should be added to spcombinerules.txt.
		/// </summary>
		public bool? StaticPropCombineSuggestRules { get; set; }

		/// <summary>
		/// Set the minimum number of props in a combine group required to create a combined prop.
		/// Tip: Valve had this set to 3 for the new Dust 2.
		/// </summary>
		public bool? StaticPropCombineMinInstances { get; set; }

		/// <summary>
		/// Prints the combine rules? Hasn't been working for me.
		/// </summary>
		public bool? StaticPropCombinePrintCombineRules { get; set; }

		/// <summary>
		/// Does this color the instances?
		/// </summary>
		public bool? StaticPropCombineColorInstances { get; set; }

		/// <summary>
		/// Don't delete the autogenerated QCs and unpacked model files after finishing.
		/// </summary>
		public bool? KeepSources { get; set; }

		/// <summary>
		/// Combine props, even if they have differing Render in Fast Reflections settings.
		/// </summary>
		public bool? CombineIgnoreFastReflection { get; set; }

		/// <summary>
		/// Combine props, even if they have differing Ignore Normals settings.
		/// </summary>
		public bool? CombineIgnoreNormals { get; set; }

		/// <summary>
		/// Combine props, even if they have differing Disable Shadows settings.
		/// </summary>
		public bool? CombineIgnoreNoShadow { get; set; }

		/// <summary>
		/// Combine props, even if they have differing Disable Vertex lighting settings.
		/// </summary>
		public bool? CombineIgnoreNoVertexLighting { get; set; }

		/// <summary>
		/// Combine props, even if they have differing Disable flashlight settings.
		/// </summary>
		public bool? CombineIgnoreNoFlashlight { get; set; }

		/// <summary>
		/// Combine props, even if they have differing Disable Self-Shadowing settings.
		/// </summary>
		public bool? CombineIgnoreNoSelfShadowing { get; set; }

		/// <summary>
		/// Combine props, even if they have differing Disable ShadowDepth settings.
		/// </summary>
		public bool? CombineIgnoreDisableShadowDepth { get; set; }

		#endregion

		#region NonFunctional Options

		/// <summary>
		/// Force it to write physics data for linux multiplayer servers. (It will automatically write this data if it finds certain entities like info_player_terrorist, info_player_deathmatch, info_player_teamspawn, info_player_axis, or info_player_coop.)
		/// </summary>
		public bool? LinuxData { get; set; }

		/// <summary>
		/// Force it to not write physics data for linux multiplayer servers, even if there are multiplayer entities in the map.
		/// </summary>
		public bool? NoLinuxData { get; set; }

		/// <summary>
		/// Use virtual (not precomputed) displacement collision models
		/// </summary>
		public bool? VirtualDispPhysics { get; set; }

		/// <summary>
		/// Enable mandatory Xbox 1 optimisation.
		/// </summary>
		public bool? Xbox { get; set; }

		#endregion

		public bool? UsingSlammin { get; set; }
	}

	public static partial class Extensions
	{

	}
}
