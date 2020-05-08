using System;
using System.Collections.Generic;
namespace HotFix_Project.Config
{
    public class MaterialsConfig : ConfigBase
    {
        public class MaterialsObject
        {
            public Int32 Id;
            public String MaterialType;
            public String MaterialName;
            public String MaterialInfo;
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
                MaterialsObject materialsObj = new MaterialsObject();

                string str = configArr[i];
                string[] data = str.Split('|');
                materialsObj.Id = int.Parse(data[0]);
                materialsObj.MaterialType = data[1];
                materialsObj.MaterialName = data[2];
                materialsObj.MaterialInfo = data[3];

                materialsList.Add(materialsObj);
            }
        }

        public MaterialsConfig.MaterialsObject GetListConfigElementByID(int id)
        {
            MaterialsObject materialsObj = null;
            for (int i = 0; i < materialsList.Count; i++)
            {
                if (materialsList[i].Id == id)
                {
                    materialsObj = materialsList[i];
                }
            }
            return materialsObj;
        }

        public List<MaterialsObject> materialsList = new List<MaterialsObject>();



        /*以下数据是解析xml格式表的。暂时不用。暂时用上面 */
        //public override void Load(SecurityElement element)
        //{
        //    if (element.Children != null)
        //    {
        //        foreach (SecurityElement childrenElement in element.Children)
        //        {
        //            MaterialsObject materialsObj = new MaterialsObject();
        //            Int32.TryParse(childrenElement.Attribute("Id"), out materialsObj.Id);
        //            materialsObj.MaterialType = childrenElement.Attribute("MaterialType");
        //            materialsObj.MaterialName = childrenElement.Attribute("MaterialName");
        //            materialsObj.MaterialInfo = childrenElement.Attribute("MaterialInfo");
        //            materialsDic[materialsObj.Id] = materialsObj;
        //        }
        //    }
        //}
        //public MaterialsConfig.MaterialsObject GetConfigElementByID(int id)
        //{
        //    MaterialsObject materialsObj = null;
        //    materialsDic.TryGetValue(id, out materialsObj);
        //    return materialsObj;
        //}
        //public Dictionary<int, MaterialsObject> materialsDic = new Dictionary<int, MaterialsObject>();
    }
}
