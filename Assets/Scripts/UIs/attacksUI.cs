using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attacksUI : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //Hide when game ends
        if (gameManager.Instance.playerDeath == true || gameManager.Instance.petDeath == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}
