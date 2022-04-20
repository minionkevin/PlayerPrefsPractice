using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// PlayerPrefs数据管理类 统一管理数据的存储和读取
/// </summary>
public class PlayerPrefsDataMgr
{
    private static PlayerPrefsDataMgr instance = new PlayerPrefsDataMgr();
    public static PlayerPrefsDataMgr Instance
    {
        get { return instance; }
    }
    private PlayerPrefsDataMgr() { }

    public void SaveData(object data,string keyName)
    {
        //得到所有字段
        Type dataType = data.GetType();
        FieldInfo[] infos = dataType.GetFields();

        //定义key keyName_数据类型_字段类型_字段名
        //遍历得到字段 储存
        string saveKeyName = "";
        FieldInfo info;
        for (int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            saveKeyName = keyName + "_" + dataType.Name + "_" + info.FieldType.Name + "_" + info.Name;
            Debug.Log(saveKeyName);

            //储存
            SaveValue(info.GetValue(data), saveKeyName);
        }
        PlayerPrefs.Save();
    }

    private void SaveValue(object value, string keyName)
    {
        Type fieldType = value.GetType();
        if(fieldType == typeof(int))
        {
            Debug.Log("---int---" + keyName);
            PlayerPrefs.SetInt(keyName, (int)value);
        }
        else if (fieldType == typeof(float))
        {
            Debug.Log("---float---" + keyName);
            PlayerPrefs.SetFloat(keyName, (float)value);
        }
        else if (fieldType == typeof(string))
        {
            Debug.Log("---string---" + keyName);
            PlayerPrefs.SetString(keyName, value.ToString());
        }
        else if (fieldType == typeof(bool))
        {
            Debug.Log("bool---" + keyName);
            PlayerPrefs.SetInt(keyName, (bool)value?1:0);
        }
        //判断list泛型类型
        else if (typeof(IList).IsAssignableFrom(fieldType))
        {
            Debug.Log("存储list" + keyName);
            //父类装子类
            IList list = value as IList;
            //存数量
            PlayerPrefs.SetInt(keyName, list.Count);
            int index = 0;
            //存具体的值
            foreach(object obj in list)
            {
                SaveValue(obj, keyName + index);
                ++index;
            }
        }
        //dictionary
        else if (typeof(IDictionary).IsAssignableFrom(fieldType))
        {
            IDictionary dictionary = value as IDictionary;
            PlayerPrefs.SetInt(keyName, dictionary.Count);
            int index = 0;
            foreach(object key in dictionary.Keys)
            {
                SaveValue(key, keyName + "_key_" + index);
                SaveValue(dictionary[key], keyName + "_value_" + index);
                ++index;
            }
        }
        //自定义类
        else
        {
            SaveData(value, keyName);
        }
    }
    public object LoadData(Type type, string keyName)
    {
        object data = Activator.CreateInstance(type);

        //得到所有字段
        FieldInfo[] infos = type.GetFields();
        string loadKeyName = "";
        FieldInfo info;
        for(int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            //和储存一模一样才能找到
            loadKeyName = keyName + "_" + type.Name + "_" + info.FieldType.Name + "_" + info.Name;
            Debug.Log(loadKeyName);
            //填充数据
            info.SetValue(data, LoadValue(info.FieldType, loadKeyName));
        }
        
        return data;
    }
    private object LoadValue(Type fieldType,string keyName)
    {
        if (fieldType == typeof(int))
        {
            return PlayerPrefs.GetInt(keyName, 0);
        }
        else if (fieldType == typeof(float))
        {
            return PlayerPrefs.GetFloat(keyName, 0);
        }
        else if (fieldType == typeof(string))
        {
            return PlayerPrefs.GetString(keyName, "");
        }
        else if (fieldType == typeof(bool))
        {
            return PlayerPrefs.GetInt(keyName, 0) == 1 ? true : false;
        }
        else if(typeof(IList).IsAssignableFrom(fieldType))
        {
            int count = PlayerPrefs.GetInt(keyName, 0);
            IList list = Activator.CreateInstance(fieldType) as IList;
            for(int i = 0; i < count; i++)
            {
                //得到list中泛型类型
                list.Add(LoadValue(fieldType.GetGenericArguments()[0], keyName + i));
            }
            return list;
        }
        else if(typeof(IDictionary).IsAssignableFrom(fieldType))
        {
            int count = PlayerPrefs.GetInt(keyName, 0);
            IDictionary dic = Activator.CreateInstance(fieldType) as IDictionary;
            Type[] index = fieldType.GetGenericArguments();
            for(int i = 0; i < count; i++)
            {
                //key,value
                dic.Add(LoadValue(index[0], keyName +"_key_" + i), 
                    LoadValue(index[1], keyName + "_key_" + i));
            }
            return dic;
        }
        //自定义类型
        else
        {
            return LoadData(fieldType, keyName);
        }
    }

}
