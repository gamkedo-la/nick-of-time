using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControlSystem : MonoBehaviour
{
	[System.Serializable]
	public class GameObjectsArray
	{
		public GameObject[] objects = null;
	};

	public bool forceActivate = false;
	public bool selection = false;
	public int row = 0;
	public int col = 0;
	public GameObjectsArray[] gridObjects = null; //gridObjects[row].objects[col]

	[Space]
	public float delay = 0.1f;
	
	[Space]
	public string[] horizontalInputs = null;
	public string[] verticalInputs = null;

	[Space]
	public string[] selectionInputs = null;
	public string[] cancelationInputs = null;

	private float timer = 0f;

	void Start()
	{

	}

	void RowLimit()
	{
		if (row < 0) row = gridObjects.GetLength(0) - 1;
		if (row >= gridObjects.GetLength(0)) row = 0;
	}

	void ColLimit()
	{
		if (col < 0) col = gridObjects[row].objects.GetLength(0) - 1;
		if (col >= gridObjects[row].objects.GetLength(0)) col = 0;
	}

	void Update()
	{
		RowLimit();
		ColLimit();

		if (selection)
			gridObjects[row].objects[col].GetComponent<ScaleOnMouseOver>().OnSelection();

		if (forceActivate || GetComponent<MenuAnimatorState>().getState())
		{
			if (timer <= 0f)
			{
				foreach (var i in verticalInputs)
				{
					int inputValue = Mathf.FloorToInt(Input.GetAxisRaw(i));

					do
					{
						if (selection)
						{
							row -= inputValue;
							if (inputValue != 0) timer = delay;
						}
						else if (inputValue != 0)
							selection = true;

						RowLimit();
					}
					while (!gridObjects[row].objects[col].activeSelf || !gridObjects[row].objects[col].transform.parent.gameObject.activeSelf);
				}

				foreach (var i in horizontalInputs)
				{
					int inputValue = Mathf.FloorToInt(Input.GetAxisRaw(i));

					do
					{
						if (selection)
						{
							col -= inputValue;
							if(inputValue != 0) timer = delay;
						}
						else if (inputValue != 0)
							selection = true;

						ColLimit();
					}
					while (!gridObjects[row].objects[col].activeSelf || !gridObjects[row].objects[col].transform.parent.gameObject.activeSelf);
				}

				foreach (var i in selectionInputs)
				{
					if (Input.GetButtonDown(i) && selection)
					{
						timer = delay;

						gridObjects[row].objects[col].GetComponent<ButtonToScene>()?.OnSelection();
						gridObjects[row].objects[col].GetComponent<SubMenuActivationButton>()?.OnSelection();
						gridObjects[row].objects[col].GetComponent<ButtonToCallFunction>()?.OnSelection();
						gridObjects[row].objects[col].GetComponent<ButtonToToggle>()?.OnSelection();
						gridObjects[row].objects[col].GetComponent<ResumeAction>()?.OnSelection();
					}
				}

				foreach (var i in cancelationInputs)
				{
					if (Input.GetButtonDown(i))
					{
						selection = false;
						row = 0;
						col = 0;

						timer = delay;
					}
				}
			}
			else
			{
				timer -= Time.unscaledDeltaTime;
			}
		}
	}
}
