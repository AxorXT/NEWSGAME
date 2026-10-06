using UnityEngine;

public class SectionManager : MonoBehaviour
{
    [Header("Paneles de las secciones")]
    public GameObject preciosPanel;
    public GameObject gananciaPanel;
    public GameObject inversionPanel;

    private void Start()
    {
        MostrarPrecios();
    }

    public void MostrarPrecios()
    {
        OcultarTodos();
        preciosPanel.SetActive(true);
    }

    public void MostrarGanancia()
    {
        OcultarTodos();
        gananciaPanel.SetActive(true);
    }

    public void MostrarInversion()
    {
        OcultarTodos();
        inversionPanel.SetActive(true);
    }

    private void OcultarTodos()
    {
        preciosPanel.SetActive(false);
        gananciaPanel.SetActive(false);
        inversionPanel.SetActive(false);
    }
}