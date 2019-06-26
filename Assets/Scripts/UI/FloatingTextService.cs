/**
 * Description: Shows floating text style texts and numbers.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class FloatingTextService : MonoBehaviour
{
	public static FloatingTextService Instance { get; private set; }

	[SerializeField] private GameObject floatingTextStandard = null;

	private void Awake( )
	{
		if ( Instance != null && Instance != this )
			Destroy( this );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	void Start( )
	{
		Assert.IsNotNull( floatingTextStandard, $"Please assign <b>Floating Text Standard</b> field: <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
	}

	/// <summary>
	/// Shows a standard Floating Text.
	/// </summary>
	/// <param name="position">In-game world start position.</param>
	/// <param name="text">Text to display.</param>
	/// <param name="tintColor">Text color.</param>
	/// <param name="speedMultiplier"></param>
	/// <param name="size">Scale of the text object.</param>
	public GameObject ShowFloatingTextStandard( Vector3 position, string text, Color tintColor, float speedMultiplier = 1.0f, float scale = 1.0f )
	{
		GameObject go = Instantiate( floatingTextStandard, position, Quaternion.identity );

		FloatingText ft = go.GetComponent<FloatingText>( );
		ft.SetPrameters( text, tintColor, speedMultiplier, scale );

		return go;
	}
}
