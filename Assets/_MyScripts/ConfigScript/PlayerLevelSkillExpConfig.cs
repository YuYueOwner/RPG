using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace HotFix_Project.Config
{
    public class PlayerLevelSkillExpConfig : ConfigBase
    {
        public class PlayerLevelSkillExpObject
        {
            public Int32 Level;
            public Int32 MaxExp;
        }

        public override void InitConfig(string[] configArr)
        {
            //从2开始是因为01是属性和字段类型
            for (int i = 2; i < configArr.Length; i++)
            {
                PlayerLevelSkillExpObject playerLevelSkillExpObj = new PlayerLevelSkillExpObject();
                string str = configArr[i];
                string[] data = str.Split('|');
                playerLevelSkillExpObj.Level = int.Parse(data[0]);
                playerLevelSkillExpObj.MaxExp = int.Parse(data[1]);
            }
        }

        public PlayerLevelSkillExpConfig.PlayerLevelSkillExpObject GetListConfigElmentByID(int Level)
        {
            PlayerLevelSkillExpObject playerLevelSkillExpObj = null;
            for (int i = 0; i < playerLevelSkillExpList.Count; i++)
            {
                if (playerLevelSkillExpList[i].Level == Level)
                {
                    playerLevelSkillExpObj = playerLevelSkillExpList[i];
                }
            }
            return playerLevelSkillExpObj;
        }

        public List<PlayerLevelSkillExpObject> playerLevelSkillExpList = new List<PlayerLevelSkillExpObject>();
    }
}

