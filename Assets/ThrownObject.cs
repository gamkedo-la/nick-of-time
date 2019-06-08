using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObject : MonoBehaviour
{
	public Vector3 startPos = Vector3.zero;
	public Vector3 throwVelocity = Vector3.zero;
	public float throwRotation = 0f;
	public bool breakableBreaksOnCollision = false;

	private Rigidbody2D rb;

    void Start()
    {
		gameObject.GetComponent<Collider2D>().isTrigger = false;
		rb = gameObject.AddComponent<Rigidbody2D>();
		rb.gravityScale = 0f;
		rb.freezeRotation = true;
    }
	
    void Update()
    {
		startPos += throwVelocity * Time.deltaTime;
		transform.position = startPos;
		transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + throwRotation);
    }

	/*
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player" || collider.name.Contains("Player"))
		{
			if (throwVelocity == Vector3.zero)
			{
				collider.GetComponent<PlayerController>().weaponPossession.weaponID = gameObject.GetComponent<WeaponPossession>().weaponID;
				Destroy(gameObject);
			}
		}
		else if(collider.tag != "PlayerAttack")
		{
			Debug.Log(collider.name);
			throwVelocity = Vector3.zero;
			throwRotation = 0f;
		}
	}
	*/

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" || collision.gameObject.name.Contains("Player"))
		{
			if (throwVelocity == Vector3.zero)
			{
				collision.gameObject.GetComponent<PlayerController>().weaponPossession.weaponID = gameObject.GetComponent<WeaponPossession>().weaponID;
				Destroy(gameObject);
			}
		}
		else if (collision.gameObject.tag != "PlayerAttack")
		{
			Debug.Log(collision.gameObject.name);
			throwVelocity = Vector3.zero;
			throwRotation = 0f;
		}
	}
}
