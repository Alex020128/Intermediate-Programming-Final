using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frosting : MonoBehaviour
{
    public GameObject frostCircle;
    public GameObject player;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        frostCircle = GameObject.Find("displayCircle");
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(frostCircle != null)
        {
            if (frostCircle.GetComponent<frostCircle>().casting && Vector3.Distance(player.transform.position, transform.position) <= 5)
            {
                rb.velocity *= 0.3f;
            }
        }
    }
}
