namespace System.IO
{
	public static class Extensions
	{
		public static void CopyAll(this DirectoryInfo source, DirectoryInfo target, bool overwrite = false)
		{
			Directory.CreateDirectory(target.FullName);

			// Copy each file into the new directory.
			foreach (var fi in source.GetFiles())
				fi.CopyTo(Path.Combine(target.FullName, fi.Name), overwrite);

			// Copy each subdirectory using recursion.
			foreach (var diSourceSubDir in source.GetDirectories())
			{
				var nextTargetSubDir =
					target.CreateSubdirectory(diSourceSubDir.Name);
				CopyAll(diSourceSubDir, nextTargetSubDir);
			}
		}
	}
}
