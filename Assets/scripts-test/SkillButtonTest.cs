﻿using UnityEngine;

public class SkillButtonTest : MonoBehaviour
{


    public void ResetSkill()
    {
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().PlayerEquipWeaponID = 1;
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().PlayerEquipArmorID = 16;
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().InitSkillExp();
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().InitSkillLv();
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().InitSkillLock();
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().InitSkillQuene();
        // GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SkillLoad();
    }





    public void Unlock()
    {
        int a = Random.Range(1, 990);


        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SkillUnlock(a);
    }


    public void LvUp()
    {
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().AddSkillExp(555, 1000);
        float a = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SkillExp[555];
        Debug.Log("技能555获得1000经验值，总经验值为" + a);
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SkillLvUp(555);
    }



    //假数据
    public void SetWeapon()
    {
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().PlayerEquipWeaponID = 1;
        GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().PlayerEquipArmorID = 16;
    }






}
