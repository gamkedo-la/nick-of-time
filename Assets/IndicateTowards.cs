﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicateTowards : MonoBehaviour
{
	public enum IndicateToEntity
	{
		Player1,
		Player2,
		Enemy,
		NPC
	};

	[SerializeField] private IndicateToEntity type = IndicateToEntity.Player2;
	[SerializeField] private Camera cam;
	[SerializeField] private Vector4 rectangleConfinement;
	[SerializeField] private bool setAngle = true;
	[SerializeField] private float angleOffset = 0f;

	[HideInInspector] public GameObject objectToIndicate;

    void Start()
    {
		if (type == IndicateToEntity.Player1)
		{
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			objectToIndicate = players[0].name == "Player1" ? players[0] : players[1];
		}
		else if (type == IndicateToEntity.Player2)
		{
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			objectToIndicate = players[0].name == "Player2" ? players[0] : players[1];
		}
	}
	
    void Update()
    {
		Vector3 pos = transform.position;
		Vector3 target = objectToIndicate.transform.position;

		Vector2 sub = pos - target;
		float angle = Mathf.Atan2(sub.y, sub.x);

		pos = target;

		if (pos.x < transform.parent.transform.position.x + (rectangleConfinement.x * cam.orthographicSize))
			pos.x = transform.parent.transform.position.x + (rectangleConfinement.x * cam.orthographicSize);
		if (pos.y < transform.parent.transform.position.y + (rectangleConfinement.y * cam.orthographicSize))
			pos.y = transform.parent.transform.position.y + (rectangleConfinement.y * cam.orthographicSize);
		if (pos.x > transform.parent.transform.position.x + (rectangleConfinement.z * cam.orthographicSize))
			pos.x = transform.parent.transform.position.x + (rectangleConfinement.z * cam.orthographicSize);
		if (pos.y > transform.parent.transform.position.y + (rectangleConfinement.w * cam.orthographicSize))
			pos.y = transform.parent.transform.position.y + (rectangleConfinement.w * cam.orthographicSize);

		transform.rotation = Quaternion.Euler(0f, 0f, (angle * Mathf.Rad2Deg) + angleOffset);
		transform.position = pos;
    }
}
