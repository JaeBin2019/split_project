using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public ObjectLabeler objectLabeler;  // ObjectLabeler를 참조할 변수 추가

    void Start()
    {
        float randomDis = Random.Range(700, 1201) * 0.1F;
        Vector3 cameraPosition = new Vector3(randomDis, 44.5f, 0);
        Vector3 cameraRotation = new Vector3(18, -90, 0);

        // 메인 카메라 위치와 회전 설정
        Camera.main.transform.position = cameraPosition;
        Camera.main.transform.eulerAngles = cameraRotation;
        Camera.main.nearClipPlane = 0.1f;

        // 스크린샷 찍기
        StartCoroutine(TakeScreenshotAndLabel());
    }

    IEnumerator TakeScreenshotAndLabel()
    {
        string screenshotFilePath = TakeScreenshot();
        Debug.Log($"Screenshot saved to: {screenshotFilePath}");

        // 스크린샷이 저장될 때까지 대기
        yield return new WaitForSeconds(1.0f);

        // 오브젝트 라벨링
        if (objectLabeler != null)
        {
            objectLabeler.LabelObjects(screenshotFilePath);
        }
        else
        {
            Debug.LogError("ObjectLabeler reference not set in CameraScript!");
        }
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

        // 스크린샷을 파일로 저장
        ScreenCapture.CaptureScreenshot(filePath);
        return filePath;
    }
}
