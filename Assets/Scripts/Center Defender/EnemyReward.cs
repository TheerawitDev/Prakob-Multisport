using UnityEngine;
using DG.Tweening;

public class EnemyReward : MonoBehaviour
{
    public int scoreValue = 10;
    public GameObject deathEffect;

    [Header("Death Animation (DOTween)")]
    public Transform visualTransform;
    public float lieDownDuration = 0.5f;
    public float lieDownRotationZ = -90f;
    public float sinkDepth = 0.5f;

    public void GiveRewardAndDie()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        EnemyWalk2D walkScript = GetComponent<EnemyWalk2D>();
        if (walkScript != null) walkScript.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        DamageDealer2D damageDealer = GetComponent<DamageDealer2D>();
        if (damageDealer != null) damageDealer.enabled = false;

        Transform targetToAnimate = visualTransform != null ? visualTransform : transform;
        targetToAnimate.DOKill();

        Sequence deathSequence = DOTween.Sequence();

        deathSequence.Append(targetToAnimate.DOShakeRotation(0.2f, new Vector3(0, 0, 20), 10, 90));
        deathSequence.Join(targetToAnimate.DOShakeScale(0.2f, 0.2f, 10, 90));

        deathSequence.Append(targetToAnimate.DORotate(new Vector3(0, 0, lieDownRotationZ), lieDownDuration));
        deathSequence.Join(targetToAnimate.DOMoveY(transform.position.y - sinkDepth, lieDownDuration));

        SpriteRenderer sr = targetToAnimate.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            deathSequence.Join(sr.DOFade(0f, lieDownDuration));
        }

        deathSequence.AppendInterval(0.5f);
        deathSequence.OnComplete(() => Destroy(gameObject));
    }
}