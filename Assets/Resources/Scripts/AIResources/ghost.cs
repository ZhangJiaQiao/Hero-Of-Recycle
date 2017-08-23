using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghost : MonoBehaviour {

	public float maxVelocity = 50;
    //	public Vector3 playerPosition;
    public UIProgressBar HPBar;
	public Transform player;
	private Vector3 targetPosition;
	private bool findThePlayer = false;
	private Animator animator;
	public characterProperty CharacterProperty;
	private bool isAttacking = false;
	private float countTime = 0.0f;
	void Start () {
		targetPosition = player.position;
		CharacterProperty = this.gameObject.GetComponent<characterProperty>();
		animator = GetComponent<Animator> ();
	}

	void Update () {
        HPBar.GetComponent<HpUISlider>().UpdateVal(CharacterProperty.life / 100);
		findPlayer ();						//if the player is close enough to track

		if (isAttacking) {
			directionCtrl ();
			attack ();
			return;
		}
		Debug.Log ("should move");
		directionCtrl ();					//trun direction to the player
		move ();					//move and attack player
	}

	void FixedUpdate() {
		if (CharacterProperty.speed <= 0.0f)
			CharacterProperty.speed = 0.0f;
		if (CharacterProperty.damageValue <= 0.0f)
			CharacterProperty.damageValue = 0.0f;
		if (CharacterProperty.life <= 0) {
			animator.SetBool ("dead", true);
		}
        if (CharacterProperty.life >= 100)
        {
            CharacterProperty.life = 100;
        }
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.name != "Bullet(Clone)")
			return;
		if (collision.gameObject.tag == this.gameObject.tag) {
			CharacterProperty.speed -= 0.1f;
			CharacterProperty.life -= 40;
			CharacterProperty.damageValue -= 5f;
		} else {
			CharacterProperty.speed += 0.1f;
			CharacterProperty.life += 40;
			CharacterProperty.damageValue += 5f;			
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
		return Vector3.Distance (transform.position, player.position) < 10;
	}

	void directionCtrl() {
		string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;
		if (!findThePlayer && currentClip != "die")
			return;
		Vector3 targetDir = targetPosition - transform.position;
		float step = 10 * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		transform.rotation = Quaternion.LookRotation(newDir);
	}


	void move() {
		string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;

		if (!findThePlayer || currentClip == "die")
			return;
		Vector3 v = GetComponent<Rigidbody> ().velocity;
		if (CloseToTrack () && !closeToAttack ()) {
			Debug.Log(currentClip);
			if (currentClip != "idle" && currentClip != "die") {
				if (v.sqrMagnitude < maxVelocity) {
					v +=  CharacterProperty.speed * transform.forward.normalized;
					GetComponent<Rigidbody> ().velocity = v;	
				}
			}
			animator.SetBool ("run", true);		
		} else if (closeToAttack ()) {
			animator.SetBool ("run", true);	
			isAttacking = true;
		} else {
			animator.SetBool ("idle", true);
		}
		targetPosition = player.position;
	}

	void attack() {
		string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;
		
		Vector3 v = GetComponent<Rigidbody> ().velocity;
		if (currentClip != "idle" && currentClip != "die") {
			if (v.sqrMagnitude < maxVelocity) {
				v = 25 * CharacterProperty.speed * transform.forward.normalized;
				GetComponent<Rigidbody> ().velocity = v;
			}
		}
		countTime += Time.deltaTime;
		if (countTime > 2) {
			isAttacking = false;
			targetPosition = player.position;
			Debug.Log ("position " + targetPosition);
			countTime = 0.0f;
		}
	}

}
