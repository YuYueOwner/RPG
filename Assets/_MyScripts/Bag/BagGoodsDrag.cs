using System.Collections;
using UnityEngine;

public class BagGoodsDrag : UIDragDropItem
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

    void OnClick()
    {
        AudioManager.Instance.PlaySound(1);

        if (UICamera.currentTouchID == -2)
        {

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
        int parentName;
        if (int.TryParse(transform.parent.name, out parentName) == false) yield return null;
        //UIManager.Instance.SetVisible(UIPanelName.SceneStart_GoodsInfoPanel, true);
    }


    private BoxCollider recordParentCollider;
    //通过重写的鼠标监听事件(开始拖动)
    public override void StartDragging()
    {
        base.StartDragging();
        this.GetComponent<UISprite>().depth = 10;
        recordParentCollider = this.transform.parent.GetComponent<BoxCollider>();
        Debug.LogError(this.transform.parent.name);
    }


    /// <summary>
    /// 重写父类里的拖拽方法
    /// </summary>
    /// <param name="surface"></param>
    ///  protected   访问仅限于包含类或从包含类派生的类型
    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        this.GetComponent<UISprite>().depth = 4;
        //如果放下时撞到的物品是空格子
        if (surface.tag == "Cell")
        {
            recordParentCollider.enabled = true;
            //物品交换 （通过改变父物体来转移位置）
            this.transform.parent = surface.transform;
            //位置归零
            this.transform.localPosition = Vector3.zero;
            this.transform.parent.GetComponent<BoxCollider>().enabled = false;
        }
        //如果当下时撞到的是装备
        else if (surface.tag == "Goods")
        {
            Transform Parent = null;
            //开始交换  
            Parent = this.transform.parent;         //把撞到的(surface)装备的父物体取出来
            this.transform.parent = surface.transform.parent;   //把撞到的物体移动过来(把自己的父物体给surface)
            surface.transform.parent = Parent;                      //自己移动到想被交换的位置
            //交换完成 位移归零 （交换时是位移的改变 缩放没有变）
            surface.transform.localPosition = transform.localPosition = Vector3.zero;
        }
        else
        {
            //回到原来的位置
            transform.localPosition = Vector3.zero;
            UIManager.Instance.SetVisible(UIPanelName.SceneStart_DiscardGoodsPanel, true);
        }
    }
}
