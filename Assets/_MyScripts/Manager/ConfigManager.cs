using HotFix_Project.Config;
using System.IO;
using UnityEngine;

public class ConfigManager
{
    public static readonly ConfigManager Instance = new ConfigManager();


    //路径  
    private string fullPath = "Assets/Resources/Config" + "/";
    private TextAsset txt;

    public void AddConfig()
    {
        //获取指定路径下面的所有资源文件  
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            Debug.Log(string.Format("一共有{0}张表", files.Length / 2));

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                //Debug.Log("Name:" + files[i].Name);
                string name = files[i].Name.Replace(".txt", "");
                InitConfig(name);
            }
        }
    }

    public void InitConfig(string name)
    {
        Debug.Log("表格名字:" + name);
        txt = Resources.Load("Config/" + name, typeof(TextAsset)) as TextAsset;
        string text = txt.text;
        if (text.Length > 0)
        {
            string[] configArr = text.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            ConfigBase configBase;
            if (name == "TaskItems")
            {
                //任务物品表
                configBase = new TaskItemsConfig();
            }
            else if (name == "Materials")
            {
                //材料表
                configBase = new MaterialsConfig();
            }
            else if (name == "Weapon")
            {
                //武器表
                configBase = new WeaponConfig();
            }
            else if (name == "Consumables")
            {
                // 消耗品表
                configBase = new ConsumablesConfig();

            }
            else if (name == "Armor")
            {
                // 防具表
                configBase = new ArmorConfig();
            }
            else if (name == "ActivityMain")
            {
                // 测试表
                configBase = new ActivityMainConfig();
            }
            else
            {
                Debug.LogError("没有找到这张表");
                return;
            }
            configBase.InitConfig(configArr);
            DataTableManager.Instance.dicConfig[name] = configBase;
        }
    }






}
