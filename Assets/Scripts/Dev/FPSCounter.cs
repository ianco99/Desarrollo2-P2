using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    void Update()
    {
        int fps = (int)(1.0f / Time.unscaledDeltaTime);
        text.text = "" + fps;
    }
}
