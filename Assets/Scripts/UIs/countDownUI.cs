using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class countDownUI : MonoBehaviour
{
    //Components
    public TMP_Text countDown;
    public AudioSource audioSource;
    public AudioClip moreEnemies;

    //Bools
    public bool countDownStart;

    // Start is called before the first frame update
    void Start()
    {
        //Assign variables
        countDown = GetComponent<TMP_Text>();
        audioSource = GetComponent<AudioSource>();
        countDownStart = false;
    }
    
    //The sequence of count down coroutines
    private IEnumerator countDown2(float wait)
    {
        yield return new WaitForSeconds(wait);
        countDown.text = "2";
    }
    private IEnumerator countDown1(float wait)
    {
        yield return new WaitForSeconds(wait);
        countDown.text = "1";
    }
    private IEnumerator countDownText(float wait)
    {
        yield return new WaitForSeconds(wait);
        countDown.text = "More Monsters Appeared!";
        yield return new WaitForSeconds(wait);
        //Spawn new waves of enemies
        GameObject.Find("enemySpawner").GetComponent<enemySpawner>().spawnWave();
        countDownStart = false;
    }

    public IEnumerator countDownCoroutineLoop()
    {
        countDownStart = true;
        countDown.text = "3";
        countDownSFX();
        //Start the sequence of coroutines
        yield return StartCoroutine(countDown2(1f));
        yield return StartCoroutine(countDown1(1f));
        yield return StartCoroutine(countDownText(1f));
    }

    public void countDownSFX()
    {
        //Play the SFX when countdown
        audioSource.Stop();
        audioSource.clip = moreEnemies;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            //Hide when not counting down
            if(countDownStart == false)
            {
                countDown.text = "";
            }
        }
        else
        {
            //Hide when game ends
            countDown.enabled = false;
        }

    }
}
