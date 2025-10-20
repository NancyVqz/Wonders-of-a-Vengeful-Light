using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform[] shootPoints;
    [SerializeField] private Transform player;
    [SerializeField] private float delayBetweenShots = 0.2f;
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite explosive;

    [Header("Random Shot Settings")]
    [SerializeField] private int bulletsPerRandomAttack = 5;

    [Header("Direct Shot Settings")]
    [SerializeField] private Transform directShootPoint;

    public void StartRandomBulletAttack()
    {
        StartCoroutine(RandomBulletAttackCoroutine());
    }

    public void ShootAtPlayerWithDisappear()
    {
        StartCoroutine(ShootAtPlayerCoroutine());
    }

    private IEnumerator RandomBulletAttackCoroutine()
    {

        List<Transform> usedPoints = new List<Transform>();

        for (int i = 0; i < bulletsPerRandomAttack; i++)
        {
            Transform shootPoint = GetRandomShootPointAvoidingOverlap(usedPoints);
            usedPoints.Add(shootPoint);

            Vector3 direction = Vector3.down; 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

            GameObject bullet = BossBulletPool.instance.GetBullet(shootPoint.position, rotation, Vector2.zero);

            if (bullet != null)
            {
                MissileBoss missileScript = bullet.GetComponent<MissileBoss>();
                if (missileScript != null)
                {
                    missileScript.SetDirection(direction);
                    missileScript.SetSprite(normal);
                }
            }

            yield return new WaitForSeconds(delayBetweenShots);
        }
    }

    private IEnumerator ShootAtPlayerCoroutine()
    {
        if (player == null || BossBulletPool.instance == null)
        {
            yield break;
        }

        Vector3 targetPos = player.position;
        Vector2 direction = (targetPos - directShootPoint.position).normalized;
        if (direction == Vector2.zero)
        {
            direction = Vector2.down;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

        GameObject bullet = BossBulletPool.instance.GetBullet(directShootPoint.position, rotation, Vector2.zero);

        if (bullet != null)
        {
            MissileBoss missileScript = bullet.GetComponent<MissileBoss>();
            if (missileScript != null)
            {
                missileScript.canExplode = true;
                missileScript.SetDirection(direction);
                missileScript.SetSprite(explosive);
            }
        }
    }

    private Transform GetRandomShootPointAvoidingOverlap(List<Transform> used)
    {
        List<Transform> available = new List<Transform>();

        foreach (Transform point in shootPoints)
        {
            if (!used.Contains(point))
                available.Add(point);
        }

        if (available.Count == 0)
            return shootPoints[Random.Range(0, shootPoints.Length)];

        return available[Random.Range(0, available.Count)];
    }
}
