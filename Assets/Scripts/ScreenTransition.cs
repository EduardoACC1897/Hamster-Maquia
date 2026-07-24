using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField]
    private Image transitionImage;

    [SerializeField]
    private float fadeDuration = 0.5f;

    public IEnumerator FadeOut()
    {
        yield return transitionImage
            .DOFade(1f, fadeDuration)
            .WaitForCompletion();
    }

    public IEnumerator FadeIn()
    {
        yield return transitionImage
            .DOFade(0f, fadeDuration)
            .WaitForCompletion();
    }
}