using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bone : MonoBehaviour {
	public float maxVelocity = 50;
    public UIProgressBar HPBar;
    //	public Vector3 playerPosition;
    public Transform player;
	private bool findThePlayer = false;
	private Animator animator;
	public characterProperty CharacterProperty;
	public delegate void destroy();//死亡毁灭委托
	public event destroy destroyEvent;//事件

	void Start () {
		CharacterProperty = GetComponent<characterProperty>();
		animator = GetComponent<Animator> ();
		player = SSDirector.getInstance ().currentSceneController.getPlayer ();
	}

	void Update () {
        //Debug.Log(CharacterProperty.life);
        HPBar.GetComponent<HpUISlider>().UpdateVal(CharacterProperty.life / 100);
        findPlayer();                       //if the player is close enough to track
        directionCtrl();                    //trun direction to the player
        moveAndAttack();					//move and attack player
	}

	void FixedUpdate() {
		if (CharacterProperty.speed <= 0.0f)
			CharacterProperty.speed = 0.0f;
		if (CharacterProperty.damageValue <= 0.0f)
			CharacterProperty.damageValue = 0.0f;
		if (CharacterProperty.life <= 0) {
			animator.SetBool ("dead", true);
			if (destroyEvent != null) {
				destroyEvent ();
                destroyItself();
				destroyEvent = null;
			}
		}
        if (CharacterProperty.life >= 100)
        {
            CharacterProperty.life = 100;
        }
    }

	void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "knife")
        {
            CharacterProperty.speed -= 0.15f;
            CharacterProperty.damageValue -= 2f;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
		if (collision.gameObject.name != "Bullet(Clone)")
			return;
		//Debug.Log ("hit the bullet!");
		if (collision.gameObject.tag == this.gameObject.tag) {
			CharacterProperty.speed -= 0.1f;
			CharacterProperty.life -= collision.gameObject.GetComponent<Bullet>().GetDamage();
			CharacterProperty.damageValue -= 2f;
		} else {
			CharacterProperty.speed += 0.1f;
			CharacterProperty.life += collision.gameObject.GetComponent<Bullet>().GetDamage();
			CharacterProperty.damageValue += 2f;			
		}
	}

	void findPlayer() {
		if (CloseToTrack()) {
			findThePlayer = true;
		}
	}

	bool CloseToTrack() {
		return Vector3.Distance (transform.position, player.position) < 100;
	}

	bool closeToAttack() {
		return Vector3.Distance (transform.position, player.position) < 4;
	}

	void directionCtrl() {
		string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;
		if (!findThePlayer && currentClip != "die")
			return;
		Vector3 targetDir = player.position - transform.position;
		float step = 10 * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		transform.rotation = Quaternion.LookRotation(newDir);
	}


	void moveAndAttack() {
		string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;
		//Debug.Log (currentClip);
		if (!findThePlayer || currentClip == "die")
			return;
		Vector3 v = GetComponent<Rigidbody> ().velocity;
		if (CloseToTrack () && !closeToAttack ()) {
			if (currentClip == "attack")
				return;
			if (currentClip != "idle" && currentClip != "die") {
				if (v.sqrMagnitude < maxVelocity) {
					v += CharacterProperty.speed * transform.forward.normalized;
					GetComponent<Rigidbody> ().velocity = v;	
				}
			}
			animator.SetBool ("run", true);
		} else if (closeToAttack ()) {
			animator.SetTrigger ("attack");
		} else {
			animator.SetBool ("idle", true);
		}
	}

    void destroyItself()
    {
        StartCoroutine("Dispear");
    }

    IEnumerator Dispear()
    {
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);
    }
}
