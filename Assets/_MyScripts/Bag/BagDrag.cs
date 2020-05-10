﻿using UnityEngine;
using System.Collections;

public class BagDrag : UIDragDropItem
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
        //鼠标右键点击逻辑，若点击装备则走装备判断逻辑
        //（是否可以装备，是-装备或替换/否-弹出提示），若点击消耗品则走消耗品判断逻辑（使用该消耗品）。
        if (UICamera.currentTouchID == -2)
        {

        }
    }


    //0.5s显示详细信息面板
    IEnumerator Show()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_GoodsInfoPanel, true);
    }


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
        if (surface.tag == "Cell")
        {
            //物品交换 （通过改变父物体来转移位置）
            this.transform.parent = surface.transform;
            //位置归零
            this.transform.localPosition = Vector3.zero;
        }
        //如果当下时撞到的是装备
        else if (surface.tag == "Goods")
        {
            Transform Prent = null;
            //开始交换  
            Prent = surface.transform.parent;         //把撞到的(surface)装备的父物体取出来
            surface.transform.parent = this.transform.parent;   //把撞到的物体移动过来(把自己的父物体给surface)
            this.transform.parent = Prent;                      //自己移动到想被交换的位置
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