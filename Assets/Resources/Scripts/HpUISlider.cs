using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUISlider : MonoBehaviour {
    public UISprite sprite;
    public UILabel label;
    public Color[] colors = new Color[] { Color.red, Color.yellow, Color.green };
    private float MaxHp = 100;
    UIProgressBar mBar;
    // Use this for initialization
    void Start()
    {
        mBar = GetComponent<UIProgressBar>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         *根据var的值调整颜色，并且设置颜色过渡
         */
        if (sprite == null || colors.Length == 0) return;
        float val = mBar.value;
        val *= (colors.Length - 1);
        int startIndex = Mathf.FloorToInt(val);

        Color c = colors[0];

        if (startIndex >= 0)
        {
            if (startIndex + 1 < colors.Length)
            {
                float factor = (val - startIndex);
                c = Color.Lerp(colors[startIndex], colors[startIndex + 1], factor);
            }
            else if (startIndex < colors.Length)
            {
                c = colors[startIndex];
            }
            else c = colors[colors.Length - 1];
        }
        label.text = string.Format("{0}/{1}", val * 100, MaxHp);
        c.a = sprite.color.a;
        sprite.color = c;
    }

    public void UpdateVal(float value)
    {
        mBar.value = value;//更新UISlider的值
    }

	public float getVal() {
		return mBar.value;
	}
}
