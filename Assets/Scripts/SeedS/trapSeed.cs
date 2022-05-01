using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapSeed : MonoBehaviour
{
    //Stats
    public float collectTime = 0;
    public bool collecting;

    public void OnTriggerStay2D(Collider2D collision)
    {
        //Be collected when colliding with player for 2s
        if (collision != null && collision.gameObject.tag == "Player")
        {
            collecting = true;
            collectTime += Time.deltaTime;

            if (collectTime > 2)
            {
                gameManager.Instance.seedCarried += 1;
                gameManager.Instance.petHealth += 3;
                //Update the current q skill to vine traps
                GameObject.Find("Player").GetComponent<playerMovement>().equipment = "Trap";
                GameObject.Find("getEquipmentUI").GetComponent<getEquipmentUI>().faded = false;
                GameObject.Find("Player").GetComponent<playerMovement>().seedSFX();
                Destroy(this.gameObject);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        collecting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Reset timer when not colliding
        if (!collecting)
        {
            collectTime = 0;
        }
    }
}
