public class SkillDefendPanel : UIScene
{
    private UIButton Attack_Button;
    private UIButton Defend_Button;
    private UIButton Back_Button;

    private void Awake()
    {
        Attack_Button = Helper.GetChild<UIButton>(this.transform, "Attack_Button");
        Defend_Button = Helper.GetChild<UIButton>(this.transform, "Defend_Button");
        Back_Button = Helper.GetChild<UIButton>(this.transform, "Back_Button");
    }
    protected override void Start()
    {
        base.Start();
        Attack_Button.onClick.Add(new EventDelegate(Attack));
        Defend_Button.onClick.Add(new EventDelegate(Defend));
        Back_Button.onClick.Add(new EventDelegate(Back));
    }

    //攻
    private void Attack()
    {
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_SkillAttackPanel, true);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_SkillDefendPanel, false);
    }

    //守
    private void Defend()
    {

    }

    //返回
    private void Back()
    {
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_SkillDefendPanel, false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_OpenBagPanel, true);

    }
}
