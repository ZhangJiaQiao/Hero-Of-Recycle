using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {
	public float maxVelocity_fb;
	public float maxVelocity_rl;
	public GameObject gun;
    public UIProgressBar HPBar;
    public UIProgressBar MPBar;
    private Role role;
	private float jumpColdTime = 0;
    private bool isTalking = false;
	private bool isDied;
    public GameObject Doll;
    private GameObject g;
    private AudioSource _audioSource;
    public AudioClip footSound;

    void Start () {
		isDied = false;
		maxVelocity_fb = 5;
		maxVelocity_rl = 5;
        role = GetComponent<Role>();
		role.destoryEvent += die;
		Renderer r = GetComponent<Renderer> ();
		r.enabled = false;
        _audioSource = this.gameObject.GetComponent<AudioSource>();
	}

	void Update () {
        HPBar.GetComponent<HpUISlider>().UpdateVal(role.hp / 100);
        MPBar.GetComponent<MpUISlider>().UpdateVal(role.mp / 100);
		if (Input.GetKey(KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
		}
        if(!isTalking)
        {
            directionCtrl();
            move();
			jump ();
        }
        if(isDied)
        {
            Camera.main.transform.LookAt(g.transform);
        }
        /*
        if(gun.GetComponent<Animator>().GetBool("shoot") == true)
        {
            _audioSource.clip = gunSound;
            if(_audioSource.isPlaying && _audioSource.clip.name == "gun") { }
            else _audioSource.Play();
        }
        */
    }

    void directionCtrl() {
		if (!isDied) {
			float rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * 2;
			float rotationY = transform.localEulerAngles.x;
			transform.localEulerAngles = new Vector3 (rotationY, rotationX, 0);
		}
	}

	void move() {
		if (!isDied) {
			Animator animator = gun.GetComponent<Animator> ();
			bool isMoving = false;
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
				Vector3 v = GetComponent<Rigidbody> ().velocity;
				Vector3 tmp = transform.forward.normalized;
				float projection = Vector3.Dot(v, tmp) / tmp.sqrMagnitude;
				if (Mathf.Abs(projection) < maxVelocity_fb) {
					v += transform.forward.normalized;
					GetComponent<Rigidbody> ().velocity = v;	
				}
				isMoving = true;
				animator.SetBool("walk", true);
                _audioSource.clip = footSound;
                if (_audioSource.isPlaying && _audioSource.clip.name == "foot") { }
                else _audioSource.Play();
            } 
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
				Vector3 v = GetComponent<Rigidbody> ().velocity;
				Vector3 tmp = transform.forward.normalized;
				float projection = Vector3.Dot(v, tmp * -1) / tmp.sqrMagnitude;
				if (Mathf.Abs(projection) < maxVelocity_fb) {
					v -= transform.forward.normalized;
					GetComponent<Rigidbody> ().velocity = v;	
				}
				isMoving = true;
				animator.SetBool("walk", true);
                _audioSource.clip = footSound;
                if (_audioSource.isPlaying && _audioSource.clip.name == "foot") { }
                else _audioSource.Play();
            }
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				Vector3 v = GetComponent<Rigidbody> ().velocity;
				Vector3 tmp = transform.right.normalized;
				float projection = Vector3.Dot(v, tmp) / tmp.sqrMagnitude;
				if (Mathf.Abs(projection) < maxVelocity_rl) {
					v -= transform.right.normalized;
					GetComponent<Rigidbody> ().velocity = v;	
				}
				isMoving = true;
				animator.SetBool("walk", true);
                _audioSource.clip = footSound;
                if (_audioSource.isPlaying && _audioSource.clip.name == "foot") { }
                else _audioSource.Play();
            } 
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				Vector3 v = GetComponent<Rigidbody> ().velocity;
				Vector3 tmp = transform.right.normalized;
				float projection = Vector3.Dot(v, tmp * -1) / tmp.sqrMagnitude;
				if (Mathf.Abs(projection) < maxVelocity_rl) {
					v += transform.right.normalized;
					GetComponent<Rigidbody> ().velocity = v;	
				}
				isMoving = true;
				animator.SetBool("walk", true);
                _audioSource.clip = footSound;
                if (_audioSource.isPlaying && _audioSource.clip.name == "foot") { }
                else _audioSource.Play();
            } 

			if(!isMoving) {
				animator.SetBool ("walk", false);
			} 	
		}
	}

	void jump() {
		if (!isDied) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (jumpColdTime <= 0) {
					GetComponent<Rigidbody> ().AddForce (transform.up.normalized * 8, ForceMode.Impulse);
					jumpColdTime = 1;
				} 
			} else {
				if (jumpColdTime > 0) {
					jumpColdTime -= Time.deltaTime;
				}
			} 	
		}
	}

    public void BeginTalk()
    {
        isTalking = true;
		gun.GetComponent<Gun> ().BeginTalk ();
    }
    public void StopTalk()
    {
        isTalking = false;
		gun.GetComponent<Gun> ().StopTalk ();
    }

    void OnTriggerEnter(Collider collision)
    {
		if (!isDied) {
			if(collision.gameObject.tag == "otherTrash" || collision.gameObject.tag == "recyclableTrash"
				|| collision.gameObject.tag == "foodTrash" || collision.gameObject.tag == "harmfulTrash")
			{
				pain p = Singleton<pain>.Instance;
				p.showPain ();
				role.hp -= collision.gameObject.GetComponent<Weapon>().damageValue;
			}
		}
    }
	public void die() {
		this.isDied = true;
        Camera.main.transform.parent = null;
        this.gun.SetActive(false);
        g = Instantiate<GameObject>(Doll);
        g.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z + 2);
        g.transform.forward = transform.forward;
    }
}
 