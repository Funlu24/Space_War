using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; // <-- BU KÜTÜPHANEYİ EKLEDİK (TextMeshPro için şart)

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Enemy Spawning")]
    public GameObject[] EnemyPrefabs;
    public float minInstantiateValue;
    public float maxInstantiateValue;
    public float enemyDestroyTime = 10f;

    [Header("Effects")]
    public GameObject ParticleEffect;
    public GameObject MuzzleFlashEffect;

    [Header("UI & Panels")]
    public GameObject StartMenu;
    public GameObject PausePanel;
    
    // --- BURAYI DEĞİŞTİRDİK ---
    // Artık "Text" değil "TextMeshProUGUI" istiyoruz.
    public TextMeshProUGUI scoreText;       
    public TextMeshProUGUI bestScoreStart;  
    public TextMeshProUGUI bestScorePause;  

    // SKOR DEĞİŞKENLERİ
    int score = 0;
    int highScore = 0;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();

        StartMenu.SetActive(true);
        PausePanel.SetActive(false);
        Time.timeScale = 0f;
        
        InvokeRepeating("InstantiateEnemy", 1f, 1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausePanel.activeSelf) PauseGameButton(false);
            else PauseGameButton(true);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateHighScoreText();
        }
    }

    void UpdateHighScoreText()
    {
        if(bestScoreStart != null) bestScoreStart.text = "Best Score: " + highScore;
        if(bestScorePause != null) bestScorePause.text = "Best: " + highScore;
    }

    void InstantiateEnemy()
    {
        Vector3 Enemypos = new Vector3(Random.Range(minInstantiateValue, maxInstantiateValue), 6f);
        int randomIndex = Random.Range(0, EnemyPrefabs.Length); 
        GameObject enemy = Instantiate(EnemyPrefabs[randomIndex], Enemypos, Quaternion.Euler(0, 0, 180f));
        Destroy(enemy, enemyDestroyTime);
    }

    public void StartGameButton()
    {
        score = 0; 
        scoreText.text = "Score: 0";
        
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

    public void GameOver()
    {
        StartMenu.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Oyun Bitti.");
    }

   public void QuitGame()
    {
        Debug.Log("Oyundan çıkış yapıldı!"); // Konsolda çalıştığını görmek için

        // Gerçek oyunda (Build alındığında) uygulamayı kapatır
        Application.Quit();

        // Unity Editöründe test ederken "Play" modunu durdurur
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}