using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyGoodsOnlyOnePanel : UIScene
{
    private UIButton Cancel_Button;
    private UIButton Sure_Button;
    private void Awake()
    {
        Cancel_Button = Helper.GetChild<UIButton>(this.transform, "Cancel_Button");
        Sure_Button = Helper.GetChild<UIButton>(this.transform, "Sure_Button");
    }

    protected override void Start()
    {
        base.Start();
        Cancel_Button.onClick.Add(new EventDelegate(Cancel));
        Sure_Button.onClick.Add(new EventDelegate(Sure));
    }

    private void Cancel()
    {
        AudioManager.Instance.PlaySound(1);
        DealPanel._instance.RevertMerchantItemSelectState();
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_BuyGoodsOnlyOnePanel, false);
    }

    //确定购买一件物品
    private void Sure()
    {
        AudioManager.Instance.PlaySound(1);
        DealPanel._instance.RevertMerchantItemSelectState();
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_BuyGoodsOnlyOnePanel, false);
    }
}
