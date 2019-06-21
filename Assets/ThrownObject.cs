using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObject : MonoBehaviour
{
	public Vector3 startPos = Vector3.zero;
	public Vector3 throwVelocity = Vector3.zero;
	public float throwRotation = 0f;
	public bool noMove = false;
	public bool breakableBreaksOnCollision = false;

	private Rigidbody2D rb;

    void Start()
    {
		gameObject.GetComponent<Collider2D>().isTrigger = false;
		rb = gameObject.AddComponent<Rigidbody2D>();

		if (rb != null)
		{
			rb.gravityScale = 0f;
			rb.freezeRotation = true;
		}
    }
	
    void Update()
    {
		if (!noMove)
		{
			startPos += throwVelocity * Time.deltaTime;
			transform.position = startPos;
			transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + throwRotation);
		}
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
			throwVelocity = Vector3.zero;
			throwRotation = 0f;
			gameObject.layer = LayerMask.NameToLayer("Default");
		}
	}

	public int GetDirection()
	{
		if (throwVelocity.y > 0) return 0;
		else if (throwVelocity.x > 0) return 1;
		else if (throwVelocity.y < 0) return 2;
		else if (throwVelocity.x < 0) return 3;

		return -1;
	}
}
