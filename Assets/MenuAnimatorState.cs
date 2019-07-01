using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimatorState : MonoBehaviour
{
	public bool setState = true;

    void Start()
    {
		GetComponent<Animator>().SetBool("state", setState);
    }

	public void stateON() { GetComponent<Animator>().SetBool("state", true); }

	public void stateOFF() { GetComponent<Animator>().SetBool("state", false); }
}
