using UnityEngine;

public class CollisionPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.playerHealth -= 1;

            UIPlayerHealth uiScript = FindAnyObjectByType<UIPlayerHealth>();
            uiScript.UpdateHealthDisplay(GameManager.instance.playerHealth);
        }
    }
}
