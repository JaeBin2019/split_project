using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    void Start()
    {
        // randValues
        int floorTextureRand = Random.Range(1, 5);
        int realFloorTextureRand = Random.Range(1, 5);
        int longiTextureRand = Random.Range(1, 5);
        int plateTextureRand = Random.Range(1, 5);

        // 랜덤으로 1부터 26 사이의 숫자 선택
        // int randomIndexForAll = Random.Range(1, 27);
        int randomIndexForAll = Random.Range(1, 18);

        // 론지 사이 랜덤 거리 생성 / mm 로 변환
        float randomDis = Random.Range(500, 851) * 0.0005F;

        // Suffixes
        string[] longiSuffixes = { "LF", "LA", "LT" };
        int longiRandomSuffixIndex = Random.Range(0, longiSuffixes.Length);
        string randomLongiSuffix = longiSuffixes[longiRandomSuffixIndex];

        string[] plateSuffixes = { "CP01", "CP02", "CP03", "CP04", "CP05", "CP06" };

        string[] slotHoleSuffixesA = { "AH", "AA", "AG", "AJ" };
        string[] slotHoleSuffixesT = { "TE", "TG" };
        string[] selectedSlotHoleSuffixes = { };

        if (randomLongiSuffix == "LF" || randomLongiSuffix == "LA")
        {
            selectedSlotHoleSuffixes = slotHoleSuffixesA;
        }
        else if (randomLongiSuffix == "LT")
        {
            selectedSlotHoleSuffixes = slotHoleSuffixesT;
        }

        int sloteHoleRandomSuffixIndex = Random.Range(0, selectedSlotHoleSuffixes.Length);
        int plateRandomSuffixIndex = Random.Range(0, plateSuffixes.Length);

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
        string longiFileName = $"longi_{randomIndexForAll}{randomLongiSuffix}";

        // slothole
        string randomSlotHoleSuffix = selectedSlotHoleSuffixes[sloteHoleRandomSuffixIndex];
        string slotHoleFileName = $"slot_hole_{randomIndexForAll}{randomSlotHoleSuffix}";

        // plate
        string randomPlateSuffix = plateSuffixes[plateRandomSuffixIndex];
        string plateFileName = $"plate{randomIndexForAll}_{randomPlateSuffix}";

        // Resources 폴더에서 모델을 로드
        GameObject longiModel = Resources.Load<GameObject>($"longi/{longiFileName}");
        GameObject slotHoleModel = Resources.Load<GameObject>($"slot_hole/{slotHoleFileName}");
        GameObject plateModel = Resources.Load<GameObject>($"plate/{plateFileName}");
        GameObject floorModel = Resources.Load<GameObject>("floor/floor");
        GameObject realFloorModel = Resources.Load<GameObject>("floor/real_floor");

        // make R hole
        makeRHole(randomIndexForAll, randomSlotHoleSuffix, randomDis);

        // 원하는 좌표에 오브젝트를 생성
        // 1 => 1m, 1m 만큼 앞 뒤로 이동
        Vector3 spawnLongi1 = new Vector3(-1, 0, -randomDis);
        Vector3 spawnLongi2 = new Vector3(1, 0, randomDis);

        Vector3 spawnSlotHole1 = new Vector3(0, 0, -randomDis);
        Vector3 spawnSlotHole2 = new Vector3(0, 0, randomDis);

        Vector3 spawnFloor = new Vector3(0, 0, 0);
        Vector3 spawnRealFloor = new Vector3(0, 0, 0);


        // longi
        GameObject longiInstance1 = Instantiate(longiModel, spawnLongi1, Quaternion.identity);
        GameObject longiInstance2 = Instantiate(longiModel, spawnLongi2, Quaternion.Euler(0, 180, 0));
        Material longiMaterial = new Material(longiShader);
        longiMaterial.mainTexture = longiTexture;

        switch (longiTextureRand)
        {
            case 1:
                SetTiling(longiMaterial, 2.0f, 3.6f, 0.0f, 0.07f);
                break;
            case 2:
                SetTiling(longiMaterial, 2.0f, 3.7f, 0.0f, 0.0f);
                break;
            case 3:
                SetTiling(longiMaterial, 2.0f, 3.9f, 0.0f, 0.0f);
                break;
            case 4:
                SetTiling(longiMaterial, 2.0f, 4.5f, 0.0f, 0.0f);
                break;
        }

        ApplyMaterial(longiInstance1, longiMaterial);
        ApplyMaterial(longiInstance2, longiMaterial);
        SetLayer(longiInstance1, 6);
        SetLayer(longiInstance2, 6);

        // sloteHole
        GameObject slotHoleInstance1 = Instantiate(slotHoleModel, spawnSlotHole1, Quaternion.identity);
        GameObject slotHoleInstance2 = Instantiate(slotHoleModel, spawnSlotHole2, Quaternion.Euler(0, 180, 0));
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
        float thick_w = 0.001f * getLongiThick_w(randomIndexForAll);

        Vector3 spawnPlate1 = new Vector3(0.01f, 0, -randomDis + thick_w);
        Vector3 spawnPlate2 = new Vector3(0.01f, 0, randomDis - thick_w);

        if (plateChoice == 1)
        {
            GameObject plateInstance1 = Instantiate(plateModel, spawnPlate1, Quaternion.identity);
            Material plateMaterial = new Material(plateShader);

            switch (plateTextureRand)
            {
                case 1:
                    SetTiling(plateMaterial, 3.14f, 2.42f, 0.0f, 0.0f);
                    break;
                case 2:
                    SetTiling(plateMaterial, 3.13f, 2.42f, 0.0f, 0.0f);
                    break;
                case 3:
                    SetTiling(plateMaterial, 3.75f, 2.4f, 0.0f, 0.0f);
                    break;
                case 4:
                    SetTiling(plateMaterial, 3.6f, 2.4f, 0.0f, 0.0f);
                    break;
            }

            plateMaterial.mainTexture = plateTexture;
            ApplyMaterial(plateInstance1, plateMaterial);
            SetLayer(plateInstance1, 6);
        }
        else if (plateChoice == 2)
        {
            GameObject plateInstance2 = Instantiate(plateModel, spawnPlate2, Quaternion.Euler(0, 180, 0));
            Material plateMaterial = new Material(plateShader);
            plateMaterial.mainTexture = plateTexture;
            ApplyMaterial(plateInstance2, plateMaterial);
            SetLayer(plateInstance2, 6);
        }
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
        float thick_w = 0.001f * getLongiThick_w(index);

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

        /*
            여기서 r_rand 값 입력값으로 변경
           
        */
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
        // 만드는 중
        else if (r_rand == 2)
        {
            rHoleInstance1 = Instantiate(rHoleModel, spawnRHole3, Quaternion.identity);
            rHoleInstance2 = Instantiate(rHoleModel, spawnRHole4, Quaternion.Euler(0, 180, 0));
        }

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
}
