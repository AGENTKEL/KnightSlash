using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class GameUI : MonoBehaviour
{
    [Header("HUD UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject joystick;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private TextMeshProUGUI finalCoinText;
    [SerializeField] private TextMeshProUGUI finalWaveText;
    [SerializeField] private TextMeshProUGUI finalCoinText2;
    [SerializeField] private TextMeshProUGUI finalWaveText2;
    
    public Slider musicSlider;

    public PlayerHealth playerHealth;
    public string respawnID;

    private float elapsedTime = 0f;

    private void Start()
    {
        Time.timeScale = 1f;
        MusicManager.Instance.LoadVolume();

        if (YG2.envir.isDesktop)
        {
            joystick.SetActive(false);
        }
        
        if (YG2.envir.isMobile)
        {
            joystick.SetActive(true);
        }
        
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSlider.value = savedVolume;
        musicSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void Update()
    {
        // Обновляем UI только если не завершена игра
        if (gameOverPanel.activeSelf) return;

        elapsedTime += Time.deltaTime;
        int seconds = Mathf.FloorToInt(elapsedTime);
        timerText.text = $"{seconds}";

        coinText.text = $"{GameManager.instance.GetCoinCount()}";
        waveText.text = $"{EnemyWaveSpawner.Instance.waveCount}";
    }

    public void ShowGameOverUI()
    {
        gameOverPanel.SetActive(true);
        finalCoinText.text = $"{700}";
        finalWaveText.text = $"{EnemyWaveSpawner.Instance.waveCount}";
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        GameManager.instance.coins = 0;
        SceneManager.LoadScene("Menu");
    }
    
    public void GoToMainMenuLose()
    {
        Time.timeScale = 1f;
        YG2.saves.coins += 700;
        YG2.SaveProgress();
        GameManager.instance.coins = 0;
        SceneManager.LoadScene("Menu");
    }
    
    public void GoToMainMenuWin()
    {
        Time.timeScale = 1f;
        YG2.saves.coins += 1500;
        YG2.SaveProgress();
        GameManager.instance.coins = 0;
        SceneManager.LoadScene("Menu");
    }
    
    public void NewGame()
    {
        Time.timeScale = 1f;
        YG2.saves.coins += 700;
        GameManager.instance.coins = 0;
        SceneManager.LoadScene("Main");
    }
    
    public void NewGameWin()
    {
        Time.timeScale = 1f;
        YG2.saves.coins += 1500;
        GameManager.instance.coins = 0;
        SceneManager.LoadScene("Main");
    }
    
    public void Pause()
    {
        Time.timeScale = 0f;
    }
    
    public void Unpause()
    {
        Time.timeScale = 1f;
    }
    
    public void ChangeLanguage(string language)
    {
        YG2.SwitchLanguage(language);
    }
    
    private void OnEnable()
    {
        YG2.onRewardAdv += OnReward;
    }
    
    private void OnDisable()
    {
        YG2.onRewardAdv -= OnReward;
    }
    
    public void MyRewardAdvShow(string id)
    {
        YG2.RewardedAdvShow(id);
    }
    
    private void OnReward(string id)
    {
        if (id == respawnID)
        {
            Time.timeScale = 1f;
            playerHealth.Respawn();
            gameOverPanel.SetActive(false);
        }
    }
    
    void OnVolumeChanged(float volume)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(volume);
        }
    }

    public void GameWin()
    {
        gameWinPanel.SetActive(true);
        finalCoinText2.text = $"{"1500"}";
        finalWaveText2.text = $"{EnemyWaveSpawner.Instance.waveCount}";
        Time.timeScale = 0f;
    }
}
