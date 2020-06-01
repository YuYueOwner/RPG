using UnityEngine;

public class GoodsInfoPanel : UIScene
{
    public static GoodsInfoPanel _instance;
    public UILabel GoodsTitelLabel;
    public GameObject goBg_Sprite;
    private void Awake()
    {
        _instance = this;
        goBg_Sprite = Helper.GetChild(this.transform, "Bg_Sprite");
        GoodsTitelLabel = Helper.GetChild<UILabel>(this.transform, "GoodsTitelLabel");
        PlayerInfoManager.Instance.ItemName = Helper.GetChild<UILabel>(this.transform, "GoodsPropertyLabel");
        PlayerInfoManager.Instance.ItemDesc = Helper.GetChild<UILabel>(this.transform, "GoodsDescribeLabel");
        PlayerInfoManager.Instance.ItemExp = Helper.GetChild<UILabel>(this.transform, "GoodsExpLabel");
    }
    protected override void Start()
    {
        base.Start();
    }
}
