using UnityEngine;
using System.Collections;

public class EnergyVfx : MonoBehaviour
{
    [SerializeField] private GameObject energyVfx;

    public void EnergyParticles()
    {
        StartCoroutine(VfxParticles());
    }

    private IEnumerator VfxParticles()
    {
        energyVfx.SetActive(true);

        yield return new WaitForSeconds(2);

        energyVfx.SetActive(false);
    }
}
