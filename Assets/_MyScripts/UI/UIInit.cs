using UnityEngine;

public class UIInit : MonoBehaviour
{
    private UIManager uiManager;
    private void Start()
    {
        Object obj = FindObjectOfType(typeof(UIManager));
        if (obj != null)
            uiManager = obj as UIManager;
        if (uiManager == null)
        {
            GameObject manager = new GameObject("UIManager");
            uiManager = manager.AddComponent<UIManager>();
        }
        uiManager.InitializeUIs();
        uiManager.SetUIVisible();
    }
    //void Update()
    //{
    //    if (NetManager.GetInstance().WebSocket == null)
    //    {
    //        //提示点击事件

    //    }
    //}


}
