﻿using UnityEngine;

public class BagGoodsItem : MonoBehaviour
{
    private bool isDownShift = false;
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            isDownShift = true;

        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            isDownShift = false;
        }
    }

    void OnClick()
    {
        int id = 0, num = 0;
        id = int.Parse(this.name);
        UILabel lb_num = transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
        num = int.Parse(lb_num.text);
        //左键从包裹中把该物品全部移动到待售卖区
        if (isDownShift && UICamera.currentTouchID == -1)
        {
            if (transform.tag == "Goods")
            {
                //从包裹中把该物品全部移动到待售卖区
                DealPanel._instance.RefreshSellGoods(id, num, true);
            }
            else if (transform.tag == "BagGoods")
            {
                //从待售卖区把该物品全部移动到包裹中
                DealPanel._instance.RefreshBagGoods(id, num);
            }
            else
            {
                Debug.LogError("当前物品tag错误");
                return;
            }
            transform.GetComponent<UISprite>().spriteName = "-1";
            transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = "-1";
            lb_num.text = "0";
            transform.name = "GoodsSprite1";
            lb_num.gameObject.SetActive(false);
        } //右键选择一个物品
        else if (UICamera.currentTouchID == -2)
        {
            if (transform.tag == "Goods")
            {
                DealPanel._instance.RefreshSellGoods(id, 1, false);

            }
            else if (transform.tag == "BagGoods")
            {
                DealPanel._instance.RefreshBagGoods(id, 1);
            }
            else
            {
                Debug.LogError("当前物品tag错误");
                return;
            }
            if ((num - 1) > 0)
            {
                lb_num.text = (num - 1) + "";
            }
            else
            {
                lb_num.text = "0";
                lb_num.gameObject.SetActive(false);
                transform.GetComponent<UISprite>().spriteName = "-1";
                transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = "-1";
                transform.name = "GoodsSprite1";
            }
        }
    }
}
