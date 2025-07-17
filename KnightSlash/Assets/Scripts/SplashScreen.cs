using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class SplashScreen : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject sounds;
    public GameObject splashScreen;
    public Image splashImage;          // Assign in inspector
    public float scaleDuration = 3f;   // Duration of scaling
    public AudioSource audioSource;
    
    private void Start()
    {
        if (GameManager.instance.firstGameStart)
        {
            mainMenu.SetActive(false);
            sounds.SetActive(false);
            // Set initial scale
            splashImage.rectTransform.localScale = Vector3.one * 0.7f;
        
            audioSource.Play();

            // Start scale animation
            StartCoroutine(ScaleSplashImage());
            GameManager.instance.firstGameStart = false;
        }
        else
        {
            splashScreen.SetActive(false);
        }
    }
    
    private IEnumerator ScaleSplashImage()
    {
        float elapsed = 0f;
        Vector3 startScale = Vector3.one * 0.7f;
        Vector3 targetScale = Vector3.one * 0.5f;

        while (elapsed < scaleDuration)
        {
            float t = elapsed / scaleDuration;
            splashImage.rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        splashImage.rectTransform.localScale = targetScale;
        StartGame();
        YG2.GameReadyAPI();
        if (!YG2.saves.firstGame)
        {
            YG2.saves.firstGame = true;
            YG2.SaveProgress();
            SceneManager.LoadScene("Main");
        }
    }

    private void StartGame()
    {
        mainMenu.SetActive(true);
        sounds.SetActive(true);
        splashScreen.SetActive(false);
        GameManager.instance.firstGameStart = false;
    }
}
