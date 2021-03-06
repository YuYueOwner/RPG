﻿public class OpenBagPanel : UIScene
{
    private UIButton OpenBag_Button;
    private UIButton OpenSkill_Button;
    private UIButton OpenDeal_Button;



    private void Awake()
    {
        OpenBag_Button = Helper.GetChild(this.transform, "OpenBag_Button").GetComponent<UIButton>();
        OpenSkill_Button = Helper.GetChild(this.transform, "OpenSkill_Button").GetComponent<UIButton>();
        OpenDeal_Button = Helper.GetChild(this.transform, "OpenDeal_Button").GetComponent<UIButton>();
    }
    protected override void Start()
    {
        base.Start();
        OpenBag_Button.onClick.Add(new EventDelegate(OpenBag));
        OpenSkill_Button.onClick.Add(new EventDelegate(OpenSkill));
        OpenDeal_Button.onClick.Add(new EventDelegate(OpenDeal));

    }

    public void OpenBag()
    {
        AudioManager.Instance.PlaySound(1);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_OpenBagPanel, false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_BagPanel, true);
        //SceneManager.LoadScene("SceneStart");
    }

    public void OpenSkill()
    {
        AudioManager.Instance.PlaySound(1);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_OpenBagPanel, false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_SkillAttackPanel, true);
        SkillAttackPanel._instance.OnCreateOwnSkillItem();
        SkillAttackPanel._instance.OnCreateSkillAttackItem();

    }

    public void OpenDeal()
    {
        AudioManager.Instance.PlaySound(1);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_OpenBagPanel, false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_DealPanel, true);
    }
}
