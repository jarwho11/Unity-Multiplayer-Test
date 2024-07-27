using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCount : MonoBehaviour
{
    float fps;
    [SerializeField] TMPro.TextMeshProUGUI FPSCounterUI;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CalculateFPS", 1, 1);
    }

    void CalculateFPS()
    {
        fps = (1f / Time.unscaledDeltaTime);
        FPSCounterUI.text = fps.ToString();
    }
}
