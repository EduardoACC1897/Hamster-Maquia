using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ContactDamage : MonoBehaviour
{
    #region Settings

    [SerializeField]
    private int damage = 1;

    #endregion

    #region Unity Messages

    private void OnCollisionEnter2D(
        Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(
            out PlayerHealth playerHealth))
        {
            return;
        }

        playerHealth.TakeDamage(
            damage,
            transform.position);
    }

    #endregion
}