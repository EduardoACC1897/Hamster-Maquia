using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    #region References

    [SerializeField]
    private Transform initialSpawnPoint;

    private PlayerController controller;

    #endregion

    #region State

    private Transform currentRespawnPoint;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        controller = GetComponent<PlayerController>();

        if (initialSpawnPoint != null)
        {
            currentRespawnPoint =
                initialSpawnPoint;
        }
        else
        {
            currentRespawnPoint =
                transform;
        }
    }

    #endregion

    #region Public Methods

    public void SetRespawnPoint(
        Transform checkpoint)
    {
        currentRespawnPoint = checkpoint;
    }

    public void Respawn()
    {
        if (currentRespawnPoint == null)
            return;

        transform.position =
            currentRespawnPoint.position;

        controller.ApplyImpulse(
            Vector2.zero);
    }

    #endregion
}