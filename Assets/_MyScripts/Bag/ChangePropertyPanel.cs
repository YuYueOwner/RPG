﻿public class ChangePropertyPanel : UIScene
{
    private UIButton Sure_Button;
    private UIButton Cancel_Button;

    private void Awake()
    {
        Sure_Button = Helper.GetChild<UIButton>(this.transform, "Sure_Button");
        Cancel_Button = Helper.GetChild<UIButton>(this.transform, "Cancel_Button");
    }
    protected override void Start()
    {
        base.Start();
        Sure_Button.onClick.Add(new EventDelegate(Sure));
        Cancel_Button.onClick.Add(new EventDelegate(Cancel));
    }

    private void Sure()
    {
        AudioManager.Instance.PlaySound(1);

        PlayerPrefsManager.Instance.SetPlayerPrefs(true);
        BagPanel._instance.ChangePropertySureButton();
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_ChangePropertyPanel, false);
    }

    private void Cancel()
    {
        AudioManager.Instance.PlaySound(1);

        PlayerPrefsManager.Instance.SetPlayerPrefs(false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_ChangePropertyPanel, false);
    }
}
