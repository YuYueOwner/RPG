using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitDealPanel : UIScene
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
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_QuitDealPanel, false);
    }
    private void Sure()
    {
        AudioManager.Instance.PlaySound(1);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_QuitDealPanel, false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_DealPanel, false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_OpenBagPanel, true);


    }
}
