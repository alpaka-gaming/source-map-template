// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
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
	public class VRAD : Tools, ISlammin
	{

		public VRAD() : base("vrad.exe")
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
				// Effects
				.Add("-ldr", Ldr)
				.Add("-hdr", Hdr)
				.Add("-both", Both)
				.Add("-fast", Fast)
				.Add("-final", Final)
				.Add("-extrasky {value}", ExtraSky)
				.Add("-lights {value}", Lights)
				.Add("-bounce {value}", Bounce)
				.Add("-smooth {value}", Smooth)
				.Add("-luxeldensity {value}", LuxelDensity)
				.Add("-reflectivityScale {value}", ReflectivityScale)
				.Add("-softsun {value}", SoftSun)
				.Add("-StaticPropLighting", StaticPropLighting)
				.Add("-StaticPropPolys", StaticPropPolys)
				.Add("-TextureShadows", TextureShadows)
				.Add("-aoscale {value}", AoScale) // Insurgency, Day_of_Infamy
				.Add("-aoradius {value}", AoRadius) // Insurgency, Day_of_Infamy
				.Add("-aosamples {value}", AoSamples) // Insurgency, Day_of_Infamy
				.Add("-StaticPropBounce {value}", StaticPropBounce) // CS_Global_Offensive
				.Add("-StaticPropLightingOld", StaticPropLightingOld) // CS_Global_Offensive
				.Add("-choptexlights", ChopTexLights) // Black_Mesa_(Source)
				.Add("-extratransfers", ExtraTransfers) // Black_Mesa_(Source)
				.Add("-transferscale {value}", TransferScale) // Black_Mesa_(Source)
				.Add("-satthresh {value}", SatThresh) // Black_Mesa_(Source)
				.Add("-satthreshscale {value}", SatThreshScale) // Black_Mesa_(Source)
				.Add("-ambientocclusion", AmbientOcclusion) // Black_Mesa_(Source)
				.Add("-experimentalambientocclusion", ExperimentalAmbientOcclusion)// Black_Mesa_(Source)
				.Add("-cascadeshadows", CascadeShadows) // Black_Mesa_(Source)
				.Add("-realskylight", RealSkyLight) // Black_Mesa_(Source)
				.Add("-realskylightscale {value}", RealSkyLightScale) // Black_Mesa_(Source)
				.Add("-directsunlightisforadditivemode", DirectSunLightIsForAdditiveMode) // Black_Mesa_(Source)
				.Add("-ambient {value}", Ambient != null ? $"[{string.Join(" ", Ambient)}]" : null) // CS_Global_Offensive, Dark_Messiah_of_Might_and_Magic
				.Add("-PortalTraversalLighting", PortalTraversalLighting) // Portal_Community_Edition
				.Add("-PortalTraversalAO", PortalTraversalAO) // Portal_Community_Edition
				// Performance
				.Add("-low", Low)
				.Add("-threads", Threads)
				.Add("-mpi", Mpi)
				.Add("-mpi_pw {value}", MpiPw)
				.Add("-noextra", NoExtra)
				.Add("-chop {value}", Chop)
				.Add("-maxchop {value}", MaxChop)
				.Add("-dispchop {value}", DispChop)
				.Add("-LargeDispSampleRadius", LargeDispSampleRadius)
				.Add("-compressconstant {value}", CompressConstant)
				.Add("-fastambient", FastAmbient)
				.Add("-LeafAmbientSampleReduction {value}", LeafAmbientSampleReduction) // CS_Global_Offensive
				.Add("-noao", NoAo) // Insurgency, Day_of_Infamy
				.Add("-StaticPropSampleScale {value}", StaticPropSampleScale) // CS_Global_Offensive
				.Add("-disppatchradius {value}", DisppatchRadius)
				.Add("-ambientfromleafcenters", AmbientFromLeafCenters) // CS_Global_Offensive
				// Debugging
				.Add("-rederrors", RedErrors)
				.Add("-vproject {value}", VProject)
				.Add("-game {value}", Game)
				.Add("-insert_search_path {value}", InsertSearchPath)
				.Add("-verbose", Verbose)
				.Add("-novconfig", NoVConfig)
				.Add("-dump", Dump)
				.Add("-dumpnormals", DumpNormals)
				.Add("-debugextra", DebugExtra)
				.Add("-dlightmap", DLightMap)
				.Add("-stoponexit", StopOnExit)
				.Add("-nodetaillight", NoDetailLight)
				.Add("-centersamples", CenterSamples)
				.Add("-loghash", LogHash)
				.Add("-onlydetail", OnlyDetail)
				.Add("-maxdispsamplesize {value}", MaxDispSampleSize)
				.Add("-FullMinidump", FullMinidump)
				.Add("-OnlyStaticProps", OnlyStaticProps)
				.Add("-StaticPropNormals", StaticPropNormals)
				.Add("-noskyboxrecurse", NoSkyboxRecurse)
				.Add("-nossprops", NoSSProps)
				.Add("-dumppropmaps", DumpPropMaps) // Source_2013_Multiplayer
				.Add("-unlitdetail", UnlitDetail) // CS_Global_Offensive
				//
				.Add("{value}", Input);
			return base.ConfigureProcessArguments(arguments);
		}

		#region Effects

		public bool? Ldr { get; set; }

		public bool? Hdr { get; set; }

		/// <summary>
		/// Whether to compile Standard Dynamic Range lighting, High Dynamic Range lighting, or both respectively.
		///		Note: Since , SDR support was dropped. Thus making -ldr and -both obsolete in engine branches made on or after .
		///			  Instead, -hdr is required in order to produce proper lighting for your map.
		///		Note: defaults to compiling HDR automatically.
		/// </summary>
		public bool? Both { get; set; }

		/// <summary>
		/// Compiles quick low quality lighting. Used for quick previewing.
		/// Note: -fast will cause random and miscolored splotching to appear in darker areas. As well as shadowed edges around Displacements. It is advised to not ship your map using -fast.
		/// </summary>
		public override bool? Fast { get; internal set; }

		/// <summary>
		/// Increases the quality of light_environment and indirect lighting by spending more time firing rays. Sets-StaticPropSampleScale to 16.
		/// </summary>
		public bool? Final { get; set; }

		/// <summary>
		/// Trace N times as many rays for indirect light and sky ambient.
		/// (-final is equivalent to -extrasky 16; normal is equivalent to -extrasky 1)
		/// </summary>
		public int? ExtraSky { get; set; }

		/// <summary>
		/// Load a custom lights file in addition to lights.rad and the map-specific lights file. Include the file extension in the parameter.
		/// </summary>
		public string Lights { get; set; }

		/// <summary>
		/// Set the maximum number of light ray bounces. (default: 100).
		/// </summary>
		public virtual ushort? Bounce { get; internal set; }

		/// <summary>
		/// Set the threshold for smoothing groups, in degrees (default: 45).
		/// </summary>
		public int? Smooth { get; set; }

		/// <summary>
		/// Scale down all luxels. Default (and maximum) value is 1.
		/// </summary>
		public float? LuxelDensity { get; set; }

		/// <summary>
		/// Scale the $reflectivity of all textures. Default 1.0 (Does not work in CS)
		/// </summary>
		public float? ReflectivityScale { get; set; }

		/// <summary>
		/// Treat the sun as an area light source of this many degrees. Produces soft shadows. Recommended values are 0-5, default is 0. Identical to the SunSpreadAngle parameter for light_environment, use that instead.
		/// </summary>
		public float? SoftSun { get; set; }

		/// <summary>
		/// Generate per-vertex lighting on prop_statics; always enabled for light_spot entities. Disables info_lighting entities on props without bump maps. Does not work on props with bump maps, except in CS.
		/// Warning: This can increase your map's filesize substantially. Disable vertex lighting for props that don't need it to keep filesize low.
		/// Note: In , in order to get proper lighting on your static props, you will need to run VRAD with this command.
		/// </summary>
		public bool? StaticPropLighting { get; set; }

		/// <summary>
		/// Use the actual meshes of static props to generate shadows instead of using their collision meshes. This results in far more accurate shadowing.
		/// </summary>
		public bool? StaticPropPolys { get; set; }

		/// <summary>
		/// Generates lightmap shadows from $translucent surfaces of models (NOT brushes) that are specified in a lights file and being used with prop_static. Usually requires -StaticPropPolys to have any effect.
		/// Note: A surface will need a low lightmap scale for most texture shadows to be recognisable.
		/// Note: This will not work if a translucent texture's $basetexture parameter in the VMT contains the .vtf file extension.
		/// </summary>
		public bool? TextureShadows { get; set; }

		/// <summary>
		/// Scales the intensity of VRAD's simulated ambient occlusion. 1.0 is default.
		/// Tip: Valve uses 1.5 for the new Dust 2.
		/// </summary>
		public float? AoScale { get; set; }

		/// <summary>
		/// Set the radius of VRAD's simulated ambient occlusion. To do: Figure out what exactly this does.
		/// </summary>
		public float? AoRadius { get; set; }

		/// <summary>
		/// How many samples to use for VRAD's simulated ambient occlusion.
		/// </summary>
		public int? AoSamples { get; set; }

		/// <summary>
		/// Number of static prop light bounces to simulate. The default is 0.
		/// Tip: Valve uses 3 static prop bounces for the new Dust 2.
		/// Note: 	Any static props that you want light to bounce off of must also have their "Enable Bounced Lighting" keyvalue set.
		/// </summary>
		public int? StaticPropBounce { get; set; }

		/// <summary>
		/// Will use the old lighting algorithm on props, light affects them much more.
		/// </summary>
		public bool? StaticPropLightingOld { get; set; }

		/// <summary>
		/// Enables chopping of texture lights generated from a lights file. Control texture light quality with lightmap density in Hammer. Dramatically increases both texture light quality and compile time.
		/// </summary>
		public bool? ChopTexLights { get; set; }

		/// <summary>
		/// Enable overscaling of light transfers.
		/// </summary>
		public bool? ExtraTransfers { get; set; }

		/// <summary>
		/// This is the scale factor of light transfers. Increased values make surfaces transfer extra light (scale of 2-4 suggested). Default 1.0.
		/// </summary>
		public float? TransferScale { get; set; }

		/// <summary>
		/// This is the threshold that checks how saturated a material color is. Used with -satthreshscale. Default 0.4.
		/// </summary>
		public float? SatThresh { get; set; }

		/// <summary>
		/// The amount to scale light transfers from surfaces that pass the saturation threshold. Default 3.0.
		/// </summary>
		public float? SatThreshScale { get; set; }

		/// <summary>
		/// Enable lightmapped ambient occlusion
		/// </summary>
		public bool? AmbientOcclusion { get; set; }

		/// <summary>
		/// Use an improved algorithm for the ambient occlusion
		/// </summary>
		public bool? ExperimentalAmbientOcclusion { get; set; }

		/// <summary>
		/// indicates that lightmap alpha data is interleved in the lighting lump, required for CSM.
		/// </summary>
		public bool? CascadeShadows { get; set; }

		/// <summary>
		/// Enables VRAD to compute skylight ambient color by using actual values from skybox.
		/// </summary>
		public bool? RealSkyLight { get; set; }

		/// <summary>
		/// Scale factor of -realskylight intensity. Default: 1.0
		/// </summary>
		public float? RealSkyLightScale { get; set; }

		/// <summary>
		/// Toggles direct sunlight for additive mode.
		/// </summary>
		public bool? DirectSunLightIsForAdditiveMode { get; set; }

		/// <summary>
		/// Sets the ambient term. Can be used to tweak lightmap color. Appears to just mix the color into all lightmaps.
		/// </summary>
		public int[] Ambient { get; set; }

		/// <summary>
		/// Enables static lights to go through static worldportals.
		/// </summary>
		public bool? PortalTraversalLighting { get; set; }

		/// <summary>
		/// Enables static light AO to go through static worldportals.
		/// </summary>
		public bool? PortalTraversalAO { get; set; }

		#endregion

		#region Performance

		/// <summary>
		/// Use VMPI to distribute computations.
		/// </summary>
		public bool? Mpi { get; set; }

		/// <summary>
		/// Use a password to choose a specific set of VMPI workers.
		/// </summary>
		public string MpiPw { get; set; }

		/// <summary>
		/// Disable supersampling. This will lead to blockier, more inaccurate lighting.
		/// </summary>
		public bool? NoExtra { get; set; }

		/// <summary>
		/// Smallest number of luxel widths for a bounce patch, used on edges. (Default: 4)
		/// </summary>
		public int? Chop { get; set; }

		/// <summary>
		/// Coarsest allowed number of luxel widths for a patch, used in face interiors. (Default: 4)
		/// </summary>
		public int? MaxChop { get; set; }

		/// <summary>
		/// Smallest acceptable luxel width of displacement patch face. (Default: 8)
		/// </summary>
		public int? DispChop { get; set; }

		/// <summary>
		/// This can be used if there are splotches of bounced light on terrain. The compile will take longer, but it will gather light across a wider area.
		/// </summary>
		public bool? LargeDispSampleRadius { get; set; }

		/// <summary>
		/// Compress lightmaps whose color variation is less than this many units. To do: Find out if this is branch specific, as it doesn't work with TF2.
		/// </summary>
		public int? CompressConstant { get; set; }

		/// <summary>
		/// Uses low quality per-leaf ambient sampling to save compute time.
		/// </summary>
		public bool? FastAmbient { get; set; }

		/// <summary>
		/// Reduction factor for ambient samples. Defaults to 1.0.
		/// </summary>
		public float? LeafAmbientSampleReduction { get; set; }

		/// <summary>
		/// Disables compiling simulated ambient occlusion for lightmaps.
		/// </summary>
		public bool? NoAo { get; set; }

		/// <summary>
		/// Regulates the generated per-vertex prop_static lighting.
		/// slow: 16 (high quality); default: 4 (normal); fast: 0.25 (low quality)
		/// Note: -final is the equivalent of having -StaticPropSampleScale 16.
		/// </summary>
		public float? StaticPropSampleScale { get; set; }

		/// <summary>
		/// Sets the maximum radius allowed for displacement patches.
		/// </summary>
		public float? DisppatchRadius { get; set; }

		/// <summary>
		/// Samples ambient lighting from the center of the leaf.
		/// </summary>
		public bool? AmbientFromLeafCenters { get; set; }

		#endregion

		#region Debugging

		/// <summary>
		/// Emit red light when "a luxel has no samples".
		/// </summary>
		public bool? RedErrors { get; set; }

		/// <summary>
		/// Includes an extra base directory for mounting additional content (like Gameinfo.txt entries). Useful if you want to separate some assets from the mod for whatever reason.
		/// </summary>
		public string InsertSearchPath { get; set; }

		/// <summary>
		/// Don't bring up graphical UI on vproject errors.
		/// </summary>
		public bool? NoVConfig { get; set; }

		/// <summary>
		/// Dump patches to debug files.
		/// </summary>
		public bool? Dump { get; set; }

		/// <summary>
		/// Write normals to debug .txt files.
		/// </summary>
		public bool? DumpNormals { get; set; }

		/// <summary>
		/// Places debugging data in lightmaps to visualize supersampling.
		/// </summary>
		public bool? DebugExtra { get; set; }

		/// <summary>
		/// Force direct lighting into different lightmap than radiosity.
		/// </summary>
		public bool? DLightMap { get; set; }

		/// <summary>
		/// Wait for a keypress on exit.
		/// </summary>
		public bool? StopOnExit { get; set; }

		/// <summary>
		/// Don't light detail props.
		/// </summary>
		public bool? NoDetailLight { get; set; }

		/// <summary>
		/// Move sample centers.
		/// </summary>
		public bool? CenterSamples { get; set; }

		/// <summary>
		/// Log the sample hash table to samplehash.txt.
		/// </summary>
		public bool? LogHash { get; set; }

		/// <summary>
		/// Only light detail props and per-leaf lighting.
		/// </summary>
		public bool? OnlyDetail { get; set; }

		/// <summary>
		/// Set max displacement sample size (default: 512).
		/// </summary>
		public int? MaxDispSampleSize { get; set; }

		/// <summary>
		/// Write large minidumps on crash.
		/// </summary>
		public bool? FullMinidump { get; set; }

		/// <summary>
		/// Only perform direct static prop lighting.
		/// </summary>
		public bool? OnlyStaticProps { get; set; }

		/// <summary>
		/// When lighting static props, just show their normal vector.
		/// </summary>
		public bool? StaticPropNormals { get; set; }

		/// <summary>
		/// Turn off recursion into 3d skybox (skybox shadows on world).
		/// </summary>
		public bool? NoSkyboxRecurse { get; set; }

		/// <summary>
		/// Globally disable self-shadowing on static props.
		/// </summary>
		public bool? NoSSProps { get; set; }

		/// <summary>
		/// Dump computed prop lightmaps.
		/// </summary>
		public bool? DumpPropMaps { get; set; }

		/// <summary>
		/// Disables lighting for detail props.
		/// </summary>
		public bool? UnlitDetail { get; set; }

		#endregion

		public bool? UsingSlammin { get; set; }
	}

	public static partial class Extensions
	{
		#region Bounce

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="bounce"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetBounce<T>(this T toolSettings, ushort bounce) where T : VRAD
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Bounce = bounce;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetBounce<T>(this T toolSettings) where T : VRAD
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Bounce = null;
			return toolSettings;
		}

		#endregion
	}
}
