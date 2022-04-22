using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class enemyBullet : MonoBehaviour
{
    //Enemy bullet stats
    [SerializeField]
    public float turnSpeed = 360; // degrees per second
    [SerializeField]
    public float lifeTimer;
    public float moveSpeed = 10;

    //Target position
    public Vector3 targetRotation;
    public Transform player;

    private void Awake()
    {
        lifeTimer = 3.0f;
    }

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Decrease player health, emits particle, set player invincible time, trigger sreenshake when hits the player
        if (collision.gameObject.tag == "Player" && gameManager.Instance.playerInvinsible == false
                                                          && gameManager.Instance.playerDeath == false)
        {
            gameManager.Instance.playerHealth -= 1;
            //GameObject.Find("Player").GetComponent<playerMovement>().Particle.Emit(5);
            //GameObject.Find("Player").GetComponent<playerMovement>().hurtSFX();
            Camera.main.transform.DOShakePosition(0.5f, new Vector3(0.5f, 0.5f, 0));
            gameManager.Instance.playerInvinsibleTime = 0;
            scoreManager.Instance.rangeHit += 1;
            gameManager.Instance.playerInvinsible = true;
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
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            transform.parent = null;
            if (lifeTimer <= 0)
            {
                //Set the bullet to inactive
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            //Sets the rotation of the bullet towards the player
            Vector3 playerPosition = player.position;
            Vector2 direction = playerPosition - transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            targetRotation = new Vector3(0, 0, angle);
            transform.rotation = Quaternion.Euler(targetRotation);

            //Let the bullet starts from the player
            transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
