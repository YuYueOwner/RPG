public class GoodsInfoPanel : UIScene
{
    public static GoodsInfoPanel _instance;
    public UILabel GoodsTitelLabel;

    private void Awake()
    {
        _instance = this;
        GoodsTitelLabel = Helper.GetChild<UILabel>(this.transform, "GoodsTitelLabel");
        PlayerInfoManager.Instance.ItemName = Helper.GetChild<UILabel>(this.transform, "GoodsPropertyLabel");
        PlayerInfoManager.Instance.ItemDesc = Helper.GetChild<UILabel>(this.transform, "GoodsDescribeLabel");

    }
    protected override void Start()
    {
        base.Start();
    }
}
