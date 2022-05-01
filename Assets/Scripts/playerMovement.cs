using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class playerMovement : MonoBehaviour
{
	//Player stats
	[SerializeField] private float jumpForce = 700f;// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;// How much to smooth out the movement
	[SerializeField] private bool airControl = true;// Whether or not a player can steer while jumping;
	private Vector3 currentVelocity = Vector3.zero;
	public string equipment;
	public float horizontalMove = 0f;
	public float moveSpeed = 10f;

	//Components
	private Rigidbody2D rb;
	public Animator animator;
	public ParticleSystem hurtParticle;
	public GameObject pet;

	//Player bools
	public bool isGrounded;// Whether or not the player is grounded.
	public bool facingRight = true;  // For determining which way the player is currently facing.
	public bool jump = false;
	public bool shootBullet;
	public bool shootCannon;
	public bool placeTrap; 
	public bool castCircle;

	//Player SFXs
	public AudioSource audioSource;
	public AudioClip hurtSound;
	public AudioClip seedSound;
	public AudioClip deathSound;
	public AudioClip attackSound;
	public AudioClip walkSound;
	public AudioClip jumpSound;
	public AudioClip trapSound;

	private void Awake()
	{
		//Assign variables
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		hurtParticle = GetComponent<ParticleSystem>();
		audioSource = GetComponent<AudioSource>();

		pet = GameObject.Find("Pet");
		GameObject.Find("qKeyEquipment").GetComponent<TMP_Text>().text = "";

		equipment = "None";
	}

	private void FixedUpdate()
	{
		//Move the player, and make sure player only jump once
		Move(horizontalMove,jump);
		jump = false;
	}

	//SFXs
	public void walkSFX()
	{
		//Play the player walk SFX
		audioSource.Stop();
		audioSource.clip = walkSound;
		audioSource.Play();
	}
	public void jumpSFX()
	{
		//Play the player jump SFX
		audioSource.Stop();
		audioSource.clip = jumpSound;
		audioSource.Play();
	}
	public void hurtSFX()
	{
		//Play the player hurt SFX
		audioSource.Stop();
		audioSource.clip = hurtSound;
		audioSource.Play();
	}
	public void seedSFX()
	{
		//Play the SFX when collect seed
		audioSource.Stop();
		audioSource.clip = seedSound;
		audioSource.Play();
	}
	public void deathSFX()
	{
		//Play the player death SFX
		audioSource.Stop();
		audioSource.clip = deathSound;
		audioSource.Play();
	}
	public void attackSFX()
	{
		//Play the player attack SFX
		audioSource.Stop();
		audioSource.clip = attackSound;
		audioSource.Play();
	}
	public void trapSFX()
	{
		//Play the place trap SFX
		audioSource.Stop();
		audioSource.clip = trapSound;
		audioSource.Play();
	}

	private void CheckGrounded()
	{
		//Shoot a ray downwards to see if the player is on ground
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

	//Show ground check
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

			// If the input is moving the player right and the player is facing left
			if (move > 0 && !facingRight)
			{
				//flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right
			else if (move < 0 && facingRight)
			{
				//flip the player.
				Flip();
			}
		}
		// If the player should jump
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
		//Keyboard controls
		horizontalMove = Input.GetAxisRaw("Horizontal");

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}
		
		//Continuing checking if the player is on ground
		CheckGrounded();
	}

    private void Update()
    {
		if (gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
		{
			//Controlling the player when the game is on
			playerControl();

			//Decrease the player velocity and jump height if carrying the pet
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

			//Use LMB and RMB to shoot knockback bullet and slow down bullet
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

			//Use Q to use the equipped skill (spawn vine trap or cast frost field)
			if (equipment == "Trap")
			{
				GameObject.Find("attacksUI").GetComponent<Animator>().SetBool("Trap",true);
				GameObject.Find("attacksUI").GetComponent<Animator>().SetBool("Frost", false);
				GameObject.Find("qKeyEquipment").GetComponent<TMP_Text>().text = "Vine Traps";

				if (Input.GetKeyDown(KeyCode.Q) && placeTrap == false && isGrounded == true)
				{
					trapSFX();
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
