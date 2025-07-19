using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    [SerializeField] private Image healthFillImage;

    [SerializeField] private GameUI gameUI;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioClip damageClip; // Звук урона
    [SerializeField] private AudioClip deathClip;  // Звук смерти (опционально)
    [SerializeField] private PlayerMovement playerMovement;
    

    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
        //playerMovement.DeductActivePlayers();

        if (audioSource != null && damageClip != null)
        {
            audioSource.PlayOneShot(damageClip);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void UpdateHealthUI()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetBool("Dead", true);

        gameUI.ShowGameOverUI();

        audioSource.PlayOneShot(deathClip);
        playerMovement.PlayDeathSound();
        music.Stop();
        Time.timeScale = 0f;
    }

    public void Respawn()
    {
        currentHealth = maxHealth;
        isDead = false;
        music.Play();
        UpdateHealthUI();
        animator.SetBool("Dead", false);
        playerMovement.PlaySpawnSound();
    }
}
