using UnityEngine;
using UnityEngine.UI;

public class NoticiasUI : MonoBehaviour
{
    [Header("Referencia al juego")]
    public StoreGameManager store;

    [Header("Recuadros de noticias")]
    public Image[] noticias = new Image[4];

    [Header("Imágenes buenas")]
    public Sprite[] noticiasBuenas;

    [Header("Imágenes malas")]
    public Sprite[] noticiasMalas;

    [Header("Dinero necesario para buenas noticias")]
    public float dineroParaBuenasNoticias = 70000f;

    private void Start()
    {
        if (store == null)
        {
            Debug.LogError("Falta asignar StoreGameManager en NoticiasUI.");
            return;
        }

        store.OnStoreUpdated += ActualizarNoticias;

        ActualizarNoticias();
    }

    private void ActualizarNoticias()
    {
        bool buenaSituacion = store.money >= dineroParaBuenasNoticias;

        Sprite[] grupoNoticias;

        if (buenaSituacion)
        {
            grupoNoticias = noticiasBuenas;
        }
        else
        {
            grupoNoticias = noticiasMalas;
        }

        if (grupoNoticias == null || grupoNoticias.Length == 0)
        {
            Debug.LogWarning("No hay imágenes de noticias asignadas.");
            return;
        }

        for (int i = 0; i < noticias.Length; i++)
        {
            if (noticias[i] == null)
                continue;

            int indice = Random.Range(0, grupoNoticias.Length);

            noticias[i].sprite = grupoNoticias[indice];
        }
    }

    private void OnDestroy()
    {
        if (store != null)
        {
            store.OnStoreUpdated -= ActualizarNoticias;
        }
    }
}
