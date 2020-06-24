using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyGoodsFailedPanel : UIScene
{
    private UIButton Sure_Button;

    private void Awake()
    {
        Sure_Button = Helper.GetChild<UIButton>(this.transform, "Sure_Button");
    }
    protected override void Start()
    {
        Sure_Button.onClick.Add(new EventDelegate(Sure));
    }

    private void Sure()
    {
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_BuyGoodsFailedPanel, false);
    }
}
