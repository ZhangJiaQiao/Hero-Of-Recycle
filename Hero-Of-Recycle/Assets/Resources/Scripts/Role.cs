using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour {
    public delegate void destroy();//死亡毁灭委托
    public static event destroy destoryEvent;//事件
    private float Hp = 100;//生命值
    private float Mp = 100;//魔法值
    public float hp
    {
        get
        {
            return Hp;
        }
        set
        {
            Hp = value < 0 ? 0:value;
        }
    }
    public float mp
    {
        get
        {
            return Mp;
        }
        set
        {
            Mp = value < 0 ? 0 : value;
            Mp = value > 100 ? 100 : value;
        }
    }
}
