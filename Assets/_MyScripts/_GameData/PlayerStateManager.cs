using HotFix_Project.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//角色数据中转站
public class PlayerStateManager : MonoBehaviour
{
    public int PlayerNum;//当前所有角色个数

    public int PlayerID;//人物ID
    public string PlayerName;//人物姓名
    public string PlayerSex;//性别male/female
    public int PlayerLv;//人物等级
    public int PlayerAavailable;//当前人物是否可用，1表示可用，0表示不可用

    public int PlayerCon;//体质
    public int PlayerStr;//力道
    public int PlayerDex;//身法
    public int PlayerLuk;//根骨

    public int PlayerAvaliablePoint;//可用属性点
    public int PlayerHpMax;//最大生命值
    public int PlayerHpCurrent;//当前生命值
    public int PlayerHealth;//健康值，范围1-100
    public int PlayerHealthMax;
    public int ExpPlayer;//人物经验值
    public int ExpMaxPlayer;
    public int PlayerHeadPhotoID;//人物头像ID.1-10
    public int PlayerFullPhotoID;//人物立绘ID,1-10    
    public int PlayerMoney;//人物银两数

    public int PlayerEquipWeaponID;//已装备武器ID，初始装备乌木剑，WeaponID=1
    public int PlayerEquipArmorID;//已装备护甲ID，初始装备粗布甲，ArmorID=1
    //武器数据
    public int WeaponAttack;
    public int WeaponDex;
    public int WeaponStrength;
    public int WeaponCon;
    public int WeaponLuk;
    public int WeaponRoll;
    public int WeaponHitRate;
    public int WeaponArmorPenetration;
    public int WeaponCritical;
    //护甲数据
    public int ArmorDefence;
    public int ArmorDex;
    public int ArmorStrength;
    public int ArmorCon;
    public int ArmorLuk;
    public int ArmorRoll;
    public int ArmorDodgeRate;

    //面板显示数据total+X
    public int totalCon;//面板体质
    public int totalStr;//面板力道
    public int totalDex;//面板身法
    public int totalLuk;//面板根骨

    public float PlayerWorldPosX;//人物在世界地图中坐标X
    public float PlayerWorldPosY;//人物在世界地图中坐标Y

    public float[] SkillExp = new float[1000];//技能经验表
    public float[] SkillLv = new float[1000];//技能等级表
    public float[] SkillLock = new float[1000];//技能解锁表
    public float[] SkillExpMax = new float[3] { 2000, 10000, 30000 }; //技能升级最大经验值，1升2需2k，2升3需10k,3升4需30k
    public int[] PlayerExpMax = new int[30] { 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1200, 1400, 1600, 1800, 2000, 2200, 2400, 2600, 2800, 3000, 3300, 3600, 3900, 4200, 4500, 4800, 5100, 5400, 5700, 6000, 99999 };

    public int[] AttackQuene = new int[8];//攻击技能序列
    public int[] DefenceQuene = new int[8];//防御技能序列

    public int[] PackageItem = new int[80];
    public int[] PackageNum = new int[80];
    public List<int> aa = new List<int>();//背包物品list
    public List<int> blootlist = new List<int>();//战利品list
    public List<int> cc = new List<int>();//中转list，用于储存战利品清单中未放完的战利品
    public int Packagenums = 0;

    public int isNew;//下一次打开背包是否重置数据，1重置，0不重置
    public List<PackageItem> playerItemData = new List<PackageItem>();

    public int NpcType;//NpcType=1，装备商人1；=2，装备商人2；=3，药品商人1；=4，药品商人2；=5，杂货商人1；=6，杂货商人2
    public int NpcImageID;//Npc头像ID
    public string NpcName;//Npc姓名


    private void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        PlayerStateLoad();

        SkillLoad();
        // SetPlayUseTime();
        RecountPlayerState();
        ChangePlayerState();

    }
    //假数据
    void SetWeapon()
    {
        PlayerEquipWeaponID = 1;
        PlayerEquipArmorID = 16;
    }





    //测试数据读写
    void OnlyTest()
    {
        float[] a = new float[] { 11, 22, 33, 44, 55, 66, 77, 88, 99, 100 };
        //for(int i = 1; i < 11; i++)
        //{
        //    a[i] = i;
        //}
        SetFloatArray("SkillExp", a);           //把数组a[]中的数据以标签"SkillExp"存入PlayerPerfs中
        float[] b = new float[10];
        b = GetFloatArray("SkillExp");          //把"SkillExp"标签下的数据取出，存到b[]数组中


        for (int i = 0; i < b.Length; i++)
        {
            Debug.Log("b" + i + "=" + b[i]);
        }
    }
    //=======================================
    //同步玩家已装备物品信息
    //同步武器和防具ID
    public void RefreshPlayerEquipmentID(int weaponID, int armorID)
    {
        PlayerEquipWeaponID = weaponID;
        PlayerEquipArmorID = armorID;
        //重新计算武器及防具加成
        GetEquipmentInfo();
        //重新计算面板属性
        ChangePlayerState();
        //更新显示
        BagPanel._instance.SetPlayerAttributeInfo();
    }
    //根据武器和防具ID查找具体数据
    public void GetEquipmentInfo()
    {
        //根据武器ID和防具ID在静态数据表中获取到对应的数据
        PropConfig cfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        PropConfig.PropObject weapondata = cfgData.GetListConfigElementByID(PlayerEquipWeaponID);
        PropConfig.PropObject Armordata = cfgData.GetListConfigElementByID(PlayerEquipArmorID);
        //武器数据
        if (PlayerEquipWeaponID > 0)
        {
            WeaponAttack = weapondata.WeaponAttack;
            WeaponDex = weapondata.WeaponDex;
            WeaponStrength = weapondata.WeaponStrength;
            WeaponCon = weapondata.WeaponCon;
            WeaponLuk = weapondata.WeaponLuk;
            WeaponRoll = weapondata.WeaponRoll;
            WeaponHitRate = weapondata.WeaponHitRate;
            WeaponArmorPenetration = weapondata.WeaponArmorPenetration;
            WeaponCritical = weapondata.WeaponCritical;
        }
        //护甲数据
        if (PlayerEquipArmorID > 0)
        {
            ArmorDefence = Armordata.ArmorDefence;
            ArmorDex = Armordata.ArmorDex;
            ArmorStrength = Armordata.ArmorStrength;
            ArmorCon = Armordata.ArmorCon;
            ArmorLuk = Armordata.ArmorLuk;
            ArmorRoll = Armordata.ArmorRoll;
            ArmorDodgeRate = Armordata.ArmorDodgeRate;
        }
    }



    //============================
    //控制money
    public void ChangeMoneyPlayer(int money)
    {
        PlayerMoney = PlayerMoney + money;
        Debug.Log("获得了" + money + "两银子。");
    }


    //==============================================================================================================================
    //初始化物品清单，初始化ID及Num
    public void InitPackageItem()
    {
        int[] a = new int[80];
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = 0;
        }
        SetIntArray("PackageItem", a);
        SetIntArray("PackageNum", a);
    }

    //保存包裹物品清单至中转站
    //public void SavePackageItem1()
    //{
    //    List<int> a = PlayerInfoManager.Instance.BagId();
    //    List<int> b = PlayerInfoManager.Instance.BagNum();
    //    aa.Clear();
    //    for (int i = 0; i < a.Count; i++)
    //    {
    //        for (int j = 0; j < b[i]; j++)
    //        {
    //            aa.Add(a[i]);
    //        }
    //    }
    //    foreach (var item in aa)
    //    {
    //        Debug.Log(item);
    //    }
    //}

    //向中转站list中添加物品，测试按钮用
    public void AddItemNew(int id)
    {
        if (PlayerInfoManager.Instance.playerItemData.Count < 80)
        {
            aa.Add(id);
            PlayerInfoManager.Instance.SetItemInfo();
        }
        else
        {
            Debug.LogError("包裹已满，放入物品失败");
        }
    }


    //保存中转站的物品清单list至PlayerPrefs中
    public void SavePackageItem()
    {
        int[] array = aa.ToArray();
        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log("array=" + array[i]);
        }
        SetIntArray("PackageItem", array);
    }

    //从读取PlayerPrefs中包裹物品清单至中转站的list中
    public void LoadPackageItem()
    {

        PackageItem = GetIntArray("PackageItem");
        aa.Clear();
        int n = PackageItem.Length;
        for (int i = 0; i < n; i++)
        {
            aa.Add(PackageItem[i]);
        }
    }
    //判断当前装备是否可装备 true可以使用
    public bool CheckSkillIsCanUse(int id, int i)
    {
        PlayerLv = 5;
        //Debug.LogError("背包里的装备:" + PlayerEquipWeaponID);
        SkillConfig cfgData = DataTableManager.Instance.GetConfig<SkillConfig>("Skill");
        SkillConfig.SkillObject data = cfgData.GetListConfigElementByID(id);
        PropConfig propCfgData = DataTableManager.Instance.GetConfig<PropConfig>("Prop");
        PropConfig.PropObject propData = propCfgData.GetListConfigElementByID(PlayerEquipWeaponID);

        if ((data.SkillType == "刀" || data.SkillType == "剑" || data.SkillType == "枪" || data.SkillType == "棍" || data.SkillType == "叉" || data.SkillType == "锤"))
        {
            if (PlayerEquipWeaponID > 0)
                if (data.SkillType == propData.ItemType)
                {
                    {
                        //背包里有武器装备  可以装备技能
                        if (PlayerLv >= data.UseLv)
                        {
                            //Debug.LogError(i + "      " + id);
                            RefreshAttackQuene(i, id);
                            return true;
                        }
                    }
                }
        }
        else
        {
            if (PlayerLv >= data.UseLv)
            {
                RefreshDefenceQuene(i, id);
                return true;
            }
        }
        return false;
    }
    //更新包裹物品清单
    //每当有新物品放入，先用PlayerInfoManager.Instance.AddPackageItem()放到playerItemData中，再用PlayerInfoManager.Instance.BagId()读出数据，用SavePackageItem()存到PlayerPrefs中，最后用LoadPackageItem()读取到PackageItem里
    public void RefreshPackageItem()
    {
        SavePackageItem();
        LoadPackageItem();

    }
    //向战利品清单blootlist中放入战利品
    public void AddLoots(int id)
    {
        blootlist.Add(id);
    }
    //背包中可用格子，只有ID在32-47的物品可以叠加
    public void PackageUsable()
    {
        Packagenums = 0;
        List<int> aa = GameObject.Find("PlayerState").GetComponent<PlayerStateManager>().aa;
        Hashtable ht = new Hashtable();

        for (int i = 0; i < aa.Count; i++)
        {
            if (aa[i] >= 32 && aa[i] <= 47)
            {
                if (ht.ContainsKey(aa[i]))
                {
                    ht[aa[i]] = int.Parse(ht[aa[i]].ToString()) + 1;
                }
                else
                {
                    ht.Add(aa[i], 1);
                }
            }
            else
            {
                Packagenums++;
            }
        }
        // 再将hashTable里面的键和值相应的输出来
        IDictionaryEnumerator ie = ht.GetEnumerator();

        while (ie.MoveNext())
        {
            Packagenums++;
            //Debug.Log(ie.Key.ToString() + "记录条数：" + ie.Value);
        }
        Debug.Log("共有" + Packagenums + "个格子被占用");
    }

    //往背包中放入战利品
    public void AddPackageItem()
    {
        for (int i = 0; i < blootlist.Count; i++)
        {
            PackageUsable();
            if (Packagenums < 80)
            {
                aa.Add(blootlist[i]);
                Debug.Log("向包裹中放入物品ID=" + blootlist[i]);
                PackageUsable();
                Debug.Log("背包中共有" + Packagenums + "件物品");
            }
            else
            {
                Debug.Log("包裹已满，战利品清单第" + (i + 1) + "件物品放入失败，存入中转list中");
                cc.Add(blootlist[i]);
            }
        }
        blootlist.Clear();
    }
    //===============================================================================================================================
    //解锁格子数量
    public int UnLockNum()
    {
        int num = 0;
        num = 5 + (PlayerLv - 1) / 5;
        return num;
    }
    //===============================================================================================================================
    //技能经验表"SkillExp"
    //技能解锁表"Skill"
    //初始化技能经验表
    public void InitSkillExp()
    {
        //假设当前有1000个技能，将1000个技能的经验值全部设置为0
        float[] a = new float[1000];
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = 0;
        }
        SetFloatArray("SkillExp", a);
    }

    //将所有技能等级设置为1
    public void InitSkillLv()
    {
        //假设当前有1000个技能，将1000个技能的等级全部设置为1
        float[] a = new float[1000];
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = 1;
        }
        SetFloatArray("SkillLv", a);
    }

    //随机生成技能  测试用 type =attack攻击技能
    public Dictionary<string, List<int>> OnCreateSkill(string type)
    {
        //测试用 key 是技能类型 技能Id
        Dictionary<string, List<int>> skillIdDic = new Dictionary<string, List<int>>();
        SkillConfig cfgData = DataTableManager.Instance.GetConfig<SkillConfig>("Skill");
        //初始化30个数据
        List<int> skillData;
        //for (int i = 0; i < 30; i++)
        //{
        //    int id = UnityEngine.Random.Range(i, 30);
        //    SkillConfig.SkillObject data = cfgData.GetListConfigElementByID(id);
        //    if (skillIdDic.TryGetValue(data.SkillType, out skillData))
        //    {

        //    }
        //    else
        //    {
        //        skillData = new List<int>();
        //        skillIdDic[data.SkillType] = skillData;
        //    }

        //    skillData.Add(id);
        //}
        //正式数据
        for (int i = 0; i < SkillLock.Length; i++)
        {
            int index = (int)SkillLock[i];
            if (index > 0)
            {
                int id = i;
                SkillConfig.SkillObject data = cfgData.GetListConfigElementByID(id);
                //Debug.LogError(i + "       " + id);
                if (type == "attack")
                {
                    //攻击技能
                    if (data.SkillType == "刀" || data.SkillType == "剑" || data.SkillType == "枪" || data.SkillType == "棍" || data.SkillType == "叉" || data.SkillType == "锤")
                    {

                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    //防御技能
                    if (data.SkillType == "刀" || data.SkillType == "剑" || data.SkillType == "枪" || data.SkillType == "棍" || data.SkillType == "叉" || data.SkillType == "锤")
                    {
                        continue;
                    }
                }

                if (skillIdDic.TryGetValue(data.SkillType, out skillData))
                {

                }
                else
                {
                    skillData = new List<int>();
                    skillIdDic[data.SkillType] = skillData;
                }
                skillData.Add(id);
            }
        }
        return skillIdDic;
    }    //将所有技能设置为未解锁，0=未解锁，1=解锁，然后初始化技能，将基础技能设置为解锁
    public void InitSkillLock()
    {
        //假设当前有1000个技能，将1000个技能的全部设置为未解锁
        float[] a = new float[1000];
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = 0;
            switch (i)
            {
                case 1:
                    a[i] = 1;
                    break;
                case 169:
                    a[i] = 1;
                    break;
                case 333:
                    a[i] = 1;
                    break;
                case 497:
                    a[i] = 1;
                    break;
                case 657:
                    a[i] = 1;
                    break;
                case 817:
                    a[i] = 1;
                    break;
                case 977:
                    a[i] = 1;
                    break;
                case 985:
                    a[i] = 1;
                    break;
            }
        }
        SetFloatArray("SkillLock", a);
    }

    //初始化，将进攻技能列表和防御技能列表id都设置为0
    public void InitSkillQueen()
    {
        int[] a = new int[8];
        for (int i = 0; i < 8; i++)
        {
            a[i] = 0;
        }
        AttackQuene = a;
        DefenceQuene = a;
        Debug.Log("技能序列初始化完毕");
    }

    //从PlayerPrefs中读取技能数据至中转站
    public void SkillLoad()
    {
        SkillExp = GetFloatArray("SkillExp");//技能经验
        SkillLv = GetFloatArray("SkillLv");//技能等级
        SkillLock = GetFloatArray("SkillLock");//技能是否解锁
        AttackQuene = GetIntArray("AttackQuene");//攻击序列
        DefenceQuene = GetIntArray("DefenceQuene");//防御序列
    }

    //将中转站中技能数据储存至PlayerPrefs中
    public void SkillSave()
    {
        SetFloatArray("SkillExp", SkillExp);//技能经验
        SetFloatArray("SkillLv", SkillLv);//技能等级
        SetFloatArray("SkillLock", SkillLock);//技能是否解锁
        SetIntArray("AttackQuene", AttackQuene);//进攻序列
        SetIntArray("DefenceQuene", DefenceQuene);//防御序列
        Debug.Log("技能数据已保存");
    }

    //检验技能是否已解锁，true已解锁，false未解锁
    public bool CheckSkillUnlock(int id)
    {
        if (SkillLock[id] == 1)
        {
            return true;
        }
        else
        {
            return false;
        }


    }
    //玩家人物姓名、银两、头像
    //-----------------------------------------------------------------------------------------
    public void SetPlayerName(string name)
    {
        PlayerName = name;
    }

    public void SetPlayerImage(int id)
    {
        PlayerHeadPhotoID = id;
    }

    public void SetPlayerMoney(int money)
    {
        PlayerMoney = money;
    }
    //商人姓名、类型、头像
    //----------------------------------------------------------------------------------------
    public void SetNpcName(string name)
    {
        NpcName = name;
    }

    public void SetNpcImage(int id)
    {
        NpcImageID = id;
    }

    public void SetNpcType(int typeID)
    {
        NpcType = typeID;
    }
    //解锁技能
    public void SkillUnlock(int SkillID)
    {
        SkillLock[SkillID] = 1;
    }

    //增加技能经验值
    public void AddSkillExp(int SkillID, float exp)
    {
        SkillExp[SkillID] = SkillExp[SkillID] + exp;
    }

    //获得技能经验值
    public string GetSkillExp(int SkillID)
    {
        string str = SkillExp[SkillID] + "/" + SkillExpMax[(int)(SkillLv[SkillID]) - 1];
        return str;
    }

    //技能升级，当某技能已有经验值>=该技能对应等级的升级经验值时，（检核升级条件）升级
    public void SkillLvUp(int SkillID)
    {
        if (SkillExp[SkillID] >= SkillExpMax[(int)(SkillLv[SkillID]) - 1])
        {
            SkillLv[SkillID]++;
            SkillExp[SkillID] = 0;
            Debug.Log("技能" + SkillID + "已升级，现在等级为" + SkillLv[SkillID]);
        }
    }

    public void InitSkillQueenPlayerPrefs()
    {
        int[] a = new int[8];
        for (int i = 0; i < 8; i++)
        {
            a[i] = 0;
            a[i] = 0;
        }
        SetIntArray("AttackQuene", a);//进攻序列
        SetIntArray("DefenceQuene", a);//防御序列
    }

    //同步攻击技能序列，i为在序列中的序号，ID为技能ID，当技能为空时ID=0；
    public void RefreshAttackQuene(int i, int ID)
    {
        AttackQuene[i] = ID;
    }

    //同步防御技能序列，i为在序列中的序号，ID为技能ID，当技能为空时ID=0；
    public void RefreshDefenceQuene(int i, int ID)
    {
        DefenceQuene[i] = ID;
    }
    //获取已装备的技能数量
    public int GetSkillUseNum(string str)
    {
        int num = 0;
        if (str == "attack")
        {
            for (int i = 0; i < AttackQuene.Length; i++)
            {
                if (AttackQuene[i] > 0)
                {
                    num++;
                }
            }
        }
        else
        {
            for (int i = 0; i < DefenceQuene.Length; i++)
            {
                if (DefenceQuene[i] > 0)
                {
                    num++;
                }
            }
        }
        return num;
    }

    //卸下技能
    public void RemoveSkillQuene(string ID)
    {
        int id;
        if (int.TryParse(ID, out id))
        {
            SkillConfig cfgData = DataTableManager.Instance.GetConfig<SkillConfig>("Skill");
            SkillConfig.SkillObject data = cfgData.GetListConfigElementByID(id);
            if (data.SkillType == "刀" || data.SkillType == "剑" || data.SkillType == "枪" || data.SkillType == "棍" || data.SkillType == "叉" || data.SkillType == "锤")
            {
                for (int i = 0; i < AttackQuene.Length; i++)
                {
                    if (AttackQuene[i] == id)
                    {
                        AttackQuene[i] = 0;
                    }
                }
            }
            else
            {
                for (int i = 0; i < DefenceQuene.Length; i++)
                {
                    if (DefenceQuene[i] == id)
                    {
                        DefenceQuene[i] = 0;
                    }
                }
            }
        }
    }
    //=====================================================================================================================================

    public void PrintPlayerState()
    {
        Debug.Log("----------------");
        Debug.Log("PlayerNum=" + PlayerNum);

        Debug.Log("PlayerID=" + PlayerID);
        Debug.Log("PlayerName=" + PlayerName);
        Debug.Log("PlayerSex=" + PlayerSex);
        Debug.Log("PlayerLv=" + PlayerLv);
        Debug.Log("PlayerAavailable=" + PlayerAavailable);

        Debug.Log("PlayerCon=" + PlayerCon);
        Debug.Log("PlayerStr=" + PlayerStr);
        Debug.Log("PlayerDex=" + PlayerDex);
        Debug.Log("PlayerLuk=" + PlayerLuk);

        Debug.Log("PlayerAvaliablePoint=" + PlayerAvaliablePoint);
        Debug.Log("PlayerHpMax=" + PlayerHpMax);
        Debug.Log("PlayerHpCurrent=" + PlayerHpCurrent);
        Debug.Log("PlayerHealth=" + PlayerHealth);
        Debug.Log("PlayerHealth=" + PlayerHealthMax);
        Debug.Log("ExpPlayer=" + ExpPlayer);
        Debug.Log("ExpPlayer=" + ExpMaxPlayer);
        Debug.Log("PlayerHeadPhotoID=" + PlayerHeadPhotoID);
        Debug.Log("PlayerFullPhotoID=" + PlayerFullPhotoID);
        Debug.Log("PlayerMoney=" + PlayerMoney);

        Debug.Log("PlayerEquipWeaponID=" + PlayerEquipWeaponID);
        Debug.Log("PlayerEquipArmorID=" + PlayerEquipArmorID);

        Debug.Log("PlayerWorldPos = (" + PlayerWorldPosX + "," + PlayerWorldPosY + ")");

    }

    public void PlayerStateRefresh()
    {
        PlayerStateSave();
        PrintPlayerState();

    }








    //从PlayerPrefs中读取角色数据至中转站
    public void PlayerStateLoad()
    {
        PlayerNum = PlayerPrefs.GetInt("PlayerNum");

        PlayerID = PlayerPrefs.GetInt("PlayerID");
        PlayerName = PlayerPrefs.GetString("PlayerName");
        PlayerSex = PlayerPrefs.GetString("PlayerSex");
        PlayerLv = PlayerPrefs.GetInt("PlayerLv");
        PlayerAavailable = PlayerPrefs.GetInt("PlayerAavailable");

        PlayerCon = PlayerPrefs.GetInt("PlayerCon");
        PlayerStr = PlayerPrefs.GetInt("PlayerStr");
        PlayerDex = PlayerPrefs.GetInt("PlayerDex");
        PlayerLuk = PlayerPrefs.GetInt("PlayerLuk");
        PlayerAvaliablePoint = PlayerPrefs.GetInt("PlayerAvaliablePoint");
        PlayerHpMax = PlayerPrefs.GetInt("PlayerHpMax");
        PlayerHpCurrent = PlayerPrefs.GetInt("PlayerHpCurrent");
        PlayerHealth = PlayerPrefs.GetInt("PlayerHealth");
        PlayerHealthMax = PlayerPrefs.GetInt("PlayerHealthMax");
        ExpPlayer = PlayerPrefs.GetInt("ExpPlayer");
        ExpMaxPlayer = PlayerPrefs.GetInt("ExpMaxPlayer");

        PlayerHeadPhotoID = PlayerPrefs.GetInt("PlayerHeadPhotoID");
        PlayerFullPhotoID = PlayerPrefs.GetInt("PlayerPlayerFullPhotoIDLv");
        PlayerMoney = PlayerPrefs.GetInt("PlayerMoney");
        PlayerEquipWeaponID = PlayerPrefs.GetInt("PlayerEquipWeaponID");
        PlayerEquipArmorID = PlayerPrefs.GetInt("PlayerEquipArmorID");
        //武器数据
        WeaponAttack = PlayerPrefs.GetInt("WeaponAttack");
        WeaponDex = PlayerPrefs.GetInt("WeaponDex");
        WeaponStrength = PlayerPrefs.GetInt("WeaponStrength");
        WeaponCon = PlayerPrefs.GetInt("WeaponCon");
        WeaponLuk = PlayerPrefs.GetInt("WeaponLuk");
        WeaponRoll = PlayerPrefs.GetInt("WeaponRoll");
        WeaponHitRate = PlayerPrefs.GetInt("WeaponHitRate");
        WeaponArmorPenetration = PlayerPrefs.GetInt("WeaponArmorPenetration");
        WeaponCritical = PlayerPrefs.GetInt("WeaponCritical");
        //护甲数据
        ArmorDefence = PlayerPrefs.GetInt("ArmorDefence");
        ArmorDex = PlayerPrefs.GetInt("ArmorDex");
        ArmorStrength = PlayerPrefs.GetInt("ArmorStrength");
        ArmorCon = PlayerPrefs.GetInt("ArmorCon");
        ArmorLuk = PlayerPrefs.GetInt("ArmorLuk");
        ArmorRoll = PlayerPrefs.GetInt("ArmorRoll");
        ArmorDodgeRate = PlayerPrefs.GetInt("ArmorDodgeRate");


        PlayerWorldPosX = PlayerPrefs.GetFloat("PlayerWorldPosX");
        PlayerWorldPosY = PlayerPrefs.GetFloat("PlayerWorldPosY");

        isNew = PlayerPrefs.GetInt("isNew");
        LoadPackageItem();
        SkillLoad();

    }

    //将中转站内的当前角色数据保存至PlayerPrefs
    public void PlayerStateSave()
    {
        PlayerPrefs.SetInt("PlayerNum", PlayerNum);

        PlayerPrefs.SetInt("PlayerID", PlayerID);
        PlayerPrefs.SetString("PlayerName", PlayerName);
        PlayerPrefs.SetString("PlayerSex", PlayerSex);
        PlayerPrefs.SetInt("PlayerLv", PlayerLv);
        PlayerPrefs.SetInt("PlayerAavailable", PlayerAavailable);

        PlayerPrefs.SetInt("PlayerCon", PlayerCon);
        PlayerPrefs.SetInt("PlayerStr", PlayerStr);
        PlayerPrefs.SetInt("PlayerDex", PlayerDex);
        PlayerPrefs.SetInt("PlayerLuk", PlayerLuk);

        PlayerPrefs.SetInt("PlayerAvaliablePoint", PlayerAvaliablePoint);
        PlayerPrefs.SetInt("PlayerHpMax", PlayerHpMax);
        PlayerPrefs.SetInt("PlayerHpCurrent", PlayerHpCurrent);
        PlayerPrefs.SetInt("PlayerHealth", PlayerHealth);
        PlayerPrefs.SetInt("PlayerHealthMax", PlayerHealthMax);
        PlayerPrefs.SetInt("ExpPlayer", ExpPlayer);
        PlayerPrefs.SetInt("ExpMaxPlayer", ExpMaxPlayer);
        PlayerPrefs.SetInt("PlayerHeadPhotoID", PlayerHeadPhotoID);
        PlayerPrefs.SetInt("PlayerFullPhotoID", PlayerFullPhotoID);
        PlayerPrefs.SetInt("PlayerMoney", PlayerMoney);

        PlayerPrefs.SetInt("PlayerEquipWeaponID", PlayerEquipWeaponID);
        PlayerPrefs.SetInt("PlayerEquipArmorID", PlayerEquipArmorID);
        //武器数据
        PlayerPrefs.SetInt("WeaponAttack", WeaponAttack);
        PlayerPrefs.SetInt("WeaponDex", WeaponDex);
        PlayerPrefs.SetInt("WeaponStrength", WeaponStrength);
        PlayerPrefs.SetInt("WeaponCon", WeaponCon);
        PlayerPrefs.SetInt("WeaponLuk", WeaponLuk);
        PlayerPrefs.SetInt("WeaponRoll", WeaponRoll);
        PlayerPrefs.SetInt("WeaponHitRate", WeaponHitRate);
        PlayerPrefs.SetInt("WeaponArmorPenetration", WeaponArmorPenetration);
        PlayerPrefs.SetInt("WeaponCritical", WeaponCritical);
        //护甲数据
        PlayerPrefs.SetInt("ArmorDefence", ArmorDefence);
        PlayerPrefs.SetInt("ArmorDex", ArmorDex);
        PlayerPrefs.SetInt("ArmorStrength", ArmorStrength);
        PlayerPrefs.SetInt("ArmorCon", ArmorCon);
        PlayerPrefs.SetInt("ArmorLuk", ArmorLuk);
        PlayerPrefs.SetInt("ArmorRoll", ArmorRoll);
        PlayerPrefs.SetInt("ArmorDodgeRate", ArmorDodgeRate);

        PlayerPrefs.SetFloat("PlayerWorldPosX", PlayerWorldPosX);
        PlayerPrefs.SetFloat("PlayerWorldPosY", PlayerWorldPosY);

        PlayerPrefs.SetInt("isNew", isNew);
        SavePackageItem();
        SkillSave();
    }

    /// <summary>
    /// Returns a Float Array from a Key
    /// </summary>
    public static float[] GetFloatArray(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] stringArray = PlayerPrefs.GetString(key).Split("|"[0]);
            float[] floatArray = new float[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
                floatArray[i] = Convert.ToSingle(stringArray[i]);
            return floatArray;
        }
        return new float[0];
    }

    /// <summary>
    /// Stores a Float Array or Multiple Parameters into a Key
    /// </summary>
    public static bool SetFloatArray(string key, params float[] floatArray)
    {
        if (floatArray.Length == 0) return false;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < floatArray.Length - 1; i++)
            sb.Append(floatArray[i]).Append("|");
        sb.Append(floatArray[floatArray.Length - 1]);

        try
        {
            PlayerPrefs.SetString(key, sb.ToString());
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }


    //监听使用次数
    //void SetPlayUseTime()
    //{
    //    RegistryKey RootKey, RegKey;
    //    //项名为：HKEY_CURRENT_USER\Software
    //    RootKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
    //    //打开子项：HKEY_CURRENT_USER\Software\MyRegDataApp
    //    if ((RegKey = RootKey.OpenSubKey("TestToControlUseTime", true)) == null)
    //    {
    //        RootKey.CreateSubKey("TestToControlUseTime");       //不存在，则创建子项
    //        RegKey = RootKey.OpenSubKey("TestToControlUseTime", true);
    //        RegKey.SetValue("UseTime", (object)1);              //创建键值，存储已使用次数
    //        return;
    //    }
    //    try
    //    {
    //        object usetime = RegKey.GetValue("UseTime");        //读取键值，已使用次数
    //        print("已使用使用:" + usetime + "次");
    //        int newtime = int.Parse(usetime.ToString()) + 1;
    //        RegKey.SetValue("UseTime", (object)newtime);    //更新键值，已使用次数+1

    //        if (newtime == 1)
    //        {
    //            InitSkillExp();
    //            InitSkillLv();
    //            InitSkillLock();
    //        }
    //    }
    //    finally
    //    {
    //        Console.WriteLine("这里是finally");
    //    }

    //}

    //public void ResetUseTime()
    //{
    //    RegistryKey RootKey, RegKey;
    //    //项名为：HKEY_CURRENT_USER\Software
    //    RootKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
    //    //打开子项：HKEY_CURRENT_USER\Software\MyRegDataApp
    //    RootKey.CreateSubKey("TestToControlUseTime");       //不存在，则创建子项
    //    RegKey = RootKey.OpenSubKey("TestToControlUseTime", true);
    //    RegKey.SetValue("UseTime", (object)1);

    //    Debug.Log("运行次数已被重置");
    //}



    /// <summary>
    /// Stores a Int Array or Multiple Parameters into a Key
    /// </summary>
    public static bool SetIntArray(string key, params int[] intArray)
    {
        if (intArray.Length == 0) return false;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < intArray.Length - 1; i++)
            sb.Append(intArray[i]).Append("|");
        sb.Append(intArray[intArray.Length - 1]);

        try { PlayerPrefs.SetString(key, sb.ToString()); }
        catch (Exception e) { return false; }
        return true;
    }

    /// <summary>
    /// Returns a Int Array from a Key
    /// </summary>
    public static int[] GetIntArray(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] stringArray = PlayerPrefs.GetString(key).Split("|"[0]);
            int[] intArray = new int[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
                intArray[i] = Convert.ToInt32(stringArray[i]);
            return intArray;
        }
        return new int[0];
    }

    //获得经验、升级、加属性点
    public void ExpPlayerAdd(int exp)
    {
        int totalExp = ExpPlayer + exp;
        do
        {
            //升级检查及经验获取
            if (totalExp >= PlayerExpMax[PlayerLv - 1])
            {
                PlayerLv++;
                //Debug.Log("PlayerExpMax[PlayerLv - 2]=" + PlayerExpMax[PlayerLv - 2]);
                totalExp = totalExp - PlayerExpMax[PlayerLv - 2];
                ExpPlayer = totalExp;
                ExpMaxPlayer = PlayerExpMax[PlayerLv - 1];
                Debug.Log("获得经验值+" + exp + "点，当前经验" + ExpPlayer + "/" + ExpMaxPlayer + "，人物等级=" + PlayerLv);

                //获得自由分配属性点
                PlayerAvaliablePoint = PlayerAvaliablePoint + 4;
                BagPanel._instance.SetUsableProperty_Label(4);
                //获得固定属性点
                PlayerCon++;
                PlayerStr++;
                PlayerDex++;
                PlayerLuk++;
                ChangePlayerState();
            }
            else
            {
                ExpPlayer = totalExp;
                Debug.Log("获得经验值+" + exp + "点，当前经验" + ExpPlayer + "/" + ExpMaxPlayer + "，人物等级=" + PlayerLv);
            }
        } while (ExpPlayer >= PlayerExpMax[PlayerLv - 1]);



    }

    //从中转站取属性值
    public int GetKeyCount(string key)
    {
        int count;
        switch (key)
        {
            case "Physical":
                count = totalCon;
                return count;
            case "Strength":
                count = totalStr;
                return count;
            case "Skill":
                count = totalDex;
                return count;
            case "Bone":
                count = totalLuk;
                return count;
            case "PlayerAvaliable":
                count = PlayerAvaliablePoint;
                return count;
            case "PlayerHpCurrent":
                count = PlayerHpCurrent;
                return count;
            case "PlayerHpMax":
                count = PlayerHpMax;
                return count;
            case "PlayerExperience":
                count = ExpPlayer;
                return count;
            case "PlayerExperienceMax":
                count = ExpMaxPlayer;
                return count;
            case "Equip":
                count = PlayerEquipWeaponID;
                return count;
            case "Equip1":
                count = PlayerEquipArmorID;
                return count;
            case "PlayerHealth":
                count = PlayerHealth;
                return count;
            case "PlayerHealthMax":
                count = PlayerHealthMax;
                return count;
        }
        return 0;
    }

    //同步已装备武器ID
    public void RefreshWeaponArmorID(int weaponID, int armorID)
    {
        PlayerEquipWeaponID = weaponID;
        PlayerEquipArmorID = armorID;
    }

    //同步基础属性至中转站
    public void RefreshPlayerStateKey(string key, int value)
    {
        if (key == "PlayerAvaliable")
        {
            PlayerAvaliablePoint = PlayerAvaliablePoint + value;
        }
        else if (key == "Strength")
        {
            PlayerStr = PlayerStr + value;
        }
        else if (key == "Physical")
        {
            PlayerCon = PlayerCon + value;
        }
        else if (key == "Skill")
        {
            PlayerDex = PlayerDex + value;
        }
        else if (key == "Bone")
        {
            PlayerLuk = PlayerLuk + value;
        }
    }

    //重新计算基础属性加成
    public void RecountPlayerState()
    {
        //生命值：1点体质=50点生命值上限，基础生命值为100
        int totalCon = PlayerCon + WeaponCon + ArmorCon;
        PlayerHpMax = 100 + totalCon * 50;
        PlayerHpCurrent = PlayerHpMax;
    }

    //当更换装备后重新计算面板属性
    public void ChangePlayerState()
    {
        totalCon = PlayerCon + WeaponCon + ArmorCon;
        totalStr = PlayerStr + WeaponStrength + ArmorStrength;
        totalDex = PlayerDex + WeaponDex + ArmorDex;
        totalLuk = PlayerLuk + WeaponLuk + ArmorLuk;
        //重新计算生命值加成
        int hpmax = PlayerHpMax;
        PlayerHpMax = 100 + totalCon * 50;
        int der = PlayerHpMax - hpmax;
        PlayerHpCurrent = PlayerHpCurrent + der;
    }

    //同步人物属性至中转站
    public void SetPlayerState(int currentHP, int HealthLoss)
    {
        PlayerHpCurrent = currentHP;
        PlayerHealth = PlayerHealth - HealthLoss;
    }

    //使用物品后同步人物血量和健康值至中转站
    public void RefreshHPandHealth(int currentHP, int currentHealth)
    {
        PlayerHpCurrent = currentHP;
        PlayerHealth = currentHealth;
    }

    //同步玩家当前位置
    public void RefreshPlayerPos(float x, float y)
    {
        PlayerWorldPosX = x;
        PlayerWorldPosY = y;
    }

    //健康恢复，每过10小时恢复1点
    public void HealthRecovery(float time)
    {
        int a = (int)(time / 10);

        PlayerHealth = PlayerHealth + a;


        if (PlayerHealth > PlayerHealthMax) PlayerHealth = PlayerHealthMax;

    }







}
