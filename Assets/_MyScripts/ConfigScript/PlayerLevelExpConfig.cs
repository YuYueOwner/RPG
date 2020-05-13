using System;
using System.Collections.Generic;
namespace HotFix_Project.Config
{
    public class PlayerLevelExpConfig : ConfigBase
    {
        public class PlayerLevelExpObject
        {
            public Int32 Level;
            public Int32 MaxExp;
        }

        public override void InitConfig(string[] configArr)
        {
            //从2开始是因为01是属性和字段类型
            for (int i = 2; i < configArr.Length; i++)
            {
                PlayerLevelExpObject playerlevelexpObj = new PlayerLevelExpObject();

                string str = configArr[i];
                string[] data = str.Split('|');
                playerlevelexpObj.Level = int.Parse(data[0]);
                playerlevelexpObj.MaxExp = int.Parse(data[1]);

                playerlevelexpList.Add(playerlevelexpObj);
            }
        }

        public PlayerLevelExpConfig.PlayerLevelExpObject GetListConfigElementByID(int Level)
        {
            PlayerLevelExpObject playerlevelexpObj = null;
            for (int i = 0; i < playerlevelexpList.Count; i++)
            {
                if (playerlevelexpList[i].Level == Level)
                {
                    playerlevelexpObj = playerlevelexpList[i];
                }
            }
            return playerlevelexpObj;
        }

        public List<PlayerLevelExpObject> playerlevelexpList = new List<PlayerLevelExpObject>();
    }
}
