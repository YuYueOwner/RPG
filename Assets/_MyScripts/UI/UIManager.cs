using DevelopEngine;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelName
{
    //Scene_Start
    public const string SceneStart_OpenBagPanel = "OpenBagPanel";
    public const string SceneStart_BagPanel = "BagPanel";
    public const string SceneStart_ChangePropertyPanel = "ChangePropertyPanel";
    public const string SceneStart_EquipmentBagPanel = "EquipmentBagPanel";
    public const string SceneStart_DiscardGoodsPanel = "DiscardGoodsPanel";
    public const string SceneStart_GoodsInfoPanel = "GoodsInfoPanel";
    public const string SceneStart_EquipmentGoodsPanel = "EquipmentGoodsPanel";
    public const string SceneStart_SkillAttackPanel = "SkillAttackPanel";


}
public class UIManager : MonoSingleton<UIManager>
{

    private Dictionary<string, UIScene> mUIScene = new Dictionary<string, UIScene>();
    private Dictionary<UIAnchor.Side, GameObject> mUIAnchor = new Dictionary<UIAnchor.Side, GameObject>();

    public void InitializeUIs()
    {
        mUIAnchor.Clear();
        Object[] objs = FindObjectsOfType(typeof(UIAnchor));
        if (objs != null)
        {
            foreach (Object obj in objs)
            {
                UIAnchor uiAnchor = obj as UIAnchor;
                if (!mUIAnchor.ContainsKey(uiAnchor.side))
                    mUIAnchor.Add(uiAnchor.side, uiAnchor.gameObject);
            }
        }
        mUIScene.Clear();
        Object[] uis = FindObjectsOfType(typeof(UIScene));
        if (uis != null)
        {
            foreach (Object obj in uis)
            {
                UIScene ui = obj as UIScene;
                ui.SetVisible(false);
                mUIScene.Add(ui.gameObject.name, ui);
            }
        }
    }

    public void SetVisible(string name, bool visible)
    {
        if (visible && !IsVisible(name))
        {
            OpenScene(name);
        }
        else if (!visible && IsVisible(name))
        {
            CloseScene(name);
        }
    }

    public bool IsVisible(string name)
    {
        UIScene ui = GetUI(name);
        if (ui != null)
            return ui.IsVisible();
        return false;
    }
    private UIScene GetUI(string name)
    {
        UIScene ui;
        return mUIScene.TryGetValue(name, out ui) ? ui : null;
    }

    public T GetUI<T>(string name) where T : UIScene
    {
        return GetUI(name) as T;
    }

    private bool isLoaded(string name)
    {
        if (mUIScene.ContainsKey(name))
        {
            return true;
        }
        return false;
    }

    private void OpenScene(string name)
    {
        if (isLoaded(name))
        {
            mUIScene[name].SetVisible(true);
        }
    }
    private void CloseScene(string name)
    {
        if (isLoaded(name))
        {
            mUIScene[name].SetVisible(false);
        }
    }

    /// <summary>   /// 显示一级界面    /// </summary>
    public void SetUIVisible()
    {
        //StartGame
        SetVisible(UIPanelName.SceneStart_OpenBagPanel, true);
    }
}
