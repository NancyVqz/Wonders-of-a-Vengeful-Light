using UnityEngine;

public class Energy : MonoBehaviour
{
    private EnergyDrop dropEnergyScript;

    private void Start()
    {
        dropEnergyScript = FindAnyObjectByType<EnergyDrop>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.energy += 1;
            dropEnergyScript.ReturnDrop(gameObject);
        }
    }
}
