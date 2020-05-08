using System;
using System.Collections.Generic;

namespace HotFix_Project.Config
{
    public class ConsumablesConfig : ConfigBase
    {
        public class ConsumablesObject
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
                ConsumablesObject consumablesObj = new ConsumablesObject();

                string str = configArr[i];
                string[] data = str.Split('|');
                consumablesObj.Id = int.Parse(data[0]);
                consumablesObj.GoodsTpye = data[1];
                consumablesObj.GoodsName = data[2];
                consumablesObj.GoodsHpInc = int.Parse(data[3]);
                consumablesObj.GoodsHealthInc = int.Parse(data[4]);
                consumablesObj.GoodsInfo = data[5];

                consumablesList.Add(consumablesObj);
            }
        }

        public ConsumablesConfig.ConsumablesObject GetListConfigElementByID(int id)
        {
            ConsumablesObject consumablesObj = null;
            for (int i = 0; i < consumablesList.Count; i++)
            {
                if (consumablesList[i].Id == id)
                {
                    consumablesObj = consumablesList[i];
                }
            }
            return consumablesObj;
        }

        public List<ConsumablesObject> consumablesList = new List<ConsumablesObject>();


        /*以下数据是解析xml格式表的。暂时不用。暂时用上面 */
        //public override void Load(SecurityElement element)
        //{
        //    if (element.Children != null)
        //    {
        //        foreach (SecurityElement childrenElement in element.Children)
        //        {
        //            ConsumablesObject consumablesObj = new ConsumablesObject();
        //            Int32.TryParse(childrenElement.Attribute("Id"), out consumablesObj.Id);
        //            consumablesObj.GoodsTpye = childrenElement.Attribute("GoodsTpye");
        //            consumablesObj.GoodsName = childrenElement.Attribute("GoodsName");
        //            Int32.TryParse(childrenElement.Attribute("GoodsHpInc"), out consumablesObj.GoodsHpInc);
        //            Int32.TryParse(childrenElement.Attribute("GoodsHealthInc"), out consumablesObj.GoodsHealthInc);
        //            consumablesObj.GoodsInfo = childrenElement.Attribute("GoodsInfo");
        //            consumablesDic[consumablesObj.Id] = consumablesObj;
        //        }
        //    }
        //}
        //public ConsumablesConfig.ConsumablesObject GetConfigElementByID(int id)
        //{
        //    ConsumablesObject consumablesObj = null;
        //    consumablesDic.TryGetValue(id, out consumablesObj);
        //    return consumablesObj;
        //}
        //public Dictionary<int, ConsumablesObject> consumablesDic = new Dictionary<int, ConsumablesObject>();
    }
}
