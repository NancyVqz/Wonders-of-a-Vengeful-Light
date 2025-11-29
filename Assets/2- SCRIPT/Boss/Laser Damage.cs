using UnityEngine;
using System.Collections;

public class LaserDamage : MonoBehaviour
{
    public int damage = 1;
    private PlayerMovement playerMovement;


    void OnTriggerEnter2D(Collider2D other)
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();

        if (other.CompareTag("Player"))
        {
            playerMovement.DanioProta();

        }
    }
}
