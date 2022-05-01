using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class timeManager : Singleton<timeManager>
{
    //Total play time
    public float playerTime;
    public float finalTime;

    //Enemy and spawn stats
    public float initialSpawnSize;
    public float spawnSize;

    void Awake()
    {
        name = "TimeManager"; // Set name of object

        //Assign variables
        spawnSize = 3;
    }

    private void Start()
    {
        //Reset all the stats
        resetStats();
    }

    public void resetStats()
    {
        //Reset all the stats
        playerTime = 0;
        finalTime = 0; ;
        initialSpawnSize = 3;
        spawnSize = 3;playerTime = 0;
        finalTime = 0; ;
        initialSpawnSize = 3;
        spawnSize = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            //Start play time timer
            playerTime += Time.deltaTime;
        }
        else
        {
            //Record the total play time
            finalTime = playerTime;
        }

        //Increase the enemy stats every 60s
        if (Mathf.Floor(playerTime) % 60 == 0 && Mathf.Floor(playerTime) != 0)
        {
            //increases the maximum enemy sizecy
            spawnSize = initialSpawnSize + 1 * Mathf.Floor(playerTime / 60);
        }

        //Reset all the stats
        if ("titleScreen" == SceneManager.GetActiveScene().name)
        {
            resetStats();
        }
    }
}
