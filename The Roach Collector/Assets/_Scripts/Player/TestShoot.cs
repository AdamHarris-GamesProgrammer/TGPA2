
using UnityEngine;

public class TestShoot : MonoBehaviour
{
    public float health = 100.0f;
    
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0.0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
