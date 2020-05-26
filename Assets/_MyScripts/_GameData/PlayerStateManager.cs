using HotFix_Project.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Win32;

//角色数据中转站
public class PlayerStateManager : MonoBehaviour
{

    public static PlayerStateManager instane { get; set; }


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
    public int isNew;//下一次打开背包是否重置数据，1重置，0不重置
    public List<PackageItem> playerItemData = new List<PackageItem>();
    public static PlayerStateManager GetInstance()
    {
        if (instane == null)
        {
            instane = new PlayerStateManager();
        }
        return instane;
    }
    private void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        PlayerStateLoad();

        SkillLoad();
        //  SetPlayUseTime();


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

    //==============================================================================================================================
    //初始化物品清单
    public void InitPackageItem()
    {
        int[] a = new int[80];
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = 0;
        }
        SetIntArray("PackageItem", a);
    }




    //保存包裹物品清单
    public void SavePackageItem()
    {
        List<int> a = PlayerInfoManager.Instance.BagId();
        foreach (var item in a)
        {
            Debug.Log(item);
        }
        int[] array = a.ToArray();
        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log("array=" + array[i]);
        }
        SetIntArray("PackageItem", array);

    }

    //读取包裹物品清单
    public void LoadPackageItem()
    {

        PackageItem = GetIntArray("PackageItem");
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
    //随机生成技能  测试用
    public Dictionary<string, List<int>> OnCreateSkill()
    {
        //测试用 key 是技能类型 技能Id
        Dictionary<string, List<int>> skillIdDic = new Dictionary<string, List<int>>();
        SkillConfig cfgData = DataTableManager.Instance.GetConfig<SkillConfig>("Skill");
        //初始化30个数据
        List<int> skillData;
        for (int i = 0; i < 30; i++)
        {
            int id = UnityEngine.Random.Range(i, 900);
            SkillConfig.SkillObject data = cfgData.GetListConfigElementByID(id);
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
        return skillIdDic;
    }
    //将所有技能设置为未解锁，0=未解锁，1=解锁，然后初始化技能，将基础技能设置为解锁
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

    //解锁技能
    public void SkillUnlock(int SkillID)
    {
        SkillLock[SkillID] = 1;
    }

    //获得技能经验值
    public void AddSkillExp(int SkillID, float exp)
    {
        SkillExp[SkillID] = SkillExp[SkillID] + exp;
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
                //获得固定属性点
                PlayerCon++;
                PlayerStr++;
                PlayerDex++;
                PlayerLuk++;
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
                count = PlayerCon;
                return count;
            case "Strength":
                count = PlayerStr;
                return count;
            case "Skill":
                count = PlayerDex;
                return count;
            case "Bone":
                count = PlayerLuk;
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

    //同步基础属性至中转站
    public void RefreshPlayerState(int Con, int Str, int Dex, int Luk, int Avaliable)
    {
        PlayerCon = Con;
        PlayerStr = Str;
        PlayerDex = Dex;
        PlayerLuk = Luk;
        PlayerAvaliablePoint = Avaliable;
    }








}
