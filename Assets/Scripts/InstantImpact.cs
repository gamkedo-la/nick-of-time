using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class InstantImpact : MonoBehaviour
{
	public enum ImpactType
	{
		HP,
		Stamina
	};

	[SerializeField] private ImpactType type = ImpactType.HP;
	[SerializeField] private float value = 0.1f;
	[SerializeField] private float minCollectDistance = 0.02f;
	[SerializeField] private float magnetStrength = 0.01f;
	[SerializeField] private AudioClip impactSound;

	private AudioSource aud;

	private void Start()
	{
		aud = GetComponent<AudioSource>();
		if (aud == null)
			aud = FindObjectOfType<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name.Contains("Player"))
		{
			if (Vector2.Distance(transform.position, collision.gameObject.transform.position) <= minCollectDistance)
			{
				if (type == ImpactType.HP)
					collision.gameObject.GetComponent<HitCheck>().hp += value;
				else if (type == ImpactType.Stamina)
					collision.gameObject.GetComponent<PlayerController>().actionPoints += value;

				if (aud != null && TogglesValues.sound)
					aud.PlayOneShot(impactSound);

				Destroy(gameObject);
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, collision.gameObject.transform.position, magnetStrength);
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.name.Contains("Player"))
		{
			if (Vector2.Distance(transform.position, collision.gameObject.transform.position) <= minCollectDistance)
			{
				if (type == ImpactType.HP)
					collision.gameObject.GetComponent<HitCheck>().hp += value;
				else if (type == ImpactType.Stamina)
					collision.gameObject.GetComponent<PlayerController>().actionPoints += value;

				if (collision.gameObject.name == "Player1") ArenaScoreSystem.AddPlayer1Score(2);
				else if (collision.gameObject.name == "Player2") ArenaScoreSystem.AddPlayer2Score(2);

				if (aud != null && TogglesValues.sound)
					aud.PlayOneShot(impactSound);

				Destroy(gameObject);
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, collision.gameObject.transform.position, magnetStrength);
			}
		}
	}
}
