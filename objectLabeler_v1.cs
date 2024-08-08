using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectLabeler : MonoBehaviour
{
    public Camera mainCamera;
    public List<GameObject> objectsToLabel;
    public string outputDirectory = "Assets/Resources/labels/";

    private Dictionary<string, int> objectNameToId = new Dictionary<string, int>
    {
        { "longi", 0 },
        { "floor", 1 },
        { "realFloor", 2 },
        { "plate", 3 },
        { "slotHole", 4 },
        { "r_hole", 5 }
    };

    private const float MinSize = 0.001f;  // 아주 작은 최소 크기 값

    public void LabelObjects(string screenshotFilePath)
    {
        string screenshotFileName = Path.GetFileNameWithoutExtension(screenshotFilePath);

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        using (StreamWriter writer = new StreamWriter($"{outputDirectory}{screenshotFileName}.txt"))
        {
            foreach (GameObject obj in objectsToLabel)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Bounds bounds = renderer.bounds;
                    Vector2 min = WorldToImagePoint(bounds.min, mainCamera);
                    Vector2 max = WorldToImagePoint(bounds.max, mainCamera);

                    // 중심 좌표 및 크기 계산
                    Vector2 center = (min + max) / 2;
                    Vector2 size = max - min;

                    // 크기가 너무 작으면 최소 크기로 설정
                    if (size.x < MinSize) size.x = MinSize;
                    if (size.y < MinSize) size.y = MinSize;

                    // 중심 좌표 및 크기 정규화
                    center.x = Mathf.Clamp01(center.x);
                    center.y = Mathf.Clamp01(center.y);
                    size.x = Mathf.Clamp(size.x, 0, 1);
                    size.y = Mathf.Clamp(size.y, 0, 1);

                    // 디버그 로그로 출력
                    Debug.Log($"Object: {obj.name}, Center: {center}, Size: {size}");

                    // 객체 이름 ID 가져오기
                    int objectClassId = objectNameToId.ContainsKey(obj.name) ? objectNameToId[obj.name] : -1;
                    if (objectClassId == -1)
                    {
                        Debug.LogWarning($"Unknown object name: {obj.name}");
                        continue;
                    }

                    // YOLO 형식: <object-class> <x_center> <y_center> <width> <height>
                    string labelData = $"{objectClassId} {center.x} {center.y} {size.x} {size.y}";
                    writer.WriteLine(labelData);
                }
            }
        }

        Debug.Log($"Labels saved to: {outputDirectory}{screenshotFileName}.txt");
    }

    Vector2 WorldToImagePoint(Vector3 worldPoint, Camera cam)
    {
        Vector3 viewportPoint = cam.WorldToViewportPoint(worldPoint);
        return new Vector2(viewportPoint.x, viewportPoint.y);
    }

    Vector2 WorldToImageSize(Vector3 worldSize, Camera cam)
    {
        Vector3 min = cam.WorldToViewportPoint(worldSize - cam.transform.position);
        Vector3 max = cam.WorldToViewportPoint(worldSize + cam.transform.position);

        return new Vector2(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y));
    }
}
