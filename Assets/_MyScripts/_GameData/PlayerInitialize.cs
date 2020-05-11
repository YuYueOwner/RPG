using UnityEngine;

public class PlayerInitialize : MonoBehaviour
{
    private void Start()
    {
        //PlayerState players = PlayerInfoManager.Instance.playerState;

        //players.PlayerCon = 5;
        //players.PlayerStr = 5;
        //players.PlayerDex = 5;
        //players.PlayerLuk = 5;
        //players.PlayerAvaliablePoint = 4;
        //players.PlayerHpMax = 100;
        //players.PlayerHpCurrent = 100;
        //players.PlayerHealth = 100;
        //players.ExpPlayer = 0;
        //players.PlayerName = "张三";
        //players.PlayerMoney = 0;
        //players.PlayerLv = 1;
        //players.PlayerHeadPhotoID = 1;
        //players.PlayerFullPhotoID = 1;
        //players.PlayerEquipWeaponID = 1;
        //players.PlayerEquipArmorID = 1;

    }

    //1、初始化时生命值上限PlayerHpMax=当前生命值PlayerHpCurrent，后续当前生命值PlayerHpCurrent会根据战斗或其他情况进行调整，需要一个{get;set;}的构造。
    //2、玩家人物经验值ExpPlayer与等级经验值表进行比对，达到当前等级升级经验值时，人物等级PlayerLv+1，玩家人物经验值ExpPlayer=0，重新开始累积。
    //3、玩家武器：初始装备乌木剑，WeaponID=1。
    //4、玩家防具：初始装备粗布甲，ArmorID=1。



}
