using System;
using System.Collections;
using UnityEngine;

public class Ballista : MonoBehaviour
{
    public float detectionRange = 15f;
    public float fireRate = 2f; // Время между выстрелами
    public float rotationSpeed = 5f;

    [Header("References")]
    public Transform firePoint;
    public GameObject arrowPrefab;
    public AudioClip shootClip;
    public AudioSource audioSource;
    public Animator animator;

    private float fireCooldown = 0f;
    private Enemy currentTarget;
    public float shootDelay = 0.3f;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 180f, 0);
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        currentTarget = FindNearestEnemy();

        if (currentTarget != null)
        {
            RotateTowardsTarget(currentTarget.transform);

            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (distance <= detectionRange && fireCooldown <= 0f)
            {
                AttackTarget();
                fireCooldown = fireRate;
            }
        }
        else
        {
            SetMoving(false);
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
            if (dist < minDist && dist <= detectionRange)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        if (direction.magnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            // Отзеркаливание на 180 градусов по Y:
            lookRotation *= Quaternion.Euler(0, 180f, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            SetMoving(true);
        }
        else
        {
            SetMoving(false);
        }
    }

    void Shoot()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        if (audioSource != null && shootClip != null)
            audioSource.PlayOneShot(shootClip);

        if (arrowPrefab != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            Projectile projectile = arrow.GetComponent<Projectile>();
            if (projectile != null && currentTarget != null)
            {
                Vector3 dir = (currentTarget.transform.position + Vector3.up) - firePoint.position;
                projectile.Init(dir.normalized);
            }
        }
    }
    
    void AttackTarget()
    {
        animator.SetTrigger("Attack");
        SetMoving(false); // отключаем анимацию движения при атаке

        // Запускаем корутину выстрела с задержкой
        StartCoroutine(DelayedShoot());
    }
    
    IEnumerator DelayedShoot()
    {
        yield return new WaitForSeconds(shootDelay); // задержка перед выстрелом

        if (arrowPrefab != null && firePoint != null && currentTarget != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            Projectile proj = arrow.GetComponent<Projectile>();
            if (proj != null)
            {
                Vector3 direction = (currentTarget.transform.position - firePoint.position).normalized;
                proj.Init(direction); // передаём направление полёта
            }

            audioSource.PlayOneShot(shootClip);
        }
    }

    void SetMoving(bool isMoving)
    {
        if (animator != null)
            animator.SetBool("Moving", isMoving);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
