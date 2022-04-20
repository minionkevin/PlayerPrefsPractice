using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public int age;
    public string name;
    public float height;
    public bool sex;

    public List<int> list = new List<int> { 1, 2, 3 };
    public Dictionary<int, string> dic = new Dictionary<int, string>()
    {
        { 1,"123"},
        { 2,"234"}
    };

    public ItemInfo itemInfo = new ItemInfo(100,99);
    public List<ItemInfo> itemList = new List<ItemInfo>() { 
        new ItemInfo(1, 1), 
        new ItemInfo(2, 2) 
    };
    public Dictionary<int, ItemInfo> itemDic = new Dictionary<int, ItemInfo>()
    {
        { 3,new ItemInfo(1,10)},
        { 4,new ItemInfo(2,20)},
    };
}
public class ItemInfo
{
    public int id;
    public int num;
    public ItemInfo() { }
    public ItemInfo(int id, int num)
    {
        this.id = id;
        this.num = num;
    }
}
public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();

        /*
        PlayerInfo p = new PlayerInfo();
        PlayerPrefsDataMgr.Instance.SaveData(p, "Player1");

        PlayerInfo p2 = PlayerPrefsDataMgr.Instance.LoadData(typeof(PlayerInfo), "Player1") as PlayerInfo;
        */
        PlayerInfo p = PlayerPrefsDataMgr.Instance.LoadData(typeof(PlayerInfo), "Player1") as PlayerInfo;

        p.age = 18;
        p.name = "a";
        p.height = 99.0f;
        p.sex = true;

        PlayerPrefsDataMgr.Instance.SaveData(p, "Player1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
