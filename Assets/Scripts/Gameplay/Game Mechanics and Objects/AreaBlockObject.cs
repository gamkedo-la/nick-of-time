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

	[Space]
	public AudioClip openSound;
	public AudioClip closeSound;

	private float prevStateTransition;

	private BoxCollider2D coll;
	private SpriteRenderer spRend;

	private AudioSource aud;

    void Start()
    {
		coll = gameObject.GetComponent<BoxCollider2D>();
		spRend = gameObject.GetComponent<SpriteRenderer>();

		aud = GetComponent<AudioSource>();
		if (aud == null)
			aud = FindObjectOfType<AudioSource>();

		prevStateTransition = stateTransition;
	}
	
    void Update()
    {
		if (prevStateTransition != stateTransition)
		{
			if (stateTransition > 0.5f)
			{
				if (aud != null && TogglesValues.sound)
					aud.PlayOneShot(closeSound);
			}
			else if (stateTransition < -0.5f)
			{
				if (aud != null && TogglesValues.sound)
					aud.PlayOneShot(openSound);
			}
		}

		if (resetPosition)
		{
			prevPosition = gameObject.transform.position;
			resetPosition = false;
		}

		coll.enabled = state >= 1f;
		spRend.color = new Color(spRend.color.r, spRend.color.g, spRend.color.b, state);
		transform.position = new Vector3(prevPosition.x, prevPosition.y - (maxYOffset * (state-1f)), prevPosition.z);

		state += (stateTransition * 3f) * Time.deltaTime;

		if (state < 0f) state = 0f;
		else if (state > 1f) state = 1f;

		if(enemyStateTransition)
			stateTransition = GameObject.FindGameObjectWithTag("Enemy") == null ? -1f : 1f;

		prevStateTransition = stateTransition;
	}
}
