using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CreateConfigFile : MonoBehaviour
{
    //任务物品表
    //public class TaskItems
    //{
    //    public int Id;
    //    public string QItemType;
    //    public string QItemName;
    //    public string QItemInfo;
    //}

    //材料表
    //public class Materials
    //{
    //    public int Id;
    //    public string MaterialType;
    //    public string MaterialName;
    //    public string MaterialInfo;
    //}

    //武器表
    //public class Weapon
    //{
    //    public int Id;
    //    public string WeaponType;
    //    public string WeaponName;
    //    public int WeaponAtk;
    //    public int WeaponDex;
    //    public int WeaponStr;
    //    public int WeaponCon;
    //    public int WeaponLuk;
    //    public int WeaponRoll;
    //    public int WeaponHitRate;
    //    public int WeaponArmorPene;
    //    public int WeaponCrit;
    //    public string WeaponInfo;
    //}

    //消耗品表
    //public class Consumables
    //{
    //    public int Id;
    //    public string GoodsTpye;
    //    public string GoodsName;
    //    public int GoodsHpInc;
    //    public int GoodsHealthInc;
    //    public string GoodsInfo;
    //}

    //防具表
    //public class Armor
    //{
    //    public int Id;
    //    public string GoodsTpye;
    //    public string GoodsName;
    //    public int GoodsHpInc;
    //    public int GoodsHealthInc;
    //    public string GoodsInfo;
    //}

    //测试用
    //public class ActivityMain
    //{
    //    public string Id;
    //    public string type;
    //    public string icon;
    //    public string name;
    //    public string description_big;
    //    public string description;
    //}

    //道具表
    public class Prop
    {
        public int ItemID;
        public string ConfigType;
        public string ItemType;
        public string ItemName;
        public int WeaponAttack;
        public int WeaponDex;
        public int WeaponStrength;
        public int WeaponCon;
        public int WeaponLuk;
        public int WeaponRoll;
        public int WeaponHitRate;
        public int WeaponArmorPenetration;
        public int WeaponCritical;
        public int ArmorDefence;
        public int ArmorDex;
        public int ArmorStrength;
        public int ArmorCon;
        public int ArmorLuk;
        public int ArmorRoll;
        public int ArmorDodgeRate;
        public int ConsumableHpIncrease;
        public int ConsumableHealthIncrease;
        public int Stackable;
        public int StackingLimit;
        public string ItemInfomation;
        public int UseLevel;
        public int EquipDex;
        public int EquipStrength;
        public int EquipLuk;
    }
    [MenuItem("Tools/生成配置表解析文件")]
    static void TestData()
    {
        //CreateOne(new TaskItems());
        //CreateOne(new Materials());
        //CreateOne(new Weapon());
        //CreateOne(new Consumables());
        //CreateOne(new Armor());
        //CreateOne(new ActivityMain());
        CreateOne(new Prop());

        AssetDatabase.Refresh();
    }


    static void CreateOne(object data)
    {
        string className = data.GetType().Name;
        var porps = data.GetType().GetFields();

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("using UnityEngine;\nusing System;\nusing System.Security;\nusing System.Collections.Generic;\n");
        sb.Append("namespace HotFix_Project.Config\n");
        sb.Append("{\n");
        sb.Append(string.Format("public class {0}Config : ConfigBase\n", className));
        sb.Append("{\n");
        sb.Append(string.Format("\tpublic class {0}Object\n", className));
        sb.Append("\t{\n");
        foreach (var pro in porps)
        {
            sb.Append(string.Format("\t\tpublic {0} {1};\n", pro.FieldType.Name, pro.Name));
        }
        sb.Append("\t}\n");


        // sb.Append("#if !NETFX_CORE\n");


        System.Text.StringBuilder sb2 = new System.Text.StringBuilder();

        string cName = className + "Config";
        string pName = className + "Object";
        string sName = className.ToLower() + "Obj";
        string dicName = className.ToLower() + "Dic";

        sb2.Append("\t\t\t\t" + pName + " " + sName + " = new " + pName + "();\n");
        foreach (var pro in porps)
        {
            if (pro.FieldType == typeof(string))
            {
                sb2.Append("\t\t\t\t" + sName + "." + pro.Name + " = childrenElement.Attribute(\"" + pro.Name + "\");\n");
            }
            else
            {
                sb2.Append("\t\t\t\t" + pro.FieldType.Name + ".TryParse(childrenElement.Attribute(\"" + pro.Name + "\")" + ", out " + sName + "." + pro.Name + ");\n");
            }

        }
        sb2.Append("\t\t\t\t" + dicName + "[" + sName + ".Id]=  " + sName + ";\n");
        string fo = "\tpublic override bool Load(SecurityElement element)\n\t{\n\t\tif (element.Tag != \"Items\")\n\t\t{\n\t\t\treturn false;\n\t\t}\n\t\tif (element.Children != null)\n\t\t{\n\t\t\tforeach (SecurityElement childrenElement in element.Children)\n\t\t\t{\n" + sb2.ToString() + "\t\t\t}\n\t\t}\n\t\telse\n\t\t{\n\t\t\treturn false;\n\t\t}\n\t\treturn true;\n\t}\n";


        sb.Append(fo);


        string st = "\tpublic " + cName + " ." + pName + " GetConfigElementByID(int id)\n\t{\n\t\t" + pName + " " + sName + "= null;\n\t\t" + dicName + ".TryGetValue(id, out " + sName + ");\n\t\treturn " + sName + ";\n\t}\n";
        sb.Append(st);



        string st2 = string.Format("\tpublic Dictionary<int, {0}> {1} = new Dictionary<int, {0}>();\n", pName, dicName);
        sb.Append(st2);

        sb.Append("}\n");

        sb.Append("}");
        string path = Application.dataPath + "/_MyScripts/ConfigScript/" + className + "Config.cs";
        using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.Write(sb.ToString());
            }
        }
    }



}
