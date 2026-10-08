using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreciosUI : MonoBehaviour
{
    [Serializable]
    public class ProductoSlot
    {
        public GameObject contenedor;
        public Image imagen;
        public TMP_Text nombreTexto;
        public TMP_Text existenciasTexto;
        public Slider precioSlider;
        public TMP_Text precioTexto;
    }

    public StoreGameManager store;
    public ProductoSlot[] slots = new ProductoSlot[8];

    [Header("Botones")]
    public Button botonPrecios;
    public TMP_Text textoBotonPrecios;

    private void Start()
    {
        if (store == null)
        {
            Debug.LogError("Falta asignar StoreGameManager en PreciosUI.");
            return;
        }

        if (botonPrecios != null)
        {
            botonPrecios.onClick.AddListener(CambiarEstadoPrecios);
        }

        for (int i = 0; i < slots.Length; i++)
        {
            int indice = i;

            if (slots[i].precioSlider != null)
            {
                slots[i].precioSlider.onValueChanged.AddListener(
                    valor => CambiarPrecio(indice, valor)
                );
            }
        }

        store.OnStoreUpdated += ActualizarInterfaz;
        ActualizarInterfaz();
        ActualizarBotonPrecios();
    }

    private void ActualizarInterfaz()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            ProductoSlot slot = slots[i];

            bool existe = i < store.products.Count;
            bool disponible = existe && store.products[i].stock > 0;

            if (slot.contenedor != null)
                slot.contenedor.SetActive(disponible);

            if (!disponible)
                continue;

            StoreProduct producto = store.products[i];

            if (slot.nombreTexto != null)
                slot.nombreTexto.text = producto.productName;

            if (slot.existenciasTexto != null)
                slot.existenciasTexto.text =
                    $"Existencias: {producto.stock}";

            if (slot.precioSlider != null)
            {
                slot.precioSlider.minValue = 1;
                slot.precioSlider.maxValue = 100;

                slot.precioSlider.SetValueWithoutNotify(
                    Mathf.Clamp(
                        producto.sellingPrice,
                        slot.precioSlider.minValue,
                        slot.precioSlider.maxValue
                    )
                );

                slot.precioSlider.interactable = !store.preciosConfirmados;
            }

            if (slot.precioTexto != null)
                slot.precioTexto.text =
                    $"Precio: ${producto.sellingPrice:N0}";
        }

        ActualizarBotonPrecios();
    }

    private void CambiarPrecio(int indice, float precio)
    {
        if (store.preciosConfirmados)
            return;

        if (indice < 0 || indice >= store.products.Count)
            return;

        store.SetSellingPrice(indice, precio);

        if (slots[indice].precioTexto != null)
        {
            slots[indice].precioTexto.text =
                $"Precio: ${precio:N0}";
        }
    }

    private void OnDestroy()
    {
        if (store != null)
            store.OnStoreUpdated -= ActualizarInterfaz;
    }

    private void ActualizarBotonPrecios()
    {
        if (botonPrecios == null)
            return;

        if (textoBotonPrecios != null)
        {
            if (store.preciosConfirmados)
            {
                textoBotonPrecios.text = "AJUSTAR PRECIOS";
            }
            else
            {
                textoBotonPrecios.text = "CONFIRMAR PRECIOS";
            }
        }
    }

    private void CambiarEstadoPrecios()
    {
        if (store.preciosConfirmados)
        {
            store.DesbloquearPrecios();
        }
        else
        {
            store.ConfirmarPrecios();
        }
    }
}
