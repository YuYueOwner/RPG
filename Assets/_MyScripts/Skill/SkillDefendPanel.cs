using HotFix_Project.Config;
using UnityEngine;

public class SkillDefendPanel : UIScene
{
    private UIButton Attack_Button;
    private UIButton Defend_Button;
    private UIButton Back_Button;
    private UITable table;
    private UIGrid skillGrid;
    private UIScrollView sv;
    public static SkillDefendPanel _instance;

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

    //生成拥有的技能列表
    public void OnCreateOwnSkillItem()
    {
        SkillConfig cfgData = DataTableManager.Instance.GetConfig<SkillConfig>("Skill");
        int unLockNum = PlayerStateManager.GetInstance().UnLockNum();//解锁格子数量
        int useSkillNum = PlayerStateManager.GetInstance().GetSkillUseNum("attack");//装备该类型技能数量

        for (int i = 0; i < 8; i++)
        {
            GameObject obj = skillGrid.GetChild(i).gameObject;//Instantiate(Resources.Load("Prefabs/SkillDefendPanel_Item_Item"), Vector3.zero, Quaternion.identity) as GameObject;

            GameObject SP_Lock = obj.transform.GetChild(1).gameObject;
            UISprite sp = obj.transform.GetChild(2).GetComponent<UISprite>();

            BoxCollider box = obj.transform.GetComponent<BoxCollider>();
            BoxCollider spBox = sp.transform.GetComponent<BoxCollider>();
            obj.name = i.ToString();

            SkillConfig.SkillObject isHasData = null;
            bool unLock = false;
            if (i < unLockNum)
            {
                if (i < useSkillNum)
                {
                    int id = PlayerStateManager.GetInstance().AttackQuene[i];// UnityEngine.Random.Range(i, 977);
                    sp.name = id.ToString();
                    SkillConfig.SkillObject data = cfgData.GetListConfigElementByID(id);
                    isHasData = data;
                }
                obj.transform.tag = "OpenLockHasValueParent";
                sp.transform.tag = "OpenLockNotValue";
            }
            else
            {
                obj.transform.tag = "NotOpen";
                sp.transform.tag = "NotOpen";
                unLock = true;
            }
            SP_Lock.SetActive(unLock);
            box.enabled = isHasData != null || !unLock;
            spBox.enabled = isHasData != null || !unLock;
            if (isHasData != null)//如果有数据
            {
                int skillIcon = cfgData.GetListConfigElementByID(isHasData.SkillID).SkillIcon;
                sp.spriteName = skillIcon.ToString();
            }
            else
            {
                sp.spriteName = null;
            }
        }
    }

    //生成防御技能Item
    public void OnCreateSkillDefendItem()
    {
        SkillConfig cfgData = DataTableManager.Instance.GetConfig<SkillConfig>("Skill");
        var dic = PlayerStateManager.GetInstance().OnCreateSkill("def");

        //生成技能类型数量
        foreach (var item in dic)
        {
            GameObject trans = Instantiate(Resources.Load("Prefabs/SkillPanel_Item"), Vector3.zero, Quaternion.identity) as GameObject;
            //trans.name = i.ToString();
            trans.transform.SetParent(table.transform);
            trans.transform.localScale = Vector3.one;
            UIGrid grid = Helper.GetChild<UIGrid>(trans.transform, "Grid");
            for (int j = 0; j < item.Value.Count; j++)
            {
                GameObject obj = Instantiate(Resources.Load("Prefabs/SkillPanel_Item_Item"), Vector3.zero, Quaternion.identity) as GameObject;
                obj.transform.SetParent(grid.transform);
                obj.transform.localScale = Vector3.one;
                UISprite sp = obj.transform.GetChild(1).GetComponent<UISprite>();
                obj.transform.tag = "OwnSkill";
                sp.transform.tag = "OwnSkill";
                obj.name = item.Value[j].ToString();
                sp.name = item.Value[j].ToString();
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
                    sp.spriteName = "1";
                }
            }
            grid.Reposition();
            grid.repositionNow = true;

            Helper.GetChild<UILabel>(trans.transform, "LB_TypeName").text = item.Key;

            //List<int> list = new List<int>();//本集合存放的是该技能类型的已拥有的所有技能
            //OnCreateSkillItem.GetInstance().OnCreateSkillItemClick(grid, item.Value);
        }

        table.Reposition();
        //sv.ResetPosition();
    }

    //删除技能
    public void RevomeSkill(string id)
    {
        for (int i = 0; i < skillGrid.transform.childCount; i++)
        {
            Transform trans = skillGrid.GetChild(i);
            if (trans.name == id)
            {
                trans.GetChild(2).GetComponent<UISprite>().spriteName = "-1";
                PlayerStateManager.instane.RefreshAttackQuene(int.Parse(trans.name), int.Parse(id));
            }
        }
    }

    //判断是否有重复的技能删除掉
    public void RevomeRepetitionSkill(string id)
    {
        for (int i = 0; i < skillGrid.transform.childCount; i++)
        {
            UISprite sp = skillGrid.GetChild(i).GetChild(2).GetComponent<UISprite>();
            if (sp.name == id)
            {
                string str = skillGrid.GetChild(i).name;
                sp.spriteName = "-1";
                PlayerStateManager.instane.RefreshAttackQuene(int.Parse(str), int.Parse(id));
            }
        }
    }
}
