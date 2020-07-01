using HotFix_Project.Config;
using System.Collections.Generic;
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

    private int buyNum = 0;

    private List<MerchantGoodsConfig.MerchantGoodsObject> merchantGoodsList;

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
        SellTotalNumLabel.text = "0";
    }

    //生成左侧商人物品区中的物品
    private void CreatMerchantGoods()
    {
        //测试数据
        string npcType = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().NpcType.ToString();
        int PlayerMoney = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().PlayerMoney;
        MerchantGoodsConfig cfgData = DataTableManager.Instance.GetConfig<MerchantGoodsConfig>("MerchantGoods");

        merchantGoodsList = cfgData.GetListConfigElementByType(npcType);

        PropConfig cfgPropData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        for (int i = 0; i < merchantGoodsList.Count; i++)
        {
            MerchantGoodsConfig.MerchantGoodsObject data = merchantGoodsList[i];
            PropConfig.PropObject propData = cfgPropData.GetListConfigElementByID(data.ItemID);
            if (data.ItemNum > 0)
            {
                GameObject goMerchant = Instantiate(Resources.Load("Prefabs/Merchant_Item"), Vector3.zero, Quaternion.identity) as GameObject;
                goMerchant.name = data.ItemID.ToString();
                goMerchant.transform.SetParent(MerchantGrid.transform);
                goMerchant.transform.localPosition = Vector3.zero;
                goMerchant.transform.localScale = Vector3.one;

                if (data != null)
                {
                    //给物品Icon赋值
                    Helper.GetChild<UISprite>(goMerchant.transform, "GoodsSprite").spriteName = propData.ItemIcon;
                    //给物品名字赋值
                    Helper.GetChild<UILabel>(goMerchant.transform, "BagNameLabel").text = propData.ItemName;
                    //给物品金额赋值
                    Helper.GetChild<UILabel>(goMerchant.transform, "GoldNumLabel").text = data.SellPrice.ToString();
                    //物品数量
                    Helper.GetChild<UILabel>(goMerchant.transform, "GoodsNumLabel").text = data.ItemNum.ToString();
                    //如果物品数量是1，隐藏
                    Helper.GetChild(goMerchant.transform, "GoodsNumLabel").SetActive(data.ItemNum != 1);
                }


                //如果当前物品金额小于拥有元宝总额，字体变红
                if (int.Parse(Helper.GetChild<UILabel>(goMerchant.transform, "GoldNumLabel").text) > PlayerMoney)
                {
                    Helper.GetChild(goMerchant.transform, "InsufficientGold_Sprite").SetActive(true);
                    Helper.GetChild<UILabel>(goMerchant.transform, "BagNameLabel").color = Color.red;
                    Helper.GetChild<UILabel>(goMerchant.transform, "GoldNumLabel").color = Color.red;
                }
            }
        }
        MerchantGrid.repositionNow = true;
        MerchantGrid.Reposition();

        MerchantHeadPhotoSprite.spriteName = "HeadPhoto" + GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().NpcImageID;
        MerchantNameLabel.text = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().NpcName.ToString();
        RefreshPlayerMoney();
    }

    private void Sure()
    {
        MerchantGoodsConfig cfgData = DataTableManager.Instance.GetConfig<MerchantGoodsConfig>("MerchantGoods");
        int sum = 0;
        string npcType = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().NpcType.ToString();
        for (int i = 0; i < SellGoodsGrid.transform.childCount; i++)
        {
            Transform trans = SellGoodsGrid.transform.GetChild(i);
            int id;
            if (int.TryParse(trans.GetChild(1).name, out id) == true)
            {
                int num = int.Parse(trans.GetChild(0).GetChild(0).GetComponent<UILabel>().text);
                var data = cfgData.GetListConfigElementByID(npcType, id);
                sum += num * data.SellPrice;
                //Debug.LogError("总价格为:" + sum);
            }
        }
        int PlayerMoney = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().PlayerMoney;
        int price = PlayerMoney + sum;
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SetPlayerMoney(price);
        RefreshPlayerMoney();

        CreatSellGoods();
    }

    private void Back()
    {
        AudioManager.Instance.PlaySound(1);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_QuitDealPanel, true);
    }

    //还原物品选中状态
    public void RevertMerchantItemSelectState()
    {
        for (int i = 0; i < MerchantGrid.transform.childCount; i++)
        {
            Helper.GetChild(MerchantGrid.transform.GetChild(i), "SelectFrame").SetActive(false);
        }
    }


    //确定购买 刷新数据
    public void OnRefreshBuyData(int num)
    {
        //buyNum = num;
        int selectDealItemID = PlayerInfoManager.Instance.selectDealItemID;

        for (int i = 0; i < merchantGoodsList.Count; i++)
        {
            if (selectDealItemID == merchantGoodsList[i].ItemID)
            {
                var data = merchantGoodsList[i];
                int PlayerMoney = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().PlayerMoney;
                int price = PlayerMoney - num * data.SellPrice;
                GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SetPlayerMoney(price);
                //Debug.LogError("要购买物品的数量:" + num + "     price" + price);
                PlayerInfoManager.Instance.AddPlayerItemData(data.ItemID, num);
            }
        }
        RefeshMerchantGridRedMask(selectDealItemID, num);
    }

    //买完物品后刷新，判断当前元宝数是否买得起商人物品，是否显示红色遮罩和红色字体
    public void RefeshMerchantGridRedMask(int selectDealItemID, int num)
    {
        int PlayerMoney = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().PlayerMoney;
        for (int i = 0; i < MerchantGrid.transform.childCount; i++)
        {
            //买不起
            if (int.Parse(Helper.GetChild<UILabel>(MerchantGrid.GetChild(i), "GoldNumLabel").text) > PlayerMoney)
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
            if (MerchantGrid.GetChild(i).name == selectDealItemID.ToString())
            {
                UILabel label = Helper.GetChild<UILabel>(MerchantGrid.GetChild(i), "BagBg_Sprite/GoodsSprite/GoodsNumLabel");
                label.text = (int.Parse(label.text) - num).ToString();
                if (int.Parse(label.text) == 0)
                {
                    GameObject.Destroy(MerchantGrid.GetChild(i).gameObject);
                }

                if (int.Parse(label.text) == 1)
                {
                    Helper.GetChild(MerchantGrid.GetChild(i), "GoodsNumLabel").SetActive(false);
                }

            }
        }
        MerchantGrid.repositionNow = true;
        MerchantGrid.Reposition();

        RefreshPlayerMoney();

        //CreatBagGoods();
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        for (int i = 0; i < BagGoodsGrid.transform.childCount; i++)
        {
            Transform trans = BagGoodsGrid.transform.GetChild(i);
            UILabel lb = Helper.GetChild<UILabel>(trans, "BagGoodsNumLabel");
            if (trans.GetChild(1).name == selectDealItemID.ToString())
            {
                lb.text = (int.Parse(lb.text) + num).ToString();
                lb.gameObject.SetActive(true);
                trans.GetChild(0).gameObject.SetActive(true);
                return;
            }
        }


        for (int i = 0; i < BagGoodsGrid.transform.childCount; i++)
        {
            Transform trans = BagGoodsGrid.transform.GetChild(i);

            UILabel lb = Helper.GetChild<UILabel>(trans, "BagGoodsNumLabel");
            if (int.Parse(lb.text) <= 0)
            {
                lb.text = num.ToString();
                trans.GetChild(0).GetComponent<UISprite>().spriteName = cfgData.GetListConfigElementByID(selectDealItemID).ItemIcon;
                UISprite sp = trans.GetChild(1).GetComponent<UISprite>();
                sp.spriteName = cfgData.GetListConfigElementByID(selectDealItemID).ItemIcon;
                sp.name = selectDealItemID.ToString();
                return;
            }
        }

    }

    public void RefreshPlayerMoney()
    {
        OwnGoldNumLabel.text = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().PlayerMoney.ToString();
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
            go.tag = "Cell";
            go.transform.SetParent(BagGoodsGrid.transform);
            go.transform.localScale = Vector3.one;
            UISprite sp = go.transform.GetChild(0).GetComponent<UISprite>();
            UISprite sp1 = go.transform.GetChild(1).GetComponent<UISprite>();
            sp1.tag = "Goods";
            PackageItem data = null;

            UILabel lb_num = Helper.GetChild<UILabel>(go.transform, "BagGoodsNumLabel");

            if (itemList.Count > i)
            {
                data = itemList[i];
            }

            int itemNum = 0;
            if (data != null)//如果有数据
            {
                sp.spriteName = cfgData.GetListConfigElementByID(data.PackageItemID).ItemIcon;
                sp1.spriteName = cfgData.GetListConfigElementByID(data.PackageItemID).ItemIcon;

                //背包中物品数量
                itemNum = data.PackageItemNum > 1 ? data.PackageItemNum : 1;
                lb_num.text = itemNum.ToString();
                //Debug.LogError(data.PackageItemName + "     " + data.PackageItemNum + "    " + itemNum);
                sp.transform.name = data.PackageItemID.ToString();
                sp1.transform.name = data.PackageItemID.ToString();
                go.name = data.PackageItemID.ToString();
                lb_num.gameObject.SetActive(true);
            }
            else
            {
                lb_num.text = "0";
                sp.spriteName = "-1";
                sp1.spriteName = "-1";
                lb_num.gameObject.SetActive(false);
            }
            lb_num.gameObject.SetActive(itemNum > 1);
        }
        BagGoodsGrid.repositionNow = true;
        BagGoodsGrid.Reposition();
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
            go.tag = "BagCell";

            go.transform.SetParent(SellGoodsGrid.transform);
            go.transform.localScale = Vector3.one;
            go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "-1";
            go.transform.GetChild(1).GetComponent<UISprite>().spriteName = "-1";
            UILabel lb = go.transform.GetChild(0).GetChild(0).GetComponent<UILabel>();
            lb.text = "0";
            lb.gameObject.SetActive(false);
            go.transform.GetChild(1).GetComponent<UISprite>().tag = "BagGoods";
        }
        SellGoodsGrid.Reposition();
        SellGoodsGrid.repositionNow = true;
    }

    //判断这个id是否存在。存在并且可合并返回true
    public bool JudgeSellGoodsIdExist(int id)
    {
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        bool isMerge = cfgData.ExistIsCanOverlayByID(id);
        if (isMerge)
        {
            for (int i = 0; i < SellGoodsGrid.transform.childCount; i++)
            {
                Transform trans = SellGoodsGrid.transform.GetChild(i);
                if (trans.GetChild(1).name == id.ToString())
                {
                    return true;
                }
            }
        }
        return false;
    }

    //ctrl + 鼠标左键把物品从包裹中移动到待售物品区  isCtrl true代表是Ctrl + 鼠标左键
    public void RefreshBagGoods(int id, int num)
    {
        MerchantGoodsConfig merCfgData = DataTableManager.Instance.GetConfig<MerchantGoodsConfig>("MerchantGoods");
        string npcType = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().NpcType.ToString();
        int sumPrice = int.Parse(SellTotalNumLabel.text);
        var data = merCfgData.GetListConfigElementByID(npcType, id);
        sumPrice -= data.SellPrice * num;
        SellTotalNumLabel.text = sumPrice + "";

        //Debug.LogError(id + "      " + num + "    " + sumPrice + "   " + data.SellPrice);

        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        for (int j = 0; j < BagGoodsGrid.transform.childCount; j++)
        {
            Transform trans = BagGoodsGrid.transform.GetChild(j);
            int name;
            if (int.TryParse(trans.GetChild(1).name, out name) == false)
            {
                string icon = cfgData.GetListConfigElementByID(id).ItemIcon;
                trans.GetChild(0).GetComponent<UISprite>().spriteName = icon;
                UISprite sp = trans.GetChild(1).GetComponent<UISprite>();
                sp.spriteName = icon;
                sp.name = id.ToString();
                UILabel lb = trans.GetChild(0).GetChild(0).GetComponent<UILabel>();
                lb.text = num.ToString();
                lb.transform.parent.gameObject.SetActive(num > 1);
                lb.gameObject.SetActive(num > 1);
                return;
            }
        }
    }


    public void SetSellTotalPrice(int id, int num)
    {
        MerchantGoodsConfig merCfgData = DataTableManager.Instance.GetConfig<MerchantGoodsConfig>("MerchantGoods");
        string npcType = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().NpcType.ToString();
        int sumPrice = int.Parse(SellTotalNumLabel.text);
        var data = merCfgData.GetListConfigElementByID(npcType, id);
        sumPrice += data.SellPrice * num;
        SellTotalNumLabel.text = sumPrice + "";
        //Debug.LogError(id + "      " + num + "    " + sumPrice + "   " + data.SellPrice);
    }

    //ctrl + 鼠标左键把物品从包裹中移动到待售物品区  isCtrl true代表是Ctrl + 鼠标左键。不走合并
    public void RefreshSellGoods(int id, int num, bool isCtrl)
    {
        SetSellTotalPrice(id, num);
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        bool isMerge = cfgData.ExistIsCanOverlayByID(id);
        for (int i = 0; i < SellGoodsGrid.transform.childCount; i++)
        {
            Transform trans = SellGoodsGrid.transform.GetChild(i);
            int name;
            //先判断是否是可合并的物品
            if (isMerge && !isCtrl)
            {
                //可合并
                if (int.TryParse(trans.GetChild(1).name, out name) == true && name == id)
                {
                    UILabel lb_num = trans.GetChild(0).GetChild(0).GetComponent<UILabel>();
                    //Debug.LogError(i + "     lb_num.text   " + lb_num.text + "    " + num);
                    int sum = int.Parse(lb_num.text) + num;
                    lb_num.text = (sum).ToString();
                    lb_num.gameObject.SetActive(sum > 1);
                    return;
                }
            }
            else
            {
                if (int.TryParse(trans.GetChild(1).name, out name) == false)
                {
                    string icon = cfgData.GetListConfigElementByID(id).ItemIcon;
                    trans.GetChild(0).GetComponent<UISprite>().spriteName = icon;
                    UISprite sp = trans.GetChild(1).GetComponent<UISprite>();
                    sp.spriteName = icon;
                    sp.name = id.ToString();
                    UILabel lb = trans.GetChild(0).GetChild(0).GetComponent<UILabel>();
                    lb.text = num.ToString();
                    lb.gameObject.SetActive(num > 1);
                    return;
                }
            }
        }

        for (int j = 0; j < SellGoodsGrid.transform.childCount; j++)
        {
            Transform trans = SellGoodsGrid.transform.GetChild(j);
            int name;
            if (int.TryParse(trans.GetChild(1).name, out name) == false)
            {
                string icon = cfgData.GetListConfigElementByID(id).ItemIcon;
                trans.GetChild(0).GetComponent<UISprite>().spriteName = icon;
                UISprite sp = trans.GetChild(1).GetComponent<UISprite>();
                sp.spriteName = icon;
                sp.name = id.ToString();
                UILabel lb = trans.GetChild(0).GetChild(0).GetComponent<UILabel>();
                lb.text = num.ToString();
                lb.gameObject.SetActive(num > 1);
                return;
            }
        }
    }

}
