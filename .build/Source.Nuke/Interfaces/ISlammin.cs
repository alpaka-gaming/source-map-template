using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Nuke.Common.Tools.Source.Interfaces
{
	public interface ISlammin
	{
		bool? UsingSlammin { get; set; }

		public static Dictionary<string, string> HashTable = new Dictionary<string, string>
		{
			{"vbsp.exe", "53EDE845"},
			{"vrad.exe", "CF53BF5B"},
			{"vvis.exe", "24CF8C80"}
		};

		public enum Mode
		{
			SinglePlayer = 1,
			MultiPlayer = 2
		}

		public static void Download(Tools toolsSettings)
		{
			var urlSp = "https://github.com/alpaka-gaming/SlamminTools/releases/download/v1.6/SlamminToolsSP.zip";
			var urlMp = "https://github.com/alpaka-gaming/SlamminTools/releases/download/v1.6/SlamminToolsMP.zip";

			var mpGames = new long[] {243750};
			var mode = mpGames.Contains(toolsSettings.AppId) ? Mode.MultiPlayer : Mode.SinglePlayer;
			var binPath = Path.Combine(toolsSettings.Game, "..", "bin");
			var url = mode == Mode.SinglePlayer ? urlSp : urlMp;

			var localFile = string.Empty;
			var localDir = string.Empty;
			var fileName = "slammintools.zip";

			var toolPath = binPath;
			if (string.IsNullOrWhiteSpace(toolPath)) return;
			if (!string.IsNullOrWhiteSpace(toolPath))
			{
				localFile = Path.Combine(toolPath, fileName);
				localDir = toolPath;
			}
			if (string.IsNullOrWhiteSpace(localFile)) return;
			if (!File.Exists(localFile))
			{
				using (var client = new HttpClient())
				{
					var response = client.Send(new HttpRequestMessage(HttpMethod.Get, url));
					using var resultStream = response.Content.ReadAsStream();
					using var fileStream = File.OpenWrite(localFile);
					resultStream.CopyTo(fileStream);
				}
			}
			if (File.Exists(localFile))
			{
				ZipFile.ExtractToDirectory(localFile, localDir, true);
				var files = Directory.GetFiles(Path.Combine(localDir, mode == Mode.MultiPlayer ? "MP" : "SP")).Where(m => m.Contains(Path.GetFileNameWithoutExtension(toolsSettings.Executable) ?? string.Empty)).ToArray();
				foreach (var file in files)
					File.Move(file, Path.Combine(localDir, Path.GetFileName(file)), true);
			}
		}
	}
}
