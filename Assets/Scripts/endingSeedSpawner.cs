using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingSeedSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn = null;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < scoreManager.Instance.seedEaten; i++)
        {
            Instantiate(prefabToSpawn, new Vector2(Random.Range(1, 7), 5), Quaternion.Euler(0, 0, Random.Range(-180, 180)));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
