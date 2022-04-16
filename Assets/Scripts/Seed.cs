using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public float collectTime = 0;
    public bool collecting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        //Decrease health, emit particle, trigger sreenshake when gets hit by bullets
        if (collision != null && collision.gameObject.tag == "Player")
        {
            collecting = true;
            collectTime += Time.deltaTime;

            //particle.Emit(5);
            //Camera.main.transform.DOShakePosition(0.25f, new Vector3(0.25f, 0.25f, 0));
            if (collectTime > 2)
            {
                gameManager.Instance.seedCarried += 1;
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
        if (!collecting)
        {
            collectTime = 0;
        }
    }
}