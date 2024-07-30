using System.Collections;
using System.Collections.Generic;
// using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ObjectSpawner : MonoBehaviour
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

        // Button에 클릭 이벤트 등록
        Button button = GameObject.Find("Random").GetComponent<Button>();
        button.onClick.AddListener(SpawnObjects);
    }

    void SpawnObjects()
    {
        // randValues
        int floorTextureRand = Random.Range(1, 5);
        int realFloorTextureRand = Random.Range(1, 5);
        int longiTextureRand = Random.Range(1, 5);
        int plateTextureRand = Random.Range(1, 5);

        // 론지 사이 랜덤 거리 생성 / mm 로 변환
        float randomDis = Random.Range(500, 851) * 0.05F;

        // Suffixes
        string[] longiSuffixes = { "LF", "LA", "LT" };
        string[] plateSuffixes = { "CP01", "CP02", "CP03", "CP04", "CP05", "CP06" };
        string[] slotHoleSuffixesA = { "AH", "AA", "AG", "AJ" };
        string[] slotHoleSuffixesT = { "TE", "TG" };

        // longi
        // int Longi1 = Random.Range(1, 26);
        int Longi1 = Random.Range(1, 26);
        int Longi2 = Longi1;

        // 10% 확률로 Longi2를 다른 값으로 설정
        if (Random.value < 0.1f)
        {
            // Longi2 = Random.Range(1, 26);
            Longi2 = Random.Range(1, 18);
            while (Longi2 == Longi1)
            {
                // Longi2 = Random.Range(1, 26);
                Longi2 = Random.Range(1, 18);
            }
        }

        // Suffixes 선택
        int longiRandomSuffixIndex1 = Random.Range(0, longiSuffixes.Length);
        int longiRandomSuffixIndex2 = (Longi1 != Longi2) ? Random.Range(0, longiSuffixes.Length) : longiRandomSuffixIndex1;
        string randomLongiSuffix1 = longiSuffixes[longiRandomSuffixIndex1];
        string randomLongiSuffix2 = longiSuffixes[longiRandomSuffixIndex2];

        string[] selectedSlotHoleSuffixes1 = (randomLongiSuffix1 == "LF" || randomLongiSuffix1 == "LA") ? slotHoleSuffixesA : slotHoleSuffixesT;
        string[] selectedSlotHoleSuffixes2 = (randomLongiSuffix2 == "LF" || randomLongiSuffix2 == "LA") ? slotHoleSuffixesA : slotHoleSuffixesT;

        int sloteHoleRandomSuffixIndex1 = Random.Range(0, selectedSlotHoleSuffixes1.Length);
        int sloteHoleRandomSuffixIndex2 = (Longi1 != Longi2) ? Random.Range(0, selectedSlotHoleSuffixes2.Length) : sloteHoleRandomSuffixIndex1;
        string randomSlotHoleSuffix1 = selectedSlotHoleSuffixes1[sloteHoleRandomSuffixIndex1];
        string randomSlotHoleSuffix2 = selectedSlotHoleSuffixes2[sloteHoleRandomSuffixIndex2];

        int plateRandomSuffixIndex1 = Random.Range(0, plateSuffixes.Length);
        int plateRandomSuffixIndex2 = (Longi1 != Longi2) ? Random.Range(0, plateSuffixes.Length) : plateRandomSuffixIndex1;
        string randomPlateSuffix1 = plateSuffixes[plateRandomSuffixIndex1];
        string randomPlateSuffix2 = plateSuffixes[plateRandomSuffixIndex2];

        // plate 여부
        // 0 : 없음
        // 1 : 왼쪽 론지에만 생성
        // 2 : 오른쪽 론지에만 생성
        int plateChoice = Random.Range(0, 3);

        // Texture 지정
        Texture2D floorTexture = Resources.Load<Texture2D>($"Textures/floor{floorTextureRand}");
        Texture2D realFloorTexture = Resources.Load<Texture2D>($"Textures/floor{realFloorTextureRand}");
        Texture2D longiTexture = Resources.Load<Texture2D>($"Textures/longi{longiTextureRand}");
        Texture2D sloteHoleTexture = Resources.Load<Texture2D>("Textures/myTexture");
        Texture2D plateTexture = Resources.Load<Texture2D>($"Textures/plate{plateTextureRand}");

        // Shader 지정
        Shader floorShader = Shader.Find("Custom/FloorShader");
        Shader longiShader = Shader.Find("Custom/FloorShader");
        Shader sloteHoleShader = Shader.Find("Custom/StencilMask");
        Shader plateShader = Shader.Find("Custom/FloorShader");

        // longi
        string longiFileName1 = $"longi_{Longi1}{randomLongiSuffix1}";
        string longiFileName2 = $"longi_{Longi2}{randomLongiSuffix2}";

        // slothole
        string slotHoleFileName1 = $"slot_hole_{Longi1}{randomSlotHoleSuffix1}";
        string slotHoleFileName2 = $"slot_hole_{Longi2}{randomSlotHoleSuffix2}";

        // plate
        string plateFileName1 = $"plate{Longi1}_{randomPlateSuffix1}";
        string plateFileName2 = $"plate{Longi2}_{randomPlateSuffix2}";

        // Resources 폴더에서 모델을 로드
        GameObject longiModel1 = Resources.Load<GameObject>($"longi/{longiFileName1}");
        GameObject longiModel2 = Resources.Load<GameObject>($"longi/{longiFileName2}");
        GameObject slotHoleModel1 = Resources.Load<GameObject>($"slot_hole/{slotHoleFileName1}");
        GameObject slotHoleModel2 = Resources.Load<GameObject>($"slot_hole/{slotHoleFileName2}");
        GameObject plateModel1 = Resources.Load<GameObject>($"plate/{plateFileName1}");
        GameObject plateModel2 = Resources.Load<GameObject>($"plate/{plateFileName2}");
        GameObject floorModel = Resources.Load<GameObject>("floor/floor");
        GameObject realFloorModel = Resources.Load<GameObject>("floor/real_floor");

        // make R hole
        makeRHole(Longi1, randomSlotHoleSuffix1, randomDis);
        makeRHole(Longi2, randomSlotHoleSuffix2, randomDis);

        // 원하는 좌표에 오브젝트를 생성
        // 1 => 1m, 1m 만큼 앞 뒤로 이동
        Vector3 spawnLongi1 = new Vector3(-100, 0, -randomDis);
        Vector3 spawnLongi2 = new Vector3(100, 0, randomDis);

        Vector3 spawnSlotHole1 = new Vector3(0, 0, -randomDis);
        Vector3 spawnSlotHole2 = new Vector3(0, 0, randomDis);

        Vector3 spawnFloor = new Vector3(0, 0, 0);
        Vector3 spawnRealFloor = new Vector3(0, 0, 0);

        // longi
        GameObject longiInstance1 = Instantiate(longiModel1, spawnLongi1, Quaternion.identity);
        GameObject longiInstance2 = Instantiate(longiModel2, spawnLongi2, Quaternion.Euler(0, 180, 0));
        longiInstance1.transform.localScale *= 100;
        longiInstance2.transform.localScale *= 100;
        Material longiMaterial = new Material(longiShader);
        longiMaterial.mainTexture = longiTexture;

        switch (longiTextureRand)
        {
            case 1:
                SetTiling(longiMaterial, 2.0f, 2.0f, 0.0f, 0.06f);
                break;
            case 2:
                SetTiling(longiMaterial, 2.0f, 2.0f, 0.0f, 0.0f);
                break;
            case 3:
                SetTiling(longiMaterial, 2.0f, 2.0f, 0.0f, 0.0f);
                break;
            case 4:
                SetTiling(longiMaterial, 2.0f, 2.2f, 0.0f, 0.0f);
                break;
        }

        ApplyMaterial(longiInstance1, longiMaterial);
        ApplyMaterial(longiInstance2, longiMaterial);
        SetLayer(longiInstance1, 6);
        SetLayer(longiInstance2, 6);

        // sloteHole
        GameObject slotHoleInstance1 = Instantiate(slotHoleModel1, spawnSlotHole1, Quaternion.identity);
        GameObject slotHoleInstance2 = Instantiate(slotHoleModel2, spawnSlotHole2, Quaternion.Euler(0, 180, 0));
        slotHoleInstance1.transform.localScale *= 100;
        slotHoleInstance2.transform.localScale *= 100;
        Material slotHoleMaterial = new Material(sloteHoleShader);
        slotHoleMaterial.mainTexture = sloteHoleTexture;
        slotHoleMaterial.SetInt("_StencilID", 1);
        ApplyMaterial(slotHoleInstance1, slotHoleMaterial);
        ApplyMaterial(slotHoleInstance2, slotHoleMaterial);
        SetLayer(slotHoleInstance1, 6);
        SetLayer(slotHoleInstance2, 6);

        // floor
        GameObject floorInstance = Instantiate(floorModel, spawnFloor, Quaternion.identity);
        GameObject realFloorInstance = Instantiate(realFloorModel, spawnRealFloor, Quaternion.identity);
        floorInstance.transform.localScale *= 100;
        realFloorInstance.transform.localScale *= 100;
        Material floorMaterial = new Material(floorShader);
        Material realFloorMaterial = new Material(floorShader);
        floorMaterial.mainTexture = floorTexture;
        realFloorMaterial.mainTexture = realFloorTexture;

        switch (floorTextureRand)
        {
            case 1:
                SetTiling(floorMaterial, 0.4f, 0.4f, 0.1f, 0.0f);
                break;
            case 2:
                SetTiling(floorMaterial, 0.23f, 0.25f, 0.5f, 0.0f);
                break;
            case 3:
                SetTiling(floorMaterial, 0.228f, 0.46f, 0.5f, 0.0f);
                break;
            case 4:
                SetTiling(floorMaterial, 0.229f, 0.46f, 0.5f, 0.0f);
                break;
        }

        switch (realFloorTextureRand)
        {
            case 1:
                SetTiling(realFloorMaterial, 0.01f, 0.01f, 0.1f, 0.4f);
                break;
            case 2:
                SetTiling(realFloorMaterial, 0.01f, 0.01f, 0.1f, 0.4f);
                break;
            case 3:
                SetTiling(realFloorMaterial, 0.01f, 0.012f, 0.4f, 0.48f);
                break;
            case 4:
                SetTiling(realFloorMaterial, 0.0125f, 0.0125f, 0.5f, 0.5f);
                break;
        }

        ApplyMaterial(floorInstance, floorMaterial);
        ApplyMaterial(realFloorInstance, realFloorMaterial);
        SetLayer(floorInstance, 0);
        SetLayer(realFloorInstance, 6);

        // plate
        float thick_w1 = 0.1f * getLongiThick_w(Longi1);
        float thick_w2 = 0.1f * getLongiThick_w(Longi2);

        Vector3 spawnPlate1 = new Vector3(1f, 0, -randomDis + thick_w1);
        Vector3 spawnPlate2 = new Vector3(1f, 0, randomDis - thick_w2);

        Vector3 spawnPlate3 = new Vector3(1f, 0, randomDis);
        Vector3 spawnPlate4 = new Vector3(1f, 0, -randomDis);

        Material plateMaterial = new Material(plateShader);

        switch (plateTextureRand)
        {
            case 1:
                SetTiling(plateMaterial, 0.34f, 0.45f, 0.0f, 0.0f);
                break;
            case 2:
                SetTiling(plateMaterial, 0.34f, 0.45f, 0.0f, 0.0f);
                break;
            case 3:
                SetTiling(plateMaterial, 0.33f, 0.4f, 0.0f, 0.0f);
                break;
            case 4:
                SetTiling(plateMaterial, 0.3f, 0.3f, 0.0f, 0.0f);
                break;
        }

        // plateChoice = 2;

        if (plateChoice == 1)
        {
            if (randomSlotHoleSuffix1 == "AH" || randomSlotHoleSuffix1 == "TE" || randomSlotHoleSuffix1 == "AA")
            {
                GameObject plateInstance1 = Instantiate(plateModel1, spawnPlate1, Quaternion.identity);
                plateInstance1.transform.localScale *= 100;
                plateMaterial.mainTexture = plateTexture;
                ApplyMaterial(plateInstance1, plateMaterial);
                SetLayer(plateInstance1, 6);
            }
            else if (randomSlotHoleSuffix1 == "AG")
            {
                GameObject plateInstance1 = Instantiate(plateModel1, spawnPlate3, Quaternion.identity);
                plateInstance1.transform.localScale *= 100;
                plateMaterial.mainTexture = plateTexture;
                ApplyMaterial(plateInstance1, plateMaterial);
                SetLayer(plateInstance1, 6);
            }
        }

        else if (plateChoice == 2)
        {
            if (randomSlotHoleSuffix2 == "AH" || randomSlotHoleSuffix2 == "TE" || randomSlotHoleSuffix2 == "AA")
            {
                GameObject plateInstance2 = Instantiate(plateModel2, spawnPlate2, Quaternion.Euler(0, 180, 0));
                plateInstance2.transform.localScale *= 100;
                plateMaterial.mainTexture = plateTexture;
                ApplyMaterial(plateInstance2, plateMaterial);
                SetLayer(plateInstance2, 6);
            }
            else if (randomSlotHoleSuffix2 == "AG")
            {
                GameObject plateInstance2 = Instantiate(plateModel2, spawnPlate4, Quaternion.Euler(0, 180, 0));
                plateInstance2.transform.localScale *= 100;
                plateMaterial.mainTexture = plateTexture;
                ApplyMaterial(plateInstance2, plateMaterial);
                SetLayer(plateInstance2, 6);
            }
        }

        // UI 비활성화
        DisableCanvas("Canvas");

        // 스크린샷 찍기
        TakeScreenshot();

    }

    void makeRHole(int index, string sloteHoleSuffix, float dis)
    {
        // if rand == 0 , not make
        // if rand == 1, make left "AA", "TG"
        // if rand == 2, make right "AJ"
        // 2의 경우 
        int r_rand = 0;
        int radius = 0;
        int height = getLongiHeight(index);
        float thick_w = 0.1f * getLongiThick_w(index);

        if (sloteHoleSuffix == "AA" || sloteHoleSuffix == "TG")
        {
            r_rand = Random.Range(0, 2);
            radius = getRadius(height);
        }
        if (sloteHoleSuffix == "AJ")
        {
            r_rand = Random.Range(0, 3);
            radius = getRadius1(height);
        }

        if (r_rand == 0 || radius == 0)
        {
            return;
        }

        Texture2D rHoleTexture = Resources.Load<Texture2D>("Textures/myTexture");
        Shader rHoleShader = Shader.Find("Custom/StencilMask");
        Vector3 spawnRHole1 = new Vector3(0, 0, -dis + thick_w);
        Vector3 spawnRHole2 = new Vector3(0, 0, dis - thick_w);

        Vector3 spawnRHole3 = new Vector3(0, 0, dis);
        Vector3 spawnRHole4 = new Vector3(0, 0, -dis);

        GameObject rHoleModel = Resources.Load<GameObject>($"r_hole/r_{radius}");

        GameObject rHoleInstance1 = null;
        GameObject rHoleInstance2 = null;
        if (r_rand == 1)
        {

            rHoleInstance1 = Instantiate(rHoleModel, spawnRHole1, Quaternion.identity);
            rHoleInstance2 = Instantiate(rHoleModel, spawnRHole2, Quaternion.Euler(0, 180, 0));
        }
        else if (r_rand == 2)
        {
            rHoleInstance1 = Instantiate(rHoleModel, spawnRHole3, Quaternion.identity);
            rHoleInstance2 = Instantiate(rHoleModel, spawnRHole4, Quaternion.Euler(0, 180, 0));
        }

        rHoleInstance1.transform.localScale *= 100;
        rHoleInstance2.transform.localScale *= 100;

        Material rHoleMaterial = new Material(rHoleShader);
        rHoleMaterial.mainTexture = rHoleTexture;
        rHoleMaterial.SetInt("_StencilID", 1);
        ApplyMaterial(rHoleInstance1, rHoleMaterial);
        ApplyMaterial(rHoleInstance2, rHoleMaterial);
        SetLayer(rHoleInstance1, 6);
        SetLayer(rHoleInstance2, 6);
    }

    int getRadius(int height)
    {
        if (height <= 200)
            return 0;
        else if (height <= 300)
            return 50;
        else if (height < 450)
            return 75;
        else
            return 100;
    }

    int getRadius1(int height)
    {
        if (height < 250)
            return 0;
        else if (height < 350)
            return 50;
        else if (height < 450)
            return 75;
        else
            return 100;
    }

    int getLongiHeight(int index)
    {
        switch (index)
        {
            case 1: return 70;
            case 2: return 75;
            case 3: return 100;
            case 4: return 100;
            case 5: return 130;
            case 6: return 150;
            case 7: return 150;
            case 8: return 200;
            case 9: return 200;
            case 10: return 200;
            case 11: return 250;
            case 12: return 250;
            case 13: return 300;
            case 14: return 300;
            case 15: return 350;
            case 16: return 400;
            case 17: return 400;
            case 18: return 450;
            case 19: return 450;
            case 20: return 100;
            case 21: return 100;
            case 22: return 125;
            case 23: return 125;
            case 24: return 125;
            case 25: return 150;
            case 26: return 150;
            default: return 0;
        }
    }

    float getLongiThick_w(int index)
    {
        switch (index)
        {
            case 1: return 6f;
            case 2: return 6f;
            case 3: return 10f;
            case 4: return 13f;
            case 5: return 15f;
            case 6: return 10f;
            case 7: return 15f;
            case 8: return 15f;
            case 9: return 9f;
            case 10: return 10f;
            case 11: return 10f;
            case 12: return 12f;
            case 13: return 11f;
            case 14: return 13f;
            case 15: return 12f;
            case 16: return 11.5f;
            case 17: return 13f;
            case 18: return 11.5f;
            case 19: return 11.5f;
            case 20: return 7f;
            case 21: return 10f;
            case 22: return 7f;
            case 23: return 10f;
            case 24: return 13f;
            case 25: return 9f;
            case 26: return 12f;
            default: return 0;
        }
    }

    void ApplyMaterial(GameObject obj, Material mat)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = mat;
        }
        else
        {
            foreach (Transform child in obj.transform)
            {
                ApplyMaterial(child.gameObject, mat);
            }
        }
    }

    void SetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayer(child.gameObject, layer);
        }
    }

    void SetTiling(Material mat, float tilingX, float tilingY, float offsetX, float offsetY)
    {
        if (mat.shader.name.Contains("Universal Render Pipeline"))
        {
            mat.SetFloat("_BaseMap_ST_X", tilingX);
            mat.SetFloat("_BaseMap_ST_Y", tilingY);
            mat.SetFloat("_BaseMap_ST_Z", tilingX);
            mat.SetFloat("_BaseMap_ST_W", tilingY);
        }
        else
        {
            mat.mainTextureScale = new Vector2(tilingX, tilingY);
            mat.mainTextureOffset = new Vector2(offsetX, offsetY);
        }
    }

    private void DisableCanvas(string canvasName)
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
                canvas.enabled = false;
                Debug.Log($"{canvasName} is now disabled.");
            }
            else
            {
                Debug.LogError($"No Canvas component found on {canvasName}.");
            }
        }
        else
        {
            Debug.LogError($"No GameObject found with the name {canvasName}.");
        }
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
