using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HotFix_Project.Config
{
    public class MerchantGoodsConfig : ConfigBase
    {
        public class MerchantGoodsObject
        {
            public string NpcType;
            public int ItemID;
            public string ItemName;
            public int IfAvailable;
            public int ItemNum;
            public int SellPrice;
            public int BuyPrice;

        }

        public override void InitConfig(string[] configArr)
        {
            //从2开始是因为01是属性和字段类型
            for (int i = 2; i < configArr.Length; i++)
            {
                MerchantGoodsObject merchantGoodsObj = new MerchantGoodsObject();

                string str = configArr[i];
                string[] data = str.Split('|');
                merchantGoodsObj.NpcType = data[0];
                merchantGoodsObj.ItemID = int.Parse(data[1]);
                merchantGoodsObj.ItemName = data[2];
                merchantGoodsObj.IfAvailable = int.Parse(data[3]);
                merchantGoodsObj.ItemNum = int.Parse(data[4]);
                merchantGoodsObj.SellPrice = int.Parse(data[5]);
                merchantGoodsObj.BuyPrice = int.Parse(data[6]);
                merchantGoodsList.Add(merchantGoodsObj);
            }
        }

        public MerchantGoodsConfig.MerchantGoodsObject GetListConfigElementByID(int id)
        {
            MerchantGoodsObject merchantGoodsObj = null;
            for (int i = 0; i < merchantGoodsList.Count; i++)
            {
                if (merchantGoodsList[i].ItemID == id)
                {
                    merchantGoodsObj = merchantGoodsList[i];
                }
            }
            return merchantGoodsObj;
        }

        public List<MerchantGoodsObject> merchantGoodsList = new List<MerchantGoodsObject>();
    }
}