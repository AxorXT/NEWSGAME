using TMPro;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    [Header("Paneles de las secciones")]
    public GameObject newsPanel;
    public GameObject preciosPanel;
    public GameObject gananciaPanel;
    public GameObject inversionPanel;
    public GameObject chatPanel;

    public StoreGameManager store;
    public TMP_Text dineroTexto;

    private void Start()
    {
        NewsPanel();

        if (store == null || dineroTexto == null)
        {
            Debug.LogError("Asigna StoreGameManager y DineroTexto.");
            return;
        }

        store.OnStoreUpdated += ActualizarDinero;
        ActualizarDinero();
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

    private void ActualizarDinero()
    {
        dineroTexto.text = $"Dinero: ${store.money:N0}";
    }

    private void OnDestroy()
    {
        if (store != null)
            store.OnStoreUpdated -= ActualizarDinero;
    }
}