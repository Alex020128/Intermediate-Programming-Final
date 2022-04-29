using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapped : MonoBehaviour
{
    public bool gotTrapped;

    public Rigidbody2D rb;
    public Animator animator;

    Coroutine trappedCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private IEnumerator trappedDebuff(float wait)
    {
        //Make sure that the death particle will be shown
        yield return new WaitForSeconds(wait);
        gotTrapped = false;
        animator.SetBool("Trapped", false);
    }


    // Update is called once per frame
    void Update()
    {
        if (gotTrapped)
        {
            animator.SetBool("Trapped", true);
            trappedCoroutine = StartCoroutine(trappedDebuff(10));
            rb.velocity = new Vector2(0, 0);
        }

    }
}
