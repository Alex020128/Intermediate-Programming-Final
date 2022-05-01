using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class seedEatenBar : MonoBehaviour
{
    //Components
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        //Assign variables
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Lerps the seed eaten bar
        slider.DOValue(GameObject.Find("Pet").GetComponent<petMovement>().seedEaten / gameManager.Instance.petDemand, 0.5f);

        if (gameManager.Instance.playerDeath == true || gameManager.Instance.petDeath == true)
        {
            //Hide when the game ends
            this.gameObject.SetActive(false);
        }
    }
}
