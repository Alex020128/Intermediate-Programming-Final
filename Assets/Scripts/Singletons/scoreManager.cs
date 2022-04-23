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
        //The initial position of the UI text
        //transform.localPosition = new Vector2(0, 206.5f);
        resetStats();
    }

    public void resetStats()
    {
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
        if(SceneManager.GetActiveScene().name =="endingScreen")
        {
            playerTime = timeManager.Instance.finalTime;
        }

        if (SceneManager.GetActiveScene().name == "titleScreen")
        {
            resetStats();
        }
    }
}
