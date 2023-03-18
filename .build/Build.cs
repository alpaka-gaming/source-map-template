using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Source;
using Nuke.Common.Tools.Source.Tooling;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.Source.Tasks;

// ReSharper disable InconsistentNaming

class Build : NukeBuild
{
	/// Support plugins are available for:
	///   - JetBrains ReSharper        https://nuke.build/resharper
	///   - JetBrains Rider            https://nuke.build/rider
	///   - Microsoft VisualStudio     https://nuke.build/visualstudio
	///   - Microsoft VSCode           https://nuke.build/vscode
	public static int Main() => Execute<Build>(x => x.Compile);

	[Parameter("Configuration to build - Default is 'Fast' (local) or 'Publish' (server)")]
	readonly Configuration Configuration = !IsLocalBuild ? Configuration.Fast : Configuration.Publish;

	[Parameter("Environment to build - Default is 'Development' (local) or 'Production' (server)")]
	public string Environment = IsLocalBuild ? "Development" : "Production";

	[Solution]
	public readonly Solution Solution;

	[Parameter("Verbose")]
	public bool Verbose = !IsLocalBuild;

	AbsolutePath SourceDirectory => RootDirectory / "src";
	AbsolutePath PublishDirectory => RootDirectory / "publish";
	AbsolutePath ArtifactsDirectory => RootDirectory / "output";

	AbsolutePath ToolDirectory => Solution.Directory / "tools";
	AbsolutePath MapDirectory => SourceDirectory / "maps";

	#region Privates

	IList<Tool> _tools;

	long _steam_sdk_appid;
	string _game_name;
	string _map_name;
	string _steam_username;
	string _steam_password;
	string _source_sdk_mode;
	string _last_tag;
	string _install_dir;
	string _source_file;
	string _vmf_file;
	string _bsp_file;
	string _game_dir;
	string _bin_dir;

	#endregion

	Target Clean => _ => _
		.Executes(() =>
		{
			EnsureCleanDirectory(PublishDirectory);
			EnsureCleanDirectory(ArtifactsDirectory);
		});

	Target Prepare => _ => _
		.Executes(() =>
		{
			if (!Directory.Exists(PublishDirectory)) Directory.CreateDirectory(PublishDirectory);
			if (!Directory.Exists(ArtifactsDirectory)) Directory.CreateDirectory(ArtifactsDirectory);

			var startupPath = Solution.GetProject("_build")?.Directory ?? string.Empty;

			var config = new ConfigurationBuilder()
				.AddJsonFile(Path.Combine(startupPath, "appsettings.json"), false, true)
				.AddJsonFile(Path.Combine(startupPath, $"appsettings.{Environment}.json"), true, true)
				.Build();

			_game_name = config.GetValue<string>("definitions:GAME_NAME");
			_map_name = config.GetValue<string>("definitions:MAP_NAME");

			_source_sdk_mode = config.GetValue<string>("definitions:SOURCE_SDK_MODE");
			_steam_sdk_appid = config.GetValue<long>("definitions:STEAM_SDK_APPID");

			_steam_username = System.Environment.GetEnvironmentVariable("STEAM_USERNAME");
			_steam_password = System.Environment.GetEnvironmentVariable("STEAM_PASSWORD");

			if (string.IsNullOrWhiteSpace(_steam_username)) _steam_username = config.GetValue<string>("definitions:STEAM_USERNAME");
			if (string.IsNullOrWhiteSpace(_steam_password)) _steam_password = config.GetValue<string>("definitions:STEAM_PASSWORD");

			_tools = new List<Tool>();
			config.Bind("tools", _tools);

			try
			{
				using (var gitTag = new Process() {StartInfo = new ProcessStartInfo(fileName: "git", arguments: "tag --sort=-v:refname") {WorkingDirectory = Solution.Directory, RedirectStandardOutput = true, UseShellExecute = false}})
				{
					gitTag.Start();
					var value = gitTag.StandardOutput.ReadToEnd().Trim();
					_last_tag = new Regex(pattern: @"([a,b,v])([0-9]{1,})([a-z])", RegexOptions.Compiled).Match(value).Value;
					gitTag.WaitForExit();
				}
				if (string.IsNullOrWhiteSpace(_last_tag)) throw new NullReferenceException();
			}
			catch (Exception)
			{
				_last_tag = "a0a";
			}

			_source_file = Path.Combine(MapDirectory, $"{_map_name}.vmf");
			_vmf_file = Path.Combine(ArtifactsDirectory, $"{_map_name}_{_last_tag}.vmf");
			_bsp_file = Path.Combine(ArtifactsDirectory, $"{_map_name}_{_last_tag}.bsp");

			_install_dir = Path.Combine(ToolDirectory, "depots", _steam_sdk_appid.ToString());
			_game_dir = Path.Combine(_install_dir, _game_name);

			_bin_dir = Path.Combine(_install_dir, "bin");

			var _game_bin_dir = new DirectoryInfo(Path.Combine(_install_dir, _game_name, "bin"));
			var _sourcetest_bin_dir = new DirectoryInfo(Path.Combine(_install_dir, "sourcetest", "bin"));

			if (!_game_bin_dir.Exists) _game_bin_dir.Create();
			_sourcetest_bin_dir.CopyAll(_game_bin_dir, true);

		});

	Target Restore => _ => _
		.DependsOn(Prepare)
		.Executes(() =>
		{
			foreach (var tool in _tools)
			{
				var fileName = Path.GetFileName(tool.url);
				var localFile = Path.Combine(ToolDirectory, fileName);
				var localDir = Path.Combine(ToolDirectory, tool.name);
				if (!File.Exists(localFile))
				{
					using (var client = new HttpClient())
					{
						var response = client.Send(new HttpRequestMessage(HttpMethod.Get, tool.url));
						using var resultStream = response.Content.ReadAsStream();
						using var fileStream = File.OpenWrite(localFile);
						resultStream.CopyTo(fileStream);
					}
				}
				if (File.Exists(localFile) && !Directory.Exists(localDir))
				{
					ZipFile.ExtractToDirectory(localFile, localDir);
				}
			}

			if (_source_sdk_mode == "steamcmd")
			{
				var process = new Process();
				process.StartInfo = new ProcessStartInfo(Path.Combine(ToolDirectory, "steamcmd", "steamcmd.exe")) {Arguments = $"+force_install_dir {_install_dir} +login {_steam_username} {_steam_password} +app_update {_steam_sdk_appid} validate +quit"};
			}
			else if (_source_sdk_mode == "depotdownloader")
			{
				var process = new Process();
				process.StartInfo = new ProcessStartInfo("dotnet") {Arguments = $"{Path.Combine(ToolDirectory, "depotdownloader", "DepotDownloader.dll")} -username {_steam_username} -password {_steam_password} -app {_steam_sdk_appid} -dir {_install_dir}"};
			}

			var mp_games = new long[] {243750};
			if (mp_games.Contains(_steam_sdk_appid))
			{
				if (_tools.Any(m => m.name == "slammintools"))
				{
					var sourceDir = new DirectoryInfo(Path.Combine(ToolDirectory, "slammintools", "mp"));
					sourceDir.CopyAll(new DirectoryInfo(_bin_dir), true);
				}
			}

		});

	Target Compile => _ => _
		.DependsOn(Clean)
		.DependsOn(Restore)
		.Executes(() =>
		{
			File.Copy(_source_file, _vmf_file, true);

			Source(_ => new VBSP()
				.SetProcessWorkingDirectory(ArtifactsDirectory)
				.SetVerbose(Verbose)
				.SetGamePath(_game_dir)
				.SetInput(_vmf_file)
			);

			Source(_ => new VVIS()
				.SetProcessWorkingDirectory(ArtifactsDirectory)
				.SetVerbose(Verbose)
				.SetGamePath(_game_dir)
				//
				.SetFast(Configuration == Configuration.Fast)
				.SetNoSort(Configuration == Configuration.Fast)
				//
				.SetInput(_bsp_file)
			);

			Source(_ => new VRAD()
				.SetProcessWorkingDirectory(ArtifactsDirectory)
				.SetVerbose(Verbose)
				.SetGamePath(_game_dir)
				//
				.SetFast(Configuration == Configuration.Fast)
				.SetBounce((ushort)(Configuration == Configuration.Fast ? 2 : 100))
				//
				.SetInput(_bsp_file)
			);

			File.Delete(_vmf_file);

			if (Configuration != Configuration.Fast)
			{
				var _bsp_game_target_file = Path.Combine(_game_dir, "maps", Path.GetFileName(_bsp_file) ?? throw new InvalidOperationException());
				File.Move(_bsp_file, _bsp_game_target_file, true);

				Source(_ => new CUBEMAP()
					.SetProcessWorkingDirectory(ArtifactsDirectory)
					.SetVerbose(Verbose)
					.SetGamePath(_game_dir)
					.SetInput(_bsp_file)
				);

				var bspzip_logs = Path.ChangeExtension(_bsp_game_target_file, "log");
				Source(_ => new PACK()
					.SetProcessWorkingDirectory(ArtifactsDirectory)
					.SetVerbose(Verbose)
					.SetInput(_bsp_game_target_file)
					.SetOutput(Path.ChangeExtension(_bsp_game_target_file, "log"))
					.SetCallback(() =>
					{
						var content = File.ReadAllLines(bspzip_logs).Skip(3).Where(s => !s.EndsWith(".vhv"));
						File.WriteAllLines(Path.ChangeExtension(_bsp_game_target_file, "tmp"), content);
						File.Move(Path.ChangeExtension(_bsp_game_target_file, "tmp"), Path.ChangeExtension(_bsp_game_target_file, "log"), true);
					})
				);

				Source(_ => new PACK()
					.SetProcessWorkingDirectory(ArtifactsDirectory)
					.SetVerbose(Verbose)
					.SetInput(_bsp_game_target_file)
					.SetFileList(bspzip_logs)
					.SetCallback(() =>
					{
						File.Delete(bspzip_logs);
						File.Move(Path.ChangeExtension(_bsp_game_target_file, "bzp"), Path.ChangeExtension(_bsp_game_target_file, "bsp"), true);
					})
				);

				File.Move(_bsp_game_target_file, _bsp_file, true);
			}

		});

	Target Publish => _ => _
		.DependsOn(Compile)
		.Executes(() =>
		{

			var _bsp_target_file = Path.Combine(PublishDirectory, Path.GetFileName(_bsp_file) ?? throw new InvalidOperationException());

			var _prt_file = Path.ChangeExtension(_bsp_file, "prt");
			var _log_file = Path.ChangeExtension(_bsp_file, "log");

			var _prt_target_file = Path.ChangeExtension(_bsp_target_file, "prt");
			var _log_target_file = Path.ChangeExtension(_bsp_target_file, "log");

			if (_bsp_file != null)
				File.Move(_bsp_file, _bsp_target_file, true);
			if (_prt_file != null)
				File.Move(_prt_file, _prt_target_file, true);
			if (_log_file != null)
				File.Move(_log_file, _log_target_file, true);

			Source(_ => new BZIP()
				.SetProcessWorkingDirectory(PublishDirectory)
				.SetVerbose(Verbose)
				.SetGamePath(Path.Combine(ToolDirectory, "bzip2"))
				//
				.EnableForce()
				.EnableKeep()
				//
				.SetInput(_bsp_target_file)
			);

		});
}

public class Tool
{
	public string name { get; set; }
	public string version { get; set; }
	public string url { get; set; }
}
