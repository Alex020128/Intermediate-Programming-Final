using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attacksUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.Instance.playerDeath == true || gameManager.Instance.petDeath == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}
