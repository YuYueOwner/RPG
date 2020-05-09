using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanel : UIScene
{
    private UILabel PhysicalPower_Label;//体质
    private UIButton PhysicalPowerAdd_Button;//增加属性值按钮
    private UIButton PhysicalPowerMinus_Button;//减属性值按钮

    private UILabel Strength_Label;//力道
    private UIButton StrengthAdd_Button;//增加属性值按钮
    private UIButton StrengthMinus_Button;//减属性值按钮

    private UILabel Skill_Label;//身法
    private UIButton SkillAdd_Button;//增加属性值按钮
    private UIButton SkillMinus_Button;//减属性值按钮

    private UILabel Bone_Label;//根骨
    private UIButton BoneAdd_Button;//增加属性值按钮
    private UIButton BoneMinus_Button;//减属性值按钮

    private UILabel UsableProperty_Label;//可用属性
    private UIButton Sure_Button;
    private UILabel Hp_Label;//生命值
    private UILabel Health_Label;//健康值
    private UILabel Experience_Label;//经验值
    private UIButton Back_Button;//返回按钮
    private UILabel RoleName_Label;//人物名字
    private UILabel Money_Label;//元宝数
    private UISprite Role_Sprite;//人物图片
    private UIGrid BagGrid;
    private UILabel UsableBag_Label;//可用背包数量
    private UIButton CleanUp_Button;//整理

    private void Awake()
    {
        PhysicalPower_Label = Helper.GetChild(this.transform, "PhysicalPower_Label").GetComponent<UILabel>();
        PhysicalPowerAdd_Button = Helper.GetChild<UIButton>(PhysicalPower_Label.transform.parent, "Add_Button");
        PhysicalPowerMinus_Button = Helper.GetChild<UIButton>(PhysicalPower_Label.transform.parent, "Minus_Button");

        Strength_Label = Helper.GetChild(this.transform, "Strength_Label").GetComponent<UILabel>();
        StrengthAdd_Button = Helper.GetChild<UIButton>(Strength_Label.transform.parent, "Add_Button");
        StrengthMinus_Button = Helper.GetChild<UIButton>(Strength_Label.transform.parent, "Minus_Button");

        Skill_Label = Helper.GetChild(this.transform, "Skill_Label").GetComponent<UILabel>();
        SkillAdd_Button = Helper.GetChild<UIButton>(Skill_Label.transform.parent, "Add_Button");
        SkillMinus_Button = Helper.GetChild<UIButton>(Skill_Label.transform.parent, "Minus_Button");

        Bone_Label = Helper.GetChild(this.transform, "Bone_Label").GetComponent<UILabel>();
        BoneAdd_Button = Helper.GetChild<UIButton>(Bone_Label.transform.parent, "Add_Button");
        BoneMinus_Button = Helper.GetChild<UIButton>(Bone_Label.transform.parent, "Minus_Button");

        UsableProperty_Label = Helper.GetChild(this.transform, "UsableProperty_Label").GetComponent<UILabel>();
        Sure_Button = Helper.GetChild(this.transform, "Sure_Button").GetComponent<UIButton>();
        Hp_Label = Helper.GetChild(this.transform, "Hp_Label").GetComponent<UILabel>();
        Health_Label = Helper.GetChild(this.transform, "Health_Label").GetComponent<UILabel>();
        Experience_Label = Helper.GetChild(this.transform, "Experience_Label").GetComponent<UILabel>();
        Back_Button = Helper.GetChild(this.transform, "Back_Button").GetComponent<UIButton>();
        RoleName_Label = Helper.GetChild(this.transform, "RoleName_Label").GetComponent<UILabel>();
        Money_Label = Helper.GetChild(this.transform, "Money_Label").GetComponent<UILabel>();
        Role_Sprite = Helper.GetChild(this.transform, "Role_Sprite").GetComponent<UISprite>();
        BagGrid = Helper.GetChild<UIGrid>(this.transform, "BagGrid");

        UsableBag_Label = Helper.GetChild(this.transform, "UsableBag_Label").GetComponent<UILabel>();
        CleanUp_Button = Helper.GetChild(this.transform, "CleanUp_Button").GetComponent<UIButton>();

    }
    protected override void Start()
    {
        base.Start();
        Sure_Button.onClick.Add(new EventDelegate(Sure));
        Back_Button.onClick.Add(new EventDelegate(Back));
        CleanUp_Button.onClick.Add(new EventDelegate(CleanUp));

        UIEventListener.Get(PhysicalPowerAdd_Button.gameObject).onClick = AddProperty;
        UIEventListener.Get(StrengthAdd_Button.gameObject).onClick = AddProperty;
        UIEventListener.Get(SkillAdd_Button.gameObject).onClick = AddProperty;
        UIEventListener.Get(BoneAdd_Button.gameObject).onClick = AddProperty;
        UIEventListener.Get(PhysicalPowerMinus_Button.gameObject).onClick = MinusProperty;
        UIEventListener.Get(StrengthMinus_Button.gameObject).onClick = MinusProperty;
        UIEventListener.Get(SkillMinus_Button.gameObject).onClick = MinusProperty;
        UIEventListener.Get(BoneMinus_Button.gameObject).onClick = MinusProperty;

        if (int.Parse(UsableProperty_Label.text) <= 0)
        {
            Sure_Button.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            Sure_Button.GetComponent<BoxCollider>().enabled = true;
        }

        for (int i = 0; i < 80; i++)
        {
            GameObject go = Instantiate(Resources.Load("BagBg_Sprite"), Vector3.zero, Quaternion.identity) as GameObject;
            go.transform.SetParent(BagGrid.transform);
            go.transform.localScale = Vector3.one;
        }
        BagGrid.Reposition();
        BagGrid.repositionNow = true;

    }
    private void AddProperty(GameObject go)
    {
        UILabel propertyLabel = go.transform.parent.GetChild(0).GetComponent<UILabel>();
        propertyLabel.text = (int.Parse(propertyLabel.text) + 1).ToString();
    }

    private void MinusProperty(GameObject go)
    {
        UILabel propertyLabel = go.transform.parent.GetChild(0).GetComponent<UILabel>();
        propertyLabel.text = (int.Parse(propertyLabel.text) - 1).ToString();
    }

    private void Sure()
    {
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_ChangePropertyPanel, true);
    }

    private void Back()
    {
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_OpenBagPanel, true);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_BagPanel, false);
    }

    //整理
    private void CleanUp()
    {

    }
}

