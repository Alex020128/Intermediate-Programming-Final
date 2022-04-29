using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class seedCarriedUI : MonoBehaviour
{
    public TMP_Text seedCarried;
    
    // Start is called before the first frame update
    void Start()
    {
        seedCarried = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            seedCarried.text = "Seeds Carried: " + gameManager.Instance.seedCarried;
        }
        else
        {
            seedCarried.enabled = false;
        }
    }
}
