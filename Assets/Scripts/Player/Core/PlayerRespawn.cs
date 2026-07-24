using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class PlayerRespawn : MonoBehaviour
{
    #region References

    [SerializeField]
    private Transform SpawnPoint;

    [SerializeField]
    private CinemachineCamera virtualCamera;

    private PlayerController controller;

    [SerializeField]
    private ScreenTransition transition;

    #endregion

    #region State

    private Transform currentRespawnPoint;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        controller = GetComponent<PlayerController>();

        if (SpawnPoint != null)
        {
            currentRespawnPoint =
                SpawnPoint;
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
        StartCoroutine(
            RespawnRoutine());
    }

    #endregion

    #region Coroutines

    private IEnumerator RespawnRoutine()
    {
        if (transition != null)
        {
            yield return transition.FadeOut();
        }

        Vector3 previousPosition =
            transform.position;

        Vector3 position =
            currentRespawnPoint.position;

        position.z =
            transform.position.z;

        transform.position =
            position;

        controller.ApplyImpulse(
            Vector2.zero);

        if (virtualCamera != null)
        {
            virtualCamera.OnTargetObjectWarped(
                transform,
                position - previousPosition);
        }

        yield return null;

        if (transition != null)
        {
            yield return transition.FadeIn();
        }
    }

    #endregion
}