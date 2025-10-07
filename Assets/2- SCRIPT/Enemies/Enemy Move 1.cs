using UnityEngine;

public class EnemyMove1 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float minStopY = -2f; 
    [SerializeField] private float maxStopY = 2f;  

    private float targetY; 
    private bool hasReachedTarget = false;

    private void OnEnable()
    {
        targetY = Random.Range(minStopY, maxStopY);
        hasReachedTarget = false;
    }

    private void Update()
    {
        if (!hasReachedTarget)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);

            if (transform.position.y <= targetY)
            {
                transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
                hasReachedTarget = true;
            }
        }
    }

    public bool HasReachedTarget()
    {
        return hasReachedTarget;
    }
}
