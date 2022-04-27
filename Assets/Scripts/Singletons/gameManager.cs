using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class gameManager : Singleton<gameManager>
{
    //Player bullet
    [SerializeField]
    private GameObject seedSpawner;
    [SerializeField]
    private GameObject enemySpawner;

    //Player stats
    public int playerHealth;
    public int petHealth;

    public int petDemand;
    public int seedMax;
    public int spawnWave;

    //Player bools
    public bool playerDeath;
    public bool playerInvinsible;
    public bool petDeath;
    public bool petInvinsible;

    public bool levelCleared;

    //Player invincible timer
    public float playerInvinsibleTime;
    public float petInvinsibleTime;
    public float seedCarried;

    Coroutine endingCoroutine;

    /*//Background music loop
    public AudioSource audioSource;
    public AudioClip musicLoop;*/

    void Awake()
    {
        name = "GameManager"; // Set name of object

        seedSpawner = GameObject.Find("seedSpawner");
        enemySpawner = GameObject.Find("enemySpawner");

        //audioSource = GetComponent<AudioSource>();

        spawnWave = 1;
    }

    private void Start()
    {
        //Loops the music
        //audioSource.clip = musicLoop;

        //audioSource.Play();
        resetStats();
    }

    public void resetStats()
    {
        playerHealth = 100;
        petHealth = 50;
        playerInvinsibleTime = 0;
        petInvinsibleTime = 0;

        petDemand = 5;
        seedMax = 8;
        spawnWave = 1;

        seedCarried = 0;

        playerDeath = false;
        playerInvinsible = false;
        petDeath = false;
        petInvinsible = false;
    }

    private IEnumerator endingScreen(float wait)
    {
        //Make sure that the death particle will be shown
        yield return new WaitForSeconds(wait);
        if ("gameScreen" == SceneManager.GetActiveScene().name)
        {
            SceneManager.LoadScene("endingScreen");
            resetStats();
        }
    }

    void Update()
    {

        if ("gameScreen" == SceneManager.GetActiveScene().name)
        {
            seedSpawner = GameObject.Find("seedSpawner");
            enemySpawner = GameObject.Find("enemySpawner");
        }

        if (levelCleared)
        {
            GameObject.Find("Pet").GetComponent<Animator>().SetTrigger("Attack");
            Camera.main.transform.DOShakePosition(2f, new Vector3(0.75f, 0.75f, 0));
            petDemand += 5;
            seedMax += 5;
            spawnWave += 1;
            seedSpawner.GetComponent<seedSpawner>().spawnSome();
            enemySpawner.GetComponent<enemySpawner>().killAllEnemies();
            enemySpawner.GetComponent<enemySpawner>().spawnWave();
            levelCleared = false;
        }
        
        //The player has 1s of invincible time if gets hurt
        if (playerInvinsible == true)
        {
            playerInvinsibleTime += Time.deltaTime;
            if (playerInvinsibleTime >= 1)
            {
                playerInvinsible = false;
            }
        }
        //The pet has 1s of invincible time if gets hurt
        if (petInvinsible == true)
        {
            petInvinsibleTime += Time.deltaTime;
            if (petInvinsibleTime >= 1)
            {
                petInvinsible = false;
            }
        }

        //Player death
        if (playerHealth <= 0)
        {
            playerDeath = true;
        }
        //Pet death
        if (petHealth <= 0)
        {
            petDeath = true;
        }

        //Player's health can't be over 100
        if (playerHealth > 100)
        {
            playerHealth = 100;
        }

        if (petHealth > 50)
        {
            petHealth = 50;
        }

        if("gameScreen" == SceneManager.GetActiveScene().name)
        {
            if ((playerDeath || petDeath))
            {
                endingCoroutine = StartCoroutine(endingScreen(3f));
            }
        }

        if ("titleScreen" == SceneManager.GetActiveScene().name)
        {
            resetStats();
        }
    }
}
