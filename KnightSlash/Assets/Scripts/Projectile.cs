using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    private Vector3 direction;

    [Header("Bomb Settings")]
    public bool isBomb = false;
    public float explosionRadius = 3f;
    public AudioClip bombSound;
    private AudioSource audioSource;

    public GameObject bombParticle;
    
    [Header("Ballista Arrow")]
    public bool isBallistaArrow = false;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        if (isBomb)
        {
            audioSource = FindAnyObjectByType<SoundManager>().GetComponent<AudioSource>();
        }
    }

    public void Init(Vector3 shootDirection)
    {
        direction = shootDirection.normalized;
        gameObject.SetActive(true);
        CancelInvoke(nameof(ReturnToPool));
        Invoke(nameof(ReturnToPool), 5f);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isBomb || isBallistaArrow)
        {
            Instantiate(bombParticle, transform.position, Quaternion.identity);
            Explode();
        }
        else
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        ReturnToPool();
    }

    void Explode()
    {
        if (isBallistaArrow)
        {
            Collider[] hitColliderss = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider hit in hitColliderss)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            // Визуальные/звуковые эффекты взрыва можно здесь вызвать
            audioSource.PlayOneShot(bombSound);
            Destroy(gameObject);
            return;
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hitColliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        // Визуальные/звуковые эффекты взрыва можно здесь вызвать
        audioSource.PlayOneShot(bombSound);
    }

    void ReturnToPool()
    {
        ProjectilePoolManager.Instance.ReturnProjectile(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (isBomb)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
