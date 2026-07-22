using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Referencias de Vida")]
    [Tooltip("Arratrar aca los 3 gameobjects de las vidas")]
    [SerializeField] private GameObject[] corazones = new GameObject[3];

    [Tooltip("Arrastrar aca el texto de las vidas")]
    [SerializeField] private TextMeshProUGUI vidasTexto;

    [Header("Referencias del arma")]
    [SerializeField] private Image armaImagen;
    [SerializeField] private Image fondoAmarilloArma;

    [Header("Referencias de Barras de Esencia")]
    [SerializeField] private BarraEsenciaManager barraRoja;
    [SerializeField] private BarraEsenciaManager barraAzul;
    [SerializeField] private BarraEsenciaManager barraVerde;

    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += ActualizarCorazones;
        PlayerHealth.OnLivesChanged += ActualizarTextosVida;
        PlayerIngredientEssence.OnEssenceChanged += ActualizarEsencia;
        WeaponManager.OnWeaponChanged += ActualizarArmaVisual;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= ActualizarCorazones;
        PlayerHealth.OnLivesChanged -= ActualizarTextosVida;
        PlayerIngredientEssence.OnEssenceChanged -= ActualizarEsencia;
        WeaponManager.OnWeaponChanged -= ActualizarArmaVisual;
    }

    private void Start()
    {
        SincronizarInterfazInicial();
    }

    //funcion que sincroniza la interfaz al iniciar el juego, para que no se vea vacia
    private void SincronizarInterfazInicial()
    {
        PlayerHealth health = FindAnyObjectByType<PlayerHealth>();
        if(health != null)
        {
            ActualizarCorazones(health.CurrentHealth, health.MaxHealth);
            ActualizarTextosVida(health.CurrentLives);
        }
        else if (PlayerDataManager.Instance != null)
        {
            ActualizarTextosVida(PlayerDataManager.Instance.RemainingLives);
        }

        // sincronizar las esencias del jugador
        PlayerIngredientEssence essence = FindAnyObjectByType<PlayerIngredientEssence>();
        if (essence != null)
        {
            ActualizarEsencia(IngredientEssenceType.Red, essence.RedEssence);
            ActualizarEsencia(IngredientEssenceType.Blue, essence.BlueEssence);
            ActualizarEsencia(IngredientEssenceType.Green, essence.GreenEssence);
        }
        else if (PlayerDataManager.Instance != null)
        {
            ActualizarEsencia(IngredientEssenceType.Red, PlayerDataManager.Instance.RedEssence);
            ActualizarEsencia(IngredientEssenceType.Blue, PlayerDataManager.Instance.BlueEssence);
            ActualizarEsencia(IngredientEssenceType.Green, PlayerDataManager.Instance.GreenEssence);
        }

        // sincronizar el arma del jugador
        WeaponManager weaponController = FindAnyObjectByType<WeaponManager>();
        if (weaponController != null)
        {
            ActualizarArmaVisual(weaponController.CurrentWeapon, weaponController.RemainingUses);
        }
        else if (PlayerDataManager.Instance != null)
        {
            ActualizarArmaVisual(PlayerDataManager.Instance.EquippedWeapon, PlayerDataManager.Instance.RemainingWeaponUses);
        }
    }
    private void ActualizarCorazones(int vidaActual, int vidaMaxima)
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            if (corazones[i] != null)
            {
                corazones[i].SetActive(i < vidaActual);
            }
        }
    }
    private void ActualizarTextosVida(int vidasRestantes)
    {
        if (vidasTexto != null)
        {
            int vidasActuales = Mathf.Max(0, vidasRestantes);
            vidasTexto.text = $"x " + vidasActuales;
        }
    }

    private void ActualizarEsencia(IngredientEssenceType tipo, int cantidad)
    {
        switch (tipo)
        {
            case IngredientEssenceType.Red:
                barraRoja.ActualizarCantidad(cantidad);
                break;
            case IngredientEssenceType.Blue:
                barraAzul.ActualizarCantidad(cantidad);
                break;
            case IngredientEssenceType.Green:
                barraVerde.ActualizarCantidad(cantidad);
                break;
        }
    }
    public void ActualizarArmaVisual(WeaponData armaEquipada, int usosRestantes)
    {
      

        if(armaEquipada != null && armaEquipada.Icon != null) //variable icon de weaponData
        {
            armaImagen.enabled  = true;
            fondoAmarilloArma.enabled = true;
            armaImagen.sprite = armaEquipada.Icon;
        }
        else
        {
            armaImagen.enabled = false;
            fondoAmarilloArma.enabled = false;
        }
    }

}
