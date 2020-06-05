using HotFix_Project.Config;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager
{
    public static readonly PlayerInfoManager Instance = new PlayerInfoManager();

    //玩家角色属性信息
    private Dictionary<int, string> playerAttributeInfo = new Dictionary<int, string>();// { "Physical", "Strength", "Skill", "Bone", "PlayerAvaliablePoint" };
    //玩家信息
    public PlayerState playerState = new PlayerState();
    //玩家背包数据
    public List<PackageItem> playerItemData = new List<PackageItem>();
    //丢弃的物品或者鼠标长留的物品id
    public int SelectItemId;
    //丢弃的技能id
    public string SelectSkillId;
    //鼠标长留物品0.5秒后显示的物品名称
    public UILabel ItemName;
    //鼠标长留物品0.5秒后显示的物品介绍
    public UILabel ItemDesc;
    //鼠标长留物品0.5秒后显示的物品经验
    public UILabel ItemExp;
    //装备
    public List<UISprite> equipmentList = new List<UISprite>();


    public void SetPlayerAttributeInfo()
    {
        playerAttributeInfo[1] = "Physical";
        playerAttributeInfo[2] = "Strength";
        playerAttributeInfo[3] = "Skill";
        playerAttributeInfo[4] = "Bone";
        playerAttributeInfo[5] = "PlayerAvaliable";
        playerAttributeInfo[6] = "PlayerHpCurrent";
        playerAttributeInfo[7] = "PlayerHpMax";
        playerAttributeInfo[8] = "PlayerExperience";
        playerAttributeInfo[9] = "PlayerExperienceMax";
        playerAttributeInfo[10] = "Equip";//身上的装备id 武器
        playerAttributeInfo[11] = "Equip1";//身上的装备id1 防具
        playerAttributeInfo[12] = "PlayerHealth";
        playerAttributeInfo[13] = "PlayerHealthMax";

        playerState.PlayerCon = 50;
        playerState.PlayerStr = 50;
        playerState.PlayerDex = 50;
        playerState.PlayerLuk = 50;
        playerState.PlayerAvaliablePoint = 4;
        playerState.PlayerHpMax = 100;
        playerState.PlayerHpCurrent = 100;
        playerState.PlayerHealth = 80;
        playerState.PlayerHealthMax = 100;
        playerState.ExpPlayer = 0;
        playerState.ExpMaxPlayer = 200;
        playerState.PlayerName = "张三";
        playerState.PlayerMoney = 0;
        playerState.PlayerLv = 100;
        playerState.PlayerHeadPhotoID = 1;
        playerState.PlayerFullPhotoID = 1;
        playerState.PlayerEquipWeaponID = 1;
        playerState.PlayerEquipArmorID = 1;
    }


    public void SetItemInfo()
    {
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        //初始化20个数据
        for (int i = 0; i < 30; i++)
        {
            bool isCanOverlay = false;//是否可以叠加， false 不可以
            //Debug.LogError("============" + i);
            PackageItem item = new PackageItem();
            int id = Random.Range(i, cfgData.propObjList.Count);

            if (cfgData.ExistIsCanOverlayByID(id))
            {
                for (int j = 0; j < playerItemData.Count; j++)
                {
                    if (playerItemData[j].PackageItemID == id)
                    {
                        playerItemData[j].PackageItemNum += 1;
                        isCanOverlay = true;
                        break;
                    }
                }
            }
            if (!isCanOverlay)
            {
                PropConfig.PropObject data = cfgData.GetListConfigElementByID(id);
                item.PackageItemID = data.ItemID;
                item.PackageItemName = data.ItemName;
                item.PackageItemNum = 1;
                playerItemData.Add(item);
            }
        }
    }

    //删除背包里的物品 
    public void RemovePlayerItemData(int id)
    {
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");

        for (int i = 0; i < playerItemData.Count; i++)
        {
            if (playerItemData[i].PackageItemID == id)
            {
                if (cfgData.ExistIsCanOverlayByID(id))
                {
                    //可以叠加
                    playerItemData[i].PackageItemNum -= 1;
                    if (playerItemData[i].PackageItemNum <= 0)
                    {
                        playerItemData.Remove(playerItemData[i]);
                        BagPanel._instance.CleanUp();
                        return;
                    }
                    else
                    {
                        BagPanel._instance.CleanUp();
                    }
                }
                else
                {
                    playerItemData.Remove(playerItemData[i]);
                    BagPanel._instance.CleanUp();
                    return;
                }
            }
        }
    }

    //添加物品到背包里
    public void AddPlayerItemData(int id)
    {
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        PackageItem item = new PackageItem();

        if (cfgData.ExistIsCanOverlayByID(id))
        {
            //可以叠加
            for (int i = 0; i < playerItemData.Count; i++)
            {
                if (playerItemData[i].PackageItemID == id)
                {
                    playerItemData[i].PackageItemNum += 1;
                    return;
                }
            }
        }
        else
        {
            PropConfig.PropObject data = cfgData.GetListConfigElementByID(id);
            item.PackageItemID = data.ItemID;
            item.PackageItemName = data.ItemName;
            item.PackageItemNum = 1;
            playerItemData.Add(item);
        }
    }

    //显示鼠标现在停留的物品信息
    public void ShowItemInfo(int id)
    {
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        PropConfig.PropObject data = cfgData.GetListConfigElementByID(id);
        GoodsInfoPanel._instance.GoodsTitelLabel.text = data.ItemName;
        switch (data.ConfigType)
        {
            case "Weapon":
                ItemName.text = "攻击力    " + data.WeaponAttack + "\r\n" +
                                            "身法      " + data.WeaponDex + "\r\n" +
                                            "力道      " + data.WeaponStrength + "\r\n" +
                                            "体质      " + data.WeaponCon + "\r\n" +
                                            "根骨      " + data.WeaponLuk + "\r\n" +
                                            "命中率   " + data.WeaponHitRate;
                break;
            case "Armor":
                ItemName.text = "防御值    " + data.ArmorDefence + "\r\n" +
                                           "身法       " + data.ArmorDex + "\r\n" +
                                           "力道       " + data.ArmorStrength + "\r\n" +
                                           "体质       " + data.ArmorCon + "\r\n" +
                                           "根骨       " + data.ArmorLuk + "\r\n" +
                                           "速度       " + data.ArmorRoll;
                break;
            case "Consumables":
                ItemName.text = "生命值回复数值      " + data.ConsumableHpIncrease + "\r\n" +
                                           "健康度回复数值      " + data.ConsumableHealthIncrease;

                break;
            default:
                ItemName.text = data.ItemName;
                break;
        }
        //Debug.LogError(data.ItemID);
        ItemDesc.text = data.ItemInfomation;
    }

    public void ShowSkillItemInfo(int id)
    {
        SkillConfig cfgData = DataTableManager.Instance.GetConfig<SkillConfig>("Skill");
        SkillConfig.SkillObject data = cfgData.GetListConfigElementByID(id);
        GoodsInfoPanel._instance.GoodsTitelLabel.text = data.SkillName;
        if (data.SkillType == "剑" || data.SkillType == "刀" || data.SkillType == "枪" || data.SkillType == "棍" || data.SkillType == "叉" || data.SkillType == "锤")
        {
            ItemName.text = "类别      " + data.SkillType + "\r\n" +
                                         "品质      " + data.SkillQuality + "\r\n" +
                                         "级别      " + data.SkillLv + "\r\n" +
                                         "攻击力   " + data.SkillAtk + "\r\n" +
                                         "命中率   " + data.SkillHitRate + "\r\n" +
                                         "身法      " + data.SkillDex + "\r\n" +
                                         "破甲      " + data.SkillArmorPen + "\r\n" +
                                         "暴击      " + data.SkillCrit;
        }
        else
        {
            ItemName.text = "类别      " + data.SkillType + "\r\n" +
                                         "品质      " + data.SkillQuality + "\r\n" +
                                         "级别      " + data.SkillLv + "\r\n" +
                                         "闪避      " + data.DefenceSkillDodge + "\r\n" +
                                         "身法      " + data.SkillDex;
        }
        //Debug.LogError(data.ItemID);
        ItemDesc.text = data.SkillInfo;
        ItemExp.text = cfgData.GetSkillLevelAndExpByID(id);
    }

    //获取玩家装备信息
    public void GetEquipmentInfo()
    {
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        for (int i = 0; i < 2; i++)
        {
            int id = PlayerPrefsManager.Instance.GetIntPlayerPrefs(PlayerInfoManager.Instance.GetPlayerPrefsKey(10 + i));
            if (id > 0)
            {
                PropConfig.PropObject data = cfgData.GetListConfigElementByID(id);
                if (data != null)
                {
                    equipmentList[i].spriteName = data.ItemIcon;
                }
            }
            else
            {
                equipmentList[i].spriteName = "";
            }
        }
    }

    //设置玩家装备改变
    public void SetEquipmentChange()
    {
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        PropConfig.PropObject data = cfgData.GetListConfigElementByID(SelectItemId);
        Debug.LogError("SelectItemId" + SelectItemId);

        if (data.ConfigType == "Weapon")
        {
            string key = PlayerInfoManager.Instance.GetPlayerPrefsKey(10);
            int value = -1;
            value = PlayerPrefsManager.Instance.GetIntPlayerPrefs(key);
            //Debug.LogError("装备id:" + value);
            //10 存武器装备
            if (value > 0)
            {
                PlayerPrefsManager.Instance.SetPlayerPrefs(key, SelectItemId);
                //更换装备需要把替换下来的装备给放到背包里
                AddPlayerItemData(value);
                //把替换上去的装备从背包删除
                RemovePlayerItemData(SelectItemId);


                //判断是否清空技能数据
                PropConfig.PropObject data1 = cfgData.GetListConfigElementByID(value);
                if (data.ItemType != data1.ItemType)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().RefreshAttackQuene(j, 0);
                    }
                }
            }
            else
            {
                PlayerPrefsManager.Instance.SetPlayerPrefs(key, SelectItemId);
            }
        }
        else if (data.ConfigType == "Armor")
        {
            string key = PlayerInfoManager.Instance.GetPlayerPrefsKey(11);
            int value = -1;
            value = PlayerPrefsManager.Instance.GetIntPlayerPrefs(key);
            //Debug.LogError("装备id:" + value);
            //11存防具装备
            if (value > 0)
            {
                PlayerPrefsManager.Instance.SetPlayerPrefs(key, SelectItemId);
                //更换装备需要把替换下来的装备给放到背包里
                AddPlayerItemData(value);
                //把替换上去的装备从背包删除
                RemovePlayerItemData(SelectItemId);
            }
            else
            {
                PlayerPrefsManager.Instance.SetPlayerPrefs(key, SelectItemId);
            }
        }
        else
        {
            Debug.LogError("这个装备在表里找不到数据");
        }

        GetEquipmentInfo();

        BagPanel._instance.CleanUp();
    }

    public int GetPlayerLevel(int exp)
    {
        int level = 0;
        //PlayerLevelExpConfig cfgData = DataTableManager.Instance.GetConfig<PlayerLevelExpConfig>("PlayerLevelExp");
        //for (int i = cfgData.playerlevelexpList.Count - 1; i >= 0; i--)
        //{
        //    if (exp >= cfgData.playerlevelexpList[i].MaxExp)
        //    {
        //        level = cfgData.playerlevelexpList[i].Level;
        //        break;
        //    }
        //}
        return level;
    }


    public string GetPlayerPrefsKey(int key)
    {
        return playerAttributeInfo[key];
    }

    //判断是否能够使用消耗品 当前血量小于最大血量或者当前经验小于最大经验
    public bool ExistIsCanUseItem()
    {
        return GetPlayerAttribute(6) < GetPlayerAttribute(7) || GetPlayerAttribute(8) < GetPlayerAttribute(9);
    }

    //判断是否能够使用消耗品 当前血量小于最大血量或者当前经验小于最大经验
    public bool UseItemAddHpAndExp(int id)
    {
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        PropConfig.PropObject data = cfgData.GetListConfigElementByID(id);
        if (playerState.PlayerLv < data.UseLevel)
        {
            return false;
        }
        bool isUse = false;// true可使用

        //增加的对应属性大于0 并且当前这个属性没有到最大值
        if (data.ConsumableHpIncrease > 0 && GetPlayerAttribute(6) < GetPlayerAttribute(7))
        {
            if (data.ConsumableHpIncrease + GetPlayerAttribute(6) < GetPlayerAttribute(7))
            {
                playerState.PlayerHpCurrent = data.ConsumableHpIncrease + GetPlayerAttribute(6);
            }
            else
            {
                playerState.PlayerHpCurrent = GetPlayerAttribute(7);
            }
            isUse = false;
        }

        if (data.ConsumableHealthIncrease > 0 && GetPlayerAttribute(12) < GetPlayerAttribute(13))
        {
            if (data.ConsumableHealthIncrease + GetPlayerAttribute(12) < GetPlayerAttribute(13))
            {
                playerState.PlayerHealth = data.ConsumableHealthIncrease + GetPlayerAttribute(12);
            }
            else
            {
                playerState.PlayerHealth = GetPlayerAttribute(13);
            }
            isUse = true;
        }
        if (isUse)
        {
            string keyHp = GetPlayerPrefsKey(6);
            string keyHealth = GetPlayerPrefsKey(12);
            PlayerPrefsManager.Instance.SetPlayerPrefs(keyHp, playerState.PlayerHpCurrent);
            PlayerPrefsManager.Instance.SetPlayerPrefs(keyHealth, playerState.PlayerHealth);
            BagPanel._instance.SetPlayerAttributeInfo();
        }
        return isUse;
        //GetPlayerAttribute(6) < GetPlayerAttribute(7) || GetPlayerAttribute(8) < GetPlayerAttribute(9);
    }

    public int GetPlayerAttribute(int i)
    {
        int count = 0;
        if (i == 1)
        {
            count = playerState.PlayerCon;
        }
        else if (i == 2)
        {
            count = playerState.PlayerStr;
        }
        else if (i == 3)
        {
            count = playerState.PlayerDex;
        }
        else if (i == 4)
        {
            count = playerState.PlayerLuk;
        }
        else if (i == 5)
        {
            count = playerState.PlayerAvaliablePoint;
        }
        else if (i == 6)
        {
            count = playerState.PlayerHpCurrent;
        }
        else if (i == 7)
        {
            count = playerState.PlayerHpMax;
        }
        else if (i == 8)
        {
            count = playerState.ExpPlayer;
        }
        else if (i == 9)
        {
            count = playerState.ExpMaxPlayer;
        }
        else if (i == 12)
        {
            count = playerState.PlayerHealth;
        }
        else if (i == 13)
        {
            count = playerState.PlayerHealthMax;
        }
        else
        {
            Debug.Log("没有这个下标对应的数据");
        }
        return count;
    }

    //返回一个背包id的集合
    public List<int> BagId()
    {
        List<int> BagID = new List<int>();
        for (int i = 0; i < playerItemData.Count; i++)
        {
            BagID.Add(playerItemData[i].PackageItemID);
        }
        return BagID;
    }
}
