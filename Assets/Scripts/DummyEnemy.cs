using UnityEngine;

public class DummyEnemy : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage = 1)
    {
        Debug.Log($"Dummy recibió {damage} de daño.");
    }
}