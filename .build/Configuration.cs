using System;
using System.ComponentModel;
using System.Linq;
using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<Configuration>))]
public class Configuration : Enumeration
{
    public static Configuration Fast = new Configuration { Value = nameof(Fast) };
    public static Configuration Normal = new Configuration { Value = nameof(Normal) };
    public static Configuration Publish = new Configuration { Value = nameof(Publish) };

    public static implicit operator string(Configuration configuration)
    {
        return configuration.Value;
    }
}
