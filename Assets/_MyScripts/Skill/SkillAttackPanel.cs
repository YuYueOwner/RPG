using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackPanel : UIScene
{
    private UIButton Attack_Button;
    private UIButton Defend_Button;


    private void Awake()
    {
        Attack_Button = Helper.GetChild<UIButton>(this.transform, "Attack_Button");
        Defend_Button = Helper.GetChild<UIButton>(this.transform, "Defend_Button");

    }
    protected override void Start()
    {
        base.Start();
        Attack_Button.onClick.Add(new EventDelegate(Attack));
        Defend_Button.onClick.Add(new EventDelegate(Defend));
    }

    //攻
    private void Attack()
    {

    }

    //守
    private void Defend()
    {

    }
}
