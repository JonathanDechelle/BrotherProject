﻿using UnityEngine;
using System.Collections.Generic;

public static class CombatEnemyUtil
{
    public static List<Enemy> GetEnnemiesAround(Vector3 aPosition, float aRadius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(aPosition, aRadius);
        List<Enemy> ennemies = new List<Enemy>();
        int i = 0;
        while (i < hitColliders.Length)
        {
            Enemy enemy = hitColliders[i].gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                ennemies.Add(enemy);
            }
            i++;
        }

        return ennemies;
    }

    public static Vector3 GetRandomPositionInCircleWithRadius(Vector3 aCenter, float aRadius)
    {
        return (Random.insideUnitSphere * aRadius) + aCenter;
    }

    public static float GetDistanceBetween(Vector3 aTo, Vector3 aFrom)
    {
        Vector3 direction = aTo - aFrom;
        direction.y = 0f;
        return direction.magnitude;
    }
}
