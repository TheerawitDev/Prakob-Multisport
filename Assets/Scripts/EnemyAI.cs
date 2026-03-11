using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public string targetTag = "Player";
    public float moveSpeed = 3f;
    private Transform target;

    private void Start()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            transform.LookAt(target.position);
        }
    }
}