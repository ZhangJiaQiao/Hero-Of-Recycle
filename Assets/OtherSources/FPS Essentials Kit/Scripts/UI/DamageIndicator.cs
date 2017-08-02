using UnityEngine;

public class DamageIndicator
{
    public float indicatorAngle;
    public Vector3 damageDir;
    public float indicatorAlpha;

    public DamageIndicator(Vector3 damageDir, float indicatorAlpha)
    {
        this.damageDir = damageDir;
        this.indicatorAlpha = indicatorAlpha;
    }
}
