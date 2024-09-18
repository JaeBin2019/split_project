using UnityEngine;
using TMPro;
public class LightController : MonoBehaviour
{
    public CustomLight light1; // 필드를 public으로 변경

    public void Init()
    {
        // Instantiate one directional light
        light1 = new CustomLight("light/DirectionalLight");

        TMP_InputField X_Lotation= GameObject.Find("X_Lotation_Value").GetComponent<TMP_InputField>(); // ok
        TMP_InputField Y_Lotation = GameObject.Find("Y_Lotation_Value").GetComponent<TMP_InputField>(); // ok
        TMP_InputField IntensityInput = GameObject.Find("Intensity_Value").GetComponent<TMP_InputField>(); // ok

        // Set initial positions, rotations, intensities
        light1.SetPosition(0f, 150f, -100f);
        float randomXRotation = Random.Range(30f, 70f);  // Random X rotation between 30 and 70 degrees
        randomXRotation = ConvertInputToFloat(X_Lotation, randomXRotation); // check

        float randomYRotation = Random.Range(-40f, 0f);  // Random Y rotation between 0 and -40 degrees
        randomYRotation = ConvertInputToFloat(Y_Lotation, randomYRotation);
        light1.SetRotation(randomXRotation, randomYRotation, 0f);  // Adjust rotation to direct the light

        float intensity = 1.5f;
        intensity = ConvertInputToFloat(IntensityInput, intensity);
        light1.SetIntensity(intensity);  // 기본 밝기로 설정
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

    private float ConvertInputToFloat(TMP_InputField inputField, float origin_value)
    {
        float result;
        if (float.TryParse(inputField.text, out result))
        {
            return result;
        }
        else
        {
            Debug.LogWarning("Input is not a valid float: " + inputField.text);
            return origin_value; // 변환 실패 시 반환할 기본값 (필요에 따라 조정 가능)
        }
    }
}
