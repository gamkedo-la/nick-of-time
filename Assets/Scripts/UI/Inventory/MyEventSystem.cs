using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MyEventSystem : EventSystem
{
  protected override void OnEnable()
    {
        base.OnEnable();
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
