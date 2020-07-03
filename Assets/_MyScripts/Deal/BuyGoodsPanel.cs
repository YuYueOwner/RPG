using UnityEngine;

public class BuyGoodsPanel : UIScene
{
    public static BuyGoodsPanel _instance;
    private UIButton Minus_Button;
    private UIButton Add_Button;
    private UIButton Cancel_Button;
    private UIButton Sure_Button;
    [HideInInspector]
    public UILabel SellGoodsNumLabel;
    [HideInInspector]
    public int recordCurrentGoodsNum;
    private void Awake()
    {
        _instance = this;
        Minus_Button = Helper.GetChild<UIButton>(this.transform, "Minus_Button");
        Add_Button = Helper.GetChild<UIButton>(this.transform, "Add_Button");
        Cancel_Button = Helper.GetChild<UIButton>(this.transform, "Cancel_Button");
        Sure_Button = Helper.GetChild<UIButton>(this.transform, "Sure_Button");
        SellGoodsNumLabel = Helper.GetChild<UILabel>(this.transform, "SellGoodsNumLabel");

    }
    protected override void Start()
    {
        base.Start();
        Minus_Button.onClick.Add(new EventDelegate(Minus));
        Add_Button.onClick.Add(new EventDelegate(Add));
        Cancel_Button.onClick.Add(new EventDelegate(Cancel));
        Sure_Button.onClick.Add(new EventDelegate(Sure));
    }

    private void Minus()
    {
        SellGoodsNumLabel.text = int.Parse(SellGoodsNumLabel.text) - 5 >= 0 ? (int.Parse(SellGoodsNumLabel.text) - 5).ToString() : "0";
    }

    private void Add()
    {
        //判断是否大于上限
        SellGoodsNumLabel.text = int.Parse(SellGoodsNumLabel.text) + 5 >= recordCurrentGoodsNum ? recordCurrentGoodsNum.ToString() : (int.Parse(SellGoodsNumLabel.text) + 5).ToString();
    }

    private void Cancel()
    {
        AudioManager.Instance.PlaySound(1);
        DealPanel._instance.RevertMerchantItemSelectState();
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_BuyGoodsPanel, false);
    }

    private void Sure()
    {
        AudioManager.Instance.PlaySound(1);
        DealPanel._instance.OnRefreshBuyData((int.Parse(SellGoodsNumLabel.text)));
        DealPanel._instance.RevertMerchantItemSelectState();
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_BuyGoodsPanel, false);
    }
}
