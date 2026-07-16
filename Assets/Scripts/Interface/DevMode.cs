using UnityEngine;

public class DevMode : MonoBehaviour
{
    [Header("Referencias a las Barras de la UI")]
    [SerializeField] private BarraEsenciaManager barraRoja;
    [SerializeField] private BarraEsenciaManager barraVerde;
    [SerializeField] private BarraEsenciaManager barraAzul;

    private int rojaActual = 0;
    private int verdeActual = 0;
    private int azulActual = 0;

    /// <summary>
    /// Modificas los valores de la esencia roja
    /// </summary>
    public void ModificarRoja(int cantidad)
    {
        rojaActual = Mathf.Clamp(rojaActual + cantidad, 0, 10);
        if (barraRoja != null)
        {
            barraRoja.ActualizarCantidad(rojaActual);
        }
        Debug.Log($"[Modo Dios] Esencia Roja: {rojaActual}/10");
    }

    /// <summary>
    /// Modifica la esencia verde
    /// </summary>
    public void ModificarVerde(int cantidad)
    {
        verdeActual = Mathf.Clamp(verdeActual + cantidad, 0, 10);
        if (barraVerde != null)
        {
            barraVerde.ActualizarCantidad(verdeActual);
        }
        Debug.Log($"[Modo Dios] Esencia Verde: {verdeActual}/10");
    }

    /// <summary>
    /// Modifica la esencia azul
    /// </summary>
    public void ModificarAzul(int cantidad)
    {
        azulActual = Mathf.Clamp(azulActual + cantidad, 0, 10);
        if (barraAzul != null)
        {
            barraAzul.ActualizarCantidad(azulActual);
        }
        Debug.Log($"[Modo Dios] Esencia Azul: {azulActual}/10");
    }
}
