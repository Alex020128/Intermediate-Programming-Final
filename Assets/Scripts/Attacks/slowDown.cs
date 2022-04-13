using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowDown : MonoBehaviour
{
    public bool speedDown;
    public bool frozen;
    
    public Rigidbody2D rb;

    Coroutine speedDownCoroutine;
    Coroutine frozenCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }
    private IEnumerator speedDownDebuff(float wait)
    {
        //Make sure that the death particle will be shown
        yield return new WaitForSeconds(wait);
        speedDown = false;
    }
    private IEnumerator frozenDebuff(float wait)
    {
        //Make sure that the death particle will be shown
        yield return new WaitForSeconds(wait);
        frozen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (speedDown)
        {
            speedDownCoroutine = StartCoroutine(speedDownDebuff(3));
            rb.velocity *= 0.5f;
        }

        if (frozen)
        {
            frozenCoroutine = StartCoroutine(frozenDebuff(3));
            rb.velocity = new Vector2(0,0);
        }
    }
}
