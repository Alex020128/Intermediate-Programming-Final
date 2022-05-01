using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class rangeEnemyMovement : MonoBehaviour
{
    //rangeEnemy stats
    public float patrolSpeed;
    public float followSpeed;
    public float lineOfSite;
    public float shootingRange;
    public float fireRate = 3.0f;
    private float nextFireTime;
    public float latestDirectionChangeTime;
    public readonly float directionChangeTime = 3f;

    //Components
    public GameObject bulletParent;
    public Rigidbody2D rb;
    public Vector2 direction;
    private Transform player;
    private Transform pet;
    public ParticleSystem knockBackParticle;
    public ParticleSystem slowDownParticle;
    public ParticleSystem deathParticle;
    private Animator animator;

    //Enemy bullets
    [SerializeField]
    private GameObject prefabToSpawn = null;
    [SerializeField]
    private GameObject[] bullets = new GameObject[5];

    //Enemy bools
    public bool findPlayer;
    public bool findPet;
    public bool canPatrol;

    //Enemy SFXs
    public AudioSource audioSource;
    public AudioClip knockBackSound;
    public AudioClip frostSound;
    public AudioClip deathSound;
    public AudioClip attackSound;

    //Enemy coroutine
    Coroutine waitCoroutine;

    private void Awake()
    {
        //Assign variables
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Assign variables
        player = GameObject.Find("Player").transform;
        pet = GameObject.Find("Pet").transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        //Spawn a pool of enemy bullets
        for (int i = 0; i < bullets.Length; i++)
        {
            GameObject newBullet = Instantiate(prefabToSpawn, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity, bulletParent.transform);
            bullets[i] = newBullet;
            bullets[i].SetActive(false);
        }
    }

    private void checkGround()
    {
        //Shoot a ray downwards to see if the enemy is too close to ground, if so, change direction
        RaycastHit2D grounedRay = Physics2D.Raycast(transform.position, Vector2.down, 3.5f, 1 << 2);

        if (grounedRay.collider != null && grounedRay.collider.gameObject.GetComponent<enemyCanJump>() != null)
        {
            Debug.Log(grounedRay.collider.gameObject);

            changeDirection();
        }

        //Shoot a ray downwards to see if the enemy is too close to ceiling, if so, change direction
        RaycastHit2D ceilingRay = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up, 1.5f, 1 << 2);

        if (ceilingRay.collider != null && ceilingRay.collider.gameObject.GetComponent<enemyCanJump>() != null)
        {
            Debug.Log(ceilingRay.collider.gameObject);

            changeDirection();
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

    //Show gizmos of the shoot bullet range
    private void OnDrawGizmosSelected()
    {
        Color myCol1;

        if (findPlayer == true)
        {
            myCol1 = Color.green;
        }
        else
        {
            myCol1 = Color.red;
        }

        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f),
                      new Vector2(player.position.x, player.position.y + 0.5f) - new Vector2(transform.position.x, transform.position.y + 0.5f),
                      myCol1);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + 0.5f), shootingRange);
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

    public void shootBullet()
    {
        //let one of the waiting enemy bullets to be active
        for (int i = 0; i < bullets.Length; i++)
        {
            animator.SetTrigger("Attack");
            if (!bullets[i].activeInHierarchy)
            {
                bullets[i].SetActive(true);
                bullets[i].GetComponent<enemyBullet>().lifeTimer = 3.0f;
                bullets[i].transform.position = transform.position;
                bullets[i].transform.parent = bulletParent.transform;
                attackSFX();
                break;
            }
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

    public void movementControl()
    {
        float distanceToPlayer = 0;
        distanceToPlayer = player.position.x - transform.position.x;
        float nextSpeedX = Mathf.Sign(distanceToPlayer) * followSpeed;
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        //Move towards the player; if in shoot range, shoot bullets
        if (distanceFromPlayer > shootingRange)
        {
            if (rb.velocity.magnitude <= 2 && GetComponent<slowDown>().frozen == false && GetComponent<trapped>().gotTrapped == false)
            {
                rb.velocity += new Vector2(nextSpeedX, rb.velocity.y);
            }
        }
        else if (distanceFromPlayer <= shootingRange && nextFireTime < Time.time
                                                     && GetComponent<slowDown>().frozen == false
                                                     && GetComponent<trapped>().gotTrapped == false)
        {
            shootBullet();
            nextFireTime = Time.time + fireRate;
        }

        //Continue facing the player
        facePlayer();
    }
    public void attackPet()
    {
        float distanceToPet = 0;
        distanceToPet = pet.position.x - transform.position.x;
        float nextSpeedX = Mathf.Sign(distanceToPet) * followSpeed;
        float distanceFromPet = Vector2.Distance(pet.position, transform.position);

        //Move towards the pet; if in shoot range, shoot bullets
        if (distanceFromPet > shootingRange)
        {
            if (rb.velocity.magnitude <= 2 && GetComponent<slowDown>().frozen == false && GetComponent<trapped>().gotTrapped == false)
            {
                rb.velocity += new Vector2(nextSpeedX, rb.velocity.y);
            }
        }
        else if (distanceFromPet <= shootingRange && nextFireTime < Time.time
                                                  && GetComponent<slowDown>().frozen == false
                                                  && GetComponent<trapped>().gotTrapped == false)
        {
            shootBullet();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void changeDirection()
    {
        //Randomly moves to a direction
        direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    }
    public void facePlayer()
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

    private IEnumerator pantrolWait(float wait)
    {
        //Stays in current position for 1.5s, then change direction
        yield return new WaitForSeconds(wait);
        canPatrol = true;
        latestDirectionChangeTime = Time.time;
        changeDirection();
    }

    public void patrolControl()
    {
        //Continue checking if its too close to the ground or ceiling
        checkGround();

        //Check direction every 1.5s
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            waitCoroutine = StartCoroutine(pantrolWait(1.5f));
            canPatrol = false;
        }

        //Move towards its direction
        if (rb.velocity.magnitude <= 1 && canPatrol && GetComponent<slowDown>().frozen == false && GetComponent<trapped>().gotTrapped == false)
        {
            rb.velocity += direction * patrolSpeed;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Continue checking if can see player or pet
        checkPlayer();
        checkPet();

        //Enemy priority attack pet over player; if neither pet nor player can be seen, patron
        if (findPlayer && !findPet)
        {
            movementControl();
        }
        else if (findPet)
        {
            attackPet();
        }
        else
        {
            patrolControl();
        }
    }
}
