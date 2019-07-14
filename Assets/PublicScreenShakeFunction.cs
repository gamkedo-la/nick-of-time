using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicScreenShakeFunction : MonoBehaviour
{
	public ScreenShake screenShake;
	
	public void SmallShake() { screenShake.SmallShake(); }

	public void MediumShake() { screenShake.MediumShake(); }

	public void BigShake() { screenShake.BigShake(); }

	public void Earthquake() { screenShake.Earthquake(); }
}
