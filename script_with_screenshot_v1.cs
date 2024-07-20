using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    void Start()
    {
        float randomDis = Random.Range(700, 1201) * 0.001F;
        Vector3 cameraPosition = new Vector3(randomDis, 0.445f, 0);
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
        // 스크린샷을 파일로 저장
        ScreenCapture.CaptureScreenshot("Assets/screenshot1.png");
        // Debug.Log($"Screenshot saved to: {Application.dataPath}/{screenshotFileName}");
    }
}
