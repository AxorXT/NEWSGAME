using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueText : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text valueText;

    private void Start()
    {
        slider.minValue = 0; 
        slider.maxValue = 1000; 
        slider.wholeNumbers = true;

        UpdateValue(slider.value);
        slider.onValueChanged.AddListener(UpdateValue);
    }

    private void UpdateValue(float value)
    {
        valueText.text = Mathf.RoundToInt(value).ToString();
    }
}
