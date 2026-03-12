using UnityEngine;
using System.Collections;

public class DualCombatController : MonoBehaviour
{
    public GameObject leftHitbox;
    public GameObject rightHitbox;
    public float attackDuration = 0.1f;
    public Transform visualTransform;

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
        if (visualTransform != null)
            visualTransform.localScale = new Vector3(-Mathf.Abs(visualTransform.localScale.x), visualTransform.localScale.y, visualTransform.localScale.z);

        StartCoroutine(AttackRoutine(leftHitbox));
    }

    private void AttackRight()
    {
        if (visualTransform != null)
            visualTransform.localScale = new Vector3(Mathf.Abs(visualTransform.localScale.x), visualTransform.localScale.y, visualTransform.localScale.z);

        StartCoroutine(AttackRoutine(rightHitbox));
    }

    private IEnumerator AttackRoutine(GameObject hitbox)
    {
        if (hitbox == null) yield break;
        hitbox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        hitbox.SetActive(false);
    }
}