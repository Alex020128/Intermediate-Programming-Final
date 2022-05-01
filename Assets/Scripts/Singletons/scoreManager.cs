using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scoreManager : Singleton<scoreManager>
{
    //Components of the total score
    public float meleeEnemyKills;
    public float rangeEnemyKills;
    public float meleeHit;
    public float rangeHit;
    public float playerTime;
    public float seedEaten;

    // Start is called before the first frame update
    void Awake()
    {
        name = "ScoreManager"; // Set name of object
    }

    void Start()
    {
        //Reset all the stats
        resetStats();
    }

    public void resetStats()
    {
        //Reset all the stats
        meleeEnemyKills = 0;
        rangeEnemyKills = 0;
        meleeHit = 0;
        rangeHit = 0;
        seedEaten = 0;
        playerTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Record the time of the game
        if(SceneManager.GetActiveScene().name =="endingScreen")
        {
            playerTime = timeManager.Instance.finalTime;
        }

        //Reset all the stats
        if (SceneManager.GetActiveScene().name == "titleScreen")
        {
            resetStats();
        }
    }
}
