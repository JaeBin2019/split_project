using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public ObjectLabeler objectLabeler;  // ObjectLabeler를 참조할 변수 추가
    public ObjectSpawner objectSpawner;  // ObjectSpawner 참조 추가
    public Camera segmentationCamera;  // Segmentation 카메라를 참조할 변수 추가
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

        StartCoroutine(ProcessCombinations());
    }

    IEnumerator ProcessCombinations()
    {
        string[] longiSuffixes = { "LF", "LA", "LT" };
        string[] plateSuffixes = { "CP01", "CP02", "CP03", "CP04", "CP05", "CP06" };
        string[] slotHoleSuffixesA = { "AH", "AA", "AG", "AJ" };
        string[] slotHoleSuffixesT = { "TE", "TG" };

        for (int Longi1 = 1; Longi1 <= 26; Longi1++)
        {
            // longi1과 longi2 설정, 10% 확률로 다르게 설정
            int Longi2 = Random.Range(0f, 1f) <= 0.15f ? Random.Range(1, 27) : Longi1;

            // longiSuffix1과 longiSuffix2 설정, 10% 확률로 다르게 설정
            string longiSuffix1 = longiSuffixes[Random.Range(0, longiSuffixes.Length)];
            string longiSuffix2 = Random.Range(0f, 1f) <= 0.3f ? longiSuffixes[Random.Range(0, longiSuffixes.Length)] : longiSuffix1;

            // slotHoleSuffix를 longiSuffix에 따라 결정
            string[] selectedSlotHoleSuffixes = (longiSuffix1 == "LF" || longiSuffix1 == "LA") ? slotHoleSuffixesA : slotHoleSuffixesT;

            // plateSuffix와 slotHoleSuffix에서 두 개씩 무작위로 선택
            List<string> selectedPlateSuffixes = new List<string>(plateSuffixes);
            List<string> selectedSlotHoleSuffixesList = new List<string>(selectedSlotHoleSuffixes);
            ShuffleList(selectedPlateSuffixes);
            ShuffleList(selectedSlotHoleSuffixesList);

            string[] chosenPlateSuffixes = selectedPlateSuffixes.ToArray();
            string[] chosenSlotHoleSuffixes = selectedSlotHoleSuffixesList.ToArray();

            foreach (string plateSuffix in chosenPlateSuffixes)
            {
                foreach (string slotHoleSuffix in chosenSlotHoleSuffixes)
                {
                    float longiDistance = Random.Range(500, 851) * 0.05F;

                    // public void SpawnObjects(int longi1, int longi2, float longiDistance, string longiSuffix1, string longiSuffix2,
                    // string slotHoleSuffix1, string slotHoleSuffix2,
                    // int plateStatus, string plateSuffix, Quaternion plateRotation)

                    // ObjectSpawner에 파라미터 전달하여 오브젝트 생성
                    int plateStatus = Random.Range(1, 3);

                    Quaternion plateRotation = Quaternion.identity;

                    if (plateStatus == 2)
                        plateRotation = Quaternion.Euler(0, 180, 0);

                    objectSpawner.SpawnObjects(Longi1, Longi2, longiDistance, longiSuffix1, longiSuffix2,
                    slotHoleSuffix, slotHoleSuffix,
                    plateStatus, plateSuffix, plateRotation);

                    // 카메라 위치 설정
                    SetCameraPosition(Longi1);

                    // 스크린샷 찍기
                    yield return new WaitForSeconds(delayBeforeScreenshot);
                    string screenshotFilePath = TakeScreenshot(Longi1, Longi2, longiSuffix1, longiSuffix2, slotHoleSuffix, plateSuffix);
                    Debug.Log($"Screenshot saved to: {screenshotFilePath}");

                    // 오브젝트 라벨링
                    if (objectLabeler != null)
                    {
                        objectLabeler.LabelObjects(screenshotFilePath);
                    }
                    else
                    {
                        Debug.LogError("ObjectLabeler reference not set in CameraScript!");
                    }

                    // 텍스트 파일 생성 및 데이터 저장
                    string textFilePath = Path.ChangeExtension(Path.Combine("Assets/parameter/", Path.GetFileName(screenshotFilePath)), ".txt");
                    SaveDataToTxt(textFilePath, Longi1, Longi2, longiDistance, plateStatus, plateSuffix);

                    // 마스크 생성 완료 후 삭제 대기
                    yield return new WaitForSeconds(delayAfterScreenshot);

                    // 생성된 오브젝트 삭제
                    objectSpawner.DeleteSpawnedObjects();
                }
            }
        }

        Debug.Log("All combinations processed.");
    }

    void SetCameraPosition(int longi1)
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

        // 카메라 위치와 회전 설정
        float randomDis = Random.Range(850, 1000) * 0.1F;
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
            Debug.LogError("Segmentation Camera reference not set in CameraScript!");
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
        RenderTexture rt = new RenderTexture(screenshotWidth, screenshotHeight, 24);
        Camera.main.targetTexture = rt;
        Camera.main.Render();

        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
        screenShot.Apply();

        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

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
            int k = Random.Range(0, count);
            T value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
    }
}
