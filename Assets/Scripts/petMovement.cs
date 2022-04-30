using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class petMovement : MonoBehaviour
{
    private Transform player;
    public Animator animator;
    public ParticleSystem hurtParticle;

    Coroutine carryCoroutine;

    public float feedTime = 0;
    public float seedEaten = 0;
    
    public bool feeding;
    public bool carrying;

    //Player SFXs
    public AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip eatSound;
    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
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

    public void OnTriggerStay2D(Collider2D collision)
    {
        //Decrease health, emit particle, trigger sreenshake when gets hit by bullets
        if (collision != null && collision.gameObject.tag == "Player")
        {
            feeding = true;
            feedTime += Time.deltaTime;

            //particle.Emit(5);
            //Camera.main.transform.DOShakePosition(0.25f, new Vector3(0.25f, 0.25f, 0));
            if (feedTime > 5)
            {
                //eatSFX();
                seedEaten += gameManager.Instance.seedCarried;
                scoreManager.Instance.seedEaten += gameManager.Instance.seedCarried;
                gameManager.Instance.seedCarried = 0;
            }

            if (Input.GetKeyDown(KeyCode.F) && carrying == false)
            {
                carryCoroutine = StartCoroutine(carryTime(0.1f));
            }
            if (Input.GetKeyDown(KeyCode.F) && carrying == true && player.gameObject.GetComponent<playerMovement>().isGrounded)
            {
                carryCoroutine = StartCoroutine(carryTime(0.1f));
            }
        }

        //Decrease pet health, emits particle, set player invincible time, trigger sreenshake when hits the player
        if (collision.gameObject.tag == "Enemy"&& collision.gameObject.GetComponent<trapped>().gotTrapped == false
                                               && collision.gameObject.GetComponent<slowDown>().frozen == false
                                               && gameManager.Instance.petInvinsible == false
                                               && gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            animator.SetTrigger("Hurt");
            collision.gameObject.GetComponent<Animator>().SetTrigger("Attack");
            gameManager.Instance.petHealth -= 2;
            hurtParticle.Emit(5);
            //GameObject.Find("Player").GetComponent<playerMovement>().hurtSFX();
            Camera.main.transform.DOShakePosition(0.25f, new Vector3(0.25f, 0.25f, 0));
            gameManager.Instance.petInvinsibleTime = 0;
            scoreManager.Instance.meleeHit += 1;
            gameManager.Instance.petInvinsible = true;
        }
    }

    private IEnumerator carryTime(float wait)
    {
        //Make sure that the death particle will be shown
        yield return new WaitForSeconds(wait);
        if (!carrying)
        {
            carrying = true;
        } else
        {
            carrying = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        feeding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!feeding)
        {
            feedTime = 0;
        }

        if (carrying)
        {
            transform.position = new Vector2(player.position.x, player.position.y + 0.4f);
            feeding = true;
            feedTime = 6;
        }

        if (seedEaten >= gameManager.Instance.petDemand)
        {
            gameManager.Instance.levelCleared = true;
            seedEaten = 0;
        }
    }
}
