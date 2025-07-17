using System.Collections.Generic;
using UnityEngine;
using YG;

public class CloneMovement : MonoBehaviour
{
    public float speed = 5f;
    public Joystick joystick;
    private Animator animator;
    [SerializeField] PlayerHealth playerHealth;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        if (playerHealth != null)
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

        bool isWalking = inputDirection.sqrMagnitude > 0.01f;
        animator.SetBool("isWalking", isWalking);
        animator.SetFloat("InputY", Mathf.Clamp(inputDirection.z, -1f, 1f));
        animator.SetFloat("InputX", Mathf.Clamp(inputDirection.x, -1f, 1f));
    }
}
