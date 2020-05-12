using HotFix_Project.Config;
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

    List<UILabel> playerAttributeLable = new List<UILabel>();
    List<BoxCollider> playerAttributeBox = new List<BoxCollider>();
    List<PackageItem> itemList = new List<PackageItem>();

    private void Awake()
    {
        PhysicalPower_Label = Helper.GetChild(this.transform, "PhysicalPower_Label").GetComponent<UILabel>();
        PhysicalPowerAdd_Button = Helper.GetChild<UIButton>(PhysicalPower_Label.transform.parent, "PhysicalPower_Add_Button");
        PhysicalPowerMinus_Button = Helper.GetChild<UIButton>(PhysicalPower_Label.transform.parent, "PhysicalPower_Minus_Button");

        Strength_Label = Helper.GetChild(this.transform, "Strength_Label").GetComponent<UILabel>();
        StrengthAdd_Button = Helper.GetChild<UIButton>(Strength_Label.transform.parent, "Strength_Add_Button");
        StrengthMinus_Button = Helper.GetChild<UIButton>(Strength_Label.transform.parent, "Strength_Minus_Button");

        Skill_Label = Helper.GetChild(this.transform, "Skill_Label").GetComponent<UILabel>();
        SkillAdd_Button = Helper.GetChild<UIButton>(Skill_Label.transform.parent, "Skill_Add_Button");
        SkillMinus_Button = Helper.GetChild<UIButton>(Skill_Label.transform.parent, "Skill_Minus_Button");

        Bone_Label = Helper.GetChild(this.transform, "Bone_Label").GetComponent<UILabel>();
        BoneAdd_Button = Helper.GetChild<UIButton>(Bone_Label.transform.parent, "Bone_Add_Button");
        BoneMinus_Button = Helper.GetChild<UIButton>(Bone_Label.transform.parent, "Bone_Minus_Button");

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

        playerAttributeLable.Add(PhysicalPower_Label);
        playerAttributeLable.Add(Strength_Label);
        playerAttributeLable.Add(Skill_Label);
        playerAttributeLable.Add(Bone_Label);
        playerAttributeLable.Add(UsableProperty_Label);

        playerAttributeBox.Add(Helper.GetChild<BoxCollider>(PhysicalPower_Label.transform.parent, "PhysicalPower_Add_Button"));
        playerAttributeBox.Add(Helper.GetChild<BoxCollider>(PhysicalPower_Label.transform.parent, "Strength_Add_Button"));
        playerAttributeBox.Add(Helper.GetChild<BoxCollider>(PhysicalPower_Label.transform.parent, "Skill_Add_Button"));
        playerAttributeBox.Add(Helper.GetChild<BoxCollider>(PhysicalPower_Label.transform.parent, "Bone_Add_Button"));

    }
    protected override void Start()
    {
        base.Start();

        //Sure_Button.onClick.Add(new EventDelegate(Sure));
        Back_Button.onClick.Add(new EventDelegate(Back));
        CleanUp_Button.onClick.Add(new EventDelegate(CleanUp));

        UIEventListener.Get(Sure_Button.gameObject).onClick = Sure;
        UIEventListener.Get(PhysicalPowerAdd_Button.gameObject).onClick = AddProperty;
        UIEventListener.Get(StrengthAdd_Button.gameObject).onClick = AddProperty;
        UIEventListener.Get(SkillAdd_Button.gameObject).onClick = AddProperty;
        UIEventListener.Get(BoneAdd_Button.gameObject).onClick = AddProperty;
        UIEventListener.Get(PhysicalPowerMinus_Button.gameObject).onClick = MinusProperty;
        UIEventListener.Get(StrengthMinus_Button.gameObject).onClick = MinusProperty;
        UIEventListener.Get(SkillMinus_Button.gameObject).onClick = MinusProperty;
        UIEventListener.Get(BoneMinus_Button.gameObject).onClick = MinusProperty;

        ExistBoxIsForbidden();

        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        itemList = PlayerInfoManager.Instance.playerItemData;
        UsableBag_Label.text = (80 - PlayerInfoManager.Instance.playerItemData.Count).ToString();

        for (int i = 0; i < 80; i++)
        {
            GameObject go = Instantiate(Resources.Load("BagBg_Sprite"), Vector3.zero, Quaternion.identity) as GameObject;
            go.name = i.ToString();
            go.transform.SetParent(BagGrid.transform);
            go.transform.localScale = Vector3.one;
            //if (i <= 10)
            //{
            //    go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon1";
            //}
            //else if (i > 10 && i <= 20)
            //{
            //    go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon2";
            //}
            //else if (i > 20 && i <= 30)
            //{
            //    go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon3";
            //}
            //else if (i > 30 && i <= 40)
            //{
            //    go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon4";
            //}
            //else
            //{
            //    go.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            //    go.transform.GetChild(0).GetComponent<UISprite>().spriteName = null;
            //}
            PackageItem data = null;

            if (itemList.Count > i)
            {
                data = itemList[i];
            }
            if (data != null)
            {
                go.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
                go.transform.GetChild(0).GetComponent<UISprite>().spriteName = cfgData.GetListConfigElementByID(data.PackageItemID).ItemIcon;
            }
            else
            {
                go.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                go.transform.GetChild(0).GetComponent<UISprite>().spriteName = null;
            }
        }
        BagGrid.Reposition();
        BagGrid.repositionNow = true;
        SetPlayerAttributeInfo();
    }

    private void SetPlayerAttributeInfo()
    {
        for (int i = 0; i < playerAttributeLable.Count; i++)
        {
            string key = PlayerInfoManager.Instance.GetPlayerPrefsKey(i + 1);
            int count = PlayerPrefsManager.Instance.GetIntPlayerPrefs(key);
            if (count > 0)
            {
                playerAttributeLable[i].text = count.ToString();
            }
            else
            {
                playerAttributeLable[i].text = PlayerInfoManager.Instance.GetPlayerAttribute(i + 1).ToString();
            }
        }

    }

    private void ExistBoxIsForbidden()
    {
        bool isShwo = int.Parse(UsableProperty_Label.text) > 0;
        Sure_Button.GetComponent<BoxCollider>().enabled = isShwo;
        for (int i = 0; i < playerAttributeLable.Count; i++)
        {
            //playerAttributeLable[i].transform.parent.GetComponent<BoxCollider>().enabled = isShwo;
        }
    }


    private void AddProperty(GameObject go)
    {
        string key = "";
        int sum = int.Parse(UsableProperty_Label.text);
        if (sum > 0)
        {
            UILabel propertyLabel = go.transform.parent.GetChild(0).GetComponent<UILabel>();
            propertyLabel.text = (int.Parse(propertyLabel.text) + 1).ToString();
            if (go.name == "PhysicalPower_Add_Button")
            {
                //体质
                key = PlayerInfoManager.Instance.GetPlayerPrefsKey(1);
            }
            else if (go.name == "Strength_Add_Button")
            {
                //力道
                key = PlayerInfoManager.Instance.GetPlayerPrefsKey(2);
            }
            else if (go.name == "Skill_Add_Button")
            {
                //身法
                key = PlayerInfoManager.Instance.GetPlayerPrefsKey(3);
            }
            else if (go.name == "Bone_Add_Button")
            {
                //根骨
                key = PlayerInfoManager.Instance.GetPlayerPrefsKey(4);
            }
            PlayerPrefsManager.Instance.SetAttributePlayerPrefs(key, 1);
            PlayerPrefsManager.Instance.SetAttributePlayerPrefs(PlayerInfoManager.Instance.GetPlayerPrefsKey(5), -1);
            SetUsableProperty_Label(int.Parse(UsableProperty_Label.text) - 1);
        }
    }

    private void MinusProperty(GameObject go)
    {
        UILabel propertyLabel = go.transform.parent.GetChild(0).GetComponent<UILabel>();
        propertyLabel.text = (int.Parse(propertyLabel.text) - 1).ToString();

        string key = "";
        if (go.name == "PhysicalPower_Minus_Button")
        {
            //体质
            key = PlayerInfoManager.Instance.GetPlayerPrefsKey(1);
        }
        else if (go.name == "Strength_Minus_Button")
        {
            //力道
            key = PlayerInfoManager.Instance.GetPlayerPrefsKey(2);
        }
        else if (go.name == "Skill_Minus_Button")
        {
            //身法
            key = PlayerInfoManager.Instance.GetPlayerPrefsKey(3);
        }
        else if (go.name == "Bone_Minus_Button")
        {
            //根骨
            key = PlayerInfoManager.Instance.GetPlayerPrefsKey(4);
        }
        PlayerPrefsManager.Instance.SetAttributePlayerPrefs(key, -1);
        PlayerPrefsManager.Instance.SetAttributePlayerPrefs(PlayerInfoManager.Instance.GetPlayerPrefsKey(5), 1);
        SetUsableProperty_Label(int.Parse(UsableProperty_Label.text) + 1);
        ExistBoxIsForbidden();
    }


    private void SetUsableProperty_Label(int count)
    {
        int sum = 0;
        if (int.TryParse(count.ToString(), out sum))
        {
            UsableProperty_Label.text = sum.ToString();
        }
    }

    private void Sure(GameObject go)
    {
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_ChangePropertyPanel, true);
    }

    private void Back()
    {
        PlayerPrefsManager.Instance.SetPlayerPrefs(false);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_OpenBagPanel, true);
        UIManager.Instance.SetVisible(UIPanelName.SceneStart_BagPanel, false);
    }

    //整理
    private void CleanUp()
    {
        itemList.Sort();

        for (int i = 0; i < 80; i++)
        {
            GameObject go = Instantiate(Resources.Load("BagBg_Sprite"), Vector3.zero, Quaternion.identity) as GameObject;
            go.name = i.ToString();
            go.transform.SetParent(BagGrid.transform);
            go.transform.localScale = Vector3.one;
            if (i <= 10)
            {
                go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon1";
            }
            else if (i > 10 && i <= 20)
            {
                go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon2";
            }
            else if (i > 20 && i <= 30)
            {
                go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon3";
            }
            else if (i > 30 && i <= 40)
            {
                go.transform.GetChild(0).GetComponent<UISprite>().spriteName = "icon4";
            }
            else
            {
                go.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                go.transform.GetChild(0).GetComponent<UISprite>().spriteName = null;
            }
        }
        BagGrid.Reposition();
        BagGrid.repositionNow = true;
    }
}
