using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public List<float> BulletDamage;
    private float Damage;
    public void SetDamage(int n)
    {
        switch(n)
        {
            case 1:
                Damage = BulletDamage[0];
                break;
            case 2:
                Damage = BulletDamage[1];
                break;
            case 3:
                Damage = BulletDamage[2];
                break;
            case 4:
                Damage = BulletDamage[3];
                break;
            case 5:
                Damage = BulletDamage[4];
                break;
        }
    }
    public float GetDamage()
    {
        return Damage;
    }
	void OnCollisionEnter(Collision other) {
		myFactory mF = Singleton<myFactory>.Instance;
		if (other.gameObject.tag != "Player") {
			mF.recycleBullet(this.gameObject);	
		}
	}
}
