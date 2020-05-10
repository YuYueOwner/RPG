using UnityEngine;
using System.Collections;

public class BagDrag : UIDragDropItem
{
    //装备标签
    public string BackPackItemTag;
    //格子标签
    public string CellTag;

    //通过重写的鼠标监听事件(开始拖动)
    public override void StartDragging()
    {
        base.StartDragging();
        this.GetComponent<UISprite>().depth = 10;
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
        if (surface.tag == CellTag)
        {
            //物品交换 （通过改变父物体来转移位置）
            this.transform.parent = surface.transform;
            //位置归零
            this.transform.localPosition = Vector3.zero;
        }
        //如果当下时撞到的是装备
        else if (surface.tag == BackPackItemTag)
        {
            //开始交换  
            Transform Prent = surface.transform.parent;         //把撞到的(surface)装备的父物体取出来
            surface.transform.parent = this.transform.parent;   //把撞到的物体移动过来(把自己的父物体给surface)
            this.transform.parent = Prent;                      //自己移动到想被交换的位置
            //交换完成 位移归零 （交换时是位移的改变 缩放没有变）
            surface.transform.localPosition = transform.localPosition = Vector3.zero;
        }
        else
        {
            //回到原来的位置
            transform.localPosition = Vector3.zero;
        }
    }
}