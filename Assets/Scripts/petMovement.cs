using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class petMovement : MonoBehaviour
{
    //Components
    private Transform player;
    public Animator animator;
    public ParticleSystem hurtParticle;

    //Coroutines
    Coroutine carryCoroutine;

    //Stats
    public float feedTime = 0;
    public float seedEaten = 0;
    
    //Bools
    public bool feeding;
    public bool carrying;

    //Player SFXs
    public AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip eatSound;
    public AudioClip deathSound;
    public AudioClip carrySound;
    public AudioClip attackSound;

    // Start is called before the first frame update
    void Start()
    {
        //Assign variables
        player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        hurtParticle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    //SFXs
    public void hurtSFX()
    {
        //Play the pet hurt SFX
        audioSource.Stop();
        audioSource.clip = hurtSound;
        audioSource.Play();
    }
    public void eatSFX()
    {
        //Play the SFX when eat seed
        audioSource.Stop();
        audioSource.clip = eatSound;
        audioSource.Play();
    }
    public void deathSFX()
    {
        //Play the pet death SFX
        audioSource.Stop();
        audioSource.clip = deathSound;
        audioSource.Play();
    }
    public void carrySFX()
    {
        //Play the pet death SFX
        audioSource.Stop();
        audioSource.clip = carrySound;
        audioSource.Play();
    }
    public void attackSFX()
    {
        //Play the pet attack SFX
        audioSource.Stop();
        audioSource.clip = attackSound;
        audioSource.Play();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        //Players can feed the seed carried on him to the pet or carry/drop the pet when colliding
        if (collision != null && collision.gameObject.tag == "Player")
        {
            feeding = true;
            feedTime += Time.deltaTime;

            //Feed all the seeds carried after 5s
            if (feedTime > 5 && gameManager.Instance.seedCarried != 0)
            {
                eatSFX();
                seedEaten += gameManager.Instance.seedCarried;
                scoreManager.Instance.seedEaten += gameManager.Instance.seedCarried;
                gameManager.Instance.seedCarried = 0;
            }

            //Carry the pet or drop the pet by pressing F
            if (Input.GetKeyDown(KeyCode.F) && carrying == false)
            {
                carrySFX();
                carryCoroutine = StartCoroutine(carryTime(0.1f));
            }
            if (Input.GetKeyDown(KeyCode.F) && carrying == true && player.gameObject.GetComponent<playerMovement>().isGrounded)
            {
                carrySFX();
                carryCoroutine = StartCoroutine(carryTime(0.1f));
            }
        }

        //Decrease pet health, emits particle, change animation, set pet invincible time, trigger sreenshake when hits the enemy
        if (collision.gameObject.tag == "Enemy"&& collision.gameObject.GetComponent<trapped>().gotTrapped == false
                                               && collision.gameObject.GetComponent<slowDown>().frozen == false
                                               && gameManager.Instance.petInvinsible == false
                                               && gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            animator.SetTrigger("Hurt");
            collision.gameObject.GetComponent<Animator>().SetTrigger("Attack");
            gameManager.Instance.petHealth -= 2;
            hurtParticle.Emit(5);
            if (collision.gameObject.GetComponent<meleeEnemyMovement>() != null)
            {
                collision.gameObject.GetComponent<meleeEnemyMovement>().attackSFX();
            }
            if (collision.gameObject.GetComponent<rangeEnemyMovement>() != null)
            {
                collision.gameObject.GetComponent<rangeEnemyMovement>().attackSFX();
            }
            hurtSFX();
            Camera.main.transform.DOShakePosition(0.25f, new Vector3(0.25f, 0.25f, 0));
            gameManager.Instance.petInvinsibleTime = 0;
            scoreManager.Instance.meleeHit += 1;
            gameManager.Instance.petInvinsible = true;
        }
    }

    private IEnumerator carryTime(float wait)
    {
        //Make sure pet doesn't change state over and over again
        yield return new WaitForSeconds(wait);
        if (!carrying)
        {
            carrying = true;
        } else
        {
            carrying = false;
        }
    }

   //Player can only feed the pet when colliding
    public void OnTriggerExit2D(Collider2D collision)
    {
        feeding = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Reset the feed time timer
        if (!feeding)
        {
            feedTime = 0;
        }

        //Follows the player when being carried
        if (carrying)
        {
            transform.position = new Vector2(player.position.x, player.position.y + 0.4f);
            feeding = true;
            feedTime = 6;
        }

        //Level cleared when reached demand
        if (seedEaten >= gameManager.Instance.petDemand)
        {
            gameManager.Instance.levelCleared = true;
            seedEaten -= gameManager.Instance.petDemand;
        }
    }
}
