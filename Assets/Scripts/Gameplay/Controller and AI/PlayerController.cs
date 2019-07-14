using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
	public float walkSpeed = 1f;
	public float attackSpeed = 2f;
	public float attackSpeedTime = 0.1f;
	public float dashSpeed = 3f;
	public float dashTime = 0.2f;
	public float pushForce = 500f;
	public float throwSpeed = 2f;

	[Space]
	public GameObject dashFXObject;
	public GameObject attackFXObject;
	public GameObject pushFXObject;
	public GameObject throwFXObject;
	public GameObject walkFXObject;
	public GameObject hitComboTextObject;

	private const float MINIMUM_TIME_BETWEEN_FX = 0.25f; // seconds between FX
	private float nextFXdelay = 0f; // so we don't spam FX every frame
	
	[Space]
	public float regenerateActionPointsPerSec = 0.025f;

	[Space]
	public float attackActionDeplete = 0.4f;
	public float dashActionDeplete = 0.25f;
	public float pushActionDeplete = 0.1f;
	public float throwActionDeplete = 0.1f;

	[Space]
	public float keyComboMaxTimeGap = 0.05f;
	public string walkVerticalInput = "Vertical";
	public string walkHorizontalInput = "Horizontal";
	public string attackInput = "Fire";

	[Space]
	public float criticalHitChance = 0.05f;
	public float hitComboMaxTime = 0.5f;
	public float criticalHitTimePause = 0.01f;

	[Space]
	public float timeSlowMoTime = 10f;
	public AudioClip slowMoSound;
	public AudioClip slowMoToNormalSound;

	[Space]
	public WeaponPossession weaponPossession;

	[Space]
	public AudioClip[] weaponSounds;
	public float[] weaponSoundVolumes;
	public AudioClip dashSound;

	[HideInInspector] public int hitComboCount = 0;

	[HideInInspector] public float actionPoints = 1f;

	[HideInInspector] public int playerNo = -1;

	private float speed = 0f;
	[HideInInspector] public bool isDashing = false;
	private float dashTimer = 0f;
	private float attackSpeedTimer = 0f;

	[HideInInspector] public Vector2 trackVelocity = Vector2.zero;
	private Vector2 lastPos;

	private Rigidbody2D rigidbody;
	private Animator animator;
	private SpriteRenderer sprRenderer;
	private HitCheck hitCheck;

	private AudioSource aud = null;
	
	private Vector2 walkInput = Vector2.zero;
	private int prevWalkState = 0;

	private float keyComboTimer = 0f;
	private string comboKeys = "";
	private GameObject pushPossible = null;
	
	private float hitComboTimer = 0f;
	[HideInInspector] public int didAttackHitEnemy = 0;
	private bool wasHitCritical = false;
	static private float criticalHitTimePauseTimer = 0f;
	private GameObject lastHitComboTextObject = null;

	static private float timeSlowMoTimer = 0f;
	private float timeSlowMoEffectValue = 0f;

	private float potionAttackButtonPressMinTime = 0.5f;
	private float potionTimer = 0f;

	private string inputExtensionString = "";
	
	private void Start()
	{
		if (name == "Player1") playerNo = 1;
		else if (name == "Player2") playerNo = 2;

		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sprRenderer = GetComponent<SpriteRenderer>();
		hitCheck = GetComponent<HitCheck>();

		aud = GetComponent<AudioSource>();
		if (aud == null)
			aud = FindObjectOfType<AudioSource>();

		if (playerNo == 1)
		{
			if (TogglesValues.p1controller == "")
			{
				if (GameManager.singleGame)
					inputExtensionString = "SinglePlayer";
				else
					inputExtensionString = playerNo.ToString();
			}
			else
				inputExtensionString = TogglesValues.p1controller;
		}
		else if (playerNo == 2)
		{
			if (TogglesValues.p2controller == "")
				inputExtensionString = playerNo.ToString();
			else
				inputExtensionString = TogglesValues.p2controller;
		}
	}
	
	private void Update()
	{
		//if (Time.timeScale > 0f)
		{
			if (!animator.GetBool("isAttacking") && !animator.GetBool("isPushing") && !animator.GetBool("isThrowing"))// && !isDashing)
			{
				walkInput = new Vector2(Input.GetAxisRaw(walkHorizontalInput + inputExtensionString), Input.GetAxisRaw(walkVerticalInput + inputExtensionString));

				/*
				if (GameManager.singleGame)
				{
					if(walkInput.x == 0f)
						walkInput.x = Input.GetAxisRaw(walkHorizontalInput + "2");
					if(walkInput.y == 0f)
						walkInput.y = Input.GetAxisRaw(walkVerticalInput + "2");
				}
				*/

				if (walkInput.x != 0 && walkInput.y != 0) walkInput /= 1.5f;

				if (walkInput != Vector2.zero)
				{
					if (!isDashing)
						speed = walkSpeed * (1 + (4 * timeSlowMoEffectValue));
					
					animator.SetBool("isWalking", true);

					if (walkInput.y < 0)
					{
						animator.SetInteger("direction", 2);

						if (prevWalkState == 0)
							comboKeys += "D";
					}
					else if (walkInput.y > 0)
					{
						animator.SetInteger("direction", 0);

						if (prevWalkState == 0)
							comboKeys += "U";
					}
					else if (walkInput.x > 0)
					{
						animator.SetInteger("direction", 1);

						if (prevWalkState == 0)
							comboKeys += "R";
					}
					else if (walkInput.x < 0)
					{
						animator.SetInteger("direction", 3);

						if (prevWalkState == 0)
							comboKeys += "L";
					}

					if (walkInput.x > 0) sprRenderer.flipX = true;
					else sprRenderer.flipX = false;
				}
				else
				{
					speed = 0f;
					animator.SetBool("isWalking", false);
				}
			}
			
			//Potion
			if (Input.GetButtonDown(attackInput + inputExtensionString))
			{
				potionTimer = potionAttackButtonPressMinTime;
			}

			//Attack
			if (actionPoints >= attackActionDeplete
				&& !animator.GetBool("isAttacking")
				&& !animator.GetBool("isPushing")
				&& !animator.GetBool("isThrowing")
				&& !isDashing
				&& (Input.GetButtonUp(attackInput + inputExtensionString) /*|| GameManager.singleGame && Input.GetButtonDown(attackInput + "2")*/))
			{
				if (!(potionTimer <= -1499f && potionTimer >= -1501f))
				{
					comboKeys += "A";

					if (comboKeys != "UA" && comboKeys != "RA" && comboKeys != "DA" && comboKeys != "LA")
					{
						animator.SetBool("isAttacking", true);

						speed = attackSpeed * (1 + (4 * timeSlowMoEffectValue));

						/*
						if (playerNo == 1)
							Subtitles.AddPlayer1Subtitle("TEST_Player 1 Attacking");
						else if (playerNo == 2)
							Subtitles.AddPlayer2Subtitle("TEST_Player 2 Attacking");
						*/

						if (animator.GetBool("isWalking"))
							attackSpeedTimer = attackSpeedTime;
						else
						{
							int dir = animator.GetInteger("direction");
							if (dir == 0)
							{
								walkInput.x = 0;
								walkInput.y = 1;
							}
							else if (dir == 1)
							{
								walkInput.x = 1;
								walkInput.y = 0;
							}
							else if (dir == 2)
							{
								walkInput.x = 0;
								walkInput.y = -1;
							}
							else if (dir == 3)
							{
								walkInput.x = -1;
								walkInput.y = 0;
							}
							attackSpeedTimer = attackSpeedTime / 2f;
						}

						actionPoints -= attackActionDeplete;

						if (aud != null && TogglesValues.sound)
							aud.PlayOneShot(weaponSounds[weaponPossession.weaponID + 1], weaponSoundVolumes[weaponPossession.weaponID + 1]);
					}
				}

				potionTimer = -1000f;
			}

			//Push
			if (pushPossible != null
			&& !animator.GetBool("isPushing")
			&& !animator.GetBool("isAttacking")
			&& !animator.GetBool("isThrowing")
			&& !isDashing
			&& (comboKeys == "UU" || comboKeys == "RR" || comboKeys == "DD" || comboKeys == "LL"))
			{
				animator.SetBool("isPushing", true);
				speed = 0f;

				if (playerNo == 1)
					Subtitles.AddPlayer1Subtitle("TEST_Player 1 Pushing");
				else if (playerNo == 2)
					Subtitles.AddPlayer2Subtitle("TEST_Player 2 Pushing");

				actionPoints -= pushActionDeplete;

				pushPossible = null;

				comboKeys = "";
			}
			
			//Dash
			if (actionPoints >= dashActionDeplete
				&& !isDashing
				&& !animator.GetBool("isAttacking")
				&& !animator.GetBool("isPushing")
				&& !animator.GetBool("isThrowing")
				&& (comboKeys == "UU" || comboKeys == "RR" || comboKeys == "DD" || comboKeys == "LL"
				 || comboKeys == "UR" || comboKeys == "UL" || comboKeys == "DR" || comboKeys == "DL"
				 || comboKeys == "RU" || comboKeys == "RD" || comboKeys == "LU" || comboKeys == "LD")
				&& hitCheck.knockback == Vector2.zero)
			{
				isDashing = true;
				dashTimer = dashTime / (1 + (4 * timeSlowMoEffectValue));
				speed = dashSpeed * (1 + (4 * timeSlowMoEffectValue));
				
				gameObject.layer = 10;

				actionPoints -= dashActionDeplete;

				if (aud != null && TogglesValues.sound)
					aud.PlayOneShot(dashSound);

				comboKeys = "";
			}
			
			//Throw
			if(!animator.GetBool("isThrowing")
			&& !animator.GetBool("isAttacking")
			&& !animator.GetBool("isPushing")
			&& !isDashing
			&& (((keyComboTimer <= keyComboMaxTimeGap / 3.8f) && (comboKeys == "UA" || comboKeys == "RA" || comboKeys == "DA" || comboKeys == "LA"))
			|| ((Input.GetButtonDown(attackInput + inputExtensionString)) && (Input.GetButtonDown(walkHorizontalInput + inputExtensionString) || Input.GetButtonDown(walkVerticalInput + inputExtensionString))))
			)
			{
				animator.SetBool("isThrowing", true);
				speed = attackSpeed * (1 + (4 * timeSlowMoEffectValue));
				
				attackSpeedTimer = attackSpeedTime / 2f;

				actionPoints -= throwActionDeplete;

				if (throwFXObject && nextFXdelay < 0f)
				{
					GameObject FX = Instantiate(throwFXObject, transform.position, Quaternion.Euler(0f,0f,0f));
					nextFXdelay = MINIMUM_TIME_BETWEEN_FX;
				}

				comboKeys = "";
			}

			if (actionPoints < 1f)
			{
				actionPoints += regenerateActionPointsPerSec * Time.deltaTime * (1 + (4 * timeSlowMoEffectValue));
			}

			if (isDashing && dashTimer <= 0f)
			{
				isDashing = false;
				speed = walkSpeed * (1 + (4 * timeSlowMoEffectValue));
				gameObject.layer = 8;
			}
			else if (dashTimer > 0f || (attackSpeedTimer > 0f && attackSpeedTimer < attackSpeedTime/3f))
			{
				GameObject ghostSpriteFX = Instantiate(dashFXObject, transform.position, Quaternion.Euler(0f,0f,0f));

				SpriteRenderer GSFX_sprRend = ghostSpriteFX.GetComponent<SpriteRenderer>();
				
				if (dashFXObject && nextFXdelay < 0f)
				{
					GameObject FX = Instantiate(dashFXObject, transform.position, Quaternion.Euler(0f,0f,0f));
					FX.GetComponent<SpriteRenderer>().sprite = sprRenderer.sprite;
					nextFXdelay = MINIMUM_TIME_BETWEEN_FX;
				}
				

				GSFX_sprRend.sprite = sprRenderer.sprite;

				if (name == "Player1")
				{
					GSFX_sprRend.color = Color.red;
					GSFX_sprRend.material.color = Color.red;
				}
				else
				{
					GSFX_sprRend.color = Color.green;
					GSFX_sprRend.material.color = Color.green;
				}
			}
			
			if ((animator.GetBool("isAttacking") || animator.GetBool("isThrowing")) && attackSpeedTimer <= 0f)
				speed = 0f;

			if (keyComboTimer >= keyComboMaxTimeGap)
				comboKeys = "";
				
			if (comboKeys != "")
				keyComboTimer += Time.deltaTime;
			else
				keyComboTimer = 0f;

			if (hitComboTimer <= 0f)
				hitComboCount = 0;
			else
				hitComboTimer -= Time.deltaTime;

			if (criticalHitTimePauseTimer <= 0f)
			{
				if (Time.timeScale >= 0.05f && Time.timeScale <= 0.1f)
				{
					Time.timeScale = 1f;
				}
				else if (timeSlowMoTimer > 0f)
				{
					timeSlowMoTimer -= Time.unscaledDeltaTime;
					Time.timeScale = Mathf.Lerp(Time.timeScale, 0.5f, 0.025f);

					ImageEffect.SetImageEffectMaterialIndex(2);

					timeSlowMoEffectValue = Mathf.Lerp(timeSlowMoEffectValue, 0.25f, 0.025f);
					ImageEffect.SetImageEffectValue(timeSlowMoEffectValue);
				}
				else if(timeSlowMoTimer <= 0f)
				{
					if (Time.timeScale >= 0.5f && Time.timeScale != 1f)
					{
						if (Time.timeScale <= 0.505f && aud != null && TogglesValues.sound)
							aud.PlayOneShot(slowMoToNormalSound);

						Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.05f);

						if (Time.timeScale >= 0.98f) Time.timeScale = 1f;
					}

					if (timeSlowMoEffectValue <= 0f)
					{
						ImageEffect.SetImageEffectMaterialIndex(0);
						ImageEffect.SetImageEffectValue(0.04f);

						timeSlowMoEffectValue = 0f;
					}
					else
					{
						timeSlowMoEffectValue -= Time.unscaledDeltaTime / 5f;
						ImageEffect.SetImageEffectValue(timeSlowMoEffectValue);
					}
				}
			}
			else
			{
				Time.timeScale = 0.06f;
				criticalHitTimePauseTimer -= Time.unscaledDeltaTime;
			}

			if (potionTimer <= 0 && potionTimer > -900f)
			{
				EquipmentManager eqMan = GetComponent<EquipmentManager>();
				if (eqMan != null
					&& eqMan.GetCurrentEquipment() != null
					&& eqMan.GetCurrentEquipment().Length > 1
					&& eqMan.GetCurrentEquipment()[1] != null)
				{
					eqMan.UsePotion(GetComponent<EquipmentManager>().GetCurrentEquipment()[1]);

					if (playerNo == 1)
						Subtitles.AddPlayer1Subtitle("Potion Used");
					else if (playerNo == 2)
						Subtitles.AddPlayer2Subtitle("Potion Used");
				}
				else
				{
					if (playerNo == 1)
						Subtitles.AddPlayer1Subtitle("Potion not equipped");
					else if (playerNo == 2)
						Subtitles.AddPlayer2Subtitle("Potion not equipped");
				}

				potionTimer = -1500f;
			}
			else if(potionTimer > 0f && potionTimer > -900f)
			{
				potionTimer -= Time.deltaTime;
			}

			animator.speed = 1f * (1 + (4 * timeSlowMoEffectValue));

			attackSpeedTimer -= Time.deltaTime;
			dashTimer -= Time.deltaTime;
			nextFXdelay -= Time.deltaTime; // don't spam FX every frame

			prevWalkState = walkInput == Vector2.zero ? 0 : 1;
		}
	}

	private void pushOnCollision(Collision2D collision)
	{
		if (animator.GetBool("isPushing") == true && collision.gameObject.GetComponent<Pushable>() != null)
		{
			if (pushFXObject && nextFXdelay < 0f)
			{
				GameObject FX = Instantiate(pushFXObject, transform.position, Quaternion.Euler(0f,0f,0f));
				nextFXdelay = MINIMUM_TIME_BETWEEN_FX;
			}
			
			Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
			if (rb != null)
			{
				Vector2 force = Vector2.zero;
				int dir = animator.GetInteger("direction");
				if (dir == 0)
				{
					force.x = 0;
					force.y = pushForce;
				}
				else if (dir == 1)
				{
					force.x = pushForce;
					force.y = 0;
				}
				else if (dir == 2)
				{
					force.x = 0;
					force.y = -pushForce;
				}
				else if (dir == 3)
				{
					force.x = -pushForce;
					force.y = 0;
				}
				rb.AddForce(force, ForceMode2D.Impulse);
			}

			collision.gameObject.AddComponent<PushedObject>();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.GetComponent<Pushable>() != null)
			pushPossible = collision.gameObject;
		pushOnCollision(collision);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent<Pushable>() != null)
			pushPossible = collision.gameObject;
		pushOnCollision(collision);
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent<Pushable>() != null)
			pushPossible = null;
	}

	void FixedUpdate()
	{
		if (hitCheck.hp > 0f)
		{
			rigidbody.MovePosition(new Vector2(rigidbody.transform.position.x, rigidbody.transform.position.y) + (walkInput * speed * Time.deltaTime)
			+ (-hitCheck.knockback * Time.deltaTime));
		}
		else
		{
			rigidbody.MovePosition(new Vector2(rigidbody.transform.position.x, rigidbody.transform.position.y)
			+ (-hitCheck.knockback * Time.deltaTime));

			stopAttacking();
			stopDashing();
			stopPushing();
			stopThrowing();
		}

		trackVelocity = (rigidbody.position - lastPos) * 50;
		lastPos = rigidbody.position;

		if (Mathf.Abs(hitCheck.knockback.x) > Mathf.Abs(hitCheck.knockbackSlowDown)
		|| Mathf.Abs(hitCheck.knockback.y) > Mathf.Abs(hitCheck.knockbackSlowDown))
			hitCheck.knockback -= hitCheck.knockbackSlowDownVector;
		else
			hitCheck.knockback = Vector2.zero;
	}

	public void stopAttacking()
	{
		animator.SetBool("isAttacking", false);

		attackSpeedTimer = 0f;
	}

	public void stopDashing()
	{
		dashTimer = 0f;

		gameObject.layer = 8;
	}

	public void stopPushing()
	{
		animator.SetBool("isPushing", false);
	}

	public void stopThrowing()
	{
		animator.SetBool("isThrowing", false);

		attackSpeedTimer = 0f;
	}

	public Vector3 GetAbsolutePosition(GameObject go)
	{
		Vector3 position = Vector3.zero;

		Transform p = go.transform.parent;
		while (p != null)
		{
			position += p.position;
			p = p.transform.parent;
		}

		position += go.transform.position;

		return position;
	}

	public Quaternion GetAbsoluteRotation(GameObject go)
	{
		Quaternion rotation = Quaternion.identity;

		Transform p = go.transform.parent;
		while (p != null)
		{
			rotation = Quaternion.Euler(0f, 0f, rotation.eulerAngles.z + p.rotation.eulerAngles.z);
			p = p.transform.parent;
		}

		rotation = Quaternion.Euler(0f, 0f, rotation.eulerAngles.z + go.transform.rotation.eulerAngles.z);

		return rotation;
	}

	public void throwWeapon()
	{
		if (weaponPossession.weaponID > -1 && weaponPossession.weaponID != 2) //2 = Time Whip
		{
			if (throwFXObject && nextFXdelay < 0f)
			{
				GameObject FX = Instantiate(throwFXObject, transform.position, Quaternion.Euler(0f,0f,0f));
				nextFXdelay = MINIMUM_TIME_BETWEEN_FX;
			}

			GameObject thrownObject = Instantiate(weaponPossession.gameObject, transform.position, Quaternion.Euler(0f,0f,0f));
			ThrownObject thrownObjectScript = thrownObject.AddComponent<ThrownObject>();

			thrownObjectScript.startPos = weaponPossession.transform.position;
			thrownObject.transform.localScale = new Vector2(2f, 2f);

			thrownObjectScript.breakableBreaksOnCollision = weaponPossession.weaponID == 0 ? false : true;

			Vector3 velocity = Vector3.zero;
			int dir = animator.GetInteger("direction");
			if (dir == 0)
			{
				velocity.x = 0;
				velocity.y = throwSpeed;
				thrownObject.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
			}
			else if (dir == 1)
			{
				velocity.x = throwSpeed;
				velocity.y = 0;
			}
			else if (dir == 2)
			{
				velocity.x = 0;
				velocity.y = -throwSpeed;
				thrownObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
			}
			else if (dir == 3)
			{
				velocity.x = -throwSpeed;
				velocity.y = 0;
			}
			thrownObjectScript.throwVelocity = velocity;
			thrownObjectScript.throwRotation = 0f;

			weaponPossession.weaponID = -1;
		}
	}

	public void processHitCombo()
	{
		if (didAttackHitEnemy > 0)
		{
			hitComboCount += didAttackHitEnemy;
			hitComboTimer = hitComboMaxTime;
			
			if (hitComboCount >= 3)
			{
				if (lastHitComboTextObject != null)
				{
					lastHitComboTextObject.transform.GetChild(0).GetComponent<Animator>().Play("HitComboPop", 0, 0f);
					lastHitComboTextObject.transform.position = transform.position + new Vector3(0f, 0.72f, 0f);
				}
				else
				{
					lastHitComboTextObject = Instantiate(hitComboTextObject, transform.position + new Vector3(0f, 0.72f, 0f), Quaternion.Euler(0f, 0f, Random.Range(-5f, 5f)));
				}

				if (wasHitCritical)
				{
					Time.timeScale = 0f;
					criticalHitTimePauseTimer = criticalHitTimePause;

					if (weaponPossession.weaponID == 2) //Time Whip
					{
						timeSlowMoTimer = timeSlowMoTime;

						if (aud != null && TogglesValues.sound)
							aud.PlayOneShot(slowMoSound);

						lastHitComboTextObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshPro>().text = "slow mo!";
						lastHitComboTextObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "slow mo!";

						if (playerNo == 1)
							Subtitles.AddPlayer1Subtitle("Slow Mo! Combo x" + hitComboCount.ToString() + "!");
						else
							Subtitles.AddPlayer2Subtitle("Slow Mo! Combo x" + hitComboCount.ToString() + "!");
					}
					else
					{
						lastHitComboTextObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshPro>().text = "critical!";
						lastHitComboTextObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "critical!";

						if (playerNo == 1)
							Subtitles.AddPlayer1Subtitle("Critical! Combo x" + hitComboCount.ToString() + "!");
						else
							Subtitles.AddPlayer2Subtitle("Critical! Combo x" + hitComboCount.ToString() + "!");
					}
				}
				else
				{
					lastHitComboTextObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshPro>().text = "combo x" + hitComboCount.ToString();
					lastHitComboTextObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "combo x" + hitComboCount.ToString();

					if (playerNo == 1)
						Subtitles.AddPlayer1Subtitle("Combo x" + hitComboCount.ToString() + "!");
					else
						Subtitles.AddPlayer2Subtitle("Combo x" + hitComboCount.ToString() + "!");
				}
			}
		}
		else
		{
			hitComboCount = 0;
			hitComboTimer = 0f;
		}
		
		didAttackHitEnemy = 0;
		wasHitCritical = false;
	}

	public bool processCriticalHit()
	{
		if(hitComboCount >= 3 && !wasHitCritical)
			wasHitCritical = Random.Range(0f, 1f) < criticalHitChance * hitComboCount;

		if (wasHitCritical)
		{
			if (playerNo == 1)
				Subtitles.AddPlayer1Subtitle("Critical Hit! Combo x" + hitComboCount + "!");
			else
				Subtitles.AddPlayer2Subtitle("Critical Hit! Combo x" + hitComboCount + "!");
		}

		return wasHitCritical;
	}
}
