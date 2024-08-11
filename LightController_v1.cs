using UnityEngine;

public class LightController : MonoBehaviour
{
    private CustomLight light1;
    private CustomLight light2;

    void Start()
    {
        // Instantiate two lights
        light1 = new CustomLight("light/PointLight");
        light2 = new CustomLight("light/PointLight");

        // Set initial positions, rotations, intensities, and shadows
        light1.SetPosition(0f, 1.5f, 0f);
        light1.SetRotation(0f, 180f, 0f); // Rotate 180 degrees around Y-axis to flip
        light1.SetIntensity(4f);
        light1.EnableShadows();

        light2.SetPosition(-1f, 0f, 0.5f);
        light2.SetRotation(0f, 180f, 0f); // Rotate 180 degrees around Y-axis to flip
        light2.SetIntensity(10f);
        light2.EnableShadows();
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
            }
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
                SceneLight.shadowBias = 0.05f; // Adjust shadow bias if needed
                SceneLight.shadowNormalBias = 0.4f; // Adjust shadow normal bias if needed
            }
        }
    }
}
