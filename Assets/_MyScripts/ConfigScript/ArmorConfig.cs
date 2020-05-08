using System;
using System.Collections.Generic;

namespace HotFix_Project.Config
{
    public class ArmorConfig : ConfigBase
    {
        public class ArmorObject
        {
            public Int32 Id;
            public String GoodsTpye;
            public String GoodsName;
            public Int32 GoodsHpInc;
            public Int32 GoodsHealthInc;
            public String GoodsInfo;
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
                ArmorObject armorObj = new ArmorObject();

                string str = configArr[i];
                string[] data = str.Split('|');
                armorObj.Id = int.Parse(data[0]);
                armorObj.GoodsTpye = data[1];
                armorObj.GoodsName = data[2];
                armorObj.GoodsHpInc = int.Parse(data[3]);
                armorObj.GoodsHealthInc = int.Parse(data[4]);
                armorObj.GoodsInfo = data[5];

                armorList.Add(armorObj);
            }
        }

        public ArmorConfig.ArmorObject GetListConfigElementByID(int id)
        {
            ArmorObject armorObj = null;
            for (int i = 0; i < armorList.Count; i++)
            {
                if (armorList[i].Id == id)
                {
                    armorObj = armorList[i];
                }
            }
            return armorObj;
        }

        public List<ArmorObject> armorList = new List<ArmorObject>();

        /*以下数据是解析xml格式表的。暂时不用。暂时用上面 */
        //public override void Load(SecurityElement element)
        //{
        //    if (element.Children != null)
        //    {
        //        foreach (SecurityElement childrenElement in element.Children)
        //        {
        //            ArmorObject armorObj = new ArmorObject();
        //            Int32.TryParse(childrenElement.Attribute("Id"), out armorObj.Id);
        //            armorObj.GoodsTpye = childrenElement.Attribute("GoodsTpye");
        //            armorObj.GoodsName = childrenElement.Attribute("GoodsName");
        //            Int32.TryParse(childrenElement.Attribute("GoodsHpInc"), out armorObj.GoodsHpInc);
        //            Int32.TryParse(childrenElement.Attribute("GoodsHealthInc"), out armorObj.GoodsHealthInc);
        //            armorObj.GoodsInfo = childrenElement.Attribute("GoodsInfo");
        //            armorDic[armorObj.Id] = armorObj;
        //        }
        //    }
        //}
        //public ArmorConfig.ArmorObject GetConfigElementByID(int id)
        //{
        //    ArmorObject armorObj = null;
        //    armorDic.TryGetValue(id, out armorObj);
        //    return armorObj;
        //}
        //public Dictionary<int, ArmorObject> armorDic = new Dictionary<int, ArmorObject>();
    }
}
