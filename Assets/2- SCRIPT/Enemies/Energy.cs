using UnityEngine;

public class Energy : MonoBehaviour
{
    private EnergyDrop dropEnergyScript;
    private EnergyVfx energyVfx;

    private void Start()
    {
        dropEnergyScript = FindAnyObjectByType<EnergyDrop>();
        energyVfx = FindAnyObjectByType<EnergyVfx>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            energyVfx.EnergyParticles();
            AudioManager.instance.Play("energy");
            GameManager.instance.energy += 1;
            dropEnergyScript.ReturnDrop(gameObject);
        }
    }
}
