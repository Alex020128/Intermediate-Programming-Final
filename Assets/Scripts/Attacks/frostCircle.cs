using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frostCircle : MonoBehaviour
{
    //Stats
    private float frostTimer;
    [SerializeField]
    private float coolDownPerSecond = 30f;
    [SerializeField]
    private float coolDownTimer;

    //Components
    private SpriteRenderer sr;
    private Animator animator;

    //Bools
    public bool casting;

    //Player SFXs
    public AudioSource audioSource;
    public AudioClip frostingSound;

    private void Start()
    {
        //Assign variables
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void castCircle()
    {
        //Reset bool
        casting = true;
    }

    public void frostingSFX()
    {
        //Play the SFX when casting
        audioSource.Stop();
        audioSource.clip = frostingSound;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //Cast frost field
        while (coolDownTimer < 0.0f)
        {
            coolDownTimer += coolDownPerSecond;

            GameObject.Find("Player").GetComponent<playerMovement>().castCircle = false;
        }

        //Change animation, show the sprite, start the coroutine when casted
        if (casting)
        {
            sr.enabled = true;
            animator.SetBool("Casting",true);
            frostTimer += Time.deltaTime;
            coolDownTimer = 30;
        }
        else
        {
            frostTimer = 0;
            animator.SetBool("Casting", false);
            audioSource.Stop();
            sr.enabled = false;
            coolDownTimer -= Time.deltaTime;
        }

        //Friost field only has 10s
        if (frostTimer > 10)
        {
            casting = false;
        }

        //Make sure it follows the player all the time
        this.gameObject.transform.position = new Vector2(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y + 0.5f);
    }
}
