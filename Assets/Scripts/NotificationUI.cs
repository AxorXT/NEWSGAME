using TMPro;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    public StoreGameManager store;
    public TMP_Text mensajeTexto;

    public float duracion = 2f;

    private float temporizador;
    private bool mostrando = false;

    private void Start()
    {
        if (store != null)
        {
            store.OnNotification += MostrarMensaje;
        }

        if (mensajeTexto != null)
        {
            mensajeTexto.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!mostrando)
            return;

        temporizador -= Time.deltaTime;

        if (temporizador <= 0)
        {
            OcultarMensaje();
        }
    }

    private void MostrarMensaje(string mensaje)
    {
        if (mensajeTexto == null)
            return;

        mensajeTexto.text = mensaje;
        mensajeTexto.gameObject.SetActive(true);

        temporizador = duracion;
        mostrando = true;
    }

    private void OcultarMensaje()
    {
        mostrando = false;

        if (mensajeTexto != null)
        {
            mensajeTexto.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (store != null)
        {
            store.OnNotification -= MostrarMensaje;
        }
    }
}
