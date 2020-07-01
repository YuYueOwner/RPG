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

    private bool isMerge = false;
    public override void StartDragging()
    {
        base.StartDragging();
        //判断是否是空格子，是的话return
        int thisName;
        if (int.TryParse(transform.parent.GetChild(1).name, out thisName) == false)
        {
            this.transform.localPosition = Vector3.zero;
            return;
        }

        StopAllCoroutines();
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_GoodsInfoPanel, false);

        if (this.tag == "Goods")
        {
            isMerge = DealPanel._instance.JudgeSellGoodsIdExist(int.Parse(this.name));
            this.GetComponent<UISprite>().depth = 100;

            UILabel lb_num = Helper.GetChild<UILabel>(this.transform.parent, "BagGoodsNumLabel");
            int num = 0;
            //如果当前拖拽的格子里是有物品的
            if (int.TryParse(lb_num.text, out num) == true)
            {
                //lb_num.text = (num - 1 < 0 ? 0 : num - 1).ToString();
                lb_num.text = (num - 1).ToString();
                lb_num.gameObject.SetActive((num - 1) > 1);
                //如果当前物品是一件的话，拖拽的时候隐藏背包中的图标
                this.transform.parent.GetChild(0).gameObject.SetActive(int.Parse(lb_num.text) > 0);
            }
        }
    }

    /// <summary>
    /// 重写父类里的拖拽方法
    /// </summary>
    /// <param name="surface"></param>
    ///  protected   访问仅限于包含类或从包含类派生的类型
    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        if ((this.tag == "BagCell" || this.tag == "BagGoods") && (surface.tag == "BagCell" || surface.tag == "BagGoods"))
        {
            this.transform.localPosition = Vector3.zero;
            return;
        }

        //判断是否是空格子，是的话return
        int thisName;
        if (int.TryParse(transform.parent.GetChild(1).name, out thisName) == false)
        {
            this.transform.localPosition = Vector3.zero;
            return;
        }

        this.GetComponent<UISprite>().depth = 4;
        if (this.tag == "Goods")
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
                int name;
                if (int.TryParse(surface.transform.parent.GetChild(1).name, out name) == false)
                {
                    int id = int.Parse(this.name);
                    PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
                    UILabel lb = this.transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();

                    string icon = cfgData.GetListConfigElementByID(int.Parse(this.name)).ItemIcon;
                    surface.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = icon;
                    UISprite spSurface = surface.transform.parent.GetChild(1).GetComponent<UISprite>();
                    spSurface.spriteName = icon;
                    spSurface.name = id.ToString();
                    UILabel lbSurface = surface.transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
                    int sum = int.Parse(lb.text) + 1;
                    lbSurface.text = sum.ToString();

                    this.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = "-1";
                    UISprite sp = this.transform.parent.GetChild(1).GetComponent<UISprite>();
                    sp.spriteName = "-1";
                    sp.name = "GoodsSprite1";
                    lb.text = "0";
                    lb.gameObject.SetActive(false);
                    lbSurface.gameObject.SetActive(sum > 1);
                    lbSurface.parent.gameObject.SetActive(sum > 0);

                    surface.name = id.ToString();
                    transform.name = "BagGoods_Item(Clone)";

                    surface.transform.localPosition = transform.localPosition = Vector3.one;
                }
                else
                {
                    UILabel lb = transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
                    UILabel lb1 = surface.transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
                    int num = int.Parse(lb.text) + 1;
                    int num1 = int.Parse(lb1.text);
                    lb.text = num1.ToString();
                    lb.gameObject.SetActive(num1 > 1);
                    lb.parent.gameObject.SetActive(num1 > 0);
                    lb1.text = num.ToString();
                    lb1.gameObject.SetActive(num > 1);
                    lb1.parent.gameObject.SetActive(num > 0);

                    Transform Parent = null;
                    //开始交换  
                    Parent = this.transform.parent;         //把撞到的(surface)装备的父物体取出来
                    this.transform.parent = surface.transform.parent;   //把撞到的物体移动过来(把自己的父物体给surface)
                    surface.transform.parent = Parent;                      //自己移动到想被交换的位置
                                                                            //交换完成 位移归零 （交换时是位移的改变 缩放没有变）
                    surface.transform.localPosition = transform.localPosition = Vector3.zero;
                }
            }
            //如果放下时撞到的物品是空格子
            else if (surface.tag == "BagCell")
            {
                this.tag = "BagGoods";
                surface.tag = "Goods";
                //物品交换 （通过改变父物体来转移位置）
                this.transform.parent = surface.transform;
                //位置归零
                this.transform.localPosition = Vector3.zero;
                this.transform.parent.GetComponent<BoxCollider>().enabled = false;
                Debug.LogError("BagCell");
            }
            //如果当下时撞到的是装备
            else if (surface.tag == "BagGoods")
            {
                int surfaceNum;
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
                    }

                    //交换完成 位移归零 （交换时是位移的改变 缩放没有变）
                    surface.transform.localPosition = transform.localPosition = Vector3.zero;
                    DealPanel._instance.SetSellTotalPrice(id, 1);
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
                UISprite sp = this.GetComponent<UISprite>();
                UILabel lb = this.transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
                string icon = sp.spriteName;
                int num = int.Parse(lb.text);


                lb.gameObject.SetActive((num - 1) > 1);
                if ((num - 1) > 0)
                {

                }
                else
                {
                    this.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = "-1";
                    sp.spriteName = "-1";
                    sp.name = "GoodsSprite1";
                }
                lb.text = (num - 1) + "";
                surface.GetComponent<UISprite>().spriteName = icon;
                surface.transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = icon;
                UILabel lb1 = surface.transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
                lb1.text = int.Parse(lb1.text) + 1 + "";
                lb1.gameObject.SetActive(num > 1);
                surface.name = this.name;
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
}
