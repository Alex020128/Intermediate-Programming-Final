using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapSpawner : MonoBehaviour
{
    //Trap stats
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

    //SFX for trap
    public AudioSource audioSource;
    public AudioClip bulletSound;

    private void Start()
    {
        //Spawn 3 bullets at the beginning
        for (int i = 0; i < 3; i++)
        {
            GameObject newBullet = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            traps.Add(newBullet);
            traps[i].SetActive(false);
        }
    }

    public void placeTrap()
    {
        //let one of the waiting traps to be active
        for (int i = 0; i < traps.Count; i++)
        {
            if (!traps[i].activeInHierarchy)
            {
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
        //Spawn trap if there's still bullet not shot
        spawnTimer -= Time.deltaTime;
        while (spawnTimer < 0.0f)
        {
            spawnTimer += spawnPerSecond;
            GameObject.Find("Player").GetComponent<playerMovement>().placeTrap = false;
        }

        //Let trap "spawn" at the player's position
        transform.position = new Vector2(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y);
    }
}
