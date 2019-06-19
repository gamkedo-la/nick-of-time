/**
 * Description: Does a blinking effect.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class BlinkEffect : MonoBehaviour
{
	[SerializeField] private SpriteRenderer[] spritesToBlink = null;
	[SerializeField] private Material normalMaterial = null;
	[SerializeField] private Material blinkMaterial = null;
	[SerializeField] private float blinkTime = 0.1f;
	[SerializeField] private Color normalColor = Color.white;
	[SerializeField] private Color blinkColor = Color.white;
	[SerializeField] private bool changeColor = false;

	private bool blinking = false;

	void Start( )
	{
		Assert.AreNotEqual( spritesToBlink.Length, 0, "You need to add sprites." );
		Assert.IsNotNull( normalMaterial, "Normal material can not be empty." );
		Assert.IsNotNull( blinkMaterial, "Blink material can not be empty." );
	}

	/// <summary>
	/// Do a blink.
	/// </summary>
	public void Blink( )
	{
		Blink( blinkTime );
	}

	/// <summary>
	/// Do a blink and overwrite the blink time from the inspector.
	/// </summary>
	/// <param name="time">Time the blink will last.</param>
	public void Blink( float time )
	{
		if ( blinking )
			return;
		Debug.Log( "Blink" );
		blinking = true;
		SwapMaterial( blinkMaterial, Color.white );
		Invoke( "Unblink", time );
	}

	private void Unblink( )
	{
		SwapMaterial( normalMaterial, normalColor );
		blinking = false;
	}

	private void SwapMaterial( Material material, Color color )
	{
		foreach ( var sprite in spritesToBlink )
		{
			sprite.material = material;
			if ( changeColor )
				sprite.color = color;
		}
	}
}
