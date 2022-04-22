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

    // Start is called before the first frame update
    void Awake()
    {
        meleeEnemyKills = 0;
        rangeEnemyKills = 0;
        meleeHit = 0;
        rangeHit = 0;

        name = "ScoreManager"; // Set name of object
    }

    void Start()
    {
        //The initial position of the UI text
        //transform.localPosition = new Vector2(0, 206.5f);
    }

    // Update is called once per frame
    void Update()
    {
        playerTime = timeManager.Instance.playerTime;
    }
}
