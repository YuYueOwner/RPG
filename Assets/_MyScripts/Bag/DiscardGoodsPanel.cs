public class DiscardGoodsPanel : UIScene
{
    private UIButton Sure_Button;
    private UIButton Cancel_Button;
    private string type;

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

        PlayerInfoManager.Instance.RemovePlayerItemData(PlayerInfoManager.Instance.SelectItemId);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_DiscardGoodsPanel, false);
    }

    public void SetType()
    {


    }

    private void Cancel()
    {
        AudioManager.Instance.PlaySound(1);

        UIManager.Instance.SetVisible(UIPanelName.SceneStart_DiscardGoodsPanel, false);
    }
}
