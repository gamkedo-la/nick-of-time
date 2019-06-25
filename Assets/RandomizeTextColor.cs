using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomizeTextColor : MonoBehaviour
{
	public Color[] colors;
	
    void Start()
    {
		GetComponent<TextMeshPro>().color = colors[Mathf.FloorToInt(Random.Range(0f, colors.Length))];
    }
}
