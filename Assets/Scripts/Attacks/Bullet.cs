using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    //Stats
    [SerializeField]
    public float turnSpeed = 360; // degrees per second
    [SerializeField]
    public float lifeTimer;
    public float moveSpeed = 13;

    [SerializeField] private float playerBulletForce = 70f;

    public Rigidbody2D rb;
    private ParticleSystem knockBackParticle;
 //Target position
    public Vector3 targetRotation;

    private void Awake()
    {
        lifeTimer = 3.0f;
        rb = GetComponent<Rigidbody2D>();
        knockBackParticle = GetComponent<ParticleSystem>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Decrease health, emit particle, trigger sreenshake when gets hit by bullets
        if (collision != null && collision.gameObject.tag == "Enemy")
        {
            if(collision.gameObject.GetComponent<meleeEnemyMovement>() != null)
            {
                collision.gameObject.GetComponent<meleeEnemyMovement>().knockBackParticle.Emit(3);
            }
            if (collision.gameObject.GetComponent<rangeEnemyMovement>() != null)
            {
                collision.gameObject.GetComponent<rangeEnemyMovement>().knockBackParticle.Emit(3);
            }
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(rb.velocity.normalized * playerBulletForce, ForceMode2D.Impulse);
            this.gameObject.SetActive(false);
        }

        if (collision != null && collision.gameObject.tag != "Enemy"
                              && collision.gameObject.tag != "Player"
                              && collision.gameObject.tag != "Attacks"
                              && collision.gameObject.tag != "Pet"
                              && collision.gameObject.tag != "Seed")
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //The bullet can't fly forever, so a lifetimer is set
        lifeTimer -= Time.deltaTime;

            if (lifeTimer <= 2.5f)
            {
            //Move along the latest rotation(towards the player)

            rb.velocity = transform.right * moveSpeed;
                transform.parent = null;
                if (lifeTimer <= 0)
                {    
                //Set the bullet to inactive
                this.gameObject.SetActive(false);
                }
            } else
            {
            //Sets the rotation of the bullet towards the player
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            targetRotation = new Vector3(0, 0, angle);
            transform.rotation = Quaternion.Euler(targetRotation);

            //Let the bullet starts from the player
            if (GameObject.Find("Player").GetComponent<playerMovement>().facingRight)
            {
                transform.localPosition = new Vector3(0.7f, 1.2f, 0);
            }
            else
            {
                transform.localPosition = new Vector3(-0.7f, 1.2f, 0);
            }
        }
    }
        

}