using UnityEngine;

public class DiscardGoodsPanel : UIScene
{
    public static DiscardGoodsPanel _instance;
    private UIButton Sure_Button;
    private UIButton Cancel_Button;
    private string configType;

    private void Awake()
    {
        _instance = this;
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
        if (configType != null && configType == "SkillGrid")
        {
            SkillAttackPanel._instance.RevomeSkill(PlayerInfoManager.Instance.SelectSkillId);
            GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().RemoveSkillQuene(PlayerInfoManager.Instance.SelectSkillId);
        }
        else if (configType != null && configType == "DefGrid")
        {
            SkillDefendPanel._instance.RevomeSkill(PlayerInfoManager.Instance.SelectSkillId);
            GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().RemoveSkillQuene(PlayerInfoManager.Instance.SelectSkillId);
        }
        else
        {
            PlayerInfoManager.Instance.RemovePlayerItemData(PlayerInfoManager.Instance.SelectItemId);
        }
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_DiscardGoodsPanel, false);
    }

    public void SetType(string str)
    {
        configType = str;
    }

    private void Cancel()
    {
        AudioManager.Instance.PlaySound(1);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_DiscardGoodsPanel, false);
    }
}
