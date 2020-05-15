public class EquipmentBagPanel : UIScene
{
    private UIButton Sure_Button;

    private void Awake()
    {
        Sure_Button = Helper.GetChild<UIButton>(this.transform, "Sure_Button");
    }

    protected override void Start()
    {
        base.Start();
        Sure_Button.onClick.Add(new EventDelegate(Sure));
    }

    private void Sure()
    {
        AudioManager.Instance.PlaySound(1);

        UIManager.Instance.SetVisible(UIPanelName.SceneStart_EquipmentBagPanel, false);
    }
}
