using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MenuUI : MonoBehaviour
{
    public Slider musicSlider;
    public TextMeshProUGUI coinCount;
    
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSlider.value = savedVolume;
        musicSlider.onValueChanged.AddListener(OnVolumeChanged);
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        coinCount.text = YG2.saves.coins.ToString();
    }
    
    
    void OnVolumeChanged(float volume)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(volume);
        }
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void ChangeLanguage(string language)
    {
        YG2.SwitchLanguage(language);
    }
    
}
