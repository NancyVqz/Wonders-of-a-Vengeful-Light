using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrop : MonoBehaviour
{
    [Header("Drop Settings")]
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private int maxDropsInPool = 10;
    [SerializeField] private float dropLifetime = 2f;

    private Queue<GameObject> dropQueue;
    private List<GameObject> activeDrops;

    private void Start()
    {
        InitializeDropPool();
    }

    private void InitializeDropPool()
    {
        dropQueue = new Queue<GameObject>();
        activeDrops = new List<GameObject>();

        for (int i = 0; i < maxDropsInPool; i++)
        {
            GameObject drop = Instantiate(dropPrefab);
            drop.SetActive(false);
            dropQueue.Enqueue(drop);
        }
    }

    public void SpawnDrop(Vector3 position)
    {
        if (dropQueue.Count == 0)
        {
            Debug.LogWarning("No hay drops disponibles en el pool!");
            return;
        }

        GameObject drop = dropQueue.Dequeue();
        drop.SetActive(true);
        drop.transform.position = position;
        drop.transform.rotation = Quaternion.identity;

        activeDrops.Add(drop);

        StartCoroutine(ReturnDropToPool(drop, dropLifetime));
    }

    private IEnumerator ReturnDropToPool(GameObject drop, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (drop.activeInHierarchy)
        {
            ReturnDrop(drop);
        }
    }

    public void ReturnDrop(GameObject drop)
    {
        if (drop != null && drop.activeInHierarchy)
        {
            drop.SetActive(false);
            dropQueue.Enqueue(drop);
            activeDrops.Remove(drop);
        }
    }

    public void ClearAllDrops()
    {
        for (int i = activeDrops.Count - 1; i >= 0; i--)
        {
            ReturnDrop(activeDrops[i]);
        }
    }

    private void OnDisable()
    {
        ClearAllDrops();
    }
}
