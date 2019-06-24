using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AreaBlockObject : MonoBehaviour
{
	[Range(0f,1f)] public float state = 0f;

	[SerializeField] private float maxYOffset = 0.16f;

	[Space]
	public bool enemyStateTransition = true;
	public float stateTransition = 0f;

	[Space]
	public bool resetPosition;
	public Vector3 prevPosition;

	private BoxCollider2D coll;
	private SpriteRenderer spRend;

    void Start()
    {
		coll = gameObject.GetComponent<BoxCollider2D>();
		spRend = gameObject.GetComponent<SpriteRenderer>();
    }
	
    void Update()
    {
		if (resetPosition)
		{
			prevPosition = gameObject.transform.position;
			resetPosition = false;
		}

		coll.enabled = state >= 1f;
		spRend.color = new Color(spRend.color.r, spRend.color.g, spRend.color.b, state);
		transform.position = new Vector3(prevPosition.x, prevPosition.y - (maxYOffset * (state-1f)), prevPosition.z);

		state += stateTransition * Time.deltaTime;

		if (state < 0f) state = 0f;
		else if (state > 1f) state = 1f;

		if(enemyStateTransition)
			stateTransition = GameObject.FindGameObjectWithTag("Enemy") == null ? -1f : 1f;
    }
}
