using UnityEngine;

public class DummyEnemy : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage)
    {
        Debug.Log($"Dummy recibió {damage} de daño.");
    }
}