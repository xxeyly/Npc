using UnityEngine;

[ExecuteInEditMode]
public class CanvasWorldScaleAutoConvert : MonoBehaviour
{
    public Canvas worldCanvas;
    public float currentWidth = 1920;
    public float currentHigh = 1080;

    // Update is called once per frame
    void Update()
    {
        if (worldCanvas != null)
        {
            worldCanvas.transform.localScale = new Vector3(currentWidth / 1920f, currentHigh / 1080f, 1);
        }
    }
}