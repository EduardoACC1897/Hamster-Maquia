using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
public class NectarBubble : MonoBehaviour
{
    #region References

    [Header("References")]

    [SerializeField]
    private Transform visual;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Collider2D hitbox;

    #endregion

    #region Settings

    [Header("Gameplay")]

    [SerializeField]
    private float bounceForce = 18f;

    [SerializeField]
    private float respawnTime = 2f;

    [Header("Sprites")]

    [SerializeField]
    private Sprite normalSprite;

    [SerializeField]
    private Sprite burstSprite;

    #endregion

    #region State

    private bool active = true;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        if (hitbox == null)
        {
            hitbox =
                GetComponent<Collider2D>();
        }

        if (visual == null &&
            transform.childCount > 0)
        {
            visual =
                transform.GetChild(0);
        }

        if (spriteRenderer == null &&
            visual != null)
        {
            spriteRenderer =
                visual.GetComponent<SpriteRenderer>();
        }
    }

    private void OnTriggerEnter2D(
        Collider2D other)
    {
        if (!active)
            return;

        if (!other.TryGetComponent(
            out PlayerController player))
        {
            return;
        }

        StartCoroutine(
            BubbleRoutine(player));
    }

    #endregion

    #region Coroutines

    private IEnumerator BubbleRoutine(
        PlayerController player)
    {
        active = false;

        hitbox.enabled = false;

        player.ApplyImpulse(
            new Vector2(
                player.TargetVelocity.x,
                bounceForce));

        visual.DOKill();

        spriteRenderer.sprite =
            burstSprite;

        Sequence burst =
            DOTween.Sequence();

        burst.Append(
            visual.DOScale(
                1.3f,
                0.08f));

        burst.Append(
            visual.DOScale(
                0f,
                0.12f));

        yield return burst.WaitForCompletion();

        yield return new WaitForSeconds(
            respawnTime);

        spriteRenderer.sprite =
            normalSprite;

        visual.localScale =
            Vector3.zero;

        Sequence respawn =
            DOTween.Sequence();

        respawn.Append(
            visual.DOScale(
                1.15f,
                0.22f)
            .SetEase(Ease.OutBack));

        respawn.Append(
            visual.DOScale(
                1f,
                0.08f));

        yield return respawn.WaitForCompletion();

        hitbox.enabled = true;

        active = true;
    }

    #endregion
}