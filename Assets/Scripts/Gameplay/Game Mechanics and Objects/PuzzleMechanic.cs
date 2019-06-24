using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PuzzleMechanic : MonoBehaviour
{
	[SerializeField] private bool state = false;

	enum MechanicType
	{
		Indicator,
		Switch,
		Auto_Switch,
		Wall_Switch,
		Pressure_Plate,
		Spikes_Trap
	};

	[SerializeField] private MechanicType type;
	
	[Header("State Sprites")]
	[SerializeField] private Sprite offSprite;
	[SerializeField] private Sprite onSprite;
	
	[Header("State Objects")]
	[SerializeField] private GameObject[] offObjects;
	[SerializeField] private GameObject[] onObjects;
	
	[Header("Object Activation Persistance")]
	[SerializeField] private bool keepOffObjectsInactive = false;
	[SerializeField] private bool keepOnObjectsActive = false;
	
	[Header("State Conditions")]
	[SerializeField] private bool enableConditions = false;
	[SerializeField] private PuzzleMechanic[] OR;
	[SerializeField] private PuzzleMechanic[] AND;
	[SerializeField] private bool NOT = false;
	[SerializeField] private bool noEffectWhenOn = false;
	[SerializeField] private bool noEffectWhenOff = false;
	[SerializeField] private PuzzleMechanic[] updateOnStateChange;
	
	[Header("Extra")]
	[SerializeField] private float rebound = 0f;
	[SerializeField] private float weightRequired = 1f;

	[HideInInspector] public bool refresh = false;

	private float triggered = 0f;
	private bool prevState = false;

	private float reboundTimer = 0f;
	private bool reboundTimerSet = false;

	private SpriteRenderer spRend;

	void Start()
    {
		spRend = gameObject.GetComponent<SpriteRenderer>();
		prevState = !state;
    }
	
    void Update()
    {
		if (rebound > 0f && triggered < weightRequired)
		{
			if (reboundTimer <= 0f)
			{
				if (state)
				{
					if (!reboundTimerSet)
					{
						reboundTimer = rebound;
						reboundTimerSet = true;
					}
					else
					{
						state = false;
						reboundTimer = 0f;
						reboundTimerSet = false;
					}
				}
			}
			else
			{
				reboundTimer -= Time.deltaTime;
			}
		}

		if (prevState != state || refresh)
		{
			foreach (var p in updateOnStateChange)
				p.refresh = true;

			if (enableConditions)
			{
				bool OR_C = false;
				bool AND_C = true;
				foreach (var p in OR)
				{
					if (p.GetState())
					{
						OR_C = true;
						break;
					}
				}

				if (AND.Length <= 0)
					AND_C = false;
				else
				{
					foreach (var p in AND)
					{
						if (!p.GetState())
						{
							AND_C = false;
							break;
						}
					}
				}

				if (!((state && noEffectWhenOn)
				|| (!state && noEffectWhenOff)))
				{
					state = OR_C || AND_C;
					if (NOT) state = !state;
				}
			}

			if (!state)
			{
				spRend.sprite = offSprite;

				if (!keepOffObjectsInactive)
					foreach (var obj in offObjects)
					{
						AreaBlockObject abo = obj.GetComponent<AreaBlockObject>();
						if (abo == null)
							obj.SetActive(true);
						else
							abo.stateTransition = 1f;
					}

				if (!keepOnObjectsActive)
					foreach (var obj in onObjects)
					{
						AreaBlockObject abo = obj.GetComponent<AreaBlockObject>();
						if (abo == null)
							obj.SetActive(false);
						else
							abo.stateTransition = -1f;
					}
			}
			else
			{
				spRend.sprite = onSprite;

				foreach (var obj in onObjects)
				{
					AreaBlockObject abo = obj.GetComponent<AreaBlockObject>();
					if (abo == null)
						obj.SetActive(true);
					else
						abo.stateTransition = 1f;
				}

				foreach (var obj in offObjects)
				{
					AreaBlockObject abo = obj.GetComponent<AreaBlockObject>();
					if (abo == null)
						obj.SetActive(false);
					else
						abo.stateTransition = -1f;
				}
			}

			refresh = false;
		}

		prevState = state;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (type == MechanicType.Auto_Switch)
		{
			if (collision.gameObject.name.Contains("Player"))
			{
				state = true;

				triggered = weightRequired;
			}
		}
		else if (type == MechanicType.Switch)
		{
			ThrownObject to = collision.gameObject.GetComponent<ThrownObject>();

			if (collision.gameObject.tag == "PlayerAttack" && to == null)
			{
				if (collision.gameObject.transform.parent.parent.parent.gameObject.GetComponent<Animator>().GetBool("isAttacking"))
				{
					state = !state;

					triggered = weightRequired;
				}
			}
		}
		else if (type == MechanicType.Wall_Switch)
		{
			if (collision.gameObject.tag == "PlayerAttack")
			{
				state = !state;

				ThrownObject to = collision.gameObject.GetComponent<ThrownObject>();

				if (to != null)
					collision.gameObject.tag = "Untagged";

				triggered = weightRequired;
			}
		}
		else if (type == MechanicType.Pressure_Plate)
		{
			WeightMechanic weightMech = collision.gameObject.GetComponent<WeightMechanic>();

			if (weightMech != null)
			{
				triggered += weightMech.weight;

				state = triggered >= weightRequired;
			}
		}
		else
		{
			triggered = weightRequired;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (type == MechanicType.Auto_Switch)
		{
			if (collision.gameObject.name.Contains("Player"))
			{
				if (rebound <= 0f)
					state = false;

				triggered = 0f;
			}
		}
		else if (type == MechanicType.Pressure_Plate)
		{
			WeightMechanic weightMech = collision.gameObject.GetComponent<WeightMechanic>();

			if (weightMech != null)
			{
				triggered -= weightMech.weight;

				if (rebound <= 0f)
					state = triggered >= weightRequired;
			}
		}
		else
		{
			triggered = 0f;
		}
	}

	public bool GetState()
	{
		return state;
	}
}
