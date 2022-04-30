using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class meleeEnemyMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    public float patrolSpeed;
    public float followSpeed;
    public float lineOfSite;
    
    public Animator animator;
    private Transform player;
    private Transform pet;
    public Transform edgeDetector;
    public Rigidbody2D rb;
    public ParticleSystem knockBackParticle;
    public ParticleSystem slowDownParticle;
    public ParticleSystem deathParticle;

    public bool canJump;
    public bool canPatrol;
    public bool movingRight;
    public bool findPlayer;
    public bool findPet;

    //public AudioSource audioSource;

    //public AudioClip hurtSound;

    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = GameObject.Find("Player").transform;
        pet = GameObject.Find("Pet").transform;
        edgeDetector = transform.Find("edgeDetector").transform;
        //audioSource = GetComponent<AudioSource>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + 0.5f), lineOfSite);
    }

    public void hurtSFX()
    {
        //audioSource.Stop();
        //audioSource.clip = hurtSound;
        //audioSource.Play();
    }


    private void checkJump()
    {
        RaycastHit2D grounedRay = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.down, 1f, 1 << 2);

        //Debug.Log(grounedRay.collider.gameObject);

        if (grounedRay.collider != null && grounedRay.collider.gameObject.GetComponent<enemyCanJump>() != null)
        {
            Debug.Log(grounedRay.collider.gameObject);

            if (player.position.y > transform.position.y)
            {
                canJump = true;
            }
        }
        else
        {
            canJump = false;
        }
    }

    private void checkEdge()
    {
        RaycastHit2D edgeRay = Physics2D.Raycast(edgeDetector.position, Vector2.down, 1f, 1 << 6);

        //Debug.Log(grounedRay.collider.gameObject);

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


    public void patrolControl()
    {
        float distanceToPlayer = 0;
        distanceToPlayer = edgeDetector.position.x - transform.position.x;
        float nextSpeedX = Mathf.Sign(distanceToPlayer) * patrolSpeed;

        if (rb.velocity.magnitude <= 1 && canPatrol && GetComponent<slowDown>().frozen == false && GetComponent<trapped>().gotTrapped == false)
        {
            rb.velocity += new Vector2(nextSpeedX, rb.velocity.y);
          
        }

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

        checkEdge();
    }

    public void movementControl()
    {
        float distanceToPlayer = 0;
        distanceToPlayer = player.position.x - transform.position.x;
        float nextSpeedX = Mathf.Sign(distanceToPlayer) * followSpeed;
        if (rb.velocity.magnitude <= 3 && GetComponent<slowDown>().frozen == false && GetComponent<trapped>().gotTrapped == false)
        {
            rb.velocity += new Vector2(nextSpeedX, rb.velocity.y);
        }
       
        checkJump();
        ChangeDirection();
    }
    public void attackPet()
    {
        float distanceToPet = 0;
        distanceToPet = pet.position.x - transform.position.x;
        float nextSpeedX = Mathf.Sign(distanceToPet) * followSpeed;
        if (rb.velocity.magnitude <= 3 && GetComponent<slowDown>().frozen == false && GetComponent<trapped>().gotTrapped == false)
        {
            rb.velocity += new Vector2(nextSpeedX, rb.velocity.y);
        }

        checkJump();
    }
    protected void ChangeDirection()
    {
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
        //Decrease player health, emits particle, set player invincible time, trigger sreenshake when hits the player
        if (collision.collider.gameObject.tag == "Player" && GetComponent<trapped>().gotTrapped == false
                                                          && GetComponent<slowDown>().frozen == false
                                                          && gameManager.Instance.playerInvinsible == false
                                                          && gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            animator.SetTrigger("Attack");
            gameManager.Instance.playerHealth -= 2;
            GameObject.Find("Player").GetComponent<Animator>().SetTrigger("Hurt");
            GameObject.Find("Player").GetComponent<playerMovement>().hurtParticle.Emit(5);
            //GameObject.Find("Player").GetComponent<playerMovement>().hurtSFX();
            Camera.main.transform.DOShakePosition(0.5f, new Vector3(0.5f, 0.5f, 0));
            gameManager.Instance.playerInvinsibleTime = 0;
            scoreManager.Instance.meleeHit += 1;
            gameManager.Instance.playerInvinsible = true;
        }
    }

    private void FixedUpdate()
    {
        canJump = false;

        checkPlayer();
        checkPet();

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

        if (canJump)
        {
            animator.SetTrigger("Jump");
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
