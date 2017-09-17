using UnityEngine;
using UnityEngine.UI;
using System.IO;  
using System.Collections;
using System.Collections.Generic;

public class boss : MonoBehaviour {
	public float maxVelocity = 50;
	//	public Vector3 playerPosition;
	public Transform player;
	private bool findThePlayer = false;
	private Animator animator;
	public characterProperty CharacterProperty;
	private bool readyForSkill = true;
	private float timer = 60.0f;
	private string[] trashName = { "battery", "bone", "china", "clothe", "dirtypaper", "dusty","fruit", "glass", "greens", "ink", "leave",
		"light", "medicine", "metal", "milk", "oil", "once", "paper", "pet", "plastic", "rise", "smoke", "tea", "杀虫剂"};
	void Start () {
		CharacterProperty = this.gameObject.GetComponent<characterProperty>();
		animator = GetComponent<Animator> ();
	}

	void Update () {
		useSkill();
		findPlayer ();						//if the player is close enough to track
		directionCtrl ();					//trun direction to the player
		moveAndAttack ();					//move and attack player
	}

	void FixedUpdate() {
		if (CharacterProperty.speed <= 0.0f)
			CharacterProperty.speed = 0.0f;
		if (CharacterProperty.damageValue <= 0.0f)
			CharacterProperty.damageValue = 0.0f;
		if (CharacterProperty.life <= 0) {
			animator.SetBool ("dead", true);
			destroyItself();
		}
		if (CharacterProperty.life >= 100)
		{
			CharacterProperty.life = 100;
		}
	}


	void useSkill() {
		string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;
		if (readyForSkill && currentClip == "Idle") {
			animator.SetTrigger ("skill");
			creatMonsters ();
			changeTag ();
			readyForSkill = false;
		}
		timer -= Time.deltaTime;
		if (timer < 0.0f) {
			readyForSkill = true;
			timer = 60.0f;
		}
	}


	void creatMonsters () {
		List<string> types = new List<string>{"foodTrash", "recyclableTrash", "otherTrash", "harmfulTrash"};
		for (int i = 0; i < 3; i++) {
			int type = Random.Range (0, types.Count);
			myFactory mF = Singleton<myFactory>.Instance;
			GameObject monster = mF.getMonster (types [type]);
			switch (types [type]) {
			case "foodTrash":
				{
					ghost g = monster.GetComponent<ghost> ();
					if (g != null) {
						g.destroyEvent += monsterDestroyHandler;
					} else {
						crab c = monster.GetComponent<crab> ();
						c.destroyEvent += monsterDestroyHandler;
					}
					break;
				}
			case "harmfulTrash":
				{
					ghost g = monster.GetComponent<ghost> ();
					g.destroyEvent += monsterDestroyHandler;
					break;
				}
			case "recyclableTrash":
				{
					bone b = monster.GetComponent<bone> ();
					b.destroyEvent += monsterDestroyHandler;
					break;
				}
			case "otherTrash":
				{
					bone b = monster.GetComponent<bone> ();
					b.destroyEvent += monsterDestroyHandler;
					break;
				}
			}
			monster.transform.position = this.transform.position + new Vector3 (Random.Range(1, 4), 0, Random.Range(1, 4));
		}
	}

	void monsterDestroyHandler() {
	}

	void findPlayer() {
		if (CloseToTrack()) {
			findThePlayer = true;
		}
	}

	void changeTag () {
		int randomNum = Random.Range (0, 24);				//所有垃圾的种类数目。
		string nameOfTrash = trashName[randomNum];
		string tag = "";
		if (nameOfTrash == "battery" || nameOfTrash == "ink" || nameOfTrash == "light" || nameOfTrash == "medicine" || nameOfTrash == "oil" || nameOfTrash == "杀虫剂")
			tag = "harmfulTrash";
		else if (nameOfTrash == "clothe" || nameOfTrash == "glass" || nameOfTrash == "metal" || nameOfTrash == "milk" || nameOfTrash == "paper" || nameOfTrash == "plastic")
			tag = "recyclableTrash";
		else if (nameOfTrash == "bone" || nameOfTrash == "fruit" || nameOfTrash == "greens" || nameOfTrash == "leave" || nameOfTrash == "rise" || nameOfTrash == "tea")
			tag = "foodTrash";
		else if (nameOfTrash == "china" || nameOfTrash == "dirtypaper" || nameOfTrash == "dusty" || nameOfTrash == "pet" || nameOfTrash == "smoke")
			tag = "otherTrash";

		Transform PanelOfBoss = this.transform.Find ("Panel");
		GameObject Label = PanelOfBoss.Find ("Label").gameObject;
		GameObject Texture = PanelOfBoss.Find ("Texture").gameObject;
		Label.GetComponent<UILabel>().text = nameOfTrash;
		UITexture textureComponent = Texture.GetComponent<UITexture> ();
		textureComponent.mainTexture = Resources.Load ("Texture/label/" + nameOfTrash) as Texture2D;
		GameObject.Find("core").tag = tag;					//最好修改一下。
	}


	bool CloseToTrack() {
		return Vector3.Distance (transform.position, player.position) < 100;
	}

	bool closeToAttack() {
		return Vector3.Distance (transform.position, player.position) < 4;
	}

	void directionCtrl() {
		string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;
		if (!findThePlayer && currentClip != "n2010_die")
			return;
		Vector3 targetDir = player.position - transform.position;
		float step = 10 * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		transform.rotation = Quaternion.LookRotation(newDir);
	}


	void moveAndAttack() {
		string currentClip = animator.GetCurrentAnimatorClipInfo (0) [0].clip.name;
		if (!findThePlayer || currentClip == "n2010_die")
			return;
		Vector3 v = GetComponent<Rigidbody> ().velocity;
		if (CloseToTrack () && !closeToAttack ()) {
			if (currentClip == "Run") {
				if (v.sqrMagnitude < maxVelocity) {
					v += CharacterProperty.speed * transform.forward.normalized;
					GetComponent<Rigidbody> ().velocity = v;	
				}
			}
			animator.SetBool ("run", true);
		} else if (closeToAttack ()) {
			int attackType = Random.Range (1, 4);
			animator.SetTrigger ("attack" + attackType);
		} else {
			animator.SetBool ("Idle", true);
		}
	}

	void substract() {
		CharacterProperty.speed -= 0.1f;
		CharacterProperty.life -= 40;
		CharacterProperty.damageValue -= 2f;
		Debug.Log ("substract");
	}

	void add() {
		CharacterProperty.speed += 0.1f;
		CharacterProperty.life += 40;
		CharacterProperty.damageValue += 2f;
		Debug.Log ("add");
	}

	void destroyItself()
	{
		StartCoroutine("Dispear");
	}

	IEnumerator Dispear()
	{
		yield return new WaitForSeconds(5);
		this.gameObject.SetActive(false);
	}
}
