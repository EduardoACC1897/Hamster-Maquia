using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class BarraEsenciaManager : MonoBehaviour
{
    private Image imagenUI;
    private bool esModoDaltonismo = false;
    private int cantidadActual = 0; //9 imagenes

    [Header("Sprites de la barra normal")]
    [Tooltip("Arrastrar aca los 9 sprites de las barras")]
    [SerializeField] private Sprite[] spriteNormal = new Sprite[10];

    [Header("Sprites de la barra daltonica")]
    [Tooltip("Arrastrar aca las 9 imagenes daltonicas")]
    [SerializeField] private Sprite[] spriteDaltonismo = new Sprite[10];

    void Awake()
    {
        imagenUI = GetComponent<Image>();
    }

    void OnEnable()
    {
        PauseManager.ColorblindChanged += CambiarModoDaltonismo;

        esModoDaltonismo = PlayerPrefs.GetInt("Daltonismo", 0) == 1;

        ActualizarVisualizacion();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnDisable()
    {
        PauseManager.ColorblindChanged -= CambiarModoDaltonismo;
    }

    public void ActualizarCantidad(int nuevaCantidad)
    {
        //evitar que aumenta mas de 10
        cantidadActual = Mathf.Clamp(nuevaCantidad, 0, 10);
        ActualizarVisualizacion();
    }

    private void CambiarModoDaltonismo(bool esDaltonismo)
    {
        esModoDaltonismo = esDaltonismo;
        ActualizarVisualizacion();
    }
    private void ActualizarVisualizacion()
    {
        if (cantidadActual == 0)
        {
            imagenUI.enabled = false;
            return;
        }
        
        //si hay al menos 1 esencia se muestra la barra
        imagenUI.enabled = true;
        //no empezar desde 0
        int index = cantidadActual - 1;

        if (esModoDaltonismo)
        {
            imagenUI.sprite = spriteDaltonismo[index];
        }
        else
        {
            imagenUI.sprite = spriteNormal[index];
        }
    }
}
