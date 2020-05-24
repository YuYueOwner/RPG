﻿using System.Collections.Generic;
using UnityEngine;

public class SkillAttackPanel : UIScene
{
    private UIButton Attack_Button;
    private UIButton Defend_Button;
    private UIButton Back_Button;
    private UITable table;
    private UIScrollView sv;
    public static SkillAttackPanel _instance;

    private void Awake()
    {
        _instance = this;
        Attack_Button = Helper.GetChild<UIButton>(this.transform, "Attack_Button");
        Defend_Button = Helper.GetChild<UIButton>(this.transform, "Defend_Button");
        Back_Button = Helper.GetChild<UIButton>(this.transform, "Back_Button");

        table = Helper.GetChild<UITable>(this.transform, "Table");
        sv = Helper.GetChild<UIScrollView>(this.transform, "SV");
    }
    protected override void Start()
    {
        base.Start();
        Attack_Button.onClick.Add(new EventDelegate(Attack));
        Defend_Button.onClick.Add(new EventDelegate(Defend));
        Back_Button.onClick.Add(new EventDelegate(Back));
    }

    //攻
    private void Attack()
    {
    }

    //守
    private void Defend()
    {
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_SkillAttackPanel, false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_SkillDefendPanel, true);
    }

    //返回
    private void Back()
    {
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_SkillAttackPanel, false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_OpenBagPanel, true);

    }


    //生成防御技能Item
    public void OnCreateSkillDefendItem()
    {
        //生成技能类型数量
        for (int i = 0; i < 3; i++)
        {
            GameObject trans = Instantiate(Resources.Load("Prefabs/SkillDefendPanel_Item"), Vector3.zero, Quaternion.identity) as GameObject;
            trans.name = i.ToString();
            trans.transform.SetParent(table.transform);
            trans.transform.localScale = Vector3.one;
            UIGrid grid = Helper.GetChild<UIGrid>(trans.transform, "Grid");
            for (int j = 0; j < 9; j++)
            {
                GameObject item = Instantiate(Resources.Load("Prefabs/SkillDefendPanel_Item_Item"), Vector3.zero, Quaternion.identity) as GameObject;
                item.transform.SetParent(grid.transform);
                item.transform.localScale = Vector3.one;
            }
            grid.Reposition();
            grid.repositionNow = true;
            //需要判断哪个解锁了                           
            switch (i)
            {
                case 0:
                    Helper.GetChild<UILabel>(trans.transform, "LB_TypeName").text = "刀";
                    break;
                case 1:
                    Helper.GetChild<UILabel>(trans.transform, "LB_TypeName").text = "剑";
                    break;
                default:
                    Helper.GetChild<UILabel>(trans.transform, "LB_TypeName").text = "枪";
                    break;
            }
            List<int> list = new List<int>();//本集合存放的是该技能类型的已拥有的所有技能
            OnCreateSkillItem.GetInstance().OnCreateSkillItemClick(grid, list);
        }
        table.Reposition();
        sv.ResetPosition();
    }
}
