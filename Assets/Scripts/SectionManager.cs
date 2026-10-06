using UnityEngine;

public class SectionManager : MonoBehaviour
{
    [Header("Paneles de las secciones")]
    public GameObject newsPanel;
    public GameObject preciosPanel;
    public GameObject gananciaPanel;
    public GameObject inversionPanel;
    public GameObject chatPanel;

    private void Start()
    {
        NewsPanel();
    }

    public void ChatPanel()
    {
        OcultarTodos();
        chatPanel.SetActive(true);
    }
    public void NewsPanel()
    {
        OcultarTodos();
        newsPanel.SetActive(true);
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
        newsPanel.SetActive(false);
        chatPanel.SetActive(false);
    }
}