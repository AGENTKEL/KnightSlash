using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Joystick joystick;
    private Rigidbody rb;
    private Animator animator;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] private bool isMobile;

    [Header("Player Clones")]
    [SerializeField] private List<GameObject> playerModels; // список всех моделей
    private int activePlayerCount = 0;

    public AudioSource playerAudioSource;
    public AudioClip spawnClip;
    public AudioClip multiplyClip;
    public AudioClip deathClip;
    public AudioClip ballistaClip;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponentInChildren<Animator>();
        PlaySpawnSound();
        SetActivePlayers(activePlayerCount); // Активировать только одну модель в начале
    }

    void FixedUpdate()
    {
        if (playerHealth.isDead) return;

        Vector3 inputDirection;

        if (YG2.envir.isMobile)
        {
            inputDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        }
        else
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            inputDirection = new Vector3(h, 0, v);
        }

        Vector3 move = inputDirection.normalized * speed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        bool isWalking = inputDirection.sqrMagnitude > 0.01f;
        animator.SetBool("isWalking", isWalking);
        animator.SetFloat("InputY", Mathf.Clamp(inputDirection.z, -1f, 1f));
        animator.SetFloat("InputX", Mathf.Clamp(inputDirection.x, -1f, 1f));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Multy"))
        {

            if (GameManager.instance.coins >= 200 && activePlayerCount < playerModels.Count)
            {
                GameManager.instance.SpendCoinsGameplay(200);
                activePlayerCount++;
                playerAudioSource.PlayOneShot(multiplyClip);
                SetActivePlayers(activePlayerCount);
                other.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Not enough coins or max players reached.");
            }
        }
        
        if (other.CompareTag("Ballista"))
        {

            if (GameManager.instance.coins >= 300)
            {
                GameManager.instance.SpendCoinsGameplay(300);
                playerAudioSource.PlayOneShot(ballistaClip);
                other.GetComponent<Multiply>().BuyBallista();
            }
            else
            {
                Debug.Log("Not enough coins for ballista.");
            }
        }
    }

    private void SetActivePlayers(int count)
    {
        for (int i = 0; i < playerModels.Count; i++)
        {
            playerModels[i].SetActive(i < count);
        }
    }

    public void DeductActivePlayers()
    {
        activePlayerCount--;
        SetActivePlayers(activePlayerCount);
    }
    
    public void PlaySpawnSound()
    {
        playerAudioSource.PlayOneShot(spawnClip);
    }
    
    public void PlayDeathSound()
    {
        playerAudioSource.PlayOneShot(deathClip);
    }
}
