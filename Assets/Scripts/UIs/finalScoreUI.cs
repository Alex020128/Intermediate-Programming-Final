using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class finalScoreUI : MonoBehaviour
{
    //UI text
    public TMP_Text finalScore;

    //Total score
    public float Score;

    private void Awake()
    {
        finalScore = GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Score = Mathf.Floor(scoreManager.Instance.meleeEnemyKills * 100 +
                                scoreManager.Instance.rangeEnemyKills * 150 +
                                scoreManager.Instance.playerTime * 10 -
                                scoreManager.Instance.meleeHit * 10 -
                                scoreManager.Instance.rangeHit * 5);


        finalScore.text = "Score: " + Score;

        if (Score < 0)
        {
            Score = 0;
        }
    }
}
