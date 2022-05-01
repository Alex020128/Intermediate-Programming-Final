using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class petHealthUI : MonoBehaviour
{
    //Components
    public TMP_Text petHealth;

    // Start is called before the first frame update
    void Start()
    {
        //Assign variables
        petHealth = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //Display the pet heath
        if (gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            petHealth.text = "Pet Health: " + gameManager.Instance.petHealth;
        }
        else
        {
            //Hide this when the game ends
            petHealth.enabled = false;
        }
    }
}
