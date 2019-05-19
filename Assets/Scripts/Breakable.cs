using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {
	
	public GameObject[] pieces;

	void Start () {
		
	}
	
	void Update () {
		
	}
	
	void OnCollisionStay2D( Collision2D coll ) {
		if(coll.gameObject.CompareTag("PlayerAttack")
			&& coll.gameObject.transform.parent.parent.GetComponent<Animator>().GetBool("isAttacking"))
		{
			for(int i = 0; i < pieces.GetLength(0); i++)
			{
				Instantiate(pieces[i], transform.position, Quaternion.Euler(0f,0f,Random.Range(0f,360f)));
			}
			Destroy(gameObject);
		}
	}
	void OnTriggerStay2D( Collider2D coll ) {
		if(coll.gameObject.CompareTag("PlayerAttack")
			&& coll.gameObject.transform.parent.parent.GetComponent<Animator>().GetBool("isAttacking"))
		{
			for(int i = 0; i < pieces.GetLength(0); i++)
			{
				Instantiate(pieces[i], transform.position, Quaternion.Euler(0f,0f,Random.Range(0f,360f)));
			}
			Destroy(gameObject);
		}
	}
}
