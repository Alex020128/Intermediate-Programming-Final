using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapped : MonoBehaviour
{
    //Bools
    public bool gotTrapped;
    
    //Components
    public Rigidbody2D rb;
    public Animator animator;

    //Trapped SFXs
    public AudioSource audioSource;
    public AudioClip trappedSound;

    //Coroutines
    Coroutine trappedCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        //Assign variables
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private IEnumerator trappedDebuff(float wait)
    {
        //Make sure that the SFX only plays once
        if (audioSource.clip != trappedSound)
        {
            trappedSFX();
        }
        //Trap the enemy for 10s
        yield return new WaitForSeconds(wait);
        gotTrapped = false;
        animator.SetBool("Trapped", false);
    }
    public void trappedSFX()
    {
        //Play the enemy trapped SFX
        audioSource.Stop();
        audioSource.clip = trappedSound;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //Traps the enemy, change animation and start coroutine
        if (gotTrapped)
        {
            animator.SetBool("Trapped", true);
            trappedCoroutine = StartCoroutine(trappedDebuff(10));
            rb.velocity = new Vector2(0, 0);
        }
    }
}
