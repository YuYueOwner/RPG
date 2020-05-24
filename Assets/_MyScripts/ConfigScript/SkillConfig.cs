using System;
using System.Collections.Generic;
namespace HotFix_Project.Config
{
    public class SkillConfig : ConfigBase
    {
        public class SkillObject
        {
            public Int32 SkillID;
            public String SkillType;
            public String SkillQuality;
            public String SkillLv;
            public String SkillName;
            public Int32 SkillTime;
            public Int32 SkillAtk;
            public Int32 SkillRoll;
            public Int32 SkillHitRate;
            public Int32 SkillDex;
            public Int32 SkillArmorPen;
            public Int32 SkillCrit;
            public Int32 UseLv;
            public Int32 DefenceSkillDodge;
            public Int32 DefenceSkillRoll;
            public Int32 DefenceSkillAtk;
            public Int32 DefenceSkillDef;
            public string SkillInfo;
        }

        public override void InitConfig(string[] configArr)
        {
            //从2开始是因为01是属性和字段类型
            for (int i = 2; i < configArr.Length; i++)
            {
                SkillObject skillObj = new SkillObject();
                string str = configArr[i];
                string[] data = str.Split('|');
                skillObj.SkillID = int.Parse(data[0]);
                skillObj.SkillType = data[1];
                skillObj.SkillQuality = data[2];
                skillObj.SkillLv = data[3];
                skillObj.SkillName = data[4];
                skillObj.SkillTime = int.Parse(data[5]);
                skillObj.SkillAtk = int.Parse(data[6]);
                skillObj.SkillRoll = int.Parse(data[7]);
                skillObj.SkillHitRate = int.Parse(data[8]);
                skillObj.SkillDex = int.Parse(data[9]);
                skillObj.SkillArmorPen = int.Parse(data[10]);
                skillObj.SkillCrit = int.Parse(data[11]);
                skillObj.UseLv = int.Parse(data[12]);
                skillObj.DefenceSkillDodge = int.Parse(data[13]);
                skillObj.DefenceSkillRoll = int.Parse(data[14]);
                skillObj.DefenceSkillAtk = int.Parse(data[15]);
                skillObj.DefenceSkillDef = int.Parse(data[16]);
                skillObj.SkillInfo = data[17];
                skillObjList.Add(skillObj);
            }
        }

        //根据物品id获取这个物品整条数据
        public SkillConfig.SkillObject GetListConfigElementByID(int id)
        {
            SkillObject skillObj = null;
            for (int i = 0; i < skillObjList.Count; i++)
            {
                if (skillObjList[i].SkillID == id)
                {
                    skillObj = skillObjList[i];
                }
            }
            return skillObj;
        }

        public List<SkillObject> skillObjList = new List<SkillObject>();


    }
}
