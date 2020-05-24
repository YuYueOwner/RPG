using HotFix_Project.Config;
using UnityEngine;

public class SkillAttackPanel : UIScene
{
    private UIButton Attack_Button;
    private UIButton Defend_Button;
    private UIButton Back_Button;
    private UITable table;
    private UIGrid skillGrid;
    private UIScrollView sv;
    public static SkillAttackPanel _instance;

    private void Awake()
    {
        _instance = this;
        Attack_Button = Helper.GetChild<UIButton>(this.transform, "Attack_Button");
        Defend_Button = Helper.GetChild<UIButton>(this.transform, "Defend_Button");
        Back_Button = Helper.GetChild<UIButton>(this.transform, "Back_Button");

        skillGrid = Helper.GetChild<UIGrid>(this.transform, "SkillGrid");
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

    //生成拥有的技能列表
    public void OnCreateOwnSkillItem()
    {
        SkillConfig cfgData = DataTableManager.Instance.GetConfig<SkillConfig>("Skill");
        for (int i = 0; i < 3; i++)
        {
            int id = UnityEngine.Random.Range(i, 900);
            GameObject obj = skillGrid.GetChild(i).gameObject;//Instantiate(Resources.Load("Prefabs/SkillDefendPanel_Item_Item"), Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(skillGrid.transform);
            obj.transform.localScale = Vector3.one;
            obj.transform.tag = "Untagged";
            UISprite sp = obj.transform.Find("SP_Icon").GetComponent<UISprite>();
            sp.transform.tag = "Untagged";

            BoxCollider box = obj.transform.GetComponent<BoxCollider>();

            SkillConfig.SkillObject data = cfgData.GetListConfigElementByID(id);
            bool isHasData = data != null;
            box.enabled = isHasData;
            if (isHasData)//如果有数据
            {
                sp.spriteName = "ParrySkill_05";// cfgData.GetListConfigElementByID(data.SkillID).;
            }
            else
            {
                sp.spriteName = "";
            }
        }
        skillGrid.Reposition();
    }

    //生成防御技能Item
    public void OnCreateSkillAttackItem()
    {
        SkillConfig cfgData = DataTableManager.Instance.GetConfig<SkillConfig>("Skill");
        var dic = PlayerStateManager.GetInstance().OnCreateSkill();

        //生成技能类型数量
        foreach (var item in dic)
        {
            GameObject trans = Instantiate(Resources.Load("Prefabs/SkillDefendPanel_Item"), Vector3.zero, Quaternion.identity) as GameObject;
            //trans.name = i.ToString();
            trans.transform.SetParent(table.transform);
            trans.transform.localScale = Vector3.one;
            UIGrid grid = Helper.GetChild<UIGrid>(trans.transform, "Grid");
            for (int j = 0; j < item.Value.Count; j++)
            {
                GameObject obj = Instantiate(Resources.Load("Prefabs/SkillDefendPanel_Item_Item"), Vector3.zero, Quaternion.identity) as GameObject;
                obj.transform.SetParent(grid.transform);
                obj.transform.localScale = Vector3.one;
                obj.transform.tag = "Untagged";
                UISprite sp = obj.transform.Find("SP_Icon").GetComponent<UISprite>();
                sp.transform.tag = "Untagged";

                BoxCollider box = obj.transform.GetComponent<BoxCollider>();

                SkillConfig.SkillObject data = cfgData.GetListConfigElementByID(item.Value[j]);
                bool isHasData = data != null;
                box.enabled = isHasData;
                if (isHasData)//如果有数据
                {
                    sp.spriteName = "ParrySkill_05";// cfgData.GetListConfigElementByID(data.SkillID).;
                }
                else
                {
                    sp.spriteName = "";
                }
            }
            grid.Reposition();
            grid.repositionNow = true;

            Helper.GetChild<UILabel>(trans.transform, "LB_TypeName").text = item.Key;

            //List<int> list = new List<int>();//本集合存放的是该技能类型的已拥有的所有技能
            //OnCreateSkillItem.GetInstance().OnCreateSkillItemClick(grid, item.Value);
        }

        table.Reposition();
        sv.ResetPosition();
    }
}
