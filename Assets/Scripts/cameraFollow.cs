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

    //A dark background to emphasize the text
    public GameObject deathBackground;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        //deathBackground = GameObject.Find("deathBackground");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Disable the background at first
        //deathBackground.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Follows the player, keeps lerping towards
        Vector2 targetPos = new Vector2 (player.position.x, player.position.y);
        Vector2 smoothPos = Vector2.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        smoothPos.x = Mathf.Clamp(smoothPos.x,-31f, 31f);
        smoothPos.y = Mathf.Clamp(smoothPos.y, -35, 35);
        transform.position = new Vector3(smoothPos.x, smoothPos.y + yPos, -15.0f);
    }

    private void Update()
    {
        //Enable the background when player is dead
        //if (gameManager.Instance.death == true)
        //{
            //deathBackground.SetActive(true);
        //}
    }
}
