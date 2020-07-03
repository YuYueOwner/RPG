using HotFix_Project.Config;
using System.Collections;
using UnityEngine;

public class DealBagDrag : UIDragDropItem
{
    //鼠标悬停0.5s后显示物品详细信息，移开消失
    void OnHover(bool isOver)
    {
        if (isOver)
        {
            StartCoroutine(Show());
        }
        else
        {
            StopAllCoroutines();
            UIManager.Instance.SetVisible(UIPanelName.SceneStart_GoodsInfoPanel, false);
        }
    }
    protected override void Update()
    {
        base.Update();
    }

    //0.5s显示详细信息面板
    IEnumerator Show()
    {
        yield return new WaitForSeconds(0.5f);
        int thisName;
        if (int.TryParse(transform.name, out thisName) == false) yield return null;
        if (thisName > 0)
        {
            //显示背包中物品内容
            PlayerInfoManager.Instance.ShowItemInfo(int.Parse(transform.name));
            //根据鼠标点击的位置显示详细信息面板
            Vector3 worldPoint = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
            UIManager.Instance.SetVisible(UIPanelName.SceneStart_GoodsInfoPanel, true);
            if (UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition).x >= 0.9f)
            {
                if (UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition).y <= 0.1f)
                {
                    GoodsInfoPanel._instance.goBg_Sprite.transform.position = new Vector3(worldPoint.x - 0.4f, worldPoint.y + 0.4f, worldPoint.z);
                }
                else
                {
                    GoodsInfoPanel._instance.goBg_Sprite.transform.position = new Vector3(worldPoint.x - 0.4f, worldPoint.y - 0.4f, worldPoint.z);
                }
            }
            else
            {
                if (UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition).y <= 0.1f)
                {
                    GoodsInfoPanel._instance.goBg_Sprite.transform.position = new Vector3(worldPoint.x + 0.4f, worldPoint.y + 0.4f, worldPoint.z);
                }
                else
                {
                    GoodsInfoPanel._instance.goBg_Sprite.transform.position = new Vector3(worldPoint.x + 0.4f, worldPoint.y - 0.4f, worldPoint.z);
                }
            }
        }
    }
    //物品是否可以合并
    private bool isMerge = false;
    private bool isBagMerge = false;
    #region 开始拖拽
    public override void StartDragging()
    {
        base.StartDragging();
        StopAllCoroutines();
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_GoodsInfoPanel, false);
        //获取到格子中的物品数量
        UILabel BagGoodsNumLabel = Helper.GetChild<UILabel>(transform.parent, "BagGoodsNumLabel");
        int GoodsNum = int.Parse(BagGoodsNumLabel.text);
        //判断拖拽的一刻如果是空格子的话return
        if (GoodsNum <= 0)
        {
            this.transform.localPosition = Vector3.zero;
            return;
        }
        else //当前拖拽的格子里是有物品的
        {
            //让被拖拽的UI在最上层
            this.GetComponent<UISprite>().depth = 100;

            //是否可以合并
            isMerge = DealPanel._instance.JudgeSellGoodsIdExist(int.Parse(this.name));
            isBagMerge = DealPanel._instance.JudgeBagGoodsIdExist(int.Parse(this.name));
            //拖拽的一刻数量减一
            JudgeGoodsNumAddOrMinus(BagGoodsNumLabel, false);
        }
    }
    #endregion

    #region 封装判断当前物品数量 +1 还是 -1
    private void JudgeGoodsNumAddOrMinus(UILabel bagGoodsNum, bool isAdd)
    {
        //获取到格子中的物品数量
        UILabel BagGoodsNumLabel = bagGoodsNum;
        int GoodsNum = int.Parse(BagGoodsNumLabel.text);
        BagGoodsNumLabel.text = isAdd == true ? (GoodsNum + 1).ToString() : (GoodsNum - 1).ToString();
        //如果是+，判断是否 > 1, 如果大于1，则显示脚标数量，否则反之
        BagGoodsNumLabel.gameObject.SetActive(isAdd == true ? (GoodsNum + 1) > 1 : (GoodsNum - 1) > 1);
        //拖拽的时候是否显示物品背景icon
        this.transform.parent.GetChild(0).gameObject.SetActive(isAdd == true ? (GoodsNum + 1) > 0 : (GoodsNum - 1) > 0);
    }
    #endregion

    #region 封装获取当前icon名字
    private string GetIconName()
    {
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        return cfgData.GetListConfigElementByID(int.Parse(this.name)).ItemIcon;
    }
    #endregion

    //拖拽放下的一刻
    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);

        this.GetComponent<UISprite>().depth = 4;

        //代售物品区内，物品在代售物品区内进行移动，拖拽物体和碰撞物体都是代售区内的物品，拖拽无效，所以数量+1
        if ((transform.tag == "BagGoods" && surface.tag == "BagGoods"))
        {
            JudgeGoodsNumAddOrMinus(Helper.GetChild<UILabel>(transform.parent, "BagGoodsNumLabel"), true);
            transform.localPosition = Vector3.zero;
            return;
        }

        //自己碰撞自己返回
        if (transform.name == surface.name && transform.name == surface.transform.parent.GetChild(0).name)
        {
            UILabel lb = transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
            int num = int.Parse(lb.text) + 1;
            lb.text = num.ToString();
            lb.gameObject.SetActive(num > 1);
            transform.parent.GetChild(0).gameObject.SetActive(num > 0);
            transform.localPosition = Vector3.zero;
            return;
        }

        if (this.tag == "Goods")//对背包中的物品进行操作
        {
            //如果当下时撞到的是装备
            if (surface.tag == "Goods")
            {
                //先+1
                JudgeGoodsNumAddOrMinus(Helper.GetChild<UILabel>(transform.parent, "BagGoodsNumLabel"), true);
                //对换
                ChangeGoods(surface);
            }
            //如果当下时撞到的是装备
            else if (surface.tag == "BagGoods")
            {
                //如果交易区的格子物品数量<=0 或者 格子中的物品id是当前拖拽的物品id
                if (int.Parse(Helper.GetChild<UILabel>(surface.transform.parent, "BagGoodsNumLabel").text) <= 0 || this.name == surface.name)
                {
                    //获取背包中物品数量
                    int num = int.Parse(Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel").text);
                    int id = int.Parse(this.name);

                    if (isMerge)//true，有可以合并的
                    {
                        //Debug.LogError(id + "   合并   " + num);
                        DealPanel._instance.RefreshSellGoods(id, 1, false);
                    }
                    else//碰撞到的是空格子
                    {
                        DragOneGoods(surface);
                        DealPanel._instance.SetSellTotalPrice(id, 1);
                    }

                    if (num <= 0)
                    {
                        //如果背包中的数量<=0，说明没有物品了，刷新那组格子的数据
                        RefreshOneCellData();
                    }
                }
            }
            else
            {
                JudgeGoodsNumAddOrMinus(Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel"), true);
                Debug.LogError("回到原来的位置");
            }
        }
        else if (this.tag == "BagGoods")
        {
            //如果当下时撞到的是装备
            if (surface.tag == "Goods")
            {
                if (int.Parse(Helper.GetChild<UILabel>(surface.transform.parent, "BagGoodsNumLabel").text) <= 0 || this.name == surface.name)
                {
                    //获取代售区中物品数量
                    int num = int.Parse(Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel").text);
                    int id = int.Parse(this.name);
                    DealPanel._instance.RefreshSellTotalNum(id, 1);
                    DragOneGoods(surface);
                    if (num <= 0)
                    {
                        //如果数量<=0，说明没有物品了，刷新那组格子的数据
                        RefreshOneCellData();
                    }
                }
            }
            else
            {
                //回到原来的位置
                JudgeGoodsNumAddOrMinus(Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel"), true);
            }
        }
        transform.localPosition = Vector3.zero;
    }

    #region 封装交换物品
    private void ChangeGoods(GameObject goSurface)
    {
        string changeName = "";
        string changeIconName = "";
        string changeGoodsNum = "";
        string changeBackGroundName = "";
        string changeBackGroundIconName = "";
        bool changeBackSpriteStatue = false;
        bool changeGoodsNumStatue = false;
        //换当前物品的名字
        changeName = this.name;
        this.name = goSurface.name;
        goSurface.name = changeName;
        //换当前物品的icon
        changeIconName = this.GetComponent<UISprite>().spriteName;
        this.GetComponent<UISprite>().spriteName = goSurface.GetComponent<UISprite>().spriteName;
        goSurface.GetComponent<UISprite>().spriteName = changeIconName;
        //换当前物品的数量
        changeGoodsNum = Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel").text;
        Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel").text = Helper.GetChild<UILabel>(goSurface.transform.parent, "BagGoodsNumLabel").text;
        Helper.GetChild<UILabel>(goSurface.transform.parent, "BagGoodsNumLabel").text = changeGoodsNum;
        //换当前物品的Parent的GetChild(0)的名字
        changeBackGroundName = this.transform.parent.GetChild(0).name;
        this.transform.parent.GetChild(0).name = goSurface.transform.parent.GetChild(0).name;
        goSurface.transform.parent.GetChild(0).name = changeBackGroundName;
        //换当前物品的Parent的GetChild(0)的Icon
        changeBackGroundIconName = this.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName;
        this.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = goSurface.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName;
        goSurface.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = changeBackGroundIconName;
        //换当前物品的显隐状态
        changeBackSpriteStatue = this.transform.parent.GetChild(0).gameObject.activeSelf;
        this.transform.parent.GetChild(0).gameObject.SetActive(goSurface.transform.parent.GetChild(0).gameObject.activeSelf);
        goSurface.transform.parent.GetChild(0).gameObject.SetActive(changeBackSpriteStatue);
        //换当前数量的显隐状态
        changeGoodsNumStatue = Helper.GetChild(this.transform.parent, "BagGoodsNumLabel").activeSelf;
        Helper.GetChild(this.transform.parent, "BagGoodsNumLabel").SetActive(Helper.GetChild(goSurface.transform.parent, "BagGoodsNumLabel").activeSelf);
        Helper.GetChild(goSurface.transform.parent, "BagGoodsNumLabel").SetActive(changeGoodsNumStatue);
        //位置归零
        goSurface.transform.localPosition = transform.localPosition = Vector3.zero;
    }
    #endregion

    #region 封装拖拽一个物品，碰到另一个物体身上后
    private void DragOneGoods(GameObject goSurface)
    {
        //换当前物品的名字
        goSurface.name = this.name;
        //换当前物品的icon
        goSurface.GetComponent<UISprite>().spriteName = this.GetComponent<UISprite>().spriteName;
        //换当前物品的数量并判断是否显示脚标和背景图片
        JudgeGoodsNumAddOrMinus(Helper.GetChild<UILabel>(goSurface.transform.parent, "BagGoodsNumLabel"), true);
        //换当前物品的Parent的GetChild(0)的名字
        goSurface.transform.parent.GetChild(0).name = this.transform.parent.GetChild(0).name;
        //换当前物品的Parent的GetChild(0)的Icon
        goSurface.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = this.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName;
    }
    #endregion

    #region 还原一组格子的数据
    private void RefreshOneCellData()
    {
        this.name = "GoodsSprite1";
        this.GetComponent<UISprite>().spriteName = "-1";
        Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel").text = "0";
        Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel").gameObject.SetActive(false);
        this.transform.parent.GetChild(0).name = "GoodsSprite";
        this.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = "-1";
        this.transform.parent.GetChild(0).gameObject.SetActive(true);
    }
    #endregion
}
