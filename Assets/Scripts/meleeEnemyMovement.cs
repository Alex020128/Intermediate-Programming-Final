using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeEnemyMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 50f;
    public float followSpeed;
    public float lineOfSite;
    //public Animator animator;

    private Transform player;
    public Rigidbody2D rb;

    public bool canJump;

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

    private void CheckGrounded()
    {
        RaycastHit2D grounedRay = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, 1 << 3);

        Debug.Log(grounedRay.collider, gameObject);

        if (grounedRay.collider != null && grounedRay.collider.gameObject.GetComponent<enemyCanJump>() != null)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }
    public void OnDrawGizmos()
    {
        Color myCol;

        if (canJump == true)
        {
            myCol = Color.green;
        }
        else
        {
            myCol = Color.red;
        }

        Debug.DrawRay(transform.position, Vector2.down, myCol);
    }


    public void movementControl()
    {
        float distanceToPlayer = 0;
        distanceToPlayer = player.position.x - transform.position.x;
        rb.velocity = new Vector2(Mathf.Sign(distanceToPlayer) * followSpeed, rb.velocity.y);

        if (canJump)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }
        CheckGrounded();
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

    private void FixedUpdate()
    {
        canJump = false;
    }

    // Update is called once per frame
    void Update()
    {

        //string clipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSite)
        {
            rb.isKinematic = false;
            //transform.position = Vector2.MoveTowards(this.transform.position, player.position, followSpeed * Time.deltaTime);
            movementControl();

        }   
    }
}
