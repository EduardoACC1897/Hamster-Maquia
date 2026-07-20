using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PauseManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Paneles de UI")]
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject panelConfiguraciones;

    [Header("Configuraciones")]
    [SerializeField] private Slider sliderVolumen;
    [SerializeField] private Toggle daltonismo;

    private bool juegoPausado = false;

    //barras de esencia cambian el archivo png
    public delegate void OnColorblindChanged(bool active);
    public static event OnColorblindChanged ColorblindChanged;

    void Start()
    {
        panelPausa.SetActive(false);
        panelConfiguraciones.SetActive(false);
        Time.timeScale = 1f;

        float volumenGuardado = PlayerPrefs.GetFloat("VolumenJuego", 1f);
        sliderVolumen.value = volumenGuardado;
        AudioListener.volume = volumenGuardado;

        //estado daltonito
        bool daltonismoActivo = PlayerPrefs.GetInt("Daltonismo", 0) == 1;
        daltonismo.isOn = daltonismoActivo;

    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (CookingManager.Instance != null && CookingManager.Instance.isKitchenOpen)
            {
                return;
            }
            AlternarMenuPausa();
        }
    }
    public void AlternarMenuPausa()
    {  
        if (panelConfiguraciones != null && panelConfiguraciones.activeSelf)
        {
            CerrarConfiguraciones();
            return;
        }

        
        if (juegoPausado)
        {
            Reanudar();
        }
        else
        {
            Pausar();
        }
    }
    //Opciones de menu
    public void Pausar()
    {
        juegoPausado = true;
        panelPausa.SetActive(true);
        panelConfiguraciones.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Reanudar()
    {
        juegoPausado = false;
        panelPausa.SetActive(false);
        panelConfiguraciones.SetActive(false);
        Time.timeScale = 1f;
    }

    public void AbrirConfiguraciones()
    {
        panelPausa.SetActive(false);
        panelConfiguraciones.SetActive(true);
    }

    public void CerrarConfiguraciones()
    {
        panelConfiguraciones.SetActive(false);
        panelPausa.SetActive(true);
    }

    public void SalirMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    //Manejo de configuraciones

    public void CambiarVolumen(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("VolumenJuego", valor);
    }

    public void CambiarModoDaltonismo(bool activado)
    {
        PlayerPrefs.SetInt("Daltonismo", activado ? 1 : 0);

        ColorblindChanged?.Invoke(activado);
    }

}
