using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreUI : MonoBehaviour
{
    //UI text
    public TMP_Text score;

    //Total score
    public float Score;

    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //Record your score, which is related to total play time, total kills of the enemies, and the number of times when you get hit
        if (gameManager.Instance.playerDeath == false)
        {
            Score = Mathf.Floor(scoreManager.Instance.meleeEnemyKills * 100 + 
                                scoreManager.Instance.rangeEnemyKills * 150 +
                                scoreManager.Instance.playerTime * 10 - 
                                scoreManager.Instance.meleeHit * 100 -
                                scoreManager.Instance.rangeHit * 50);
        }

        //Score cannot be negative
        if (Score < 0)
        {
            Score = 0;
        }

        {
            //Display total score
            score.text = "Score: " + Score;
        }
    }
}
