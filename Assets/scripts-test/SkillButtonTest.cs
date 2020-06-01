using UnityEngine;

public class SkillButtonTest : MonoBehaviour
{

    private PlayerStateManager playerStateManager;

    private void Awake()
    {
        playerStateManager = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>();
    }


    public void ResetSkill()
    {
        playerStateManager.PlayerLv = 1000;
        playerStateManager.PlayerEquipWeaponID = 1;
        playerStateManager.PlayerEquipArmorID = 16;
        playerStateManager.InitSkillExp();
        playerStateManager.InitSkillLv();
        playerStateManager.InitSkillLock();
        playerStateManager.InitSkillQuene();
        // GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().SkillLoad();
    }





    public void Unlock()
    {
        int a = Random.Range(1, 990);


        playerStateManager.SkillUnlock(a);
    }


    public void LvUp()
    {
        playerStateManager.AddSkillExp(555, 1000);
        float a = playerStateManager.SkillExp[555];
        Debug.Log("技能555获得1000经验值，总经验值为" + a);
        playerStateManager.SkillLvUp(555);
    }
}
