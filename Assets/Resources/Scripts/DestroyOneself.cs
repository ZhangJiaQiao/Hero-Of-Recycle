using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOneself : MonoBehaviour {
	public float DestroyTime = 5;
	// Use this for initialization
	void Start () {
		destroyItself ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void destroyItself()
	{
		StartCoroutine("Dispear");
	}

	IEnumerator Dispear()
	{
		yield return new WaitForSeconds(DestroyTime);
		Debug.Log ("I should be destroyed");
		GameObject.Destroy (this.gameObject);
	}
}
