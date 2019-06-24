using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantImpact : MonoBehaviour
{
	public enum ImpactType
	{
		HP,
		Stamina
	};

	[SerializeField] private ImpactType type = ImpactType.HP;
	[SerializeField] private float value = 0.1f;
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name.Contains("Player"))
		{
			if (type == ImpactType.HP)
				collision.gameObject.GetComponent<HitCheck>().hp += value;
			else if (type == ImpactType.Stamina)
				collision.gameObject.GetComponent<PlayerController>().actionPoints += value;

			Destroy(gameObject);
		}
	}
}
