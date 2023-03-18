﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using JetBrains.Annotations;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Source.Tooling;

namespace Nuke.Common.Tools.Source
{
	[PublicAPI]
	[ExcludeFromCodeCoverage]
	public static class Tasks
	{
		public static IReadOnlyCollection<Output> Source(Configure<Tools> configurator)
		{
			return Source(configurator(new Tools()));
		}

		public static IReadOnlyCollection<Output> Source(Tools toolsSettings = null)
		{
			toolsSettings = toolsSettings ?? throw new NullReferenceException("ToolPath is not defined");
			if (!toolsSettings.Skip)
			{
				using var process = ProcessTasks.StartProcess(toolsSettings);
				process.AssertZeroExitCode();
				if (!string.IsNullOrWhiteSpace(toolsSettings.Output))
					File.WriteAllText(toolsSettings.Output, process.Output.StdToText());
				toolsSettings.Callback?.Invoke();
				return process.Output;
			}
			return null;
		}
	}
}