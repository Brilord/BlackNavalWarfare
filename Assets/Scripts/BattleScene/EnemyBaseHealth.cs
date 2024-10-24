using UnityEngine;

public class EnemyBaseHealth : MonoBehaviour
{
    public float baseHP = 20000f;
    public float maxHP = 20000f;

    // Method to reduce the base's HP
    public void ApplyDamage(float damageAmount)
    {
        baseHP -= damageAmount;

        // Clamp the value so HP doesn't go below zero
        baseHP = Mathf.Clamp(baseHP, 0, maxHP);
    }

    // Method to get the current HP
    public float GetCurrentHP()
    {
        return baseHP;
    }

    // Method to get the maximum HP
    public float GetMaxHP()
    {
        return maxHP;
    }

    // Method to check if the base is destroyed (HP <= 0)
    public bool IsDestroyed()
    {
        return baseHP <= 0;
    }
}
