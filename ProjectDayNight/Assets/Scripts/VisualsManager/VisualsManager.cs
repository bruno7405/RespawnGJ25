using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VisualsManager : MonoBehaviour
{
    [SerializeField] Light2D globalLight;

    private static VisualsManager instance;
    public static VisualsManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void LightsOff()
    {
        globalLight.intensity = 0;
    }

    public void LightsOn()
    {
        globalLight.intensity = 1;
    }
}
