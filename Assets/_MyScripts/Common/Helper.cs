using System;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static GameObject GetChild(Transform trans, string childName)
    {
        Transform child = trans.Find(childName);

        if (child != null)
        {
            return child.gameObject;
        }

        int count = trans.childCount;
        GameObject go = null;

        for (int i = 0; i < count; ++i)
        {
            child = trans.GetChild(i);
            go = GetChild(child, childName);

            if (go != null)
            { return go; }
        }

        return null;
    }

    public static T GetChild<T>(Transform trans, string childName) where T : Component
    {
        GameObject go = GetChild(trans, childName);

        if (go == null)
        { return null; }

        return go.GetComponent<T>();
    }

    public delegate TKey SelectHandler<T, TKey>(T source);

    public static T Min<T, TKey>(T[] array, SelectHandler<T, TKey> handler)
    where TKey : IComparable
    {
        var min = array[0];

        for (int i = 1; i < array.Length; i++)
        {
            if (handler(min).CompareTo(handler(array[i])) > 0)
            { min = array[i]; }
        }
        return min;
    }

    //时间戳转换
    public static string GetDateTime(int timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        DateTime dt = dtStart.AddSeconds(timeStamp);
        string t = dt.ToString("yyyy/MM/dd HH:mm:ss");
        return t;
    }



    #region by tony
    public static T GetComponentByName<T>(GameObject go, string name)
       where T : Component
    {
        T[] buffer = go.GetComponentsInChildren<T>(true);
        if (buffer != null)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != null && buffer[i].name == name)
                {
                    return buffer[i];
                }
            }
        }
        return null;
    }

    public static T[] GetComponentsByName<T>(GameObject go)
        where T : Component
    {
        T[] buffer = go.GetComponentsInChildren<T>(true);

        return buffer;
    }

    public static GameObject GetGameObjectByName(GameObject objInput, string strFindName)
    {
        GameObject ret = null;
        if (objInput != null)
        {
            Transform[] objChildren = objInput.GetComponentsInChildren<Transform>(true);
            if (objChildren != null)
            {
                for (int i = 0; i < objChildren.Length; ++i)
                {
                    if ((objChildren[i].name == strFindName))
                    {
                        ret = objChildren[i].gameObject;
                        break;
                    }
                }
            }
        }
        return ret;
    }

    public static List<GameObject> GetGameObjectsByName(GameObject objInput, string strFindName)
    {
        List<GameObject> list = new List<GameObject>();
        Transform[] objChildren = objInput.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < objChildren.Length; ++i)
        {
            if ((objChildren[i].name.Contains(strFindName)))
            {
                list.Add(objChildren[i].gameObject);
            }
        }

        return list;
    }

    public static GameObject InstantiatePrefab(GameObject prefab, GameObject parent)
    {
        GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.name = prefab.name;
        if (parent != null)
        {
            obj.transform.parent = parent.transform;
        }
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        return obj;
    }
    #endregion

}
