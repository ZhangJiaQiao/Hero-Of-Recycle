using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monstersCreator : MonoBehaviour {
 	public List<Transform> spawnPoints;
	public List<string> types;
	public float coldTime;
	public int totalAmount;
	private int currentAmount;
	public int max;
	private bool isCreate;

	void Start() {
		GetComponent<Renderer> ().enabled = false;
		currentAmount = 0;
		if (types.Count == 0) {
			types = new List<string>{"foodTrash", "recyclableTrash", "otherTrash", "harmfulTrash"};	
		}
		isCreate = false;
	}

	void Update () {
		if (isCreate) {
			if (totalAmount > 0 && currentAmount < max && coldTime <= 0 && spawnPoints.Count > 0 && max > 0) {
				createMonster ();
				coldTime = 5;
			}

			if (coldTime > 0) {
				coldTime -=  Time.deltaTime;
			}	
		}
	}

	private void createMonster() {
		int type = Random.Range (0, types.Count);
		int pos = Random.Range (0, spawnPoints.Count);
		myFactory mF = Singleton<myFactory>.Instance;
		GameObject monster =  mF.getMonster (types[type]);
		switch(types[type]) {
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
		monster.transform.position = spawnPoints[pos].position;
		currentAmount++;
		totalAmount--;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && SSDirector.currentScene != 1) {
			isCreate = true;
		}
	}

	public void monsterDestroyHandler() {
		this.currentAmount--;
	}
}
	
