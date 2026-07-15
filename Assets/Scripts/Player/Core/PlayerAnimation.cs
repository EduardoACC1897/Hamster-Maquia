using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimation : MonoBehaviour
{
    #region References

    [Header("References")]

    [SerializeField]
    private Transform visual;

    #endregion

    #region Death Animation

    [Header("Death Animation")]

    [SerializeField]
    private float deathJumpHeight = 1.5f;

    [SerializeField]
    private float deathJumpDuration = 0.3f;

    [SerializeField]
    private float deathPauseDuration = 0.6f;

    [SerializeField]
    private float deathFallDistance = 8f;

    [SerializeField]
    private float deathFallDuration = 0.9f;

    #endregion

    #region Flash

    [Header("Invulnerability Flash")]

    [SerializeField]
    private float flashInterval = 0.1f;

    #endregion

    #region State

    private SpriteRenderer visualRenderer;

    private Sequence deathSequence;

    private Coroutine flashCoroutine;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        if (visual != null)
        {
            visualRenderer =
                visual.GetComponent<SpriteRenderer>();
        }
    }

    #endregion

    #region Public Methods

    public IEnumerator PlayDeathAnimation(
        bool restorePosition)
    {
        if (visual == null)
            yield break;

        Vector3 startPosition =
            visual.localPosition;

        deathSequence?.Kill();

        deathSequence = DOTween.Sequence();

        deathSequence

            .Append(
                visual.DOLocalMoveY(
                    startPosition.y + deathJumpHeight,
                    deathJumpDuration)
                .SetEase(Ease.OutQuad))

            .AppendInterval(
                deathPauseDuration)

            .Append(
                visual.DOLocalMoveY(
                    startPosition.y - deathFallDistance,
                    deathFallDuration)
                .SetEase(Ease.InQuad));

        yield return deathSequence.WaitForCompletion();

        if (restorePosition)
        {
            visual.localPosition =
                startPosition;
        }
    }

    public void StartFlash(
        float duration)
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(
                flashCoroutine);
        }

        flashCoroutine =
            StartCoroutine(
                FlashRoutine(duration));
    }

    public void StopFlash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(
                flashCoroutine);

            flashCoroutine = null;
        }

        if (visualRenderer != null)
        {
            visualRenderer.enabled = true;
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator FlashRoutine(
        float duration)
    {
        if (visualRenderer == null)
            yield break;

        float timer = 0f;

        while (timer < duration)
        {
            visualRenderer.enabled = false;

            yield return new WaitForSeconds(
                flashInterval);

            timer += flashInterval;

            visualRenderer.enabled = true;

            yield return new WaitForSeconds(
                flashInterval);

            timer += flashInterval;
        }

        visualRenderer.enabled = true;

        flashCoroutine = null;
    }

    #endregion
}