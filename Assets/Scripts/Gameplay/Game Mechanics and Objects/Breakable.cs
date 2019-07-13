using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
	public bool doDestroy = true;
	public bool enablePiecesParticles = false; //if the object contains children that are supposed to spread/burst out when attacking
	public GameObject[] pieces;
	public GameObject[] explosion;

	public AudioClip breakSound;

	private AudioSource aud;

	private bool breakLock = false;

	void Start () {
		aud = GetComponent<AudioSource>();
		if (aud == null)
			aud = FindObjectOfType<AudioSource>();
	}
	
	void Update () {
		
	}

	public void BreakObject()
	{
		if (!breakLock)
		{
			for (int i = 0; i < pieces.GetLength(0); i++)
				Instantiate(pieces[i], transform.position, Quaternion.Euler(0f, 0f, 0f));

			Instantiate(explosion[Random.Range(0, explosion.Length)], transform.position, Quaternion.Euler(0f, 0f, 0f));

			if (aud != null && TogglesValues.sound)
				aud.PlayOneShot(breakSound);

			if (enablePiecesParticles)
			{
				for (int i = 0; i < gameObject.transform.childCount; i++)
				{
					PiecesParticle pp = gameObject.transform.GetChild(i).GetComponent<PiecesParticle>();

					if (pp != null)
						pp.enabled = true;

					gameObject.transform.GetChild(i).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
					gameObject.transform.GetChild(i).GetComponent<Collider2D>().enabled = true;
				}
			}

			if (doDestroy)
				Destroy(gameObject);
			else
			{
				Collider2D col = gameObject.GetComponent<Collider2D>();
				if (col) col.enabled = false;
				enabled = false;
			}

			breakLock = true;
		}
	}
	
	void OnCollisionStay2D( Collision2D coll )
	{
		ThrownObject to = coll.gameObject.GetComponent<ThrownObject>();

		if(coll.gameObject.CompareTag("PlayerAttack")
		&& coll.gameObject.layer != 15 //PiecesCollision
		&& ((to != null && to.breakableBreaksOnCollision)
		|| (to == null && coll.gameObject.transform.parent.parent.parent.GetComponent<Animator>().GetBool("isAttacking"))))
			BreakObject();
	}
	void OnTriggerStay2D( Collider2D coll )
	{
		ThrownObject to = coll.gameObject.GetComponent<ThrownObject>();

		if (coll.gameObject.CompareTag("PlayerAttack")
		&& coll.gameObject.layer != 15 //PiecesCollision
		&& ((to != null && to.breakableBreaksOnCollision)
		|| (to == null && coll.gameObject.transform.parent.parent.parent.GetComponent<Animator>().GetBool("isAttacking"))))
			BreakObject();
	}
}
