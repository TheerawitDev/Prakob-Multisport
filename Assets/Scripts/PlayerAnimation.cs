using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    public PlayerController2D playerController;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerController == null) return;

        animator.SetFloat("Speed", Mathf.Abs(playerController.Velocity.x));
        animator.SetFloat("VerticalVelocity", playerController.Velocity.y);
        animator.SetBool("IsGrounded", playerController.IsGrounded);
        animator.SetBool("IsCrouching", playerController.IsCrouching);
    }
}