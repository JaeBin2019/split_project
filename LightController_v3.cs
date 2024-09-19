using UnityEngine;

public class LightController : MonoBehaviour
{
    public CustomLight light1; // 필드를 public으로 변경

    void Start()
    {
        // Instantiate one directional light
        light1 = new CustomLight("light/SpotLight");

        // Set initial positions, rotations, intensities
        light1.SetPosition(40f, 95f, -145f);
        float randomXRotation = Random.Range(30f, 40f);  // Random X rotation between 30 and 70 degrees
        float randomYRotation = Random.Range(0f, 2f);  // Random Y rotation between 0 and -40 degrees
        light1.SetRotation(randomXRotation, randomYRotation, 0f);  // Adjust rotation to direct the light
        light1.SetIntensity(150000f);  // 기본 밝기로 설정
        light1.EnableShadows();  // 그림자 활성화
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

            // Set the light type to Spotlight
            SceneLight.type = LightType.Spot;
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
                SceneLight.shadows = LightShadows.Soft; // 그림자를 활성화 (Soft 선택)
            }
        }

        public void DisableShadows()
        {
            if (SceneLight != null)
            {
                SceneLight.shadows = LightShadows.None; // 그림자를 비활성화
            }
        }
    }
}
