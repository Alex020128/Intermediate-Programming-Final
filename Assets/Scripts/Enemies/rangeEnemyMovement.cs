using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class rangeEnemyMovement : MonoBehaviour
{
    //Enemy stats
    public float patrolSpeed;
    public float followSpeed;
    public float lineOfSite;
    public float shootingRange;
    public float fireRate = 3.0f;
    private float nextFireTime;

    public float latestDirectionChangeTime;
    public readonly float directionChangeTime = 3f;

    public GameObject bulletParent;
    public Rigidbody2D rb;
    public Vector2 direction;
    private Transform player;
    private Transform pet;
    private ParticleSystem particle;
    private Animator animator;

    Coroutine waitCoroutine;

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
    public AudioClip hurtSound;

    //Enemy coroutine
    Coroutine deathCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        pet = GameObject.Find("Pet").transform;

        //Spawn a pool of enemy bullets
        for (int i = 0; i < bullets.Length; i++)
        {
            GameObject newBullet = Instantiate(prefabToSpawn, transform.position, Quaternion.identity, bulletParent.transform);
            bullets[i] = newBullet;
            bullets[i].SetActive(false);
        }

        //The health of the enemy increases over time

        particle = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void checkGround()
    {
        RaycastHit2D grounedRay = Physics2D.Raycast(transform.position, Vector2.down, 5f, 1 << 2);

        //Debug.Log(grounedRay.collider.gameObject);

        if (grounedRay.collider != null && grounedRay.collider.gameObject.GetComponent<enemyCanJump>() != null)
        {
            Debug.Log(grounedRay.collider.gameObject);

            changeDirection();
        }

        RaycastHit2D ceilingRay = Physics2D.Raycast(transform.position, Vector2.up, 1.5f, 1 << 2);

        //Debug.Log(grounedRay.collider.gameObject);

        if (ceilingRay.collider != null && ceilingRay.collider.gameObject.GetComponent<enemyCanJump>() != null)
        {
            Debug.Log(ceilingRay.collider.gameObject);

            changeDirection();
        }
    }

    private void checkPlayer()
    {
        RaycastHit2D[] sightRay = Physics2D.RaycastAll(transform.position, player.position - transform.position, lineOfSite, (1 << 6 | 1 << 0));

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
        RaycastHit2D[] sightRay = Physics2D.RaycastAll(transform.position, pet.position - transform.position, lineOfSite, (1 << 6 | 1 << 0));

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

        Debug.DrawRay(transform.position, player.position - transform.position, myCol1);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }

    public void hurtSFX()
    {
        //Play the enemy hurt SFX
        audioSource.Stop();
        audioSource.clip = hurtSound;
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
                break;
            }
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
            //GameObject.Find("Player").GetComponent<playerMovement>().Particle.Emit(5);
            //GameObject.Find("Player").GetComponent<playerMovement>().hurtSFX();
            Camera.main.transform.DOShakePosition(0.5f, new Vector3(0.5f, 0.5f, 0));
            gameManager.Instance.playerInvinsibleTime = 0;
            scoreManager.Instance.meleeHit += 1;
            gameManager.Instance.playerInvinsible = true;
        }
    }

    private IEnumerator deathExplode(float wait)
    {
        //Make sure that the death particle will be shown
        yield return new WaitForSeconds(wait);
        scoreManager.Instance.rangeEnemyKills += 1;
        Destroy(this.gameObject);
    }

    public void movementControl()
    {
        float distanceToPlayer = 0;
        distanceToPlayer = player.position.x - transform.position.x;
        float nextSpeedX = Mathf.Sign(distanceToPlayer) * followSpeed;
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        
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
    }
    public void attackPet()
    {
        float distanceToPet = 0;
        distanceToPet = pet.position.x - transform.position.x;
        float nextSpeedX = Mathf.Sign(distanceToPet) * followSpeed;
        float distanceFromPet = Vector2.Distance(pet.position, transform.position);

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
        direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    }

    private IEnumerator pantrolWait(float wait)
    {
        //Make sure that the death particle will be shown
        yield return new WaitForSeconds(wait);
        canPatrol = true;
        latestDirectionChangeTime = Time.time;
        changeDirection();
    }

    public void patrolControl()
    {
        checkGround();

        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            waitCoroutine = StartCoroutine(pantrolWait(1.5f));
            canPatrol = false;
        }

        if (rb.velocity.magnitude <= 1 && canPatrol && GetComponent<slowDown>().frozen == false && GetComponent<trapped>().gotTrapped == false)
        {
            rb.velocity += direction * patrolSpeed;
        }
    }

        // Update is called once per frame
        void Update()
    {
        checkPlayer();
        checkPet();
        //string clipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        //float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (findPlayer && !findPet) //distanceFromPlayer < lineOfSite
        {
            movementControl();
        }
        else if (findPet) //distanceFromPlayer < lineOfSite
        {
            attackPet();
        }
        else
        {
            patrolControl();
        }
        
    }
}
