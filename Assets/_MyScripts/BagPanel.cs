using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanel : UIScene
{
    private UILabel PhysicalPower_Label;//体质
    private UILabel Strength_Label;//力道
    private UILabel Skill_Label;//身法
    private UILabel Bone_Label;//根骨
    private UILabel UsableProperty_Label;//可用属性
    private UIButton Sure_Button;
    private UILabel Hp_Label;//生命值
    private UILabel Health_Label;//健康值
    private UILabel Experience_Label;//经验值
    private UIButton Back_Button;//返回按钮
    private UILabel RoleName_Label;//人物名字
    private UILabel Money_Label;//元宝数
    private UISprite Role_Sprite;//人物图片
    private UILabel UsableBag_Label;//可用背包数量
    private UIButton CleanUp_Button;//整理
    private void Awake()
    {
        PhysicalPower_Label = Helper.GetChild(this.transform, "PhysicalPower_Label").GetComponent<UILabel>();
        Strength_Label = Helper.GetChild(this.transform, "Strength_Label").GetComponent<UILabel>();
        Skill_Label = Helper.GetChild(this.transform, "Skill_Label").GetComponent<UILabel>();
        Bone_Label = Helper.GetChild(this.transform, "Bone_Label").GetComponent<UILabel>();
        UsableProperty_Label = Helper.GetChild(this.transform, "UsableProperty_Label").GetComponent<UILabel>();
        Sure_Button = Helper.GetChild(this.transform, "Sure_Button").GetComponent<UIButton>();
        Hp_Label = Helper.GetChild(this.transform, "Hp_Label").GetComponent<UILabel>();
        Health_Label = Helper.GetChild(this.transform, "Health_Label").GetComponent<UILabel>();
        Experience_Label = Helper.GetChild(this.transform, "Experience_Label").GetComponent<UILabel>();
        Back_Button = Helper.GetChild(this.transform, "Back_Button").GetComponent<UIButton>();
        RoleName_Label = Helper.GetChild(this.transform, "RoleName_Label").GetComponent<UILabel>();
        Money_Label = Helper.GetChild(this.transform, "Money_Label").GetComponent<UILabel>();
        Role_Sprite = Helper.GetChild(this.transform, "Role_Sprite").GetComponent<UISprite>();
        UsableBag_Label = Helper.GetChild(this.transform, "UsableBag_Label").GetComponent<UILabel>();
        CleanUp_Button = Helper.GetChild(this.transform, "CleanUp_Button").GetComponent<UIButton>();
    }
    protected override void Start()
    {
        base.Start();
        Sure_Button.onClick.Add(new EventDelegate(Sure));
        Back_Button.onClick.Add(new EventDelegate(Back));
        CleanUp_Button.onClick.Add(new EventDelegate(CleanUp));
    }

    private void Sure()
    {

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
