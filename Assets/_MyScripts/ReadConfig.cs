using HotFix_Project.Config;
using UnityEngine;

public class ReadConfig : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ConfigManager.Instance.AddConfig();
        ActivityMainConfig activitymainList = DataTableManager.Instance.GetConfig<ActivityMainConfig>("ActivityMain");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
