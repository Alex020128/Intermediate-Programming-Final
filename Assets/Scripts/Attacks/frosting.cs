using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frosting : MonoBehaviour
{
    //GameObjects
    public GameObject frostCircle;
    public GameObject player;

    //Components
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        //Assign variables
        frostCircle = GameObject.Find("displayCircle");
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Slow down the velocity of melee enemies when inside the frost field (while on ground); slow down the velocity of range enemies when inside the frost field
        if (frostCircle != null)
        {
            if (GetComponent<meleeEnemyMovement>() != null)
            {
                if (frostCircle.GetComponent<frostCircle>().casting &&
                Vector3.Distance(player.transform.position, transform.position) <= 3 &&
                GetComponent<meleeEnemyMovement>().isGrounded == true)
                {
                    rb.velocity *= 0.3f;
                }
            }
            else
            {
                if (frostCircle.GetComponent<frostCircle>().casting &&
                                Vector3.Distance(player.transform.position, transform.position) <= 3)
                {
                    rb.velocity *= 0.3f;
                }
            }
        }
    }
}
