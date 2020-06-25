using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        if (UICamera.currentTouchID == -2)
        {
            AudioManager.Instance.PlaySound(1);
            for (int i = 0; i < this.transform.parent.childCount; i++)
            {
                Helper.GetChild(this.transform.parent.GetChild(i), "SelectFrame").SetActive(int.Parse(this.name) == i);
            }
            UIManager.Instance.SetVisible(UIPanelName.SceneStart_BuyGoodsOnlyOnePanel, true);
        }
    }
}
