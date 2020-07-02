using UnityEngine;

public class SkillButtonTest : MonoBehaviour
{

    //private PlayerStateManager playerStateManager;

    //private void Awake()
    //{
    //    playerStateManager = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>();
    //}


    public void ResetSkill()
    {
        //playerStateManager.PlayerLv = 1000;
        //playerStateManager.PlayerEquipWeaponID = 1;
        //playerStateManager.PlayerEquipArmorID = 16;
        //playerStateManager.InitSkillExp();
        //playerStateManager.InitSkillLv();
        //playerStateManager.InitSkillLock();
        //playerStateManager.InitSkillQueenPlayerPrefs();
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().InitSkillExp();
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().InitSkillLv();
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().InitSkillLock();
    }




    public void Unlock()
    {
        int a = Random.Range(1, 990);


        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SkillUnlock(a);
        Debug.Log("解锁了第" + a + "个技能");
    }


    public void LvUp()
    {
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().AddSkillExp(555, 1000);
        float a = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SkillExp[555];
        Debug.Log("技能555获得1000经验值，总经验值为" + a);
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SkillLvUp(555);
    }


    //随机生成包裹中的物品，用以测试
    public void RandomPackageItem()
    {
        int a = Random.Range(1, 80);
        for (int i = 0; i < a; i++)
        {
            int b = Random.Range(1, 48);
            GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().AddLoots(b);
        }
        Debug.Log("已重新随机生成loot物品");

    }

    //随机设置当前商人种类、头像、姓名
    public void SetNpcRandom()
    {
        int a = Random.Range(1, 6);
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SetNpcType(a);
        Debug.Log("随机重设商人类型=" + a);
        int b = Random.Range(1, 3);
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SetNpcImage(b);
        Debug.Log("随机设置商人头像=" + b);
        string c = "杂货商老万" + a;
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SetNpcName(c);

        //随机玩家人物数据
        string d = "无名张三";
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SetPlayerName(d);
        int e = 50;
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SetPlayerMoney(e);
        int f = 1;
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SetPlayerImage(f);
    }


}
