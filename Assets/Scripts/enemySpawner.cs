using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    //Bullet stats
    [SerializeField]
    private GameObject prefabToSpawn = null;
    [SerializeField]
    private GameObject prefabMeleeEnemy = null;
    [SerializeField]
    private GameObject prefabRangeEnemy = null;

    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> Enemies
    {
        get
        {
            return enemies;
        }
    }

    [SerializeField]
    private List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> SpawnPoints
    {
        get
        {
            return spawnPoints;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        spawnWave();
    }

    public void spawnWave()
    {
        //Spawn 3 seeds in the space
        for (int w = 0; w < gameManager.Instance.spawnWave; w++)
        {
            int spawnWhere = Random.Range(0, 6);
            GameObject spawnPoint = spawnPoints[spawnWhere];

            //Spawn 3 seeds in the space
            for (int i = 0; i < timeManager.Instance.spawnSize; i++)
            {
                int chance = Random.Range(0, 100);
                if (chance <= 20)
                {
                    prefabToSpawn = prefabRangeEnemy;
                }
                else
                {
                    prefabToSpawn = prefabMeleeEnemy;
                }

                GameObject newEnemy = Instantiate(prefabToSpawn, new Vector2(Random.Range(spawnPoint.transform.position.x - 1, spawnPoint.transform.position.x + 1),
                                                                 spawnPoint.transform.position.y),Quaternion.identity);
                enemies.Add(newEnemy);
                //seeds[i].SetActive(false);
            }
        }
    }

    public void killAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].GetComponent<meleeEnemyMovement>() != null)
            {
                scoreManager.Instance.meleeEnemyKills += 1;
            }
            /*if (enemies[i].GetComponent<rangeEnemyMovement>() != null)
            {
                scoreManager.Instance.meleeEnemyKills += 1;
            }*/

            Destroy(enemies[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Delete the dead enemies from the list
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.Remove(enemies[i]);
            }
        }
    }
}
