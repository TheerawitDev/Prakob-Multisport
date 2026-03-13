using UnityEngine;
using System.Collections;

public class DualCombatController : MonoBehaviour
{
    public GameObject leftHitbox;
    public GameObject rightHitbox;
    public ParticleSystem leftParticle;
    public ParticleSystem rightParticle;
    public float attackDuration = 0.1f;
    public float attackCooldown = 0.2f;
    public Transform visualTransform;

    private bool canAttack = true;

    private void Start()
    {
        if (GameInputManager.Instance == null) return;
        GameInputManager.Instance.OnLeftPressed += AttackLeft;
        GameInputManager.Instance.OnRightPressed += AttackRight;

        if (leftHitbox != null) leftHitbox.SetActive(false);
        if (rightHitbox != null) rightHitbox.SetActive(false);
    }

    private void OnDestroy()
    {
        if (GameInputManager.Instance == null) return;
        GameInputManager.Instance.OnLeftPressed -= AttackLeft;
        GameInputManager.Instance.OnRightPressed -= AttackRight;
    }

    private void AttackLeft()
    {
        if (!canAttack) return;

        if (visualTransform != null)
            visualTransform.localScale = new Vector3(-Mathf.Abs(visualTransform.localScale.x), visualTransform.localScale.y, visualTransform.localScale.z);

        if (leftParticle != null)
        {
            leftParticle.Stop();
            leftParticle.Play();
        }

        StartCoroutine(AttackRoutine(leftHitbox));
    }

    private void AttackRight()
    {
        if (!canAttack) return;

        if (visualTransform != null)
            visualTransform.localScale = new Vector3(Mathf.Abs(visualTransform.localScale.x), visualTransform.localScale.y, visualTransform.localScale.z);

        if (rightParticle != null)
        {
            rightParticle.Stop();
            rightParticle.Play();
        }

        StartCoroutine(AttackRoutine(rightHitbox));
    }

    private IEnumerator AttackRoutine(GameObject hitbox)
    {
        canAttack = false;

        if (hitbox != null) hitbox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        if (hitbox != null) hitbox.SetActive(false);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}