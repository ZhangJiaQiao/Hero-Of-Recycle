using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour {
    public float speed = 1;
    private bool move = false;
    private Vector3 Target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(move)
        {
            Move();
        }
	}

    public void canMove(Vector3 Tar)
    {
        Target = Tar;
        move = true;
    }

    public void Move()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, Target, speed * Time.deltaTime);
        this.GetComponent<Animator>().SetInteger("State", 2);
        this.transform.LookAt(Target);
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Talk()
    {
        move = false;
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        this.GetComponent<Animator>().SetInteger("State", 1);
    }
}
