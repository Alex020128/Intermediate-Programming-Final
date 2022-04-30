using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public float collectTime = 0;
    public bool collecting;

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
                gameManager.Instance.petHealth += 3;
                GameObject.Find("Player").GetComponent<playerMovement>().seedSFX();
                Destroy(this.gameObject);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        collecting = false;
    }

    public void OnDrawGizmos()
	{
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.25f), Vector2.down, Color.green);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up, Color.green);
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
