using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedSpawner : MonoBehaviour
{
    //Seeds stats
    [SerializeField]
    private GameObject prefabToSpawn = null;
    [SerializeField]
    private GameObject prefabFrostSeed = null;
    [SerializeField]
    private GameObject prefabTrapSeed = null;
    [SerializeField]
    private GameObject prefabSeed = null;
    [SerializeField]
    private float spawnPerSecond = 20f;
    [SerializeField]
    private float spawnTimer;
    [SerializeField]
    private List<GameObject> seeds = new List<GameObject>();
    public List<GameObject> Seeds
    {
        get
        {
            return seeds;
        }
    }

    //SFX for bullet
    public AudioSource audioSource;
    public AudioClip bulletSound;

    private void Start()
    {
        spawnPerSecond = 10f;

        spawnSome();

        //audioSource = GetComponent<AudioSource>();
    }

    //public void bulletSFX()
    //{
    //Play the bullet SFX
    //audioSource.Stop();
    //audioSource.clip = bulletSound;
    //audioSource.Play();
    //}

    public void spawnSome()
    {
        //Spawn 3 seeds in the space
        for (int i = 0; i < 3; i++)
        {
            int chance = Random.Range(0, 100);
            if (chance <= 20)
            {
                prefabToSpawn = prefabFrostSeed;
            }
            else if(21<= chance && chance <= 40)
            {
                prefabToSpawn = prefabTrapSeed;
            }
            else
            {
                prefabToSpawn = prefabSeed;
            }

            GameObject newSeed = Instantiate(prefabToSpawn, new Vector2(Random.Range(-10, 10), Random.Range(-5, 5)), Quaternion.identity);
            seeds.Add(newSeed);
            //seeds[i].SetActive(false);

            bool canSpawn = false;

            //Debug.Log(grounedRay.collider.gameObject);

            while (!canSpawn)
            {
                newSeed.transform.position = new Vector2(Random.Range(-10, 10), Random.Range(-5, 5));
                RaycastHit2D grounedRay = Physics2D.Raycast(newSeed.transform.position, Vector2.down, 0.5f, 1 << 6);
                RaycastHit2D ceilingRay = Physics2D.Raycast(newSeed.transform.position, Vector2.up, 0.5f, 1 << 6);

                if (grounedRay.collider != null && grounedRay.collider.gameObject.GetComponent<jumpObject>() != null
                                                && ceilingRay.collider == null)
                {
                    Debug.Log(grounedRay.collider.gameObject);
                    canSpawn = true;
                }
                else
                {
                    //seeds[i].transform.position = new Vector2(Random.Range(-10, 10), Random.Range(-5, 5));
                    canSpawn = false;
                }
            }
        }
    }

    public void spawnSeed()
    {
        //let one of the waiting bullets to be active
        if(seeds.Count < gameManager.Instance.seedMax)
        {
            int chance = Random.Range(0, 100);
            if (chance <= 20)
            {
                prefabToSpawn = prefabFrostSeed;
            }
            else if (21 <= chance && chance <= 40)
            {
                prefabToSpawn = prefabTrapSeed;
            }
            else
            {
                prefabToSpawn = prefabSeed;
            }

            GameObject newSeed = Instantiate(prefabToSpawn, new Vector2(Random.Range(-13, 13), Random.Range(-4, 22)), Quaternion.identity);
            seeds.Add(newSeed);
           
            bool canSpawn = false;

            while (!canSpawn)
            {
                newSeed.transform.position = new Vector2(Random.Range(-13, 13), Random.Range(-4, 22));
                RaycastHit2D grounedRay = Physics2D.Raycast(new Vector2(newSeed.transform.position.x, newSeed.transform.position.y - 0.25f), Vector2.down, 0.5f, 1 << 6);
                RaycastHit2D ceilingRay = Physics2D.Raycast(new Vector2(newSeed.transform.position.x, newSeed.transform.position.y + 0.5f), Vector2.up, 0.5f, 1 << 6);

                if (grounedRay.collider != null && grounedRay.collider.gameObject.GetComponent<jumpObject>() != null
                                                && ceilingRay.collider == null)
                {
                    Debug.Log(grounedRay.collider.gameObject);
                    canSpawn = true;
                }
                else
                {
                    canSpawn = false;
                }
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
            spawnSeed();
        }

        //Delete the dead enemies from the list
        for (int i = 0; i < seeds.Count; i++)
        {
            if (seeds[i] == null)
            {
                seeds.Remove(seeds[i]);
            }
        }

        //Let bullet "spawn" around the player's chest
        //transform.position = new Vector2(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y);
    }
}
