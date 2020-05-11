public class OpenBagPanel : UIScene
{
    private UIButton OpenBag_Button;

    private void Awake()
    {
        PlayerInfoManager.Instance.SetPlayerAttributeInfo();
        OpenBag_Button = Helper.GetChild(this.transform, "OpenBag_Button").GetComponent<UIButton>();
    }
    protected override void Start()
    {
        base.Start();
        OpenBag_Button.onClick.Add(new EventDelegate(OpenBag));
    }

    public void OpenBag()
    {
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_OpenBagPanel, false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_BagPanel, true);

    }
}
