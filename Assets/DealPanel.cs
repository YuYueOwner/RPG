using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealPanel : UIScene
{
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
    }

    private void Sure()
    {

    }

    private void Back()
    {
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_DealPanel, false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_OpenBagPanel, true);

    }
}
