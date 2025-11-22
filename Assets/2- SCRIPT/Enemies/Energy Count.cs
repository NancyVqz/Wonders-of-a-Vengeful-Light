using TMPro;
using UnityEngine;

public class EnergyCount : MonoBehaviour
{
    private TextMeshProUGUI contador;

    private void Start()
    {
        contador = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        contador.text = GameManager.instance.energy.ToString("000");
    }
}
