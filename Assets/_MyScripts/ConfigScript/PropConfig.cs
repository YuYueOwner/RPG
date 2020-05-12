using System;
using System.Collections.Generic;
namespace HotFix_Project.Config
{
    public class PropConfig : ConfigBase
    {
        public class PropObject
        {
            public Int32 ItemID;
            public String ConfigType;
            public String ItemType;
            public String ItemName;
            public Int32 WeaponAttack;
            public Int32 WeaponDex;
            public Int32 WeaponStrength;
            public Int32 WeaponCon;
            public Int32 WeaponLuk;
            public Int32 WeaponRoll;
            public Int32 WeaponHitRate;
            public Int32 WeaponArmorPenetration;
            public Int32 WeaponCritical;
            public Int32 ArmorDefence;
            public Int32 ArmorDex;
            public Int32 ArmorStrength;
            public Int32 ArmorCon;
            public Int32 ArmorLuk;
            public Int32 ArmorRoll;
            public Int32 ArmorDodgeRate;
            public Int32 ConsumableHpIncrease;
            public Int32 ConsumableHealthIncrease;
            public Int32 Stackable;
            public Int32 StackingLimit;
            public String ItemInfomation;
            public Int32 UseLevel;
            public Int32 EquipDex;
            public Int32 EquipStrength;
            public Int32 EquipLuk;
        }

        public override void InitConfig(string[] configArr)
        {
            //从2开始是因为01是属性和字段类型
            for (int i = 2; i < configArr.Length; i++)
            {
                PropObject propObj = new PropObject();

                string str = configArr[i];
                string[] data = str.Split('|');
                propObj.ItemID = int.Parse(data[0]);
                propObj.ConfigType = data[1];
                propObj.ItemType = data[2];
                propObj.ItemName = data[3];
                propObj.WeaponAttack = int.Parse(data[4]);
                propObj.WeaponDex = int.Parse(data[5]);
                propObj.WeaponStrength = int.Parse(data[6]);
                propObj.WeaponCon = int.Parse(data[7]);
                propObj.WeaponLuk = int.Parse(data[8]);
                propObj.WeaponRoll = int.Parse(data[9]);
                propObj.WeaponHitRate = int.Parse(data[10]);
                propObj.WeaponArmorPenetration = int.Parse(data[11]);
                propObj.WeaponCritical = int.Parse(data[12]);
                propObj.ArmorDefence = int.Parse(data[13]);
                propObj.ArmorDex = int.Parse(data[14]);
                propObj.ArmorStrength = int.Parse(data[15]);
                propObj.ArmorCon = int.Parse(data[16]);
                propObj.ArmorLuk = int.Parse(data[17]);
                propObj.ArmorRoll = int.Parse(data[18]);
                propObj.ArmorDodgeRate = int.Parse(data[19]);
                propObj.ConsumableHpIncrease = int.Parse(data[20]);
                propObj.ConsumableHealthIncrease = int.Parse(data[21]);
                propObj.Stackable = int.Parse(data[22]);
                propObj.StackingLimit = int.Parse(data[23]);
                propObj.ItemInfomation = data[24];
                propObj.UseLevel = int.Parse(data[25]);
                propObj.EquipDex = int.Parse(data[26]);
                propObj.EquipStrength = int.Parse(data[27]);
                propObj.EquipLuk = int.Parse(data[28]);

                propObjList.Add(propObj);
            }
            PlayerInfoManager.Instance.SetItemInfo();
        }

        public PropConfig.PropObject GetListConfigElementByID(int id)
        {
            PropObject propObj = null;
            for (int i = 0; i < propObjList.Count; i++)
            {
                if (propObjList[i].ItemID == id)
                {
                    propObj = propObjList[i];
                }
            }
            return propObj;
        }

        //判断是否可以装备或者消耗 返回1可以装备  返回2可以消耗  返回3不可以装备
        public PropConfig.PropObject ExistIsCanConsumeByID(int id)
        {
            PropObject propObj = null;
            for (int i = 0; i < propObjList.Count; i++)
            {
                if (propObjList[i].ItemID == id)
                {
                    if (propObjList[i].ConfigType == "Consumables")
                    {
                        //可以消耗

                    }
                    else if (propObjList[i].ConfigType == "Consumables" || propObjList[i].ConfigType == "Consumables" || propObjList[i].ConfigType == "Consumables")
                    {

                    }
                }
            }
            return propObj;
        }



        public List<PropObject> propObjList = new List<PropObject>();
    }
}
