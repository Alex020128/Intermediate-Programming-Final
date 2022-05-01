using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class meleeEnemyMovement : MonoBehaviour
{
    //meleeEnemy Stats
    [SerializeField] private float jumpForce = 10f;
    public float patrolSpeed;
    public float followSpeed;
    public float lineOfSite;
    
    //Components
    public Animator animator;
    private Transform player;
    private Transform pet;
    public Transform edgeDetector;
    public Rigidbody2D rb;
    public ParticleSystem knockBackParticle;
    public ParticleSystem slowDownParticle;
    public ParticleSystem deathParticle;

    //Bools
    public bool isGrounded;
    public bool canJump;
    public bool canPatrol;
    public bool movingRight;
    public bool findPlayer;
    public bool findPet;

    //Enemy SFXs
    public AudioSource audioSource;
    public AudioClip knockBackSound;
    public AudioClip frostSound;
    public AudioClip deathSound;
    public AudioClip attackSound;

    // Start is called before the first frame update
    private void Awake()
    {
        //Assign variables
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        //Assign variables
        player = GameObject.Find("Player").transform;
        pet = GameObject.Find("Pet").transform;
        edgeDetector = transform.Find("edgeDetector").transform;
    }

   //Show line of site
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + 0.5f), lineOfSite);
    }

    //Show sight, edge detector and ground check
    public void OnDrawGizmos()
    {
        Color myCol1;
        Color myCol2;
        Color myCol3;

        if (canJump == true)
        {
            myCol1 = Color.green;
        }
        else
        {
            myCol1 = Color.red;
        }

        if (findPlayer == true)
        {
            myCol2 = Color.green;
        }
        else
        {
            myCol2 = Color.red;
        }


        if (canPatrol == true)
        {
            myCol3 = Color.green;
        }
        else
        {
            myCol3 = Color.red;
        }

        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.down, myCol1);

        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f),
                      new Vector2(player.position.x, player.position.y + 0.5f) - new Vector2(transform.position.x, transform.position.y + 0.5f), myCol2);

        Debug.DrawRay(edgeDetector.position, Vector2.down, myCol3);
    }

    //SFXs
    public void knockBackSFX()
    {
        //Play the enemy knockback SFX
        audioSource.Stop();
        audioSource.clip = knockBackSound;
        audioSource.Play();
    }
    public void frostSFX()
    {
        //Play the enemy frost SFX
        audioSource.Stop();
        audioSource.clip = frostSound;
        audioSource.Play();
    }
    public void deathSFX()
    {
        //Play the enemy death SFX
        audioSource.Stop();
        audioSource.clip = deathSound;
        audioSource.Play();
    }
    public void attackSFX()
    {
        //Play the enemy attack SFX
        audioSource.Stop();
        audioSource.clip = attackSound;
        audioSource.Play();
    }

    private void checkJump()
    {
        //Shoot a ray downwards to see if the enemy can jump; shoot a ray downwards to see if the enemy is on ground
        RaycastHit2D grounedRay = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.down, 1f, 1 << 2);
        RaycastHit2D isGrounedRay = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.down, 1f, 1 << 6);

        if (grounedRay.collider != null && grounedRay.collider.gameObject.GetComponent<enemyCanJump>() != null)
        {
            if (player.position.y > transform.position.y)
            {
                canJump = true;
            }
        }
        else
        {
            canJump = false;
        }

        if (isGrounedRay.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void checkEdge()
    {
        //Shoot a ray downwards (from the edge detector) to see if the enemy need to trun around
        RaycastHit2D edgeRay = Physics2D.Raycast(edgeDetector.position, Vector2.down, 1f, 1 << 6);

        if (edgeRay.collider != null)
        {
            Debug.Log(edgeRay.collider.gameObject);
            canPatrol = true;
        }
        else
        {
            canPatrol = false;
        }
    }
    
    private void checkPlayer()
    {
        //Shoot a ray to see if the enemy can see the player (while not blocked by walls)
        RaycastHit2D[] sightRay = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y + 0.5f),
                                                       new Vector2(player.position.x, player.position.y + 0.5f) - new Vector2(transform.position.x, transform.position.y + 0.5f),
                                                       lineOfSite, (1 << 6 | 1 << 0));

        if (Vector3.Distance(player.position, transform.position) <= lineOfSite && sightRay[0].collider.gameObject.GetComponent<playerMovement>() != null)
        {
            findPlayer = true;
        }
        else
        {
            findPlayer = false;
        }
    }
    private void checkPet()
    {
        //Shoot a ray to see if the enemy can see the pet (while not blocked by walls)
        RaycastHit2D[] sightRay = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y + 0.5f),
                                                       new Vector2(pet.position.x, pet.position.y + 0.5f) - new Vector2(transform.position.x, transform.position.y + 0.5f),
                                                       lineOfSite, (1 << 6 | 1 << 0));

        if (Vector3.Distance(pet.position, transform.position) <= lineOfSite && sightRay[0].collider.gameObject.GetComponent<petMovement>() != null)
        {
            findPet = true;
        }
        else
        {
            findPet = false;
        }
    }

    public void patrolControl()
    {
        float distanceToPlayer = 0;
        distanceToPlayer = edgeDetector.position.x - transform.position.x;
        float nextSpeedX = Mathf.Sign(distanceToPlayer) * patrolSpeed;

        //Move towards its direction
        if (rb.velocity.magnitude <= 1 && canPatrol && GetComponent<slowDown>().frozen == false && GetComponent<trapped>().gotTrapped == false)
        {
            rb.velocity += new Vector2(nextSpeedX, rb.velocity.y);
          
        }

        //Turn around when at edges
        if (!canPatrol)
        {
            if (movingRight)
            {
                transform.rotation = Quaternion.Euler(0, -180, 0);

                movingRight = false;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                movingRight = true;
            }
        } 

        //Continue checking if at edges
        checkEdge();
    }

    public void movementControl()
    {
        float distanceToPlayer = 0;
        distanceToPlayer = player.position.x - transform.position.x;
        float nextSpeedX = Mathf.Sign(distanceToPlayer) * followSpeed;
        
        //Move towards the player
        if (rb.velocity.magnitude <= 3 && GetComponent<slowDown>().frozen == false && GetComponent<trapped>().gotTrapped == false)
        {
            rb.velocity += new Vector2(nextSpeedX, rb.velocity.y);
        }
        
        //Continue checking if can jump and facing the player
        checkJump();
        ChangeDirection();
    }
    public void attackPet()
    {
        float distanceToPet = 0;
        distanceToPet = pet.position.x - transform.position.x;
        float nextSpeedX = Mathf.Sign(distanceToPet) * followSpeed;

        //Move towards the pet
        if (rb.velocity.magnitude <= 3 && GetComponent<slowDown>().frozen == false && GetComponent<trapped>().gotTrapped == false)
        {
            rb.velocity += new Vector2(nextSpeedX, rb.velocity.y);
        }

        //Continue checking if can jump
        checkJump();
    }
    protected void ChangeDirection()
    {
        //Continue facing the player
        float distanceToPlayer = player.position.x - transform.position.x;
        if (distanceToPlayer > 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (distanceToPlayer < -1)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        //Decrease player health, emits particle, set player invincible time, emit particles and trigger sreenshake when hits the player
        if (collision.collider.gameObject.tag == "Player" && GetComponent<trapped>().gotTrapped == false
                                                          && GetComponent<slowDown>().frozen == false
                                                          && gameManager.Instance.playerInvinsible == false
                                                          && gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            animator.SetTrigger("Attack");
            gameManager.Instance.playerHealth -= 2;
            GameObject.Find("Player").GetComponent<Animator>().SetTrigger("Hurt");
            GameObject.Find("Player").GetComponent<playerMovement>().hurtParticle.Emit(5);
            attackSFX();
            GameObject.Find("Player").GetComponent<playerMovement>().hurtSFX();
            Camera.main.transform.DOShakePosition(0.5f, new Vector3(0.5f, 0.5f, 0));
            gameManager.Instance.playerInvinsibleTime = 0;
            scoreManager.Instance.meleeHit += 1;
            gameManager.Instance.playerInvinsible = true;
        }
    }

    private void FixedUpdate()
    {
        //Make sure enemy only jump once
        canJump = false;

        //Continue checking if can see player or pet
        checkPlayer();
        checkPet();

        //Enemy priority attack pet over player; if neither pet nor player can be seen, patron
        if (findPlayer && !findPet)
        {
            movementControl();
        }
        else if(findPet)
        {
            attackPet();
        }else
        {
            patrolControl();
        }

        //Jump upwards
        if (canJump)
        {
            animator.SetTrigger("Jump");
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
        }
    }
}
