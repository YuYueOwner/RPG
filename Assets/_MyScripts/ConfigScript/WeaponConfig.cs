using System;
using System.Collections.Generic;

namespace HotFix_Project.Config
{
    public class WeaponConfig : ConfigBase
    {
        public class WeaponObject
        {
            public Int32 Id;
            public String WeaponType;
            public String WeaponName;
            public Int32 WeaponAtk;
            public Int32 WeaponDex;
            public Int32 WeaponStr;
            public Int32 WeaponCon;
            public Int32 WeaponLuk;
            public Int32 WeaponRoll;
            public Int32 WeaponHitRate;
            public Int32 WeaponArmorPene;
            public Int32 WeaponCrit;
            public String WeaponInfo;
        }

        public override void InitConfig(string[] configArr)
        {
            //string sb = configArr[0];//属性
            //string sb1 = configArr[1];//字段类型
            //string[] arr = sb.Split('|');
            //foreach (var item in arr)
            //{
            //    Debug.Log("属性名称:" + item);
            //}

            //从2开始是因为01是属性和字段类型
            for (int i = 2; i < configArr.Length; i++)
            {
                WeaponObject weaponObj = new WeaponObject();

                string str = configArr[i];
                string[] data = str.Split('|');
                weaponObj.Id = int.Parse(data[0]);
                weaponObj.WeaponType = data[1];
                weaponObj.WeaponName = data[2];
                weaponObj.WeaponAtk = int.Parse(data[3]);
                weaponObj.WeaponDex = int.Parse(data[4]);
                weaponObj.WeaponStr = int.Parse(data[5]);
                weaponObj.WeaponCon = int.Parse(data[6]);
                weaponObj.WeaponLuk = int.Parse(data[7]);
                weaponObj.WeaponRoll = int.Parse(data[8]);
                weaponObj.WeaponHitRate = int.Parse(data[9]);
                weaponObj.WeaponArmorPene = int.Parse(data[10]);
                weaponObj.WeaponCrit = int.Parse(data[11]);
                weaponObj.WeaponInfo = data[12];

                weaponList.Add(weaponObj);
            }
        }

        public WeaponConfig.WeaponObject GetListConfigElementByID(int id)
        {
            WeaponObject weaponObj = null;
            for (int i = 0; i < weaponList.Count; i++)
            {
                if (weaponList[i].Id == id)
                {
                    weaponObj = weaponList[i];
                }
            }
            return weaponObj;
        }

        public List<WeaponObject> weaponList = new List<WeaponObject>();



        /*以下数据是解析xml格式表的。暂时不用。暂时用上面 */
        //public override void Load(SecurityElement element)
        //{
        //    if (element.Children != null)
        //    {
        //        foreach (SecurityElement childrenElement in element.Children)
        //        {
        //            WeaponObject weaponObj = new WeaponObject();
        //            Int32.TryParse(childrenElement.Attribute("Id"), out weaponObj.Id);
        //            weaponObj.WeaponType = childrenElement.Attribute("WeaponType");
        //            weaponObj.WeaponName = childrenElement.Attribute("WeaponName");
        //            Int32.TryParse(childrenElement.Attribute("WeaponAtk"), out weaponObj.WeaponAtk);
        //            Int32.TryParse(childrenElement.Attribute("WeaponDex"), out weaponObj.WeaponDex);
        //            Int32.TryParse(childrenElement.Attribute("WeaponStr"), out weaponObj.WeaponStr);
        //            Int32.TryParse(childrenElement.Attribute("WeaponCon"), out weaponObj.WeaponCon);
        //            Int32.TryParse(childrenElement.Attribute("WeaponLuk"), out weaponObj.WeaponLuk);
        //            Int32.TryParse(childrenElement.Attribute("WeaponRoll"), out weaponObj.WeaponRoll);
        //            Int32.TryParse(childrenElement.Attribute("WeaponHitRate"), out weaponObj.WeaponHitRate);
        //            Int32.TryParse(childrenElement.Attribute("WeaponArmorPene"), out weaponObj.WeaponArmorPene);
        //            Int32.TryParse(childrenElement.Attribute("WeaponCrit"), out weaponObj.WeaponCrit);
        //            weaponObj.WeaponInfo = childrenElement.Attribute("WeaponInfo");
        //            weaponDic[weaponObj.Id] = weaponObj;
        //        }
        //    }
        //}
        //public WeaponConfig.WeaponObject GetConfigElementByID(int id)
        //{
        //    WeaponObject weaponObj = null;
        //    weaponDic.TryGetValue(id, out weaponObj);
        //    return weaponObj;
        //}
        //public Dictionary<int, WeaponObject> weaponDic = new Dictionary<int, WeaponObject>();
    }
}
