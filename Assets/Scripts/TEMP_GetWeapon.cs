using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_GetWeapon : MonoBehaviour
{
	public int weaponID = 0;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name.Contains("Player"))
		{
			collision.gameObject.GetComponent<PlayerController>().weaponPossession.weaponID = weaponID;
			Destroy(gameObject);
		}
	}
}
