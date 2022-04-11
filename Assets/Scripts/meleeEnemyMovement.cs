using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeEnemyMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float playerBulletForce = 10f;
    public float followSpeed;
    public float lineOfSite;
    //public Animator animator;

    private Transform player;
    public Rigidbody2D rb;

    Coroutine slowDownCoroutine;

    public bool canJump;
    public bool findPlayer;
    public bool slowDown;

    //public AudioSource audioSource;

    //public AudioClip hurtSound;

    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.Find("Player").transform;
        //audioSource = GetComponent<AudioSource>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
    }

    public void hurtSFX()
    {
        //audioSource.Stop();
        //audioSource.clip = hurtSound;
        //audioSource.Play();
    }

    private void checkJump()
    {
        RaycastHit2D grounedRay = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, 1 << 2);


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
    private void checkPlayer()
    {
        RaycastHit2D[] sightRay = Physics2D.RaycastAll(transform.position, player.position - transform.position, 5f, (1 << 6 | 1 << 0));

        if (Vector3.Distance(player.position, transform.position) <= 5.0f && sightRay[0].collider.gameObject.GetComponent<playerMovement>() != null)
        {
            findPlayer = true;
        }
        else
        {
            findPlayer = false;
        }
    }
    public void OnDrawGizmos()
    {
        Color myCol1;
        Color myCol2;

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

        Debug.DrawRay(transform.position, Vector2.down, myCol1);
        Debug.DrawRay(transform.position, player.position - transform.position, myCol2);
    }

    public void movementControl()
    {
        float distanceToPlayer = 0;
        distanceToPlayer = player.position.x - transform.position.x;
        
        if (!slowDown)
        {
            rb.velocity = new Vector2(Mathf.Sign(distanceToPlayer) * followSpeed, rb.velocity.y);
        }

        if (canJump)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }
        checkJump();

        ChangeDirection();
    }
    protected void ChangeDirection()
    {
        float distanceToPlayer = player.position.x - transform.position.x;
        if (distanceToPlayer > 1)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }

        if (distanceToPlayer < -1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Decrease health, emit particle, trigger sreenshake when gets hit by bullets
        if (collision.gameObject.tag == "Bullet")
        {

            //particle.Emit(5);
            //Camera.main.transform.DOShakePosition(0.25f, new Vector3(0.25f, 0.25f, 0));
            slowDown = true;
            slowDownCoroutine = StartCoroutine(slowDownDebuff(1f));
            rb.AddForce((transform.position - collision.gameObject.transform.position) * playerBulletForce);
            collision.gameObject.SetActive(false);
        }
    }
    private IEnumerator slowDownDebuff(float wait)
    {
        //Make sure that the death particle will be shown
        yield return new WaitForSeconds(wait);
        slowDown = false;
    }

    private void FixedUpdate()
    {
        canJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayer();
        //string clipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        //float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (findPlayer) //distanceFromPlayer < lineOfSite
        {
            //transform.position = Vector2.MoveTowards(this.transform.position, player.position, followSpeed * Time.deltaTime);
            movementControl();
        }
    }
}
