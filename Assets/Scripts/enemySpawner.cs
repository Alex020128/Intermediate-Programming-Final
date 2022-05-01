using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    //Enemy and enemy spawnpoint stats
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

    Coroutine killCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn the first wave of enemies
        spawnWave();
    }

    public void spawnWave()
    {
        //Spawn waves of enemies in the space
        for (int w = 0; w < gameManager.Instance.spawnWave; w++)
        {
            int spawnWhere = Random.Range(0, spawnPoints.Count);
            GameObject spawnPoint = spawnPoints[spawnWhere];

            //80% of spawning melee enemy; 20% of spawning range enemy
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

                //Make sure the wave spawns on enemy spawnpoints
                GameObject newEnemy = Instantiate(prefabToSpawn, new Vector2(Random.Range(spawnPoint.transform.position.x - 1, spawnPoint.transform.position.x + 1),
                                                                 spawnPoint.transform.position.y),Quaternion.identity);
                enemies.Add(newEnemy);
            }
        }
    }

    public void killAllEnemies()
    {
        //Emit particles, record death and start coroutine when the level is cleared
        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].GetComponent<meleeEnemyMovement>() != null)
            {
                enemies[i].GetComponent<meleeEnemyMovement>().deathParticle.Emit(10);
                enemies[i].gameObject.GetComponent<meleeEnemyMovement>().deathSFX();
                scoreManager.Instance.meleeEnemyKills += 1;
            }
            if (enemies[i].GetComponent<rangeEnemyMovement>() != null)
            {
                enemies[i].GetComponent<rangeEnemyMovement>().deathParticle.Emit(10);
                enemies[i].gameObject.GetComponent<rangeEnemyMovement>().deathSFX();
                scoreManager.Instance.rangeEnemyKills += 1;
            }
            killCoroutine = StartCoroutine(killAll(1f));
        }
    }

    private IEnumerator killAll(float wait)
    {
        //Destroy all the active enemies
        yield return new WaitForSeconds(wait);
        for (int i = 0; i < enemies.Count; i++)
        {
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
