using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSetDialogueToIncrementScript : MonoBehaviour
{
    public void commandSetDialogueToIncrement()
    {
		transform.parent.gameObject.GetComponent<EventSetup>().commandSetDialogueToIncrement();
    }
}
