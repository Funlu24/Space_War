using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // [GEREKSİNİM 8] Arrays: Tek bir prefab yerine dizi kullanıyoruz.
    public GameObject[] EnemyPrefabs; 

    public static GameManager instance;
    public float minInstantiateValue;
    public float maxInstantiateValue;
    public float enemyDestroyTime = 10f;

    [Header("Particle Effects")]
    public GameObject ParticleEffect;
    public GameObject MuzzleFlashEffect;

    [Header("Panels")]
    public GameObject StartMenu;
    public GameObject PausePanel;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        StartMenu.SetActive(true);
        PausePanel.SetActive(false);
        Time.timeScale = 0f;
        InvokeRepeating("InstantiateEnemy", 1f, 1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGameButton(true);
        }
    }

    void InstantiateEnemy()
    {
        Vector3 Enemypos = new Vector3(Random.Range(minInstantiateValue, maxInstantiateValue), 6f);
        
        // [GEREKSİNİM 8] Arrays: Diziden rastgele bir düşman seçimi
        int randomIndex = Random.Range(0, EnemyPrefabs.Length); 
        GameObject enemy = Instantiate(EnemyPrefabs[randomIndex], Enemypos, Quaternion.Euler(0, 0, 180f));
        
        Destroy(enemy, enemyDestroyTime);
    }

    public void StartGameButton()
    {
        StartMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseGameButton(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
            PausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            PausePanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}