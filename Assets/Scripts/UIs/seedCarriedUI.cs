using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class seedCarriedUI : MonoBehaviour
{
    //Components
    public TMP_Text seedCarried;
    
    // Start is called before the first frame update
    void Start()
    {
        //Assign variables
        seedCarried = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //Display seed carried on player
        if(gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            seedCarried.text = "Seeds Carried: " + gameManager.Instance.seedCarried;
        }
        else
        {
            //Hide this when the game ends
            seedCarried.enabled = false;
        }
    }
}
