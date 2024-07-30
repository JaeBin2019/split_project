using UnityEngine;

public class LightController : MonoBehaviour
{
    private CustomLight light1;
    private CustomLight light2;
    private CustomLight light3;

    void Start()
    {
        // Instantiate three directional lights
        light1 = new CustomLight("light/DirectionalLight");

        // Set initial positions, rotations, intensities, and shadows
        light1.SetPosition(0f, 150f, -100f);
        float randomXRotation = Random.Range(30f, 70f);  // Random X rotation between 30 and 70 degrees
        float randomYRotation = Random.Range(-40f, 0f);  // Random Y rotation between 0 and -40 degrees
        light1.SetRotation(randomXRotation, randomYRotation, 0f);  // Adjust rotation to direct the light
        light1.SetIntensity(1.5f);  // Directional light intensity typically ranges from 0 to 8
        light1.EnableShadows();
    }


    public class CustomLight
    {
        public GameObject LightGameObject { get; private set; }
        public UnityEngine.Light SceneLight { get; private set; }

        public CustomLight(string prefabPath)
        {
            // Load the light prefab from Resources
            GameObject lightPrefab = Resources.Load<GameObject>(prefabPath);

            if (lightPrefab == null)
            {
                Debug.LogError("Failed to load light prefab from Resources.");
                return;
            }

            // Instantiate the light game object
            LightGameObject = Object.Instantiate(lightPrefab, Vector3.zero, Quaternion.identity);

            // Get the Light component
            SceneLight = LightGameObject.GetComponent<UnityEngine.Light>();
            if (SceneLight == null)
            {
                Debug.LogError("Light component not found on the instantiated prefab.");
                return;
            }

            // Set the light type to Directional
            SceneLight.type = LightType.Directional;
        }

        public void SetPosition(float x, float y, float z)
        {
            LightGameObject.transform.position = new Vector3(x, y, z);
        }

        public void SetRotation(float x, float y, float z)
        {
            LightGameObject.transform.rotation = Quaternion.Euler(x, y, z);
        }

        public void SetIntensity(float intensity)
        {
            if (SceneLight != null)
            {
                SceneLight.intensity = intensity;
            }
        }

        public void EnableShadows()
        {
            if (SceneLight != null)
            {
                SceneLight.shadows = LightShadows.Soft; // You can choose Soft or Hard shadows
                SceneLight.shadowStrength = 1f; // Adjust shadow strength if needed
                SceneLight.shadowBias = 0.001f; // Lower shadow bias to improve shadow quality for thin objects
                SceneLight.shadowNormalBias = 0.1f; // Lower normal bias for better shadow quality
                SceneLight.shadowNearPlane = 0.1f; // Adjust shadow near plane to a reasonable value
                SceneLight.shadowResolution = UnityEngine.Rendering.LightShadowResolution.VeryHigh; // Increase shadow resolution
            }
        }
    }
}
