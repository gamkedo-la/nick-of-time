using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightUIButton : MonoBehaviour
{
    public void HighlightButton(Button button)
    {
        button.Select();
    }
}
