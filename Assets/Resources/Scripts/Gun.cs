using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
	public float spread;
	public Texture2D defaultTex;
	public GameObject muzzleFlash;
	public ParticleSystem smoke;
	private Camera c;
	private float shootColdTime = 0;
	private bool isTalking = false;

	void Start () {
		c = Camera.main;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update () {
		if (!isTalking) {
			float rotationX = transform.localEulerAngles.y;
			float rotationY = transform.localEulerAngles.x - Input.GetAxis ("Mouse Y");
			if (rotationY < 60 || rotationY > 325) {
				transform.localEulerAngles = new Vector3 (rotationY, rotationX, 0);		
			}
			shoot ();
		}
	}

	void OnGUI() {
		float resFactor = Mathf.Clamp((Screen.height / 7.2f), 50, 150);
		float crossW = Mathf.Clamp(resFactor / 1000, 0.01f, 0.02f);
		float crossH = resFactor / 833.3f;
		GUI.DrawTexture(new Rect((Screen.width / 2 - resFactor * crossH/2) - (resFactor * spread), Screen.height / 2 - resFactor * crossW/2, resFactor * crossH, resFactor * crossW), defaultTex);
		GUI.DrawTexture(new Rect((Screen.width / 2 - resFactor * crossH/2) + (resFactor * spread), Screen.height / 2 - resFactor * crossW/2, resFactor * crossH, resFactor * crossW), defaultTex);

		//Vertical
		GUI.DrawTexture(new Rect(Screen.width / 2 - resFactor * crossW/2, (Screen.height / 2 - resFactor * crossH/2) - (resFactor * spread), resFactor * crossW, resFactor * crossH), defaultTex);
		GUI.DrawTexture(new Rect(Screen.width / 2 - resFactor * crossW/2, (Screen.height / 2 - resFactor * crossH/2) + (resFactor * spread), resFactor * crossW, resFactor * crossH), defaultTex);
	}

	public void SetSpread (float spread)
	{
		if (this.spread != spread)
			this.spread = Mathf.Clamp(spread * 1.5f, 0.15f, 1.5f);
	}

	void shoot() {
		Animator animator = GetComponent<Animator> ();
		if(shootColdTime > 0) {
			shootColdTime -= Time.deltaTime;
			return;
		} else {
			muzzleFlash.SetActive (false);
			SetSpread (0.3f);
			animator.SetBool ("shoot", false);
		}
		//string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;
		if (Input.GetKey(KeyCode.Mouse0)) {
			/*switch(currentClip) {
			case "Idle":
				animator.SetTrigger ("standShoot");
				break;	
			case "Run":
				animator.SetTrigger ("runShoot");
				break;
			} */
			if (Cursor.lockState == CursorLockMode.None) {
				Cursor.lockState = CursorLockMode.Locked;
			}
			myFactory mF = Singleton<myFactory>.Instance;
			GameObject bullet = mF.getBullet ();
			Vector3 point = c.ScreenToWorldPoint (new Vector3 (Screen.width/2, Screen.height/2, c.nearClipPlane));
			bullet.transform.right = c.transform.forward * -1;
			bullet.transform.position = point + c.transform.forward.normalized;
			bullet.GetComponent<Rigidbody> ().AddForce (c.transform.forward.normalized * 50, ForceMode.Impulse);
			shootColdTime = 0.05f;
			muzzleFlash.SetActive (true);
			float size = Random.Range (0.2f, 0.3f);
			muzzleFlash.transform.localScale = new Vector3 (size, size, 0);
			muzzleFlash.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
			smoke.Play ();
			float spread = Random.Range (0.4f, 0.6f);
			SetSpread (spread);
			animator.SetBool ("shoot", true);
		}
	}

	public void BeginTalk()
	{
		isTalking = true;
		Animator animator = GetComponent<Animator> ();
		animator.SetBool ("walk", false);
		animator.SetBool ("shoot", false);
		muzzleFlash.SetActive (false);
	}

	public void StopTalk()
	{
		isTalking = false;
	}
}
