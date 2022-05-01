using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraFollow : MonoBehaviour
{
    //Camera stats
    public float followSpeed = 4.75f;
    public float yPos;
    
    //Player's position
    public Transform player;

    private void Awake()
    {
        //Assign variables
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Follows the player, keeps lerping towards
        Vector2 targetPos = new Vector2 (player.position.x, player.position.y);
        Vector2 smoothPos = Vector2.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        smoothPos.x = Mathf.Clamp(smoothPos.x,-7f, 7f);
        smoothPos.y = Mathf.Clamp(smoothPos.y, -3f, 19f);
        transform.position = new Vector3(smoothPos.x, smoothPos.y + yPos, -15.0f);
    }
}
