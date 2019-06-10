using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBlockObject : MonoBehaviour
{
	[Range(0f,1f)] public float state = 0f;

	[SerializeField] private float maxYOffset = 0.16f;

	private float stateTransition = 1f;

	private BoxCollider2D coll;
	private SpriteRenderer spRend;
	private Vector3 prevPosition;

    void Start()
    {
		coll = gameObject.GetComponent<BoxCollider2D>();
		spRend = gameObject.GetComponent<SpriteRenderer>();
		prevPosition = gameObject.transform.position;
    }
	
    void Update()
    {
		coll.enabled = state >= 1f;
		spRend.color = new Color(spRend.color.r, spRend.color.g, spRend.color.b, state);
		transform.position = new Vector3(prevPosition.x, prevPosition.y - (maxYOffset * (state-1f)), prevPosition.z);

		state += stateTransition * Time.deltaTime;

		if (state < 0f) state = 0f;
		else if (state > 1f) state = 1f;

		stateTransition = GameObject.FindGameObjectWithTag("Enemy") == null ? -1f : 1f;
    }
}
