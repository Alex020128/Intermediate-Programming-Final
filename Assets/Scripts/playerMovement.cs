using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.Events;

public class playerMovement : MonoBehaviour
{
	[SerializeField] private float jumpForce = 700f;// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;// How much to smooth out the movement
	[SerializeField] private bool airControl = true;// Whether or not a player can steer while jumping;

	public bool isGrounded;            // Whether or not the player is grounded.
	private Rigidbody2D rb;
	public Animator animator;
	public ParticleSystem hurtParticle;
	public bool facingRight = true;  // For determining which way the player is currently facing.
	private Vector3 currentVelocity = Vector3.zero;

	public GameObject pet;

	public float horizontalMove = 0f;
	public float moveSpeed = 10f;

	//Player bools
	public bool jump = false;
	public bool shootBullet;
	public bool shootCannon;

	public string equipment;
	
	public bool placeTrap; 
	public bool castCircle;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		hurtParticle = GetComponent<ParticleSystem>();

		pet = GameObject.Find("Pet");

		equipment = "None";
		GameObject.Find("qKeyEquipment").GetComponent<TMP_Text>().text = "";
	}

	private void FixedUpdate()
	{
		Move(horizontalMove,jump);
		jump = false;
	}

	private void CheckGrounded()
	{
		RaycastHit2D grounedRay = Physics2D.Raycast(transform.position, Vector2.down, 1f, 1 << 6);

		//Debug.Log(grounedRay.collider.gameObject);

		if (grounedRay.collider != null && grounedRay.collider.gameObject.GetComponent<jumpObject>() != null)
		{
			Debug.Log(grounedRay.collider.gameObject);
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}
	}
	public void OnDrawGizmos()
	{
		Color myCol;

		if (isGrounded == true)
		{
			myCol = Color.green;
		}
		else
		{
			myCol = Color.red;
		}

		Debug.DrawRay(transform.position, Vector2.down, myCol);
	}

	public void Move(float move,bool jump)
	{
		//only control the player if grounded or airControl is turned on
		if (isGrounded || airControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * moveSpeed, rb.velocity.y);
			animator.SetFloat("Horizontal", Mathf.Abs(move * moveSpeed));
			// And then smoothing it out and applying it to the character
			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, movementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !facingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && facingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (isGrounded && jump)
		{
			animator.SetTrigger("Jump");
			// Add a vertical force to the player.
			isGrounded = false;
			rb.AddForce(new Vector2(0f, jumpForce));
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void playerControl()
    {
		horizontalMove = Input.GetAxisRaw("Horizontal");

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}
		CheckGrounded();
	}

    private void Update()
    {
		if (gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
		{
			playerControl();

			if (pet.GetComponent<petMovement>().carrying)
			{
				jumpForce = 700f;
				moveSpeed = 5f;
			}
			else
			{
				jumpForce = 800f;
				moveSpeed = 7f;
			}

			//Use LMB and RMB to shoot arcane missiles and cast a magic circle (bullet and missile)
			if (Input.GetMouseButton(0) && shootBullet == false)
			{
				animator.SetTrigger("Attack");
				GameObject.Find("lmbBullet").GetComponent<bulletSpawner>().shootBullet();
				shootBullet = true;
			}

			if (Input.GetMouseButton(1) && shootCannon == false)
			{
				animator.SetTrigger("Attack");
				GameObject.Find("rmbBullet").GetComponent<iceCannonSpawner>().shootBullet();
				shootCannon = true;
			}

			if (equipment == "Trap")
			{
				GameObject.Find("attacksUI").GetComponent<Animator>().SetBool("Trap",true);
				GameObject.Find("attacksUI").GetComponent<Animator>().SetBool("Frost", false);
				GameObject.Find("qKeyEquipment").GetComponent<TMP_Text>().text = "Vine Traps";

				if (Input.GetKeyDown(KeyCode.Q) && placeTrap == false && isGrounded == true)
				{
					GameObject.Find("Trap").GetComponent<trapSpawner>().placeTrap();
					placeTrap = true;
				}
			}

			if (equipment == "frostCircle")
			{
				GameObject.Find("attacksUI").GetComponent<Animator>().SetBool("Trap", false);
				GameObject.Find("attacksUI").GetComponent<Animator>().SetBool("Frost", true);
				GameObject.Find("qKeyEquipment").GetComponent<TMP_Text>().text = "Frost Field";

				if (Input.GetKeyDown(KeyCode.Q) && castCircle == false)
				{
					GameObject.Find("displayCircle").GetComponent<frostCircle>().castCircle();
					castCircle = true;
				}
			}
		}
	}
}
