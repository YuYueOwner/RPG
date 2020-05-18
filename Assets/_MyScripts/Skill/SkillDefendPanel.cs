using System.Collections.Generic;
using UnityEngine;

public class SkillDefendPanel : UIScene
{
    private UIButton Attack_Button;
    private UIButton Defend_Button;
    private UIButton Back_Button;
    private UITable table;
    private UIScrollView sv;

    private void Awake()
    {
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
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_SkillAttackPanel, true);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_SkillDefendPanel, false);
    }

    //守
    private void Defend()
    {

    }

    //返回
    private void Back()
    {
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_SkillDefendPanel, false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_OpenBagPanel, true);

    }

    //生成防御技能Item
    private void OnCreateSkillDefendItem()
    {
        //生成技能类型数量
        for (int i = 0; i < 3; i++)
        {
            Transform trans = Instantiate(Resources.Load("SkillDefendPanel_Item"), Vector3.zero, Quaternion.identity) as Transform;
            UIGrid grid = trans.Find("Grid").GetComponent<UIGrid>();
            trans.Find("LB_TypeName").GetComponent<UILabel>().text = "刀";
            List<int> list = new List<int>();//本集合存放的是该技能类型的已拥有的所有技能
            OnCreateSkillItem.GetInstance().OnCreateSkillItemClick(grid, list);
        }
        table.Reposition();
        sv.ResetPosition();
    }
}
