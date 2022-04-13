using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    //Stats
    [SerializeField]
    public float lifeTimer;

    public bool enemyTrapped;

    private void Awake()
    {
        lifeTimer = 20.0f;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Decrease health, emit particle, trigger sreenshake when gets hit by bullets
        if (collision != null && collision.gameObject.tag == "Enemy" && enemyTrapped == false)
        {
            Debug.Log("233");

            //particle.Emit(5);
            //Camera.main.transform.DOShakePosition(0.25f, new Vector3(0.25f, 0.25f, 0));
            collision.gameObject.GetComponent<trapped>().gotTrapped = true;
            lifeTimer = 10.0f;
            enemyTrapped = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //The bullet can't fly forever, so a lifetimer is set
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0)
        {
            //Set the bullet to inactive
            this.gameObject.SetActive(false);
        }
    }
}
