using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager
{
    public static readonly PlayerPrefsManager Instance = new PlayerPrefsManager();

    private Dictionary<string, int> dic = new Dictionary<string, int>();

    public string GetKey(string key)
    {
        string name = PlayerInfoManager.Instance.playerState.PlayerName + "_" + key;
        return name;
    }

    public void SetAttributePlayerPrefs(string key, int count)
    {
        //key = GetKey(key);
        int sum = 0;
        if (dic.TryGetValue(key, out sum))
        {
            sum = sum + count;
        }
        else
        {
            sum = count;
        }
        dic[key] = sum;
    }

    public void SetPlayerPrefs(bool isRun = false)
    {
        if (isRun)
        {
            foreach (var item in dic)
            {
                SetAddPlayerPrefs(item.Key, item.Value);
            }
        }
        dic.Clear();
    }

    public void SetPlayerPrefs(string key, string value)
    {
        key = GetKey(key);
        PlayerPrefs.SetString(key, value);
    }

    public void SetPlayerPrefs(string key, int value)
    {
        key = GetKey(key);
        PlayerPrefs.SetInt(key, value);
    }

    public void SetAddPlayerPrefs(string key, int value)
    {
        int sum = GetIntPlayerPrefs(key) + value;
        SetPlayerPrefs(key, sum);
    }

    public string GetStringPlayerPrefs(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public int GetIntPlayerPrefs(string key)
    {
        key = GetKey(key);
        int sum = PlayerPrefs.GetInt(key);
        return sum;
    }


}
