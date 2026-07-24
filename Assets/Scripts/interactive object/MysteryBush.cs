using UnityEngine;

public class MysteryBush :
    MonoBehaviour,
    IDamageable
{
    #region References

    [Header("Essence")]

    [SerializeField]
    private GameObject[] essencePrefabs;

    [SerializeField]
    private Transform[] spawnPoints;

    [Header("Visual")]

    [SerializeField]
    private Animator visualAnimator;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite inactiveSprite;

    [SerializeField]
    private Collider2D hitbox;

    #endregion

    #region State

    private bool activated;

    #endregion

    #region IDamageable

    public void TakeDamage(int damage)
    {
        if (activated)
            return;

        activated = true;

        if (spawnPoints != null &&
            essencePrefabs != null)
        {
            int count = Mathf.Min(
                spawnPoints.Length,
                essencePrefabs.Length);

            for (int i = 0; i < count; i++)
            {
                SpawnEssence(
                    essencePrefabs[i],
                    spawnPoints[i]);
            }
        }

        if (visualAnimator != null)
        {
            visualAnimator.enabled = false;
        }

        if (spriteRenderer != null &&
            inactiveSprite != null)
        {
            spriteRenderer.sprite =
                inactiveSprite;
        }

        if (hitbox != null)
        {
            hitbox.enabled = false;
        }
    }

    #endregion

    #region Helpers

    private void SpawnEssence(
    GameObject essencePrefab,
    Transform spawnPoint)
    {
        if (essencePrefab == null)
            return;

        if (spawnPoint == null)
            return;

        Instantiate(
            essencePrefab,
            spawnPoint.position,
            Quaternion.identity);
    }

    #endregion
}