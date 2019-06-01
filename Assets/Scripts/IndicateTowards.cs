using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicateTowards : MonoBehaviour
{
	public enum IndicateToEntity
	{
		Player1,
		Player2,
		Enemy_Player1,
		Enemy_Player2,
		NPC
	};

	public IndicateToEntity type = IndicateToEntity.Player2;
	public Camera cam;
	
	public Transform bottomLeft;
	public Transform topRight;
	[SerializeField] private float ratio;

	[SerializeField] private bool setAngle = true;
	[SerializeField] private float angleOffset = 0f;

	[HideInInspector] public GameObject objectToIndicate;

	private Vector3 bottomLeftPrevPos;
	private Vector3 topRightPrevPos;

	private GameObject player1;
	private GameObject player2;

    void Start()
    {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

		if (players != null)
		{
			if (players.Length > 1)
			{
				player2 = players[0].name == "Player1" ? players[0] : players[1];
				player2 = players[0].name == "Player2" ? players[0] : players[1];
			}
			else
				player1 = players[0];

			if (type == IndicateToEntity.Player1)
				objectToIndicate = player1;
			else if (type == IndicateToEntity.Player2)
				objectToIndicate = player2;
		}
		else
		{
			Destroy(gameObject);
		}

		bottomLeftPrevPos = bottomLeft.transform.position;
		topRightPrevPos = topRight.transform.position;
	}
	
    void Update()
    {
		if (objectToIndicate != null)
		{
			Vector3 pos = transform.position;
			Vector3 target = objectToIndicate.transform.position;

			Vector2 sub = pos - target;
			float angle = Mathf.Atan2(sub.y, sub.x);

			pos = target;

			/*
			if (pos.x < transform.parent.transform.position.x + (rectangleConfinement.x * cam.orthographicSize))
				pos.x = transform.parent.transform.position.x + (rectangleConfinement.x * cam.orthographicSize);
			if (pos.y < transform.parent.transform.position.y + (rectangleConfinement.y * cam.orthographicSize))
				pos.y = transform.parent.transform.position.y + (rectangleConfinement.y * cam.orthographicSize);
			if (pos.x > transform.parent.transform.position.x + (rectangleConfinement.z * cam.orthographicSize))
				pos.x = transform.parent.transform.position.x + (rectangleConfinement.z * cam.orthographicSize);
			if (pos.y > transform.parent.transform.position.y + (rectangleConfinement.w * cam.orthographicSize))
				pos.y = transform.parent.transform.position.y + (rectangleConfinement.w * cam.orthographicSize);
				*/

			if (cam.orthographicSize * ratio > 0f)
				bottomLeft.transform.parent.localScale = new Vector2(cam.orthographicSize * ratio, cam.orthographicSize * ratio);

			if (pos.x < bottomLeft.position.x)
				pos.x = bottomLeft.position.x;
			if (pos.y < bottomLeft.position.y)
				pos.y = bottomLeft.position.y;
			if (pos.x > topRight.position.x)
				pos.x = topRight.position.x;
			if (pos.y > topRight.position.y)
				pos.y = topRight.position.y;

			transform.rotation = Quaternion.Euler(0f, 0f, (angle * Mathf.Rad2Deg) + angleOffset);
			transform.position = pos;
			
			if (type == IndicateToEntity.Enemy_Player1 && player1 != null)
			{
				SpriteRenderer sprRend = gameObject.GetComponent<SpriteRenderer>();

				Color col = sprRend.color;
				col.a = 1f - (Vector2.Distance(target, player1.transform.position) / 2.5f);
				sprRend.color = col;
			}
			else if (type == IndicateToEntity.Enemy_Player2 && player2 != null)
			{
				SpriteRenderer sprRend = gameObject.GetComponent<SpriteRenderer>();

				Color col = sprRend.color;
				col.a = 1f - (Vector2.Distance(target, player2.transform.position) / 2.5f);
				sprRend.color = col;
			}
		}
    }
}
