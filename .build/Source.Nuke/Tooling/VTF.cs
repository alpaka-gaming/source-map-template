using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using JetBrains.Annotations;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Source.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Nuke.Common.Tools.Source.Tooling
{
	/// <summary>
	///
	/// </summary>
	[PublicAPI]
	[ExcludeFromCodeCoverage]
	[Serializable]
	public class VTF : Tools, IDownloadable
	{

		public enum Formats
		{
			RGBA8888,
			ABGR8888,
			RGB888,
			BGR888,
			RGB565,
			I8,
			IA88,
			A8,
			RGB888_BLUESCREEN,
			BGR888_BLUESCREEN,
			ARGB8888,
			BGRA8888,
			DXT1,
			DXT3,
			DXT5,
			BGRX8888,
			BGR565,
			BGRX5551,
			BGRA4444,
			DXT1_ONEBITALPHA,
			BGRA5551,
			UV88,
			UVWQ8888,
			RGBA16161616F,
			RGBA16161616,
			UVLX8888,
		}

		public enum Flags
		{
			POINTSAMPLE,
			TRILINEAR,
			CLAMPS,
			CLAMPT,
			ANISOTROPIC,
			HINT_DXT5,
			NORMAL,
			NOMIP,
			NOLOD,
			MINMIP,
			PROCEDURAL,
			RENDERTARGET,
			DEPTHRENDERTARGET,
			NODEBUGOVERRIDE,
			SINGLECOPY,
			NODEPTHBUFFER,
			CLAMPU,
			VERTEXTEXTURE,
			SSBUMP,
			BORDER,
		}

		public enum ResizeMethod
		{
			NEAREST,
			BIGGEST,
			SMALLEST,
		}

		public enum ResizeFilter
		{
			POINT,
			BOX,
			TRIANGLE,
			QUADRATIC,
			CUBIC,
			CATROM,
			MITCHELL,
			GAUSSIAN,
			SINC,
			BESSEL,
			HANNING,
			HAMMING,
			BLACKMAN,
			KAISER,
		}

		public enum SharpenFilter
		{
			NONE,
			NEGATIVE,
			LIGHTER,
			DARKER,
			CONTRASTMORE,
			CONTRASTLESS,
			SMOOTHEN,
			SHARPENSOFT,
			SHARPENMEDIUM,
			SHARPENSTRONG,
			FINDEDGES,
			CONTOUR,
			EDGEDETECT,
			EDGEDETECTSOFT,
			EMBOSS,
			MEANREMOVAL,
			UNSHARP,
			XSHARPEN,
			WARPSHARP,
		}

		public enum NormalKernel
		{
			_4X,
			_3X3,
			_5X5,
			_7X7,
			_9X9,
			_DUDV,
		}

		public enum NormalHeight
		{
			ALPHA,
			AVERAGERGB,
			BIASEDRGB,
			RED,
			GREEN,
			BLUE,
			MAXRGB,
			COLORSPACE,
		}

		public enum NormalAlpha
		{
			NOCHANGE,
			HEIGHT,
			BLACK,
			WHITE,
		}

		public VTF() : base("VTFCmd.exe")
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
				.Add("-file {value}", Input)
				.Add("-folder {value}", Folder)
				.Add("-output {value}", Output)
				.Add("-prefix {value}", Prefix)
				.Add("-postfix {value}", Postfix)
				.Add("-version {value}", Version)
				.Add("-format {value}", Format?.ToString().ToLower())
				.Add("-alphaformat {value}", AlphaFormat)
				.Add("-flag {value}", Flag?.ToString().ToLower())
				.Add("-resize", Resize)
				.Add("-rmethod {value}", RMethod?.ToString().ToLower())
				.Add("-rfilter {value}", RFilter?.ToString().ToLower())
				.Add("-rsharpen {value}", RSharpen?.ToString().ToLower())
				.Add("-rwidth {value}", RWidth)
				.Add("-rheight {value}", RHeight)
				.Add("-rclampwidth {value}", RClampWidth)
				.Add("-rclampheight {value}", RClampHeight)
				.Add("-gamma", Gamma)
				.Add("-gcorrection {value}", GCorrection)
				.Add("-nomipmaps", NoMipmaps)
				.Add("-mfilter {value}", MFilter)
				.Add("-msharpen {value}", MSharpen)
				.Add("-normal", Normal)
				.Add("-nkernel {value}", NKernel?.ToString().ToLower().Substring(1))
				.Add("-nheight {value}", NHeight?.ToString().ToLower())
				.Add("-nalpha {value}", NAlpha?.ToString().ToLower())
				.Add("-nscale {value}", NScale)
				.Add("-nwrap", NWrap)
				.Add("-bumpscale {value}", BumpScale)
				.Add("-nothumbnail", NoThumbnail)
				.Add("-noreflectivity", NoReflectivity)
				.Add("-shader {value}", Shader)
				.Add("-recurse", Recurse)
				.Add("-exportformat {value}", ExportFormat)
				.Add("-silent", Silent)
				.Add("-pause", Pause);

			if (Params != null)
			{
				foreach (var param in Params)
					arguments.Add("-param {value}", $"{param.Key} {param.Value}");
			}

			return base.ConfigureProcessArguments(arguments);
		}

		/// <summary>
		/// Pause when done
		/// </summary>
		public bool? Pause
		{
			get;
			set;
		}

		/// <summary>
		/// Silent mode
		/// </summary>
		public bool? Silent => !Verbose;

		/// <summary>
		/// Convert VTF files to the format of this extension
		/// </summary>
		public string ExportFormat
		{
			get;
			set;
		}

		/// <summary>
		/// Process directories recursively
		/// </summary>
		public bool? Recurse
		{
			get;
			set;
		}

		/// <summary>
		/// Add a parameter to the material
		/// </summary>
		public Dictionary<string, string> Params
		{
			get;
			set;
		}

		/// <summary>
		/// Create a material for the texture
		/// </summary>
		public string Shader
		{
			get;
			set;
		}

		/// <summary>
		/// Don't calculate reflectivity
		/// </summary>
		public bool? NoReflectivity
		{
			get;
			set;
		}

		/// <summary>
		/// Don't generate thumbnail image
		/// </summary>
		public bool? NoThumbnail
		{
			get;
			set;
		}

		/// <summary>
		/// Engine bump mapping scale to use
		/// </summary>
		public float? BumpScale
		{
			get;
			set;
		}

		/// <summary>
		/// Wrap the normal map for tiled textures
		/// </summary>
		public bool? NWrap
		{
			get;
			set;
		}

		/// <summary>
		/// Normal map scale to use
		/// </summary>
		public float? NScale
		{
			get;
			set;
		}

		/// <summary>
		/// Normal map alpha result to set
		/// </summary>
		public NormalAlpha? NAlpha
		{
			get;
			set;
		}

		/// <summary>
		/// Normal map height calculation to use
		/// </summary>
		public NormalHeight? NHeight
		{
			get;
			set;
		}

		/// <summary>
		/// Normal map generation kernel to use
		/// </summary>
		public NormalKernel? NKernel
		{
			get;
			set;
		}

		/// <summary>
		/// Convert input file to normal map
		/// </summary>
		public bool? Normal
		{
			get;
			set;
		}

		/// <summary>
		/// Mipmap sharpen filter to use
		/// </summary>
		public string MSharpen
		{
			get;
			set;
		}

		/// <summary>
		/// Mipmap filter to use
		/// </summary>
		public string MFilter
		{
			get;
			set;
		}

		/// <summary>
		/// Don't generate mipmaps
		/// </summary>
		public bool? NoMipmaps
		{
			get;
			set;
		}

		/// <summary>
		/// Gamma correction to use
		/// </summary>
		public float? GCorrection
		{
			get;
			set;
		}

		/// <summary>
		/// Gamma correct image
		/// </summary>
		public bool? Gamma
		{
			get;
			set;
		}

		/// <summary>
		/// Maximum height to resize to
		/// </summary>
		public int? RClampHeight
		{
			get;
			set;
		}

		/// <summary>
		/// Maximum width to resize to
		/// </summary>
		public int? RClampWidth
		{
			get;
			set;
		}

		/// <summary>
		/// Resize to specific height
		/// </summary>
		public int? RHeight
		{
			get;
			set;
		}

		/// <summary>
		/// Resize to specific width
		/// </summary>
		public int? RWidth
		{
			get;
			set;
		}

		/// <summary>
		/// Resize sharpen filter to use
		/// </summary>
		public SharpenFilter? RSharpen
		{
			get;
			set;
		}

		/// <summary>
		/// Resize filter to use
		/// </summary>
		public ResizeFilter? RFilter
		{
			get;
			set;
		}

		/// <summary>
		/// Resize method to use
		/// </summary>
		public ResizeMethod? RMethod
		{
			get;
			set;
		}

		/// <summary>
		/// Resize the input to a power of 2
		/// </summary>
		public bool? Resize
		{
			get;
			set;
		}

		/// <summary>
		/// Output flags to set
		/// </summary>
		public Flags? Flag
		{
			get;
			set;
		}

		/// <summary>
		/// Ouput format to use on alpha textures
		/// </summary>
		public string AlphaFormat
		{
			get;
			set;
		}

		/// <summary>
		/// Ouput format to use on non-alpha textures
		/// </summary>
		public Formats? Format
		{
			get;
			set;
		}

		/// <summary>
		/// Ouput version
		/// </summary>
		public string Version
		{
			get;
			set;
		}

		/// <summary>
		/// Output file postfix
		/// </summary>
		public string Postfix
		{
			get;
			set;
		}

		/// <summary>
		/// Output file prefix
		/// </summary>
		public string Prefix
		{
			get;
			set;
		}

		/// <summary>
		/// Input directory search string
		/// </summary>
		public string Folder
		{
			get;
			set;
		}

		public virtual string InstallDir
		{
			get;
			set;
		}

		public override string ProcessToolPath => Path.Combine(InstallDir, GetType().Name.ToLower() + "lib", Executable);

		public string Url => "https://nemstools.github.io/files/vtflib132-bin.zip";

		public bool Download()
		{
			var localFile = string.Empty;
			var localDir = string.Empty;
			var fileName = Path.GetFileName(Url);
			if (fileName != null)
			{
				var toolPath = Path.Combine(Path.GetDirectoryName(ProcessToolPath), "..");
				if (string.IsNullOrWhiteSpace(toolPath)) return false;
				if (!string.IsNullOrWhiteSpace(toolPath))
				{
					localFile = Path.Combine(toolPath, fileName);
					localDir = Path.Combine(toolPath, GetType().Name.ToLower() + "lib");
				}
				if (string.IsNullOrWhiteSpace(localFile)) return false;
				if (!File.Exists(localFile))
				{
					using (var client = new HttpClient())
					{
						var response = client.Send(new HttpRequestMessage(HttpMethod.Get, Url));
						using var resultStream = response.Content.ReadAsStream();
						using var fileStream = File.OpenWrite(localFile);
						resultStream.CopyTo(fileStream);
					}
				}
				if (File.Exists(localFile) && !File.Exists(Path.Combine(localDir, Executable)))
				{
					ZipFile.ExtractToDirectory(localFile, localDir, true);
					var arch = System.Environment.Is64BitOperatingSystem ? "x64" : "x86";
					foreach (var file in Directory.GetFiles(Path.Combine(localDir, "bin", arch)))
						File.Move(file, Path.Combine(localDir, Path.GetFileName(file)));
					if (Directory.Exists(Path.Combine(localDir, "bin"))) Directory.Delete(Path.Combine(localDir, "bin"), true);
					if (Directory.Exists(Path.Combine(localDir, "lib"))) Directory.Delete(Path.Combine(localDir, "lib"), true);
				}
			}

			return !string.IsNullOrWhiteSpace(localFile) && File.Exists(localFile) &&
			       !string.IsNullOrWhiteSpace(localDir) && Directory.Exists(localDir) &&
			       File.Exists(Path.Combine(localDir, Executable));
		}
	}

	public static partial class VTFExtensions
	{
		#region Prefix

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="prefix"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetPrefix<T>(this T toolSettings, string prefix) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Prefix = prefix;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetPrefix<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Prefix = null;
			return toolSettings;
		}

		#endregion

		#region Postfix

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="prefix"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetPostfix<T>(this T toolSettings, string postfix) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Postfix = postfix;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetPostfix<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Postfix = null;
			return toolSettings;
		}

		#endregion

		#region Folder

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="folder"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetFolder<T>(this T toolSettings, string folder) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Folder = folder;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetFolder<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Folder = null;
			return toolSettings;
		}

		#endregion

		#region Flag

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="flag"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetFlag<T>(this T toolSettings, VTF.Flags flag) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Flag = flag;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetFlag<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Flag = null;
			return toolSettings;
		}

		#endregion

		#region Format

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="format"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetFormat<T>(this T toolSettings, VTF.Formats format) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Format = format;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetFormat<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Format = null;
			return toolSettings;
		}

		#endregion

		#region Shader

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="shader"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetShader<T>(this T toolSettings, string shader) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Shader = shader;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetShader<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Shader = null;
			return toolSettings;
		}

		#endregion

		#region Version

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="version"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetVersion<T>(this T toolSettings, string version) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Version = version;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetVersion<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Version = null;
			return toolSettings;
		}

		#endregion

		#region AlphaFormat

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="alphaFormat"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetAlphaFormat<T>(this T toolSettings, string alphaFormat) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.AlphaFormat = alphaFormat;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetAlphaFormat<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.AlphaFormat = null;
			return toolSettings;
		}

		#endregion

		#region BumpScale

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="bumpScale"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetBumpScale<T>(this T toolSettings, float? bumpScale) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.BumpScale = bumpScale;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetBumpScale<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.BumpScale = null;
			return toolSettings;
		}

		#endregion

		#region ExportFormat

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="exportFormat"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetExportFormat<T>(this T toolSettings, string exportFormat) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.ExportFormat = exportFormat;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetExportFormat<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.ExportFormat = null;
			return toolSettings;
		}

		#endregion

		#region GCorrection

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="gCorrection"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetGCorrection<T>(this T toolSettings, float? gCorrection) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.GCorrection = gCorrection;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetGCorrection<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.GCorrection = null;
			return toolSettings;
		}

		#endregion

		#region MSharpen

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="mSharpen"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetMSharpen<T>(this T toolSettings, string mSharpen) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MSharpen = mSharpen;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetMSharpen<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MSharpen = null;
			return toolSettings;
		}

		#endregion

		#region NAlpha

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="nAlpha"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetNAlpha<T>(this T toolSettings, VTF.NormalAlpha nAlpha) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NAlpha = nAlpha;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetNAlpha<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NAlpha = null;
			return toolSettings;
		}

		#endregion

		#region MFilter

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="mFilter"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetMFilter<T>(this T toolSettings, string mFilter) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MFilter = mFilter;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetMFilter<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.MFilter = null;
			return toolSettings;
		}

		#endregion

		#region RMethod

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="rMethod"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetRMethod<T>(this T toolSettings, VTF.ResizeMethod rMethod) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RMethod = rMethod;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetRMethod<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RMethod = null;
			return toolSettings;
		}

		#endregion

		#region RFilter

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="rFilter"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetRFilter<T>(this T toolSettings, VTF.ResizeFilter rFilter) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RFilter = rFilter;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetRFilter<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RFilter = null;
			return toolSettings;
		}

		#endregion

		#region RSharpen

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="rSharpen"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetRSharpen<T>(this T toolSettings, VTF.SharpenFilter rSharpen) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RSharpen = rSharpen;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetRSharpen<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RSharpen = null;
			return toolSettings;
		}

		#endregion

		#region RWidth

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="rWidth"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetRWidth<T>(this T toolSettings, int? rWidth) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RWidth = rWidth;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetRWidth<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RWidth = null;
			return toolSettings;
		}

		#endregion

		#region RHeight

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="rHeight"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetRHeight<T>(this T toolSettings, int? rHeight) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RHeight = rHeight;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetRHeight<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RHeight = null;
			return toolSettings;
		}

		#endregion

		#region RClampWidth

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="rClampWidth"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetRClampWidth<T>(this T toolSettings, int? rClampWidth) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RClampWidth = rClampWidth;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetRClampWidth<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RClampWidth = null;
			return toolSettings;
		}

		#endregion

		#region RClampHeight

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="rClampHeight"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetRClampHeight<T>(this T toolSettings, int? rClampHeight) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RClampHeight = rClampHeight;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetRClampHeight<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.RClampHeight = null;
			return toolSettings;
		}

		#endregion

		#region NKernel

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="nKernel"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetNKernel<T>(this T toolSettings, VTF.NormalKernel nKernel) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NKernel = nKernel;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetNKernel<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NKernel = null;
			return toolSettings;
		}

		#endregion

		#region NHeight

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="nHeight"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetNHeight<T>(this T toolSettings, VTF.NormalHeight nHeight) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NHeight = nHeight;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetNHeight<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NHeight = null;
			return toolSettings;
		}

		#endregion

		#region NScale

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="nScale"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetNScale<T>(this T toolSettings, float? nScale) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NScale = nScale;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetNScale<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NScale = null;
			return toolSettings;
		}

		#endregion

		#region Resize

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="resize"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetResize<T>(this T toolSettings, bool resize) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Resize = resize;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetResize<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Resize = null;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T EnableResize<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Resize = true;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T DisableResize<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Resize = false;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ToggleResize<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Resize = !toolSettings.Resize;
			return toolSettings;
		}

		#endregion

		#region Gamma

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="gamma"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetGamma<T>(this T toolSettings, bool gamma) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Gamma = gamma;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetGamma<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Gamma = null;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T EnableGamma<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Gamma = true;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T DisableGamma<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Gamma = false;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ToggleGamma<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Gamma = !toolSettings.Gamma;
			return toolSettings;
		}

		#endregion

		#region NoMipmaps

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="noMipmaps"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetNoMipmaps<T>(this T toolSettings, bool noMipmaps) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NoMipmaps = noMipmaps;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetNoMipmaps<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NoMipmaps = null;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T EnableNoMipmaps<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NoMipmaps = true;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T DisableNoMipmaps<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NoMipmaps = false;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ToggleNoMipmaps<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NoMipmaps = !toolSettings.NoMipmaps;
			return toolSettings;
		}

		#endregion

		#region Normal

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="normal"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetNormal<T>(this T toolSettings, bool normal) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Normal = normal;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetNormal<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Normal = null;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T EnableNormal<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Normal = true;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T DisableNormal<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Normal = false;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ToggleNormal<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Normal = !toolSettings.Normal;
			return toolSettings;
		}

		#endregion

		#region NWrap

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="nWrap"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetNWrap<T>(this T toolSettings, bool nWrap) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NWrap = nWrap;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetNWrap<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NWrap = null;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T EnableNWrap<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NWrap = true;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T DisableNWrap<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NWrap = false;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ToggleNWrap<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.NWrap = !toolSettings.NWrap;
			return toolSettings;
		}

		#endregion

		#region Recurse

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="recurse"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetRecurse<T>(this T toolSettings, bool recurse) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Recurse = recurse;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetRecurse<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Recurse = null;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T EnableRecurse<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Recurse = true;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T DisableRecurse<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Recurse = false;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ToggleRecurse<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Recurse = !toolSettings.Recurse;
			return toolSettings;
		}

		#endregion

		#region Pause

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="pause"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T SetPause<T>(this T toolSettings, bool pause) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Pause = pause;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetPause<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Pause = null;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T EnablePause<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Pause = true;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T DisablePause<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Pause = false;
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T TogglePause<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Pause = !toolSettings.Pause;
			return toolSettings;
		}

		#endregion

		#region Params

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T AddParam<T>(this T toolSettings, string name, string value) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			if (toolSettings.Params == null)
				toolSettings.Params = new Dictionary<string, string>();
			toolSettings.Params.Add(name, value);
			return toolSettings;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="toolSettings"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public static T ResetParams<T>(this T toolSettings) where T : VTF
		{
			toolSettings = toolSettings.NewInstance();
			toolSettings.Params = new Dictionary<string, string>();
			return toolSettings;
		}

		#endregion

	}

}
