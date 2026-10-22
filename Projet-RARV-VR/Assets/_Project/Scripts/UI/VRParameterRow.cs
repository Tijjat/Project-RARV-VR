
using System;
using TMPro;
using UnityEngine;

public class VRParameterRow : MonoBehaviour
{
    [SerializeField] private TMP_Text parameterName;
    [SerializeField] private TMP_InputField parameterInput;

    public void Setup(
        string label,
        float value,
        Action<float> onChanged)
    {
        parameterName.text = label;

        parameterInput.SetTextWithoutNotify(
            value.ToString(
                "0.###",
                System.Globalization.CultureInfo.InvariantCulture));

        parameterInput.contentType =
            TMP_InputField.ContentType.DecimalNumber;

        parameterInput.onEndEdit.RemoveAllListeners();

        parameterInput.onEndEdit.AddListener(text =>
        {
            if (float.TryParse(
                text,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out float result))
            {
                onChanged?.Invoke(result);
            }
        });
    }
}
