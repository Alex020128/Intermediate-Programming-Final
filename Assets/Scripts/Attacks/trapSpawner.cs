using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapSpawner : MonoBehaviour
{
    //Bullet stats
    [SerializeField]
    private GameObject prefabToSpawn = null;
    [SerializeField]
    private float spawnPerSecond = 10f;
    [SerializeField]
    private float spawnTimer;
    [SerializeField]
    private List<GameObject> traps = new List<GameObject>();
    public List<GameObject> Traps
    {
        get
        {
            return traps;
        }
    }

    //SFX for bullet
    public AudioSource audioSource;
    public AudioClip bulletSound;

    private void Start()
    {
        //Spawn a pool of bullets at top of the screen
        for (int i = 0; i < 3; i++)
        {
            GameObject newBullet = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            traps.Add(newBullet);
            traps[i].SetActive(false);
        }

        //audioSource = GetComponent<AudioSource>();
    }

    //public void bulletSFX()
    //{
    //Play the bullet SFX
    //audioSource.Stop();
    //audioSource.clip = bulletSound;
    //audioSource.Play();
    //}

    public void placeTrap()
    {
        //let one of the waiting bullets to be active
        for (int i = 0; i < traps.Count; i++)
        {
            if (!traps[i].activeInHierarchy)
            {
                //bulletSFX();
                traps[i].SetActive(true);
                traps[i].GetComponent<Trap>().lifeTimer = 30.0f;
                traps[i].GetComponent<Trap>().enemyTrapped = false;
                traps[i].transform.position = transform.position;
                break;
            }
        }
    }
    void Update()
    {
        //Shoot bullet if there's still bullet not shot
        spawnTimer -= Time.deltaTime;
        while (spawnTimer < 0.0f) // && gameManager.Instance.death == false
        {
            spawnTimer += spawnPerSecond;

            GameObject.Find("Player").GetComponent<playerMovement>().placeTrap = false;
        }

        //Let bullet "spawn" around the player's chest
        transform.position = new Vector2(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y);
    }
}
