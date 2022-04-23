using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timeManager : Singleton<timeManager>
{
    //Total play time
    public float playerTime;

    //UI text
    public TMP_Text time;

    //Enemy and spawn stats

    public float initialSpawnSize;
    public float spawnSize;


    void Awake()
    {
        name = "TimeManager"; // Set name of object
        
        time = GetComponent<TMP_Text>();

        //playerTime = 0;

        //initialSpawnSize = 3;
        //spawnSize = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.Instance.playerDeath == false)
        {
            //Record the total play time
            playerTime += Time.deltaTime;
            time.text = "Time: " + Mathf.Round(playerTime)+ "s"; // + "s\nEnemy Level: " + Mathf.Ceil(playerTime / 20)
        } else
        {
           //Hide this when player is dead
            time.enabled = false;
        }

        //Increase the enemy stats
        if (Mathf.Floor(playerTime) % 30 == 0 && Mathf.Floor(playerTime) != 0)
        {
  
            //increases the maximum enemy size and the spawn frequency
            spawnSize = 3 + 1 * Mathf.Floor(playerTime / 30);
            //spawnFrequency = 5 - 0.5f * Mathf.Floor(playerTime / 20);
        }
    }
}
