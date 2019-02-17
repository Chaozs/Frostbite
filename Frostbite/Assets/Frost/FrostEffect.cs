using UnityEngine;

/// <summary>
/// Controls the effect of frost on the screen. Frost accumulates when character temperature decreases.
/// </summary>
//[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Frost")]
public class FrostEffect : MonoBehaviour
{
    public float FrostAmount = 0f; //0-1 (0=minimum Frost, 1=maximum frost)
    public float EdgeSharpness = 1; //>=1
    public float minFrost = 0; //0-1
    public float maxFrost = 1; //0-1
    public float seethroughness = 0.2f; //blends between 2 ways of applying the frost effect: 0=normal blend mode, 1="overlay" blend mode
    public float distortion = 0.1f; //how much the original image is distorted through the frost (value depends on normal map)
    public Texture2D Frost; //RGBA
    public Texture2D FrostNormals; //normalmap
    public Shader Shader; //ImageBlendEffect.shader

    private Stats characterStats;

    private Material material;

    private void Awake()
    {
        material = new Material(Shader);
        material.SetTexture("_BlendTex", Frost);
        material.SetTexture("_BumpMap", FrostNormals);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!Application.isPlaying)
        {
            material.SetTexture("_BlendTex", Frost);
            material.SetTexture("_BumpMap", FrostNormals);
            EdgeSharpness = Mathf.Max(1, EdgeSharpness);
        }
        material.SetFloat("_BlendAmount", Mathf.Clamp01(Mathf.Clamp01(FrostAmount) * (maxFrost - minFrost) + minFrost));
        material.SetFloat("_EdgeSharpness", EdgeSharpness);
        material.SetFloat("_SeeThroughness", seethroughness);
        material.SetFloat("_Distortion", distortion);

        Graphics.Blit(source, destination, material);
    }

    void Start()
    {
        characterStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
    }

    void Update()
    {
        // Set frost amount depending on temperature of character
        FrostAmount = GetFrostAmount(characterStats.GetTemperature());
    }

    /// <summary>
    /// Return the frost amount depending on player temperature.
    /// </summary>
    /// <param name="temperature">Player's temperature from character Stats.</param>
    private float GetFrostAmount(int temperature)
    {
        if (temperature > 30)
        {
            return 0f;
        }
        else if (temperature > 25)
        {
            return 0.1f;
        }
        else if (temperature > 20)
        {
            return 0.2f;
        }
        else if (temperature > 15)
        {
            return 0.3f;
        }
        else if (temperature > 10)
        {
            return 0.4f;
        }
        else if (temperature > 5)
        {
            return 0.5f;
        }
        else
        {
            return 0.6f;
        }
    }
}