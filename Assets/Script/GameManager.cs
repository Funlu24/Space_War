using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject EnemyPrefab;
    public float minInstantiateValue;
    public float maxInstantiateValue;

    public float enemyDestroyTime = 10f;
    void Start()
    {
        InvokeRepeating("InstantiateEnemy", 1f, 1f);
    }

    // Update is called once per frame
    void InstantiateEnemy()
    {
        Vector3 Enemypos = new Vector3(Random.Range(minInstantiateValue, maxInstantiateValue), 6f);
        GameObject enemy = Instantiate(EnemyPrefab, Enemypos, Quaternion.Euler(0, 0, 180f));
        Destroy(enemy, enemyDestroyTime);
    }
}
