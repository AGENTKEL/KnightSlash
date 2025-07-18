using System.Collections;
using UnityEngine;

public class BallistaHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    
    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {

        animator.SetBool("Dead", true);
        StartCoroutine(DeleteObject());
    }

    private IEnumerator DeleteObject()
    {
        yield return new WaitForSeconds(3f);
        
        Destroy(gameObject);
    }
}
