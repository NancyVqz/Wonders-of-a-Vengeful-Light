using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Se daño al jugador");
            //le quita vida
        }
    }
}
