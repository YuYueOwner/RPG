using System;
using System.Collections.Generic;

namespace HotFix_Project.Config
{
    public class TaskItemsConfig : ConfigBase
    {
        public class TaskItemsObject
        {
            public Int32 Id;
            public String QItemType;
            public String QItemName;
            public String QItemInfo;
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
                TaskItemsObject taskitemsObj = new TaskItemsObject();

                string str = configArr[i];
                string[] data = str.Split('|');
                taskitemsObj.Id = int.Parse(data[0]);
                taskitemsObj.QItemType = data[1];
                taskitemsObj.QItemName = data[2];
                taskitemsObj.QItemInfo = data[3];

                taskitemsList.Add(taskitemsObj);
            }
        }

        public TaskItemsConfig.TaskItemsObject GetListConfigElementByID(int id)
        {
            TaskItemsObject taskitemsObj = null;
            for (int i = 0; i < taskitemsList.Count; i++)
            {
                if (taskitemsList[i].Id == id)
                {
                    taskitemsObj = taskitemsList[i];
                }
            }
            return taskitemsObj;
        }

        public List<TaskItemsObject> taskitemsList = new List<TaskItemsObject>();



        /*以下数据是解析xml格式表的。暂时不用。暂时用上面 */
        //public override void Load(SecurityElement element)
        //{
        //    if (element.Children != null)
        //    {
        //        foreach (SecurityElement childrenElement in element.Children)
        //        {
        //            TaskItemsObject taskitemsObj = new TaskItemsObject();
        //            Int32.TryParse(childrenElement.Attribute("Id"), out taskitemsObj.Id);
        //            taskitemsObj.QItemType = childrenElement.Attribute("QItemType");
        //            taskitemsObj.QItemName = childrenElement.Attribute("QItemName");
        //            taskitemsObj.QItemInfo = childrenElement.Attribute("QItemInfo");
        //            taskitemsDic[taskitemsObj.Id] = taskitemsObj;
        //        }
        //    }
        //}

        //public TaskItemsConfig.TaskItemsObject GetConfigElementByID(int id)
        //{
        //    TaskItemsObject taskitemsObj = null;
        //    taskitemsDic.TryGetValue(id, out taskitemsObj);
        //    return taskitemsObj;
        //}

        //public Dictionary<int, TaskItemsObject> taskitemsDic = new Dictionary<int, TaskItemsObject>();
    }
}
