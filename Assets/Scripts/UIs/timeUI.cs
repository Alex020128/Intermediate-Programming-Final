using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class timeUI : MonoBehaviour
{
    //UI text
    public TMP_Text time;

    void Awake()
    {
        time = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            time.text = "Time: " + Mathf.Round(timeManager.Instance.playerTime)+ "s"; // + "s\nEnemy Level: " + Mathf.Ceil(playerTime / 20)
        } else
        {
            //Hide this when player is dead
            time.enabled = false;
            //Destroy(this);
        }
    }
}
