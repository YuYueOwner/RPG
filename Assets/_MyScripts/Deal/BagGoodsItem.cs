using UnityEngine;

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
        //左键从包裹中把该物品全部移动到待售卖区
        if (isDownShift && UICamera.currentTouchID == -1)
        {
            Debug.LogError(this.name);
            UILabel lb_num = transform.parent.GetChild(0).GetChild(0).GetComponent<UILabel>();
            int id = 0, num = 0;
            id = int.Parse(this.name);
            num = int.Parse(lb_num.text);
            DealPanel._instance.RefreshSellGoods(id, num);

            transform.GetComponent<UISprite>().spriteName = "-1";
            transform.parent.GetChild(0).GetComponent<UISprite>().spriteName = "-1";
            lb_num.text = "0";
            transform.name = "BagGoods_Item(Clone)";
        }
    }
}
