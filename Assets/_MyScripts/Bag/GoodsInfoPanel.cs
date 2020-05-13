public class GoodsInfoPanel : UIScene
{
    public static GoodsInfoPanel _instance;
    //private UILabel GoodsPropertyLabel;
    //private UILabel GoodsDescribeLabel;
    private void Awake()
    {
        _instance = this;
        PlayerInfoManager.Instance.ItemName = Helper.GetChild<UILabel>(this.transform, "GoodsPropertyLabel");
        PlayerInfoManager.Instance.ItemDesc = Helper.GetChild<UILabel>(this.transform, "GoodsDescribeLabel");

    }
    protected override void Start()
    {
        base.Start();
    }
}
