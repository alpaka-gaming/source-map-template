namespace Nuke.Common.Tools.Source.Interfaces
{
	public interface IDownloadable
	{
		string Url { get; }
		bool Download();

		string InstallDir { get; set; }
	}
}
