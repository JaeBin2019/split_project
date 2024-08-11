using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectLabeler : MonoBehaviour
{
    public Camera segmentationCamera;  // Segmentation 마스크를 위한 별도의 카메라
    public string outputDirectory = "Assets/labeledScreenshot/";
    public int imageWidth = 1024;
    public int imageHeight = 1024;

    public float preRenderDelay = 2.0f;  // 색상 변경 후 대기 시간
    public float postRenderDelay = 1.0f;  // 스크린샷 후 대기 시간

    public List<GameObject> objectsToLabel = new List<GameObject>(); // 라벨링할 객체들을 담는 리스트

    private Dictionary<string, Color> objectColorMapping = new Dictionary<string, Color>
    {
        { "longi", Color.red },
        { "floor", Color.green },
        { "realFloor", Color.blue },
        { "plate", Color.yellow },
        { "slotHole", Color.magenta },
        { "r_hole", Color.cyan }
    };

    void Start()
    {
        // 일정 시간 기다린 후 실행하여 오브젝트 생성 시간이 확보되도록 함
        Invoke("GenerateSegmentationMasksAndLabels", 2.0f);
    }

    public void LabelObjects(string screenshotFilePath)
    {
        // 마스크 이미지를 저장할 경로를 설정 (스크린샷 경로에서 파일 이름을 그대로 사용)
        string directory = "Assets/labeledScreenshot/";
        string fileName = Path.GetFileName(screenshotFilePath);
        string maskFilePath = Path.Combine(directory, fileName);

        // 전체 객체를 하나의 이미지로 렌더링 및 동일한 파일 이름으로 마스크 생성
        StartCoroutine(RenderFullMask(maskFilePath));
    }

    void GenerateSegmentationMasksAndLabels()
    {
        foreach (KeyValuePair<string, Color> entry in objectColorMapping)
        {
            string tag = entry.Key;
            Color color = entry.Value;

            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag); // 태그로 객체를 식별
            if (objects.Length == 0)
            {
                Debug.LogWarning($"No objects found with tag: {tag}");
                continue;
            }

            SetObjectColors(objects, color);

            // 라벨링할 객체 리스트에 추가
            objectsToLabel.AddRange(objects);
        }
    }

    void SetObjectColors(GameObject[] objects, Color color)
    {
        foreach (GameObject obj in objects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true); // 오브젝트 활성화
            }

            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material.color = color;
            }
        }
    }

    IEnumerator RenderFullMask(string maskFilePath)
    {
        yield return new WaitForSeconds(preRenderDelay); // 스크린샷 찍기 전 대기

        // RenderTexture 생성 및 카메라에 할당
        RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
        segmentationCamera.targetTexture = renderTexture;
        segmentationCamera.Render();

        // 카메라 렌더링이 완료되었는지 확인한 후 스크린샷 촬영
        yield return new WaitForEndOfFrame();

        // RenderTexture를 텍스처로 변환
        RenderTexture.active = renderTexture;
        Texture2D maskTexture = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
        maskTexture.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        maskTexture.Apply();
        RenderTexture.active = null;

        // 마스크 이미지 저장 (스크린샷 파일 이름과 동일하게 저장)
        byte[] bytes = maskTexture.EncodeToPNG();
        File.WriteAllBytes(maskFilePath, bytes);
        Debug.Log($"Saved segmentation mask to {maskFilePath}");

        // Clean up
        segmentationCamera.targetTexture = null;
        renderTexture.Release(); // RenderTexture.Release를 사용하여 메모리에서 해제

        yield return new WaitForSeconds(postRenderDelay); // 스크린샷 후 대기

        // 오브젝트 색상 복원
        ResetObjectColors();
    }

    void ResetObjectColors()
    {
        foreach (GameObject obj in objectsToLabel)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material.color = Color.white; // 원래 색상으로 복원
            }
        }
    }
}
