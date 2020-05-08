using System;
using System.Collections.Generic;

namespace HotFix_Project.Config
{
    public class ActivityMainConfig : ConfigBase
    {
        public class ActivityMainObject
        {
            public String Id;
            public String type;
            public String icon;
            public String name;
            public String description_big;
            public String description;
        }

        public override void InitConfig(string[] configArr)
        {
            //从2开始是因为01是属性和字段类型
            for (int i = 2; i < configArr.Length; i++)
            {
                ActivityMainObject activitymainObj = new ActivityMainObject();

                string str = configArr[i];
                string[] data = str.Split('|');
                activitymainObj.Id = data[0];
                activitymainObj.type = data[1];
                activitymainObj.icon = data[2];
                activitymainObj.name = data[3];
                activitymainObj.description_big = data[4];
                activitymainObj.description = data[5];

                activitymainList.Add(activitymainObj);
            }

            //for (int i = 0; i < activitymainList.Count; i++)
            //{
            //    //Debug.LogError("数据:" + activitymainList[i].Id);
            //    Debug.Log("测试数据:" + activitymainList[i].Id);
            //}
        }

        public ActivityMainConfig.ActivityMainObject GetListConfigElementByID(string id)
        {
            ActivityMainObject activitymainObj = null;
            for (int i = 0; i < activitymainList.Count; i++)
            {
                if (activitymainList[i].Id == id)
                {
                    activitymainObj = activitymainList[i];
                }
            }
            return activitymainObj;
        }

        public List<ActivityMainObject> activitymainList = new List<ActivityMainObject>();

        //       public override bool Load(SecurityElement element)
        //{
        //  if (element.Tag != "Items")
        //  {
        //      return false;
        //  }
        //  if (element.Children != null)
        //  {
        //      foreach (SecurityElement childrenElement in element.Children)
        //      {
        //          ActivityMainObject activitymainObj = new ActivityMainObject();
        //          Int32.TryParse(childrenElement.Attribute("Id"), out activitymainObj.Id);
        //          activitymainObj.GoodsTpye = childrenElement.Attribute("GoodsTpye");
        //          activitymainObj.GoodsName = childrenElement.Attribute("GoodsName");
        //          Int32.TryParse(childrenElement.Attribute("GoodsHpInc"), out activitymainObj.GoodsHpInc);
        //          Int32.TryParse(childrenElement.Attribute("GoodsHealthInc"), out activitymainObj.GoodsHealthInc);
        //          activitymainObj.GoodsInfo = childrenElement.Attribute("GoodsInfo");
        //          activitymainDic[activitymainObj.Id]=  activitymainObj;
        //      }
        //  }
        //  else
        //  {
        //      return false;
        //  }
        //  return true;
        //}
        //public ActivityMainConfig .ActivityMainObject GetConfigElementByID(int id)
        //{
        //  ActivityMainObject activitymainObj= null;
        //  activitymainDic.TryGetValue(id, out activitymainObj);
        //  return activitymainObj;
        //}
        //public Dictionary<int, ActivityMainObject> activitymainDic = new Dictionary<int, ActivityMainObject>();
    }
}
