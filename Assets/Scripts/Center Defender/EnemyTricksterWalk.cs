using UnityEngine;
using System.Collections;

public class EnemyTricksterWalk : MonoBehaviour
{
    public float baseSpeed = 2f;
    public float dashSpeed = 10f;
    public float stopDistance = 3f;
    public float pauseDuration = 0.5f;

    private Transform target;
    private bool isDashing = false;
    private bool hasPaused = false;
    private bool isPaused = false;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    private void Update()
    {
        if (target == null || isPaused) return;

        float distanceToTarget = Mathf.Abs(transform.position.x - target.position.x);

        if (!hasPaused && distanceToTarget <= stopDistance)
        {
            StartCoroutine(PauseAndDash());
        }
        else
        {
            float currentSpeed = isDashing ? dashSpeed : baseSpeed;
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        }
    }

    private IEnumerator PauseAndDash()
    {
        hasPaused = true;
        isPaused = true;
        yield return new WaitForSeconds(pauseDuration);
        isPaused = false;
        isDashing = true;
    }
}