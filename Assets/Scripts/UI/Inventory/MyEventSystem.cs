using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MyEventSystem : EventSystem
{
  protected override void OnEnable()
    {
        base.OnEnable();

		if (GameManager.singleGame)
		{
			GetComponent<StandaloneInputModule>().horizontalAxis = "HorizontalSinglePlayer";
			GetComponent<StandaloneInputModule>().verticalAxis = "VerticalSinglePlayer";
			GetComponent<StandaloneInputModule>().submitButton = "FireSinglePlayer";
			GetComponent<StandaloneInputModule>().cancelButton = "InventorySinglePlayer";
		}
		else if (gameObject.name.Contains("1"))
		{
			if (TogglesValues.p1controller != "")
			{
				GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal" + TogglesValues.p1controller;
				GetComponent<StandaloneInputModule>().verticalAxis = "Vertical" + TogglesValues.p1controller;
				GetComponent<StandaloneInputModule>().submitButton = "Fire" + TogglesValues.p1controller;
				GetComponent<StandaloneInputModule>().cancelButton = "Inventory" + TogglesValues.p1controller;
			}
		}
		else if (gameObject.name.Contains("2"))
		{
			if (TogglesValues.p2controller != "")
			{
				GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal" + TogglesValues.p2controller;
				GetComponent<StandaloneInputModule>().verticalAxis = "Vertical" + TogglesValues.p2controller;
				GetComponent<StandaloneInputModule>().submitButton = "Fire" + TogglesValues.p2controller;
				GetComponent<StandaloneInputModule>().cancelButton = "Inventory" + TogglesValues.p2controller;
			}
		}
	}

    // Update is called once per frame
    protected override void Update()
    {
        EventSystem originalCurrent = EventSystem.current;
        current = this;
        base.Update();
        current = originalCurrent;
    }
}
