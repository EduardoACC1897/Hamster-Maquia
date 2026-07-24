using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Referencias de Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;


    [Header("Clips de Audio")]
    public AudioClip sonidoGolpeJugador;
    public AudioClip sonidoRecogerEsencia;

    [Header("Musica de Fondo")]
    public AudioClip musicaFondo;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        
       Instance = this;
       transform.SetParent(null);
       DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    private void Start()
    {
        if (musicaFondo != null && musicSource != null)
        {
            musicSource.clip = musicaFondo;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

   
}



