using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HoneyPuddle : MonoBehaviour
{
    #region Settings

    [SerializeField]
    [Range(0.1f, 1f)]
    private float slowMultiplier = 0.4f;

    #endregion

    #region State

    private PlayerController player;

    #endregion

    #region Unity Messages

    private void OnTriggerEnter2D(
        Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        player =
            other.GetComponent<PlayerController>();

        if (player == null)
            return;

        UpdateSlowState();
    }

    private void OnTriggerStay2D(
        Collider2D other)
    {
        if (player == null)
            return;

        UpdateSlowState();
    }

    private void OnTriggerExit2D(
        Collider2D other)
    {
        if (player == null)
            return;

        if (!other.CompareTag("Player"))
            return;

        player.ResetMovementSpeedMultiplier();

        player = null;
    }

    #endregion

    #region Private Methods

    private void UpdateSlowState()
    {
        if (player.IsGrounded)
        {
            player.SetMovementSpeedMultiplier(
                slowMultiplier);
        }
        else
        {
            player.ResetMovementSpeedMultiplier();
        }
    }

    #endregion
}