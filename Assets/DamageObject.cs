using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
	[SerializeField] private float hpDamage = 0.1f;
	[SerializeField] private bool disableObjectAfterDamage = false;
	[SerializeField] private bool destroyScriptAfterDamage = false;
	[SerializeField] private bool destroyObjectAfterDamage = false;

    void Start()
    {
        
    }
	
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name.Contains("Player"))
		{
			if (disableObjectAfterDamage) gameObject.SetActive(false);
			if (destroyScriptAfterDamage) Destroy(this);
			if (destroyObjectAfterDamage) Destroy(gameObject);
		}
	}
}
