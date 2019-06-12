using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBlockActivator : MonoBehaviour
{
	public AreaBlockObject areaBlock;
	public float toState = 1f;

	public bool destroyOnTrigger = true;

    void Start()
    {
        
    }
	
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		areaBlock.stateTransition = toState;

		if (destroyOnTrigger) Destroy(gameObject);
	}
}
