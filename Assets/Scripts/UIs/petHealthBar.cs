using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class petHealthBar : MonoBehaviour
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
        //Lerps the pet health bar
        slider.DOValue(gameManager.Instance.petHealth / 50f, 0.5f);

        if (gameManager.Instance.playerDeath == true || gameManager.Instance.petDeath == true)
        {
            //Hide this when the game ends
            this.gameObject.SetActive(false);
        }
    }
}
