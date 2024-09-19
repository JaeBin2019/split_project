using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public ObjectLabeler objectLabeler;  // ObjectLabeler를 참조할 변수 추가
    public Camera segmentationCamera;  // Segmentation 카메라를 참조할 변수 추가
    public float delayBeforeScreenshot = 0.1f; // 스크린샷 전 대기 시간
    public float delayAfterScreenshot = 0.1f; // 스크린샷 후 대기 시간
    public int screenshotWidth = 1024;  // 원하는 스크린샷 너비
    public int screenshotHeight = 1024;  // 원하는 스크린샷 높이

    void Start()
    {
        // 카메라 위치와 회전 설정
        float randomDis = Random.Range(700, 1201) * 0.1F;
        Vector3 cameraPosition = new Vector3(randomDis, 44.5f, 0);
        Vector3 cameraRotation = new Vector3(18, -90, 0);

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

        // 일정 시간 대기 후 스크린샷 찍기
        Invoke("TakeScreenshotAndLabel", delayBeforeScreenshot);
    }

    void TakeScreenshotAndLabel()
    {
        string screenshotFilePath = TakeScreenshot();
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

        // 스크린샷 후 일정 시간 대기
        Invoke("AfterScreenshotDelay", delayAfterScreenshot);
    }

    void AfterScreenshotDelay()
    {
        Debug.Log("Post-screenshot delay complete.");
        // 필요한 후속 작업이 있으면 여기에 추가
    }

    string TakeScreenshot()
    {
        string directory = "Assets/screenshot/";
        string baseFileName = "screenshot";
        string extension = ".png";

        int fileIndex = 1;
        string filePath = Path.Combine(directory, baseFileName + fileIndex + extension);

        while (File.Exists(filePath))
        {
            fileIndex++;
            filePath = Path.Combine(directory, baseFileName + fileIndex + extension);
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
}
