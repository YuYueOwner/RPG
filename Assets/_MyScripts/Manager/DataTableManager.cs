using System.Collections.Generic;

public class DataTableManager
{
    public static readonly DataTableManager Instance = new DataTableManager();

    public Dictionary<string, ConfigBase> dicConfig = new Dictionary<string, ConfigBase>();

    public T GetConfig<T>(string name) where T : ConfigBase
    {
        return (T)dicConfig[name];
    }

}
