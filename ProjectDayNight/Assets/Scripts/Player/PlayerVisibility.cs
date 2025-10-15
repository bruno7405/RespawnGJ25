using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerVisibility : MonoBehaviour
{
    private static PlayerVisibility instance;
    public static PlayerVisibility Instance => instance;

    [SerializeField] Light2D playerLight;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;
    }

    public void IncreaseLightRange(int percent)
    {
        playerLight.pointLightOuterRadius *= (1 + (percent / 100f));
        playerLight.pointLightOuterRadius = Mathf.Min(playerLight.pointLightOuterRadius, 10f);
    }
}
