using System.Collections.Generic;

public class DataTableManager
{
    public static readonly DataTableManager Instance = new DataTableManager();

    private Dictionary<string, ConfigBase> dicConfig = new Dictionary<string, ConfigBase>();

    public void AddConfig(string name, ConfigBase cfg)
    {
        dicConfig[name] = cfg;
    }

    public T GetConfig<T>(string name) where T : ConfigBase
    {
        return (T)dicConfig[name];
    }

}
