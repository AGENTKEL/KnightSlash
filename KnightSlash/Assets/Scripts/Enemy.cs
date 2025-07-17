using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public int damage = 10;

    private Transform player;
    private Animator animator;
    private float attackTimer = 0f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathClip;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = FindAnyObjectByType<EnemyAudio>().GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        attackTimer = 0f;
    }

    void Update()
    {
        if (player == null) return;

        attackTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0f;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.forward = direction;
        }
        else
        {
            if (attackTimer <= 0f)
            {
                animator.SetTrigger("Attack");
                DealDamageToPlayer();
                attackTimer = attackCooldown;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void DealDamageToPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    void Die()
    {
        audioSource.PlayOneShot(deathClip);
        GameObject coin = CoinPoolManager.Instance.GetCoin();
        coin.transform.position = transform.position + Vector3.up * 1f;
        coin.transform.rotation = Quaternion.identity;
        coin.SetActive(true);
        
        gameObject.SetActive(false);
    }
}
