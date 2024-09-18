using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using TMPro;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    // public ObjectLabeler objectLabeler;  // ObjectLabeler를 참조할 변수 추가
    public ObjectSpawner objectSpawner;  // ObjectSpawner 참조 추가
    public Camera segmentationCamera;  // Segmentation 카메라를 참조할 변수 추가
    public LightController lightController;
    public float delayBeforeScreenshot = 1.0f; // 스크린샷 전 대기 시간
    public float delayAfterScreenshot = 1.0f; // 스크린샷 후 대기 시간
    public int screenshotWidth = 1024;  // 원하는 스크린샷 너비
    public int screenshotHeight = 720;  // 원하는 스크린샷 높이

    void Start()
    {
        // 스크린샷 디렉토리가 존재하지 않으면 생성
        string directory = "Assets/screenshot/";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        Button button = GameObject.Find("Create").GetComponent<Button>();
        button.onClick.AddListener(StartMyCoroutine);
    }

    void StartMyCoroutine()
    {
        StartCoroutine(ProcessCombinations()); // 코루틴 시작
    }

    IEnumerator ProcessCombinations()
    {
        switchCanvas("Canvas", false);

        // Longi
        TMP_InputField longiNumLeft = GameObject.Find("Longi_Number_Left").GetComponent<TMP_InputField>(); // ok
        TMP_InputField longiNumRIght = GameObject.Find("Longi_Number_Right").GetComponent<TMP_InputField>(); // ok
        TMP_Dropdown longiTypeLeft = GameObject.Find("Longi_Type_Left").GetComponent<TMP_Dropdown>(); // ok
        TMP_Dropdown longiTypeRight = GameObject.Find("Longi_Type_Right").GetComponent<TMP_Dropdown>(); // ok
        TMP_InputField longiDis = GameObject.Find("Longi_Distance_Value").GetComponent<TMP_InputField>(); // ok

        // Slot Hole
        TMP_Dropdown slotHoleType = GameObject.Find("Slot_Hole_Type_Value").GetComponent<TMP_Dropdown>(); // ok
        TMP_Dropdown rHoleLocationInput = GameObject.Find("Slot_Hole_R_Hole_Value").GetComponent<TMP_Dropdown>(); // ok
        // TMP_Dropdown rHoleRight = GameObject.Find("Slot_Hole_R_Hole_Right").GetComponent<TMP_Dropdown>(); 

        // Plate
        TMP_Dropdown plateTypeLeft = GameObject.Find("Plate_Type_Value").GetComponent<TMP_Dropdown>(); // ok
        TMP_Dropdown plateLocation = GameObject.Find("Plate_Location_Value").GetComponent<TMP_Dropdown>(); // ok

        // Camera
        TMP_InputField cameraHeightInput = GameObject.Find("Camera_Height_Value").GetComponent<TMP_InputField>(); // ok
        TMP_InputField cameraDisInput = GameObject.Find("Camera_Distance_Value").GetComponent<TMP_InputField>(); // ok

        TMP_InputField repeatInput = GameObject.Find("Repeat_Value").GetComponent<TMP_InputField>(); // ok
        int repeat = ConvertInputToInt(repeatInput);

        string[] longiSuffixes = { "LF", "LA", "LT" };
        string[] plateSuffixes = { "CP01", "CP02", "CP03", "CP04", "CP05", "CP06" };
        string[] slotHoleSuffixesA = { "AH", "AA", "AG", "AJ" };
        string[] slotHoleSuffixesT = { "TE", "TG" };

        int r = 0;
        while (r < repeat)
        {

            // int Longi1 = UnityEngine.Random.Range(1, 27);
            int Longi1 = ConvertLongiLeftInputToInt(longiNumLeft); // check

            // longi1과 longi2 설정, 10% 확률로 다르게 설정
            int Longi2 = ConvertLongiRightInputToInt(longiNumRIght, Longi1); // check

            // longiSuffix1과 longiSuffix2 설정, 10% 확률로 다르게 설정
            string longiSuffix1 = GetLongiLeftDropdownValue(longiTypeLeft, longiSuffixes); // check
            string longiSuffix2 = GetLongiRightDropdownValue(longiTypeRight, longiSuffixes, longiSuffix1); // check

            // slotHoleSuffix를 longiSuffix에 따라 결정
            string[] selectedSlotHoleSuffixes = (longiSuffix1 == "LF" || longiSuffix1 == "LA") ? slotHoleSuffixesA : slotHoleSuffixesT;

            // plateSuffix와 slotHoleSuffix에서 두 개씩 무작위로 선택
            List<string> selectedPlateSuffixes = new List<string>(plateSuffixes);
            List<string> selectedSlotHoleSuffixesList = new List<string>(selectedSlotHoleSuffixes);
            ShuffleList(selectedPlateSuffixes);
            ShuffleList(selectedSlotHoleSuffixesList);

            string[] chosenPlateSuffixes = selectedPlateSuffixes.ToArray();
            string[] chosenSlotHoleSuffixes = selectedSlotHoleSuffixesList.ToArray();

            string plateSuffix = chosenPlateSuffixes[0];
            plateSuffix = GetSelectedDropdownValue(plateTypeLeft, plateSuffix); // check

            string slotHoleSuffix = chosenSlotHoleSuffixes[0];
            slotHoleSuffix = GetSelectedDropdownValue(slotHoleType, slotHoleSuffix); // check

            float longiDistance = ConvertLongiDisToFloat(longiDis); // check

            // public void SpawnObjects(int longi1, int longi2, float longiDistance, string longiSuffix1, string longiSuffix2,
            // string slotHoleSuffix1, string slotHoleSuffix2,
            // int plateStatus, int particleStatus, string plateSuffix, Quaternion plateRotation)

            // ObjectSpawner에 파라미터 전달하여 오브젝트 생성
            int plateStatus = UnityEngine.Random.Range(1, 3);
            plateStatus = GetPlateLocationDropdownValueAsInt(plateLocation, plateStatus); // check

            int particleStatus = UnityEngine.Random.Range(0, 3);

            Quaternion plateRotation = Quaternion.identity;

            if (plateStatus == 2)
                plateRotation = Quaternion.Euler(0, 180, 0);

            int rHoleLocation = GetRHoleDropdownValueAsInt(rHoleLocationInput); // check

            lightController.Init();
            objectSpawner.SpawnObjects(Longi1, Longi2, longiDistance, longiSuffix1, longiSuffix2,
            slotHoleSuffix, slotHoleSuffix,
            plateStatus, particleStatus, plateSuffix, plateRotation, rHoleLocation);

            // 카메라 위치 설정
            SetCameraPosition(Longi1, cameraHeightInput, cameraDisInput);

            // 스크린샷 찍기
            yield return new WaitForSeconds(delayBeforeScreenshot);
            string screenshotFilePath = TakeScreenshot(Longi1, Longi2, longiSuffix1, longiSuffix2, slotHoleSuffix, plateSuffix);
            UnityEngine.Debug.Log($"Screenshot saved to: {screenshotFilePath}");

            // 오브젝트 라벨링
            // if (objectLabeler != null)
            // {
            //     objectLabeler.LabelObjects(screenshotFilePath);
            // }
            // else
            // {
            //     UnityEngine.Debug.LogError("ObjectLabeler reference not set in CameraScript!");
            // }

            // 텍스트 파일 생성 및 데이터 저장
            string textFilePath = Path.ChangeExtension(Path.Combine("Assets/parameter/", Path.GetFileName(screenshotFilePath)), ".txt");
            SaveDataToTxt(textFilePath, Longi1, Longi2, longiDistance, plateStatus, plateSuffix);

            // 마스크 생성 완료 후 삭제 대기
            yield return new WaitForSeconds(delayAfterScreenshot);

            // 생성된 오브젝트 삭제
            objectSpawner.DeleteSpawnedObjects();

            r = r + 1;
        }

        UnityEngine.Debug.Log("All combinations processed.");

        switchCanvas("Canvas", true);
    }

    void SetCameraPosition(int longi1, TMP_InputField cameraHeightInput, TMP_InputField cameraDisInput) // 수정
    {
        // 카메라의 기본 높이 설정
        float cameraHeight = 44.5f;
        
        // 론지 값에 따른 카메라 높이 조정
        if ((longi1 >= 1 && longi1 <= 7) || (longi1 >= 20 && longi1 <= 26))
        {
            cameraHeight = 25.0f;
        }

        if (longi1 >= 8 && longi1 <= 14)
        {
            cameraHeight = 30.0f;
        }
        cameraHeight = ConvertInputToFloat(cameraHeightInput, cameraHeight); // check

        // 카메라 위치와 회전 설정
        float randomDis = UnityEngine.Random.Range(850, 1000) * 0.1F;
        randomDis = ConvertInputToFloat(cameraDisInput, randomDis); // check

        Vector3 cameraPosition = new Vector3(randomDis, cameraHeight, 0);
        Vector3 cameraRotation = new Vector3(10, -90, 0);

        Camera.main.transform.position = cameraPosition;
        Camera.main.transform.eulerAngles = cameraRotation;
        Camera.main.nearClipPlane = 0.1f;

        // Segmentation 카메라가 설정되어 있다면 메인 카메라와 동일한 위치와 회전으로 설정
        if (segmentationCamera != null)
        {
            segmentationCamera.transform.position = cameraPosition;
            segmentationCamera.transform.eulerAngles = cameraRotation;
        }
        else
        {
            UnityEngine.Debug.LogError("Segmentation Camera reference not set in CameraScript!");
        }
    }

    string TakeScreenshot(int longi1, int longi2, string longiSuffix1, string longiSuffix2, string slotHoleSuffix, string plateSuffix)
    {
        string directory = "Assets/screenshot/";
        string baseFileName = $"screenshot_{longi1}_{longi2}_{longiSuffix1}_{longiSuffix2}_{slotHoleSuffix}_{plateSuffix}";
        string extension = ".png";

        string filePath = Path.Combine(directory, baseFileName + extension);

        int fileIndex = 1;
        while (File.Exists(filePath))
        {
            filePath = Path.Combine(directory, baseFileName + "_" + fileIndex + extension);
            fileIndex++;
        }

        // 원하는 크기의 RenderTexture 생성
        //RenderTexture rt = new RenderTexture(screenshotWidth, screenshotHeight, 24);
        //Camera.main.targetTexture = rt;
        //Camera.main.Render();

        //RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGB24, false);
        //screenShot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
        //screenShot.Apply();
        //Post-Processing을 적용시킨 상태로 스크린샷 촬영(유니티포럼에서 긁어옴)
        RenderTexture transformedRenderTexture = null;
        RenderTexture renderTexture = RenderTexture.GetTemporary(
            Screen.width,
            Screen.height,
            24,
            RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Default,
            1);

        ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);
        transformedRenderTexture = RenderTexture.GetTemporary(
            screenShot.width,
            screenShot.height,
            24,
            RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Default,
            1);
        Graphics.Blit(
            renderTexture,
            transformedRenderTexture,
            new Vector2(1.0f, -1.0f),
            new Vector2(0.0f, 1.0f));
        RenderTexture.active = transformedRenderTexture;
        screenShot.ReadPixels(
            new Rect(0, 0, screenShot.width, screenShot.height),
            0, 0);
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);
        if (transformedRenderTexture != null)
        {
            RenderTexture.ReleaseTemporary(transformedRenderTexture);
        }

        screenShot.Apply();

        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        // Destroy(rt);

        // 파일로 저장
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);

        return filePath;
    }

    void SaveDataToTxt(string filePath, int longi1, int longi2, float longiDistance, int plateStatus, string plateSuffix)
    {
        // 파일의 디렉터리 경로를 추출
        string directory = Path.GetDirectoryName(filePath);

        // 디렉터리가 존재하지 않으면 생성
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // ObjectSpawner의 MakeTxt 메서드를 호출하여 데이터 작성
            // MakeTxt(StreamWriter writer, int longi1, int longi2, float longiDistance, int plateStatus, string plateSuffix)
            objectSpawner.MakeTxt(writer, longi1, longi2, longiDistance, plateStatus, plateSuffix);
        }
    }

    // 리스트 섞기 함수 추가
    void ShuffleList<T>(List<T> list)
    {
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            int k = UnityEngine.Random.Range(0, count);
            T value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
    }

    private int ConvertLongiLeftInputToInt(TMP_InputField inputField)
    {
        UnityEngine.Debug.LogWarning("ConvertLongiLeftInputToInt");
        int result;
        if (int.TryParse(inputField.text, out result))
        {
            if (result >= 1 && result <= 26)
            {
                return result;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Input is not within the range 1-26: " + result);
                return 0; // 범위 밖일 때 반환할 기본값 (필요에 따라 조정 가능)
            }
        }
        else
        {
            return UnityEngine.Random.Range(1, 27); // 변환 실패 시 반환할 기본값 (필요에 따라 조정 가능)
        }
    }

    private int ConvertLongiRightInputToInt(TMP_InputField inputField, int Longi1)
    {
        UnityEngine.Debug.LogWarning("ConvertLongiRightInputToInt");
        int result;
        if (int.TryParse(inputField.text, out result))
        {
            if (result >= 1 && result <= 26)
            {
                return result;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Input is not within the range 1-26: " + result);
                return 0; // 범위 밖일 때 반환할 기본값 (필요에 따라 조정 가능)
            }
        }
        else
        {
            return UnityEngine.Random.Range(0f, 1f) <= 0.15f ? UnityEngine.Random.Range(1, 27) : Longi1; // 변환 실패 시 반환할 기본값 (필요에 따라 조정 가능)
        }
    }

    private float ConvertLongiDisToFloat(TMP_InputField inputField)
    {
        UnityEngine.Debug.LogWarning("ConvertLongiDisToFloat");
        float result;
        if (float.TryParse(inputField.text, out result))
        {
            if (result >= 500f && result <= 850f)
            {
                return result;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Input is not within the range 500-850: " + result);
                return 0f; // 범위 밖일 때 반환할 기본값 (필요에 따라 조정 가능)
            }
        }
        else
        {
            return UnityEngine.Random.Range(500, 851) * 0.05F; ; // 변환 실패 시 반환할 기본값 (필요에 따라 조정 가능)
        }
    }

    private string GetLongiLeftDropdownValue(TMP_Dropdown dropdown, string[] longiSuffixes)
    {
        UnityEngine.Debug.LogWarning("GetLongiLeftDropdownValue");
        string selectedText = dropdown.options[dropdown.value].text;

        if (selectedText == "Random")
        {
            return longiSuffixes[UnityEngine.Random.Range(0, longiSuffixes.Length)];
        }
        return dropdown.options[dropdown.value].text;
    }

    private string GetLongiRightDropdownValue(TMP_Dropdown dropdown, string[] longiSuffixes, string longiSuffix1)
    {
        UnityEngine.Debug.LogWarning("GetLongiRightDropdownValue");
        string selectedText = dropdown.options[dropdown.value].text;

        if (selectedText == "Random")
        {
            return UnityEngine.Random.Range(0f, 1f) <= 0.3f ? longiSuffixes[UnityEngine.Random.Range(0, longiSuffixes.Length)] : longiSuffix1;
        }
        return dropdown.options[dropdown.value].text;
    }

    private string GetSelectedDropdownValue(TMP_Dropdown dropdown, string origin_value)
    {
        string selectedText = dropdown.options[dropdown.value].text;
        
        if (selectedText != "Random")
        {
            return selectedText;
        }
        return origin_value;
    }

    private int GetPlateLocationDropdownValueAsInt(TMP_Dropdown dropdown, int origin_value)
    {
        string selectedText = dropdown.options[dropdown.value].text;

        switch (selectedText)
        {
            case "None":
                return 0;
            case "Left":
                return 1;
            case "Right":
                return 2;
            case "Random":
                return origin_value;
            default:
                UnityEngine.Debug.LogWarning("Unexpected dropdown value: " + selectedText);
                return -1; // 예상치 못한 값에 대한 기본값
        }
    }

    private int GetRHoleDropdownValueAsInt(TMP_Dropdown dropdown)
    {
        string selectedText = dropdown.options[dropdown.value].text;

        switch (selectedText)
        {
            case "None":
                return 0;
            case "Inside":
                return 2;
            case "Outside":
                return 1;
            case "Random":
                return -1;
            default:
                UnityEngine.Debug.LogWarning("Unexpected dropdown value: " + selectedText);
                return -1; // 예상치 못한 값에 대한 기본값
        }
    }

    private void switchCanvas(string canvasName, bool enabled)
    {
        // 지정한 이름으로 Canvas를 찾습니다.
        GameObject canvasObject = GameObject.Find(canvasName);

        if (canvasObject != null)
        {
            // Canvas 컴포넌트를 가져옵니다.
            Canvas canvas = canvasObject.GetComponent<Canvas>();

            if (canvas != null)
            {
                // Canvas를 비활성화합니다.
                canvas.enabled = enabled;
                UnityEngine.Debug.Log($"{canvasName}.");
            }
            else
            {
                UnityEngine.Debug.LogError($"No Canvas component found on {canvasName}.");
            }
        }
        else
        {
            UnityEngine.Debug.LogError($"No GameObject found with the name {canvasName}.");
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
            UnityEngine.Debug.LogWarning("Input is not a valid float: " + inputField.text);
            return origin_value;
        }
    }

    private int ConvertInputToInt(TMP_InputField inputField)
    {
        int result;
        if (int.TryParse(inputField.text, out result))
        {
            return result;
        }
        else
        {
            UnityEngine.Debug.LogWarning("Input is not a valid float: " + inputField.text);
            return 1;
        }
    }
}
