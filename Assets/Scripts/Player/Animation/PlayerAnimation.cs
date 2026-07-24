using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimation : MonoBehaviour
{
    #region References

    [Header("References")]

    [SerializeField]
    private Transform visual;

    [SerializeField]
    private ScreenTransition transition;

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

    #region Heal

    [Header("Heal")]

    [SerializeField]
    private Color healColor = Color.green;

    [SerializeField]
    private float minBlinkSpeed = 3f;

    [SerializeField]
    private float maxBlinkSpeed = 18f;

    [SerializeField]
    private float healFlashDuration = 0.25f;

    #endregion

    #region State

    private SpriteRenderer visualRenderer;

    private Sequence deathSequence;

    private Coroutine flashCoroutine;

    private Tween healTween;

    private Color originalColor = Color.white;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        if (visual != null)
        {
            visualRenderer =
                visual.GetComponent<SpriteRenderer>();

            originalColor =
                visualRenderer.color;
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

        if (transition != null)
        {
            yield return transition.FadeOut();
        }

        if (restorePosition)
        {
            visual.localPosition = startPosition;
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

    public void UpdateHealingEffect(
    float progress)
    {
        if (visualRenderer == null)
            return;

        float blinkSpeed =
            Mathf.Lerp(
                minBlinkSpeed,
                maxBlinkSpeed,
                progress);

        float blink =
            Mathf.PingPong(
                Time.time * blinkSpeed,
                1f);

        float colorAmount =
            Mathf.Lerp(
                blink * 0.25f,
                1f,
                progress);

        visualRenderer.color =
            Color.Lerp(
                originalColor,
                healColor,
                colorAmount);
    }

    public void StopHealingEffect()
    {
        healTween?.Kill();

        if (visualRenderer == null)
            return;

        visualRenderer.color =
            originalColor;
    }

    public void PlayHealCompleteEffect()
    {
        if (visualRenderer == null)
            return;

        healTween?.Kill();

        visualRenderer.color =
            healColor;

        healTween =
            visualRenderer
            .DOColor(
                originalColor,
                healFlashDuration)
            .SetEase(Ease.OutQuad);
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