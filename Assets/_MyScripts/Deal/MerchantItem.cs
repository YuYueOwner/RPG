using UnityEngine;

public class MerchantItem : MonoBehaviour
{
    private bool isDownShift = false;
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isDownShift = true;

        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isDownShift = false;
        }
    }

    void OnClick()
    {
        //左键弹出数量选择框
        if (UICamera.currentTouchID == -1)
        {
            if (isDownShift == false) return;
            ShowSelectFrame();

            //先判断物品数量是否大于1
            if (Helper.GetChild<UILabel>(this.transform, "GoodsNumLabel").text != "")
            {
                //记录当前物品总数量
                BuyGoodsPanel._instance.recordCurrentGoodsNum = int.Parse(Helper.GetChild<UILabel>(this.transform, "GoodsNumLabel").text);
                //判断物品数量是否小于5
                BuyGoodsPanel._instance.SellGoodsNumLabel.text = BuyGoodsPanel._instance.recordCurrentGoodsNum < 5 ? BuyGoodsPanel._instance.recordCurrentGoodsNum.ToString() : "5";
            }
            else
            {
                BuyGoodsPanel._instance.recordCurrentGoodsNum = 1;
                BuyGoodsPanel._instance.SellGoodsNumLabel.text = "1";
            }
            UIManager.Instance.SetVisible(UIPanelName.SceneStart_BuyGoodsPanel, true);
        }
        //右键选择一个物品
        if (UICamera.currentTouchID == -2)
        {
            ShowSelectFrame();
            UIManager.Instance.SetVisible(UIPanelName.SceneStart_BuyGoodsOnlyOnePanel, true);
        }
        PlayerInfoManager.Instance.selectDealItemID = int.Parse(this.transform.name);
    }

    //显示选择框
    private void ShowSelectFrame()
    {
        AudioManager.Instance.PlaySound(1);
        for (int i = 0; i < this.transform.parent.childCount; i++)
        {
            Helper.GetChild(this.transform.parent.GetChild(i), "SelectFrame").SetActive(int.Parse(this.name) == int.Parse(this.transform.parent.GetChild(i).name));
        }
    }
}
