using UnityEngine;

public class BagGoodsItem : MonoBehaviour
{
    private bool isDownCtrl = false;
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            isDownCtrl = true;

        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            isDownCtrl = false;
        }
    }

    void OnClick()
    {
        int id = 0, num = 0;
        //判断点击的如果是空格子 return
        if (!int.TryParse(this.name, out id)) return;
        id = int.Parse(this.name);
        UILabel lb_num = transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
        num = int.Parse(lb_num.text);
        //左键从包裹中把该物品全部移动到待售卖区
        if (isDownCtrl && UICamera.currentTouchID == -1)
        {
            //点击的时候隐藏详情介绍框
            UIManager.Instance.SetVisible(UIPanelName.SceneStart_GoodsInfoPanel, false);

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
            //点击的时候隐藏详情介绍框
            UIManager.Instance.SetVisible(UIPanelName.SceneStart_GoodsInfoPanel, false);

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
            if ((num - 1) > 0)//如果还有物品
            {
                lb_num.text = (num - 1) + "";

                if ((num - 1) == 1)
                {
                    lb_num.gameObject.SetActive(false);
                }
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
