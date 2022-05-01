using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowDown : MonoBehaviour
{
    //Bools
    public bool speedDown;
    public bool frozen;
    
    //Vomonents
    public Rigidbody2D rb;
    public Animator animator;
    
    //Frozen SFXs
    public AudioSource audioSource;
    public AudioClip frozenSound;

    //Coroutines
    Coroutine speedDownCoroutine;
    Coroutine frozenCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        //Assign variables
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private IEnumerator speedDownDebuff(float wait)
    {
        //Slow down the velocity for 3s
        yield return new WaitForSeconds(wait);
        speedDown = false;
    }
    private IEnumerator frozenDebuff(float wait)
    {
        //Make sure the SFX only plays once
        if(audioSource.clip != frozenSound)
        {
            frozenSFX();
        }
        //Frozen the enemy for 3s
        yield return new WaitForSeconds(wait);
        frozen = false;
        animator.SetBool("Frozen", false);
    }

    public void frozenSFX()
    {
        //Play the enemy frozen SFX
        audioSource.Stop();
        audioSource.clip = frozenSound;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //Slow down the velocity and start coroutine
        if (speedDown)
        {
            speedDownCoroutine = StartCoroutine(speedDownDebuff(3));
            rb.velocity *= 0.5f;
        }

        //Change animation of enemies, frozen the velocity and start coroutine
        if (frozen)
        {
            animator.SetBool("Frozen",true);
            frozenCoroutine = StartCoroutine(frozenDebuff(3));
            rb.velocity = new Vector2(0,0);
        }
    }
}
