using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Range(0f, 1f)] public float musicVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolume();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();

        ApplyVolumeToAllMusic();
    }

    public void LoadVolume()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        ApplyVolumeToAllMusic();
    }

    public void ApplyVolumeToAllMusic()
    {
        foreach (AudioSource music in FindObjectsOfType<AudioSource>())
        {
            if (music.CompareTag("Music"))
            {
                music.volume = musicVolume;
            }
        }
    }
}
