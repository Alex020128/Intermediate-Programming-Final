using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    //Stats
    [SerializeField]
    public float turnSpeed = 360; // degrees per second
    [SerializeField]
    public float lifeTimer;
    public float moveSpeed = 13;

    //Target position
    public Vector3 targetRotation;

    private void Awake()
    {
        lifeTimer = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //The bullet can't fly forever, so a lifetimer is set
        lifeTimer -= Time.deltaTime;

            if (lifeTimer <= 2.5f)
            {
                //Move along the latest rotation(towards the player)
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                transform.parent = null;
                if (lifeTimer <= 0)
                {    
                //Set the bullet to inactive
                this.gameObject.SetActive(false);
                }
            } else
            {
            //Sets the rotation of the bullet towards the player
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            targetRotation = new Vector3(0, 0, angle);
            transform.rotation = Quaternion.Euler(targetRotation);

            //Let the bullet starts from the player
            transform.localPosition = new Vector3(0, 0, 0);
        }
    }
        

}