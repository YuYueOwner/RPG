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
    //鼠标长留物品0.5秒后显示的物品名称
    public UILabel ItemName;
    //鼠标长留物品0.5秒后显示的物品介绍
    public UILabel ItemDesc;
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
        playerAttributeInfo[10] = "Equip";//身上的装备id
        playerAttributeInfo[11] = "Equip1";//身上的装备id1

        playerState.PlayerCon = 100;
        playerState.PlayerStr = 100;
        playerState.PlayerDex = 100;
        playerState.PlayerLuk = 100;
        playerState.PlayerAvaliablePoint = 4;
        playerState.PlayerHpMax = 100;
        playerState.PlayerHpCurrent = 100;
        playerState.PlayerHealth = 100;
        playerState.ExpPlayer = 1000;
        playerState.ExpMaxPlayer = 10000;
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
                PropConfig.PropObject data = cfgData.propObjList[id];
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
                    }
                }
                else
                {
                    playerItemData.Remove(playerItemData[i]);
                }
            }
        }
        BagPanel._instance.CleanUp();
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
        switch (data.ConfigType)
        {
            case "Weapon":
                ItemName.text = data.ItemName + "\r\n" + "攻击力         " + data.WeaponAttack + "\r\n" +
                                                                                  "身法加成      " + data.WeaponDex + "\r\n" +
                                                                                   "力道加成      " + data.WeaponStrength + "\r\n" +
                                                                                   "体质加成      " + data.WeaponStrength + "\r\n";

                break;

            default:
                ItemName.text = data.ItemName;
                break;
        }
        Debug.LogError(data.ItemID);
        ItemDesc.text = data.ItemInfomation;
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
        Debug.LogError("SelectItemId" + SelectItemId);
        string key = PlayerInfoManager.Instance.GetPlayerPrefsKey(10);
        int value = -1;
        value = PlayerPrefsManager.Instance.GetIntPlayerPrefs(key);
        Debug.LogError("装备id:" + value);
        //储存装备先存10在存11.先判断10是否有装备，有的话找11是否有装备。有的话替换10.
        if (value > 0)
        {
            string key1 = PlayerInfoManager.Instance.GetPlayerPrefsKey(11);
            int value1 = PlayerPrefsManager.Instance.GetIntPlayerPrefs(key1);
            if (value1 > 0)
            {
                PlayerPrefsManager.Instance.SetPlayerPrefs(key, SelectItemId);
                //更换装备需要把替换下来的装备给放到背包里
                AddPlayerItemData(value);
                //把替换上去的装备从背包删除
                RemovePlayerItemData(SelectItemId);
            }
            else
            {
                PlayerPrefsManager.Instance.SetPlayerPrefs(key1, SelectItemId);
            }
        }
        else
        {
            PlayerPrefsManager.Instance.SetPlayerPrefs(key, SelectItemId);
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
            count = playerState.PlayerHpCurrent;
        }
        else if (i == 6)
        {
            count = playerState.PlayerHpMax;
        }
        else if (i == 7)
        {
            count = playerState.ExpPlayer;
        }
        else if (i == 8)
        {
            count = playerState.ExpMaxPlayer;
        }
        else
        {
            Debug.Log("没有这个下标对应的数据");
        }
        return count;
    }
}
