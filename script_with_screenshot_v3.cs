using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
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
        TakeScreenshot();
    }

    void TakeScreenshot()
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
        // Debug.Log($"Screenshot saved to: {filePath}");
    }
}
