using System;
using System.IO;

public static class PathHelper
{
    public static string GetProjectRoot()
    {
        string basePath = AppContext.BaseDirectory;
        return Directory.GetParent(basePath).Parent.Parent.Parent.FullName;
    }

    public static string GetConfigPath()
    {
        return Path.Combine(GetProjectRoot(), "Core", "Layers", "Convolution", "DataFilters", "config.json");
    }

    public static string GetFiltersPath()
    {
        return Path.Combine(GetProjectRoot(), "Core", "Layers", "Convolution", "DataFilters");
    }
    
    public static string GetDataFiltersPath()
    {
        string path = Path.Combine(GetProjectRoot(), "Core", "Layers", "Convolution", "DataFilters");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }

}