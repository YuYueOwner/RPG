using HotFix_Project.Config;
using UnityEngine;

public class DealPanel : UIScene
{
    public static DealPanel _instance;
    #region 商人
    //商人头像
    private UISprite MerchantHeadPhotoSprite;
    //商人姓名
    private UILabel MerchantNameLabel;
    //商人物品Grid
    private UIGrid MerchantGrid;
    #endregion

    #region 出售
    //总出售物品金额
    private UILabel SellTotalNumLabel;
    //出售物品Grid
    private UIGrid SellGoodsGrid;
    #endregion

    #region 背包
    //玩家头像
    private UISprite PlayerHeadPhotoSprite;
    //玩家姓名
    private UILabel PlayerNameLabel;
    //拥有银两数量
    private UILabel OwnGoldNumLabel;
    //背包物品Grid
    private UIGrid BagGoodsGrid;
    #endregion
    //确定按钮
    private UIButton SureButton;
    //返回按钮
    private UIButton BackButton;
    private void Awake()
    {
        _instance = this;
        MerchantHeadPhotoSprite = Helper.GetChild<UISprite>(this.transform, "MerchantHeadPhotoSprite");
        MerchantNameLabel = Helper.GetChild<UILabel>(this.transform, "MerchantNameLabel");
        MerchantGrid = Helper.GetChild<UIGrid>(this.transform, "MerchantGrid");

        SellTotalNumLabel = Helper.GetChild<UILabel>(this.transform, "SellTotalNumLabel");
        SellGoodsGrid = Helper.GetChild<UIGrid>(this.transform, "SellGoodsGrid");

        PlayerHeadPhotoSprite = Helper.GetChild<UISprite>(this.transform, "PlayerHeadPhotoSprite");
        PlayerNameLabel = Helper.GetChild<UILabel>(this.transform, "PlayerNameLabel");
        OwnGoldNumLabel = Helper.GetChild<UILabel>(this.transform, "OwnGoldNumLabel");
        BagGoodsGrid = Helper.GetChild<UIGrid>(this.transform, "BagGoodsGrid");

        SureButton = Helper.GetChild<UIButton>(this.transform, "SureButton");
        BackButton = Helper.GetChild<UIButton>(this.transform, "BackButton");
    }
    protected override void Start()
    {
        base.Start();
        SureButton.onClick.Add(new EventDelegate(Sure));
        BackButton.onClick.Add(new EventDelegate(Back));
        CreatMerchantGoods();
        CreatBagGoods();
        CreatSellGoods();
    }

    //生成左侧商人物品区中的物品
    private void CreatMerchantGoods()
    {
        //测试数据
        string npcType = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().NpcType.ToString();

        MerchantGoodsConfig cfgData = DataTableManager.Instance.GetConfig<MerchantGoodsConfig>("MerchantGoods");
        PropConfig cfgPropData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        for (int i = 0; i < 10; i++)
        {
            MerchantGoodsConfig.MerchantGoodsObject data = cfgData.GetListConfigElementByID(npcType, i);
            GameObject goMerchant = Instantiate(Resources.Load("Prefabs/Merchant_Item"), Vector3.zero, Quaternion.identity) as GameObject;
            goMerchant.name = i.ToString();
            goMerchant.transform.SetParent(MerchantGrid.transform);
            goMerchant.transform.localPosition = Vector3.zero;
            goMerchant.transform.localScale = Vector3.one;

            PropConfig.PropObject propData = cfgPropData.GetListConfigElementByID(i);
            if (data != null)
            {
                //给物品Icon赋值
                Helper.GetChild<UISprite>(goMerchant.transform, "GoodsSprite").spriteName = propData.ItemIcon;
                //给物品名字赋值
                Helper.GetChild<UILabel>(goMerchant.transform, "BagNameLabel").text = propData.ItemName;
                //给物品金额赋值
                Helper.GetChild<UILabel>(goMerchant.transform, "GoldNumLabel").text = data.BuyPrice.ToString();
                //物品数量
                Helper.GetChild<UILabel>(goMerchant.transform, "GoodsNumLabel").text = data.ItemNum.ToString();
            }

            //如果当前物品金额小于拥有元宝总额，字体变红
            if (int.Parse(Helper.GetChild<UILabel>(goMerchant.transform, "GoldNumLabel").text) > int.Parse(OwnGoldNumLabel.text))
            {
                Helper.GetChild(goMerchant.transform, "InsufficientGold_Sprite").SetActive(true);
                Helper.GetChild<UILabel>(goMerchant.transform, "BagNameLabel").color = Color.red;
                Helper.GetChild<UILabel>(goMerchant.transform, "GoldNumLabel").color = Color.red;
            }
        }
        MerchantGrid.repositionNow = true;
        MerchantGrid.Reposition();

        MerchantHeadPhotoSprite.spriteName = "HeadPhoto" + GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().NpcImageID;
        MerchantNameLabel.text = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().NpcName.ToString();
    }

    private void Sure()
    {

    }

    private void Back()
    {
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_DealPanel, false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_OpenBagPanel, true);
    }

    //还原物品选中状态
    public void RevertMerchantItemSelectState()
    {
        for (int i = 0; i < MerchantGrid.transform.childCount; i++)
        {
            Helper.GetChild(MerchantGrid.transform.GetChild(i), "SelectFrame").SetActive(false);
        }
    }

    //卖完物品后刷新，判断当前元宝数是否买得起商人物品，是否显示红色遮罩和红色字体
    public void RefeshMerchantGridRedMask()
    {
        for (int i = 0; i < MerchantGrid.transform.childCount; i++)
        {
            //买不起
            if (int.Parse(Helper.GetChild<UILabel>(MerchantGrid.GetChild(i), "GoldNumLabel").text) > int.Parse(OwnGoldNumLabel.text))
            {
                Helper.GetChild(MerchantGrid.GetChild(i), "InsufficientGold_Sprite").SetActive(true);
                Helper.GetChild<UILabel>(MerchantGrid.GetChild(i), "BagNameLabel").color = Color.red;
                Helper.GetChild<UILabel>(MerchantGrid.GetChild(i), "GoldNumLabel").color = Color.red;
            }
            else//买得起
            {
                Helper.GetChild(MerchantGrid.GetChild(i), "InsufficientGold_Sprite").SetActive(false);
                Helper.GetChild<UILabel>(MerchantGrid.GetChild(i), "BagNameLabel").color = Color.white;
                Helper.GetChild<UILabel>(MerchantGrid.GetChild(i), "GoldNumLabel").color = Color.white;
            }
        }
    }
    static int Compare(PackageItem r1, PackageItem r2)
    {
        return r1.PackageItemID.CompareTo(r2.PackageItemID);
    }

    //生成背包中数据
    public void CreatBagGoods()
    {
        for (int i = 0; i < BagGoodsGrid.transform.childCount; i++)
        {
            GameObject.Destroy(BagGoodsGrid.transform.GetChild(i).gameObject);
        }
        var itemList = PlayerInfoManager.Instance.playerItemData;
        itemList.Sort(Compare);

        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");

        for (int i = 0; i < 80; i++)
        {
            GameObject go = null;
            go = Instantiate(Resources.Load("Prefabs/BagGoods_Item"), Vector3.zero, Quaternion.identity) as GameObject;
            go.transform.SetParent(BagGoodsGrid.transform);
            go.transform.localScale = Vector3.one;
            UISprite sp = go.transform.GetChild(0).GetComponent<UISprite>();
            PackageItem data = null;

            if (itemList.Count > i)
            {
                data = itemList[i];
            }

            if (data != null)//如果有数据
            {
                sp.spriteName = cfgData.GetListConfigElementByID(data.PackageItemID).ItemIcon;
                //背包中物品数量
                Helper.GetChild<UILabel>(go.transform, "BagGoodsNumLabel").text = data.PackageItemNum > 1 ? data.PackageItemNum.ToString() : "";

                sp.transform.name = data.PackageItemID.ToString();
                go.name = data.PackageItemID.ToString();
            }
            else
            {
                go.transform.GetChild(0).GetComponent<UISprite>().spriteName = null;
            }
        }
        BagGoodsGrid.Reposition();
        BagGoodsGrid.repositionNow = true;
    }

    //生成卖物品的格子
    public void CreatSellGoods()
    {
        for (int i = 0; i < SellGoodsGrid.transform.childCount; i++)
        {
            GameObject.Destroy(SellGoodsGrid.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < 80; i++)
        {
            GameObject go = null;
            go = Instantiate(Resources.Load("Prefabs/BagGoods_Item"), Vector3.zero, Quaternion.identity) as GameObject;
            go.transform.SetParent(SellGoodsGrid.transform);
            go.transform.localScale = Vector3.one;
            go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "";
        }
        SellGoodsGrid.Reposition();
        SellGoodsGrid.repositionNow = true;
    }

}
