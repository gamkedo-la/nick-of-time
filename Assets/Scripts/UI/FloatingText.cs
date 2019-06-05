/**
 * Description: Floating Text control class.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class FloatingText : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI text = null;
	[SerializeField] private float speedMove = 2f;
	[SerializeField] private float speedFade = 0.5f;
	[SerializeField] private float speedSideways = 2f;
	[SerializeField] private float speedDown = 4f;

	private float multiplier = 1;
	private float gravity = 0;
	private float sideways = 0;
	private float variance = 1;

	void Start ()
	{
		Assert.IsNotNull( text, $"Please assign <b>Text</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
	}

	private void Update( )
	{
		Animate( );
	}

	private void Animate( )
	{
		gravity -= speedDown * multiplier * Time.deltaTime;
		Vector3 moveUp = Vector3.up * 4;
		Vector3 moveDown = Vector3.down * -gravity * variance;
		Vector3 moveSideways = transform.right * sideways * variance;
		float speed = speedMove * multiplier * Time.deltaTime;

		transform.position += ( moveUp + moveSideways + moveDown ) * speed;

		Color c = text.color;
		c.a -= speedFade * multiplier * Time.deltaTime;
		c.a = Mathf.Clamp( c.a, 0f, 1f );
		text.color = c;

		if ( c.a <= 0 )
			Destroy( gameObject );
	}

	public void SetPrameters ( string message, Color tintColor, float speedMultiplier, float scale )
	{
		multiplier = speedMultiplier;
		text.text = message;
		text.color *= tintColor;

		transform.localScale = Vector3.one * scale;

		sideways = speedSideways * Random.Range( 0, 2 ) > 0 ? 1 : -1;
		variance = Random.Range( 0.5f, 1.5f );
	}
}
