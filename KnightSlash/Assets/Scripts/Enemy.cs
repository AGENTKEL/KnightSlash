using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public int damage = 10;

    private Transform currentTarget;
    private PlayerHealth playerHealth;
    private BallistaHealth ballistaHealth;
    private Animator animator;
    private float attackTimer = 0f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathClip;

    void Start()
    {
        FindTarget();
        audioSource = FindAnyObjectByType<EnemyAudio>().GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        attackTimer = 0f;
        FindTarget();
        StartCoroutine(FindNewObject());
    }

    void Update()
    {
        if (currentTarget == null)
        {
            FindTarget();
            return;
        }

        attackTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, currentTarget.position);

        if (distance > attackRange)
        {
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            direction.y = 0f;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.forward = direction;
        }
        else
        {
            if (attackTimer <= 0f)
            {
                animator.SetTrigger("Attack");
                DealDamage();
                attackTimer = attackCooldown;
            }
        }
    }
    
    void FindTarget()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        GameObject[] ballistas = GameObject.FindGameObjectsWithTag("Ballista_Bot");

        Transform nearest = null;
        float minDist = Mathf.Infinity;

        if (playerObj != null)
        {
            float dist = Vector3.Distance(transform.position, playerObj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = playerObj.transform;
                playerHealth = playerObj.GetComponent<PlayerHealth>();
                ballistaHealth = null;
            }
        }

        foreach (GameObject b in ballistas)
        {
            float dist = Vector3.Distance(transform.position, b.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = b.transform;
                ballistaHealth = b.GetComponent<BallistaHealth>();
                playerHealth = null;
            }
        }

        currentTarget = nearest;
    }

    private IEnumerator FindNewObject()
    {
        yield return new WaitForSeconds(2f);
        FindTarget();
        StartCoroutine(FindNewObject());
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void DealDamage()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        else if (ballistaHealth != null)
        {
            ballistaHealth.TakeDamage(damage);
        }
    }

    void Die()
    {
        audioSource.PlayOneShot(deathClip);
        GameObject coin = CoinPoolManager.Instance.GetCoin();
        coin.transform.position = transform.position + Vector3.up * 1f;
        coin.transform.rotation = Quaternion.identity;
        coin.SetActive(true);
        
        GameObject coin2 = CoinPoolManager.Instance.GetCoin();
        coin2.transform.position = transform.position + Vector3.up * 1f + Vector3.forward * 1f;
        coin2.transform.rotation = Quaternion.identity;
        coin2.SetActive(true);
        
        gameObject.SetActive(false);
    }
}
