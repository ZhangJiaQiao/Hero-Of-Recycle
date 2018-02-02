using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour {
    public delegate void destroy();//死亡毁灭委托
    public event destroy destoryEvent;//事件
    private float Hp = 100;//生命值
    private float Mp = 100;//魔法值
    private int count = 0;//用于计时
    private int recoverTime = 120;//魔法值恢复时间
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
            Mp = value < 0 ? 0 : value > 100 ? 100 : value;
        }
    }

    private void Update()
    {
        count++;
        if (count >= recoverTime && Mp <= 100)
        {
            count = 0;
            Mp += 2;//魔法值恢复
            Mp = Mp > 100 ? 100 : Mp;
        }
        if (Hp <= 0)
        {
            if(destoryEvent != null)
            {
                destoryEvent();
				destoryEvent = null;
            }
        }
    }
}
