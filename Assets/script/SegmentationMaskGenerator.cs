using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SegmentationMaskGenerator : MonoBehaviour
{
    public Camera segmentationCamera; // Segmentation을 위한 카메라
    public int maskWidth = 1024;
    public int maskHeight = 720;
    public string outputDirectory = "Assets/SegmentationMasks/";

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
        // Ensure the output directory exists
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        StartCoroutine(GenerateSegmentationMasks());
    }

    IEnumerator GenerateSegmentationMasks()
    {
        foreach (string key in objectColorMapping.Keys)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(key); // 객체를 태그로 식별
            yield return StartCoroutine(RenderMaskForObjects(objects, key));
        }
    }

    IEnumerator RenderMaskForObjects(GameObject[] objects, string objectType)
    {
        // 설정된 색상으로 객체 렌더링
        foreach (GameObject obj in objects)
        {
            obj.GetComponent<Renderer>().material.color = objectColorMapping[objectType];
        }

        // RenderTexture 생성 및 카메라에 할당
        RenderTexture renderTexture = new RenderTexture(maskWidth, maskHeight, 24);
        segmentationCamera.targetTexture = renderTexture;
        segmentationCamera.Render();

        // RenderTexture를 텍스처로 변환
        RenderTexture.active = renderTexture;
        Texture2D maskTexture = new Texture2D(maskWidth, maskHeight, TextureFormat.RGB24, false);
        maskTexture.ReadPixels(new Rect(0, 0, maskWidth, maskHeight), 0, 0);
        maskTexture.Apply();
        RenderTexture.active = null;

        // 마스크 이미지 저장
        string filePath = $"{outputDirectory}{objectType}_mask.png";
        byte[] bytes = maskTexture.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
        Debug.Log($"Saved segmentation mask to {filePath}");

        // Clean up
        segmentationCamera.targetTexture = null;
        RenderTexture.ReleaseTemporary(renderTexture);

        // 오브젝트 색상 복원
        foreach (GameObject obj in objects)
        {
            obj.GetComponent<Renderer>().material.color = Color.white; // 원래 색상으로 복원
        }

        yield return null;
    }
}
