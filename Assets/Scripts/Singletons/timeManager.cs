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

        spawnSize = 3;
    }

    private void Start()
    {
        resetStats();
    }

    public void resetStats()
    {
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
            //Record the total play time
            playerTime += Time.deltaTime;
        }
        else
        {
            //Hide this when player is dead
            finalTime = playerTime;
            //Destroy(this);
        }

        //Increase the enemy stats
        if (Mathf.Floor(playerTime) % 60 == 0 && Mathf.Floor(playerTime) != 0)
        {
            //increases the maximum enemy size and the spawn frequency
            spawnSize = initialSpawnSize + 1 * Mathf.Floor(playerTime / 60);
        }

        if ("titleScreen" == SceneManager.GetActiveScene().name)
        {
            resetStats();
        }
    }
}
