using UnityEngine;
using YG;

public class PlayerAutoattack : MonoBehaviour
{
    public float attackRange = 10f;
    public float baseCooldown = 0.5f;
    public Transform firePoint;
    public float turnSpeed = 10f;

    [Header("Audio")]
    public AudioClip shootClip;
    [SerializeField] private AudioSource audioSource;

    [Header("Gun Attack Settings")]
    public AudioClip gunShootClip;
    [SerializeField] private AudioSource audioSourceGun;
    public float gunCooldown = 0.25f;
    
    [Header("Sword Attack Settings")]
    public float swordRange = 2f;
    public float swordCooldown = 0.6f;
    public int swordDamage = 15;
    [SerializeField] private Animator animator;
    
    [Header("Bomb Attack Settings")]
    public float bombCooldown = 2f;
    public AudioClip bombShootClip;
    [SerializeField] private AudioSource audioSourceBomb;

    private float baseAttackTimer = 0f;
    private float gunAttackTimer = 0f;
    private float swordAttackTimer = 0f;
    private float bombAttackTimer = 0f;
    private Enemy currentTarget;

    [SerializeField] private PlayerHealth playerHealth;

    public bool isClone = false;
    

    void Update()
    {
        if (playerHealth != null)
            if (playerHealth.isDead) return;

        baseAttackTimer -= Time.deltaTime;
        gunAttackTimer -= Time.deltaTime;
        swordAttackTimer -= Time.deltaTime;
        bombAttackTimer -= Time.deltaTime;

        currentTarget = FindNearestEnemy();

        if (currentTarget != null)
        {
            LookAtTarget(currentTarget.transform);

            if (YG2.saves.hasSword && swordAttackTimer <= 0f && Vector3.Distance(transform.position, currentTarget.transform.position) <= swordRange)
            {
                MeleeAttack(currentTarget);
                swordAttackTimer = swordCooldown;
                return; // при активном мече, не стреляем
            }

            if (baseAttackTimer <= 0f)
            {
                ShootProjectileNormal();
                baseAttackTimer = baseCooldown;
            }

            if (YG.YG2.saves.hasGun && gunAttackTimer <= 0f)
            {
                ShootProjectileGun();
                gunAttackTimer = gunCooldown;
            }
            
            if (YG.YG2.saves.hasBomb && bombAttackTimer <= 0f)
            {
                ShootProjectileBomb();
                bombAttackTimer = bombCooldown;
            }
        }
    }

    void LookAtTarget(Transform target)
    {
        if (isClone) return;
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }
    }

    Enemy FindNearestEnemy()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        Enemy nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist && dist <= attackRange)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    void ShootProjectileNormal()
    {
        if (firePoint == null || currentTarget == null) return;

        Vector3 targetPosition = currentTarget.transform.position + Vector3.up;
        Vector3 direction = (targetPosition - firePoint.position).normalized;

        Quaternion rotation = GetProjectileRotation(direction);

        GameObject projectile = ProjectilePoolManager.Instance.GetNormalProjectile();
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = rotation;
        projectile.SetActive(true);

        Projectile p = projectile.GetComponent<Projectile>();
        if (p != null)
        {
            p.Init(direction);
        }

        if (shootClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootClip);
        }
    }

    void ShootProjectileGun()
    {
        if (firePoint == null || currentTarget == null) return;

        Vector3 targetPosition = currentTarget.transform.position + Vector3.up;
        Vector3 direction = (targetPosition - firePoint.position).normalized;

        Quaternion rotation = GetProjectileRotation(direction);

        GameObject projectile = ProjectilePoolManager.Instance.GetGunProjectile();
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = rotation;
        projectile.SetActive(true);

        Projectile p = projectile.GetComponent<Projectile>();
        if (p != null)
        {
            p.Init(direction);
        }

        if (gunShootClip != null && audioSourceGun != null)
        {
            audioSourceGun.PlayOneShot(gunShootClip);
        }
    }
    
    void ShootProjectileBomb()
    {
        if (firePoint == null || currentTarget == null) return;

        Vector3 targetPosition = currentTarget.transform.position + Vector3.up;
        Vector3 direction = (targetPosition - firePoint.position).normalized;

        Quaternion rotation = GetProjectileRotation(direction);

        GameObject bomb = ProjectilePoolManager.Instance.GetBombProjectile();
        bomb.transform.position = firePoint.position;
        bomb.transform.rotation = rotation;
        bomb.SetActive(true);

        Projectile bombProjectile = bomb.GetComponent<Projectile>();
        if (bombProjectile != null)
        {
            bombProjectile.Init(direction);
        }

        if (bombShootClip != null && audioSourceBomb != null)
        {
            audioSourceBomb.PlayOneShot(bombShootClip);
        }
    }

    Quaternion GetProjectileRotation(Vector3 direction)
    {
        if (direction == Vector3.zero) direction = transform.forward;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 euler = lookRotation.eulerAngles;
        euler.x = 90f;
        euler.z = 0f;
        return Quaternion.Euler(euler);
    }
    
    void MeleeAttack(Enemy enemy)
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (enemy != null)
        {
            enemy.TakeDamage(swordDamage);
        }
    }
}
