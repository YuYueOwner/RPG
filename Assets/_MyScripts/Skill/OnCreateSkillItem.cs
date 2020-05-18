using HotFix_Project.Config;
using System.Collections.Generic;
using UnityEngine;

public class OnCreateSkillItem : MonoBehaviour
{
    private static OnCreateSkillItem Instance;

    public static OnCreateSkillItem GetInstance()
    {
        if (Instance == null)
        {
            Instance = new OnCreateSkillItem();
        }
        return Instance;
    }

    public void OnCreateSkillItemClick(UIGrid grid, List<int> list)
    {
        //PropConfig 换成技能表
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");

        for (int i = 0; i < list.Count; i++)
        {
            Transform trans;
            trans = Instantiate(Resources.Load("SkillDefendPanel_Item"), Vector3.zero, Quaternion.identity) as Transform;
            trans.transform.SetParent(grid.transform);
            trans.transform.localScale = Vector3.one;
            UISprite sp = trans.transform.Find("SP_Icon").GetComponent<UISprite>();
            BoxCollider box = trans.transform.GetComponent<BoxCollider>();

            PropConfig.PropObject data = cfgData.GetListConfigElementByID(list[i]);
            bool isHasData = data != null;
            box.enabled = isHasData;
            if (isHasData)//如果有数据
            {
                sp.spriteName = cfgData.GetListConfigElementByID(data.ItemID).ItemIcon;
            }
            else
            {
                sp.spriteName = "";
            }
        }
        grid.Reposition();
    }
}
