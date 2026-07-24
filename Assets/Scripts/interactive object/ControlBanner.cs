using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ControlBanner : MonoBehaviour
{
    #region References

    [Header("Checkpoint")]

    [SerializeField]
    private Transform respawnPoint;

    [Header("Visual")]

    [SerializeField]
    private Animator visualAnimator;

    [SerializeField]
    private Collider2D trigger;

    #endregion

    #region State

    private bool activated;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        if (trigger == null)
        {
            trigger = GetComponent<Collider2D>();
        }

        trigger.isTrigger = true;

        if (visualAnimator != null)
        {
            visualAnimator.enabled = false;
        }

        if (respawnPoint == null)
        {
            respawnPoint = transform;
        }
    }

    private void OnTriggerEnter2D(
        Collider2D other)
    {
        if (activated)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerRespawn playerRespawn =
            other.GetComponent<PlayerRespawn>();

        if (playerRespawn == null)
            return;

        activated = true;

        playerRespawn.SetRespawnPoint(
            respawnPoint);

        if (visualAnimator != null)
        {
            visualAnimator.enabled = true;
        }

        trigger.enabled = false;
    }

    #endregion
}