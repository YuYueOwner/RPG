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

    public void SetPlayerAttributeInfo()
    {
        playerAttributeInfo[1] = "Physical";
        playerAttributeInfo[2] = "Strength";
        playerAttributeInfo[3] = "Skill";
        playerAttributeInfo[4] = "Bone";
        playerAttributeInfo[5] = "PlayerAvaliable";


        playerState.PlayerCon = 5;
        playerState.PlayerStr = 5;
        playerState.PlayerDex = 5;
        playerState.PlayerLuk = 5;
        playerState.PlayerAvaliablePoint = 4;
        playerState.PlayerHpMax = 100;
        playerState.PlayerHpCurrent = 100;
        playerState.PlayerHealth = 100;
        playerState.ExpPlayer = 0;
        playerState.PlayerName = "张三";
        playerState.PlayerMoney = 0;
        playerState.PlayerLv = 1;
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
            //Debug.LogError("============" + i);
            PackageItem item = new PackageItem();
            int range = Random.Range(i, cfgData.propObjList.Count);
            PropConfig.PropObject data = cfgData.propObjList[range];
            item.PackageItemID = data.ItemID;
            item.PackageItemName = data.ItemName;
            item.PackageItemNum = i;
            playerItemData.Add(item);
        }
    }

    //删除背包里的物品
    public void RemovePlayerItemData(int id)
    {
        for (int i = 0; i < playerItemData.Count; i++)
        {
            if (playerItemData[i].PackageItemID == id)
            {
                playerItemData.Remove(playerItemData[i]);
            }
        }
    }

    //添加物品到背包里
    public void AddPlayerItemData(int id)
    {
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        PackageItem item = new PackageItem();

        PropConfig.PropObject data = cfgData.GetListConfigElementByID(id);
        item.PackageItemID = data.ItemID;
        item.PackageItemName = data.ItemName;
        item.PackageItemNum = 1;
        playerItemData.Add(item);
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
        else
        {
            Debug.Log("没有这个下标对应的数据");
        }
        return count;
    }
}
