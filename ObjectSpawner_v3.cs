using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    void Start()
    {
        int floorTextureRand = Random.Range(1, 3);
        int longiTextureRand = Random.Range(1, 3);
        int plateTextureRand = Random.Range(1, 3);

        Texture2D floorTexture = Resources.Load<Texture2D>($"Textures/floor{floorTextureRand}");
        Texture2D realFloorTexture = Resources.Load<Texture2D>($"Textures/floor{floorTextureRand}");
        Texture2D longiTexture = Resources.Load<Texture2D>($"Textures/longi{longiTextureRand}");
        Texture2D sloteHoleTexture = Resources.Load<Texture2D>("Textures/myTexture");
        Texture2D plateTexture = Resources.Load<Texture2D>($"Textures/plate{plateTextureRand}");


        Shader floorShader = Shader.Find("Custom/FloorShader");
        Shader longiShader = Shader.Find("Custom/FloorShader");
        Shader sloteHoleShader = Shader.Find("Custom/StencilMask");
        Shader plateShader = Shader.Find("Custom/FloorShader");

        // 랜덤으로 1부터 26 사이의 숫자 선택
        // int randomIndex = Random.Range(1, 27);
        int randomIndex = Random.Range(1, 27);

        // 론지 사이 랜덤 거리 생성 / mm 로 변환
        float randomDis = Random.Range(500, 851) * 0.0005F;

        // longi 파일 이름 생성
        string[] longiSuffixes = { "LF", "LA", "LT" };
        string randomLongiSuffix = longiSuffixes[Random.Range(0, longiSuffixes.Length)];
        string longiFileName = $"longi_{randomIndex}{randomLongiSuffix}";

        // slot_hole 파일 이름 생성
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

        string randomSlotHoleSuffix = selectedSlotHoleSuffixes[Random.Range(0, selectedSlotHoleSuffixes.Length)];
        string slotHoleFileName = $"slot_hole_{randomIndex}{randomSlotHoleSuffix}";

        // plate 파일 이름 생성
        // string[] plateSuffixes = { "CP01", "CP02","CP03","CP04","CP05","CP06" };
        string[] plateSuffixes = { "CP01" };
        string randomPlateSuffix = plateSuffixes[Random.Range(0, plateSuffixes.Length)];
        // string plateFileName = $"plate_{randomIndex}_{randomPlateSuffix}";
        string plateFileName = $"plate_18_{randomPlateSuffix}";

        // Resources 폴더에서 모델을 로드합니다.
        GameObject longiModel = Resources.Load<GameObject>($"longi/{longiFileName}");
        GameObject slotHoleModel = Resources.Load<GameObject>($"slot_hole/{slotHoleFileName}");
        GameObject plateModel = Resources.Load<GameObject>($"plate/{plateFileName}");
        GameObject floorModel = Resources.Load<GameObject>("floor/floor");
        GameObject realFloorModel = Resources.Load<GameObject>("floor/real_floor");

        // make R hole
        makeRHole(randomIndex, randomSlotHoleSuffix, randomDis);


        // 원하는 좌표에 오브젝트를 생성합니다.
        // 1 => 1m, 1m 만큼 앞 뒤로 이동
        Vector3 spawnLongi1 = new Vector3(-1, 0, -randomDis);
        Vector3 spawnLongi2 = new Vector3(1, 0, randomDis);

        Vector3 spawnSlotHole1 = new Vector3(0, 0, -randomDis);
        Vector3 spawnSlotHole2 = new Vector3(0, 0, randomDis);

        Vector3 spawnPlate1 = new Vector3(0.01f, 0, -randomDis);
        Vector3 spawnPlate2 = new Vector3(0.01f, 0, randomDis);

        Vector3 spawnFloor = new Vector3(0, 0, 0);
        Vector3 spawnRealFloor = new Vector3(0, 0, 0);


        // longi
        GameObject longiInstance1 = Instantiate(longiModel, spawnLongi1, Quaternion.identity);
        GameObject longiInstance2 = Instantiate(longiModel, spawnLongi2, Quaternion.Euler(0, 180, 0));
        Material longiMaterial = new Material(longiShader);
        longiMaterial.mainTexture = longiTexture;
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
        floorMaterial.mainTexture = floorTexture;
        ApplyMaterial(floorInstance, floorMaterial);
        ApplyMaterial(realFloorInstance, floorMaterial);
        SetLayer(floorInstance, 0);
        SetLayer(realFloorInstance, 6);

        // make plate left, right or none
        int plateChoice = Random.Range(0, 3);
        if (plateChoice == 1)
        {
            GameObject plateInstance1 = Instantiate(plateModel, spawnPlate1, Quaternion.identity);
            Material plateMaterial = new Material(plateShader);
            plateMaterial.mainTexture = plateTexture;
            ApplyMaterial(plateInstance1, plateMaterial);
            SetLayer(plateInstance1, 0);
        }
        else if (plateChoice == 2)
        {
            GameObject plateInstance2 = Instantiate(plateModel, spawnPlate2, Quaternion.Euler(0, 180, 0));
            Material plateMaterial = new Material(plateShader);
            plateMaterial.mainTexture = plateTexture;
            ApplyMaterial(plateInstance2, plateMaterial);
            SetLayer(plateInstance2, 0);
        }

    }

    void makeRHole(int index, string sloteHoleSuffix, float dis)
    {
        // if rand == 0 , not make
        // if rand == 1, make left
        // if rand == 2, make right
        int r_rand = 0;
        int radius = 0;
        int height = getLongiHeight(index);

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
        Vector3 spawnRHole1 = new Vector3(0, 0, dis);
        Vector3 spawnRHole2 = new Vector3(0, 0, -dis);

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
            rHoleInstance1 = Instantiate(rHoleModel, spawnRHole1, Quaternion.Euler(0, 180, 0));
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
}
