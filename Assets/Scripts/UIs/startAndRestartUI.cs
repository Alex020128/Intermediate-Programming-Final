using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class startAndRestartUI : MonoBehaviour
{
    //Components
    public TMP_Text startAndRestart;

    // Update is called once per frame
    void Update()
    {
        //Show different text in different scenes
        if ("titleScreen" == SceneManager.GetActiveScene().name)
        {
            startAndRestart.text = "Press Space to Start!";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("gameScreen");
            }
        }
        
        if("endingScreen" == SceneManager.GetActiveScene().name)
        {
            startAndRestart.text = "Press Space to Restart!";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("titleScreen");
            }
        }
    }
}
