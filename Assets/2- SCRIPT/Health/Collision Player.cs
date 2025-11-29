using UnityEngine;
using System.Collections;

public class CollisionPlayer : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement = FindAnyObjectByType<PlayerMovement>();

            playerMovement.DanioProta();

        }
    }
}
