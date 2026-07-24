using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookingSlotiUI : MonoBehaviour
{
    [Header("Datos de la Receta")]
    public RecipeData recipeData;

    [Header("Icono Principal")]
    public Image imgIconoComida;
    [Header("Fila Esencia Roja")]
    public GameObject filaRoja;
    public Image imgIconoRojo;
    public TextMeshProUGUI txtCostoRojo;

    [Header("Fila Esencia Verde")]
    public GameObject filaVerde;
    public Image imgIconoVerde;
    public TextMeshProUGUI txtCostoVerde;

    [Header("Fila Esencia Azul")]
    public GameObject filaAzul;
    public Image imgIconoAzul;
    public TextMeshProUGUI txtCostoAzul;

    [Header("PNG de Esencias")]
    public Sprite iconoEsenciaRoja;
    public Sprite iconoEsenciaVerde;
    public Sprite iconoEsenciaAzul;

    [Header("Boton Cocinar")]
    public Button btnCocinar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if(recipeData != null)
        {
            ConfigurarSlot();
        }
        else
        {
            Debug.LogWarning($"Error, inicio sin los datos {gameObject.name}");
        }
    }

    public void InicializarSlot(RecipeData nuevaReceta)
    {
        recipeData = nuevaReceta;
        ConfigurarSlot();
    }
    //rellena los pergaminos/ bloques
   public void ConfigurarSlot()
   {
        if (recipeData == null)
        {
            Debug.LogError($"[Cocina UI] Se intentó configurar el slot '{gameObject.name}' pero recipeData sigue siendo null.");
            return;
        }

        if (imgIconoComida != null) imgIconoComida.sprite = recipeData.recipeIcon;

        //EScribir en pantalla los costos de esencia necesarios para desbloquare el item

        ConfigurarFilaEsencia(filaRoja, imgIconoRojo, txtCostoRojo, iconoEsenciaRoja, recipeData.redEssenceCost);
        ConfigurarFilaEsencia(filaVerde, imgIconoVerde, txtCostoVerde, iconoEsenciaVerde, recipeData.greenEssenceCost);
        ConfigurarFilaEsencia(filaAzul, imgIconoAzul, txtCostoAzul, iconoEsenciaAzul, recipeData.blueEssenceCost);

        if (btnCocinar != null)
        {
            btnCocinar.onClick.RemoveAllListeners();
            btnCocinar.onClick.AddListener(IntentarCocinar);
        }

    }

    private void ConfigurarFilaEsencia(GameObject fila, Image imgIcono, TextMeshProUGUI txtCosto, Sprite spriteEsencia, int costo)
    {
        if (costo <= 0)
        {
            if (fila != null) fila.SetActive(false);
            return;
        }

        // Si la receta requiere esta esencia, activamos la fila
        if (fila != null) fila.SetActive(true);

        // Asignar el PNG de la esencia
        if (imgIcono != null && spriteEsencia != null)
        {
            imgIcono.sprite = spriteEsencia;
        }

        // Asignar la cantidad requerida
        if (txtCosto != null)
        {
            txtCosto.text = costo.ToString();
        }
    }
    private void IntentarCocinar()
    {
        if(CookingManager.Instance != null)
        {
            CookingManager.Instance.CocinarReceta(recipeData);
        }
    }
}
