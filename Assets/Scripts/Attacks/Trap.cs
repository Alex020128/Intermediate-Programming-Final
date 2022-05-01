using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    //Stats
    [SerializeField]
    public float lifeTimer;

    //Components
    public Animator animator;

    //Bools
    public bool enemyTrapped;

    private void Awake()
    {
        //Assign variables
        lifeTimer = 20.0f;
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Trap enemies when collide with enemies
        if (collision != null && collision.gameObject.tag == "Enemy" && enemyTrapped == false)
        {
            collision.gameObject.GetComponent<trapped>().gotTrapped = true;
            lifeTimer = 10.0f;
            enemyTrapped = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //The trap can't stay forever, so a lifetimer is set
        lifeTimer -= Time.deltaTime;

        //Change aniamtion sccording to state
        if (enemyTrapped)
        {
            animator.SetBool("Trapped", true);
        }
        else
        {
            animator.SetBool("Trapped", false);
        }

        if (lifeTimer <= 0)
        {
            //Set the trap to inactive
            this.gameObject.SetActive(false);
        }
    }
}
