using System.Collections;
using UnityEngine;

public class BagDragSkiil : UIDragDropItem
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
            //换成显示技能内容的
            PlayerInfoManager.Instance.ShowSkillItemInfo(int.Parse(transform.name));

            //根据鼠标点击的位置显示详细信息面板
            Vector3 worldPoint = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
            UIManager.Instance.SetVisible(UIPanelName.SceneStart_GoodsInfoPanel, true);
            if (UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition).x >= 1f)
            {
                if (UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition).y >= 0.6f)
                {
                    GoodsInfoPanel._instance.goBg_Sprite.transform.position = new Vector3(worldPoint.x - 0.5f, worldPoint.y - 0.5f, worldPoint.z);
                }
                else
                {
                    GoodsInfoPanel._instance.goBg_Sprite.transform.position = new Vector3(worldPoint.x - 0.5f, worldPoint.y + 0.5f, worldPoint.z);
                }
            }
            else
            {
                if (UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition).y >= 0.6f)
                {
                    GoodsInfoPanel._instance.goBg_Sprite.transform.position = new Vector3(worldPoint.x + 0.5f, worldPoint.y - 0.5f, worldPoint.z);
                }
                else
                {
                    GoodsInfoPanel._instance.goBg_Sprite.transform.position = new Vector3(worldPoint.x + 0.5f, worldPoint.y + 0.5f, worldPoint.z);
                }
            }
        }
    }

    Transform parent;
    private BoxCollider recordParentCollider;
    //通过重写的鼠标监听事件(开始拖动)
    public override void StartDragging()
    {
        StopAllCoroutines();
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_GoodsInfoPanel, false);
        parent = this.transform.parent;
        base.StartDragging();
        this.GetComponent<UISprite>().depth = 11;
        recordParentCollider = this.transform.parent.GetComponent<BoxCollider>();
    }


    /// <summary>
    /// 重写父类里的拖拽方法
    /// </summary>
    /// <param name="surface"></param>
    ///  protected   访问仅限于包含类或从包含类派生的类型
    protected override void OnDragDropRelease(GameObject surface)
    {
        //技能分为四个类型  上方解锁技能的父物体 OpenLockHasValueParent 上方已解锁并且有技能 OpenLockHasValue  上方已解锁没有技能 OpenLockNotValue 上方未解锁 NotOpen  技能库 OwnSkill
        base.OnDragDropRelease(surface);
        //this.GetComponent<UISprite>().depth = 11;
        //如果拖拽的上方未解锁的或者已解锁没有技能的不做任何改变
        if (this.tag == "NotOpen" || this.tag == "OpenLockNotValue" || surface.tag == "NotOpen")
        {
            this.transform.localPosition = Vector3.zero;
            return;
        }
        //如果拖拽的是上方的技能
        if (this.tag == "OpenLockHasValue")
        {
            //如果是跟上方的格子互换
            if (surface.tag == "OpenLockHasValue" || surface.tag == "OpenLockNotValue")
            {
                //装备技能的只能在上方交换
                Transform Parent = null;
                //开始交换  
                Parent = this.transform.parent;         //把撞到的(surface)技能的父物体取出来
                this.transform.parent = surface.transform.parent;   //把撞到的物体移动过来(把自己的父物体给surface)
                surface.transform.parent = Parent;                      //自己移动到想被交换的位置
                                                                        //交换完成 位移归零 （交换时是位移的改变 缩放没有变）
                surface.transform.localPosition = transform.localPosition = Vector3.zero;
                //Debug.LogError("000000" + this.tag + "   " + surface.tag);
                return;
            }
            else
            {
                //丢弃技能
                transform.localPosition = Vector3.zero;
                PlayerInfoManager.Instance.SelectSkillId = this.name;
                DiscardGoodsPanel._instance.SetType(this.parent.parent.name);
                UIManager.Instance.SetVisible(UIPanelName.SceneStart_DiscardGoodsPanel, true);
                //Debug.LogError("22222" + this.tag + "   " + surface.tag);
            }
        }
        else//拖拽的是下方的技能
        {
            //OwnSkill 类型的技能只能给 OpenLockHasValue 和 OpenLockNotValue 类型
            if (surface.tag == "OpenLockHasValue" || surface.tag == "OpenLockNotValue")
            {
                //Debug.LogError("" + this.parent.name);
                if (GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().CheckSkillIsCanUse(int.Parse(this.name), int.Parse(surface.transform.parent.name)))
                {
                    //判断要装备的技能是否已经存在，存在的话给替换掉
                    if (this.parent.parent.name == "attackGrid")
                    {
                        SkillAttackPanel._instance.RevomeRepetitionSkill(this.name);
                    }
                    else if (this.parent.parent.name == "defGrid")
                    {
                        SkillDefendPanel._instance.RevomeRepetitionSkill(this.name);
                    }
                    //替换ID
                    surface.transform.name = this.transform.name;
                    //替换icon
                    surface.GetComponent<UISprite>().spriteName = this.transform.GetComponent<UISprite>().spriteName;
                    surface.tag = "OpenLockHasValue";
                    this.tag = "OwnSkill";
                    //Debug.LogError("444444" + this.tag + "   " + surface.tag);
                }
                else
                {
                    UIManager.Instance.SetVisible(UIPanelName.SceneStart_EquipmentSkillPanel, true);
                }
            }
            this.transform.localPosition = Vector3.zero;
        }
    }
}
