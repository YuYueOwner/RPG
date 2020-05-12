using UnityEngine;

public class ReadConfig : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        ConfigManager.Instance.AddConfig();
        PlayerInfoManager.Instance.SetPlayerAttributeInfo();
    }

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
}
