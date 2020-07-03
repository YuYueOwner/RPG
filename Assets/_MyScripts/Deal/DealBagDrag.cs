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

        if ((transform.tag == "BagCell" && transform.tag == "BagGoods") || (surface.tag == "BagCell" && surface.tag == "BagGoods"))
        {
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

        if (this.tag == "Goods")
        {
            //如果当下时撞到的是装备
            if (surface.tag == "Goods")
            {
                //先+1
                JudgeGoodsNumAddOrMinus(Helper.GetChild<UILabel>(transform.parent, "BagGoodsNumLabel"), true);
                //对换
                ChangeGoods(surface);
                #region 废弃
                //int name;
                //true 是和物品交换 false是和空格子交换
                //if (int.TryParse(surface.transform.parent.GetChild(1).name, out name) == false)
                //{
                //    int id = int.Parse(this.name);
                //    string icon = GetIconName();

                //    UILabel lb = transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
                //    UISprite spSurface1 = surface.transform.parent.GetChild(0).GetComponent<UISprite>();
                //    spSurface1.spriteName = icon;
                //    UISprite spSurface = surface.transform.parent.GetChild(1).GetComponent<UISprite>();
                //    spSurface.spriteName = icon;
                //    spSurface.name = id.ToString();
                //    spSurface1.name = id.ToString();
                //    UILabel lbSurface = surface.transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
                //    int sum = int.Parse(lb.text) + 1;
                //    lbSurface.text = sum.ToString();

                //    UISprite sp1 = this.transform.parent.GetChild(0).GetComponent<UISprite>();
                //    sp1.spriteName = "-1";
                //    UISprite sp = this.transform.parent.GetChild(1).GetComponent<UISprite>();
                //    sp.spriteName = "-1";
                //    sp.name = "GoodsSprite1";
                //    sp1.name = "GoodsSprite1";
                //    lb.text = "0";
                //    lb.gameObject.SetActive(false);
                //    lbSurface.gameObject.SetActive(sum > 1);
                //    lbSurface.parent.gameObject.SetActive(sum > 0);

                //    surface.name = id.ToString();
                //    transform.name = "BagGoods_Item(Clone)";

                //    surface.transform.localPosition = transform.localPosition = Vector3.one;
                //    //Debug.LogError("Goods");
                //}
                //else
                //{
                //    UILabel lb = transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
                //    UILabel lb1 = surface.transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();

                //    int num = int.Parse(lb.text) + 1;
                //    int num1 = int.Parse(lb1.text);
                //    lb.text = num1.ToString();
                //    lb.gameObject.SetActive(num1 > 1);
                //    lb.parent.gameObject.SetActive(num1 > 0);
                //    lb1.text = num.ToString();
                //    lb1.gameObject.SetActive(num > 1);
                //    lb1.parent.gameObject.SetActive(num > 0);

                //    Transform Parent = null;
                //    //开始交换  
                //    Parent = this.transform.parent;         //把撞到的(surface)装备的父物体取出来
                //    this.transform.parent = surface.transform.parent;   //把撞到的物体移动过来(把自己的父物体给surface)
                //    surface.transform.parent = Parent;                      //自己移动到想被交换的位置
                //                                                            //交换完成 位移归零 （交换时是位移的改变 缩放没有变）
                //    surface.transform.localPosition = transform.localPosition = Vector3.zero;
                //    //Debug.LogError("Goods111");
                //    UISprite sp = transform.transform.parent.GetChild(0).GetComponent<UISprite>();
                //    UISprite sp2 = transform.transform.parent.GetChild(1).GetComponent<UISprite>();
                //    sp.spriteName = sp2.spriteName;
                //    sp.name = sp2.name;
                //    UISprite sp1 = surface.transform.parent.GetChild(0).GetComponent<UISprite>();
                //    UISprite sp4 = surface.transform.parent.GetChild(1).GetComponent<UISprite>();
                //    sp1.spriteName = sp4.spriteName;
                //    sp1.name = sp4.name;
                //}
                #endregion
            }

            #region 废弃
            //如果放下时撞到的物品是空格子
            //else if (surface.tag == "BagCell")
            //{
            //    this.tag = "BagGoods";
            //    surface.tag = "Goods";
            //    //物品交换 （通过改变父物体来转移位置）
            //    this.transform.parent = surface.transform;
            //    //位置归零
            //    this.transform.localPosition = Vector3.zero;
            //    this.transform.parent.GetComponent<BoxCollider>().enabled = false;
            //    Debug.LogError("BagCell");
            //}
            #endregion

            //如果当下时撞到的是装备
            else if (surface.tag == "BagGoods")
            {
                if (int.Parse(surface.transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>().text) <= 0 || this.name == surface.name)
                {
                    // 碰撞到的格子没有物品
                    int num = int.Parse(Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel").text);

                    int id = 0;
                    id = int.Parse(this.name);
                    if (isMerge)
                    {
                        if (num <= 0)
                        {
                            this.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = "-1";
                            UISprite sp = this.transform.parent.GetChild(1).GetComponent<UISprite>();
                            sp.name = "GoodsSprite1";
                            sp.spriteName = "-1";
                        }
                        //Debug.LogError(id + "   合并   " + num);
                        DealPanel._instance.RefreshSellGoods(id, 1, false);
                    }
                    else
                    {
                        this.tag = "BagGoods";
                        surface.tag = "Goods";
                        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");

                        string icon = cfgData.GetListConfigElementByID(int.Parse(this.name)).ItemIcon;
                        surface.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = icon;
                        surface.GetComponent<UISprite>().spriteName = icon;
                        UILabel lb = Helper.GetChild<UILabel>(surface.transform.parent, "BagGoodsNumLabel");
                        lb.text = "1";
                        lb.gameObject.SetActive(false);

                        if (num <= 0)
                        {
                            //Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel").gameObject.SetActive(false);
                            this.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = "-1";
                            UISprite sp = surface.transform.parent.GetChild(1).GetComponent<UISprite>();
                            sp.name = "GoodsSprite1";
                            sp.spriteName = "-1";
                        }

                        Transform Parent = null;
                        //开始交换  
                        Parent = this.transform.parent;         //把撞到的(surface)装备的父物体取出来
                        this.transform.parent = surface.transform.parent;   //把撞到的物体移动过来(把自己的父物体给surface)
                        surface.transform.parent = Parent;
                        //自己移动到想被交换的位置
                        //Debug.LogError("交换");
                        surface.name = this.name;
                        DealPanel._instance.SetSellTotalPrice(id, 1);
                    }

                    //交换完成 位移归零 （交换时是位移的改变 缩放没有变）
                    surface.transform.localPosition = transform.localPosition = Vector3.zero;
                }
                else
                {
                    transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                //回到原来的位置
                transform.localPosition = Vector3.zero;
                //物品数量加回去
                UILabel lb_num = Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel");
                int sum = int.Parse(lb_num.text) + 1;
                lb_num.text = sum.ToString();
                lb_num.gameObject.SetActive(sum > 1);
                Debug.LogError("回到原来的位置");
            }
        }
        else if (this.tag == "BagGoods")
        {
            if (surface.tag == "Cell")
            {
                //物品交换 （通过改变父物体来转移位置）
                this.transform.parent = surface.transform;
                //位置归零
                this.transform.localPosition = Vector3.zero;
                this.transform.parent.GetComponent<BoxCollider>().enabled = false;
            }
            //如果当下时撞到的是装备
            else if (surface.tag == "Goods")
            {
                if (int.Parse(surface.transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>().text) <= 0 || this.name == surface.name)
                {
                    int id = 0;
                    //判断点击的如果是空格子 return
                    if (!int.TryParse(this.name, out id)) return;
                    id = int.Parse(this.name);

                    DealPanel._instance.RefreshSellTotalNum(id, 1);

                    surface.name = this.name;
                    UISprite sp = this.GetComponent<UISprite>();
                    UILabel lb = this.transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
                    string icon = sp.spriteName;
                    int num = int.Parse(lb.text);
                    //lb.gameObject.SetActive((num - 1) > 1);
                    if (num > 0)
                    {

                    }
                    else
                    {
                        this.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = "-1";
                        sp.spriteName = "-1";
                        sp.name = "GoodsSprite1";
                    }
                    //lb.text = (num - 1) + "";
                    surface.GetComponent<UISprite>().spriteName = icon;
                    surface.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = icon;
                    UILabel lb1 = surface.transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
                    lb1.text = int.Parse(lb1.text) + 1 + "";
                    lb1.gameObject.SetActive(int.Parse(lb1.text) > 1);
                    lb1.transform.parent.transform.GetChild(0).gameObject.SetActive(int.Parse(lb1.text) > 1);
                }
                transform.localPosition = Vector3.zero;
            }
            else
            {
                //回到原来的位置
                transform.localPosition = Vector3.zero;
                //物品数量加回去
                UILabel lb_num = Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel");
                int sum = int.Parse(lb_num.text) + 1;
                lb_num.text = sum.ToString();
                lb_num.gameObject.SetActive(sum > 1);
            }
        }
        else
        {
            //回到原来的位置
            transform.localPosition = Vector3.zero;
            //物品数量加回去
            UILabel lb_num = Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel");
            int sum = int.Parse(lb_num.text) + 1;
            lb_num.text = sum.ToString();
            lb_num.gameObject.SetActive(sum > 1);
        }
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
}
