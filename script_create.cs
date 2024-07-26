using System.Collections;
using System.Collections.Generic;
// using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;  // Button을 사용하기 위해 추가
using TMPro;

public class ObjectSpawner1 : MonoBehaviour
{
    void Start()
    {
        // Button에 클릭 이벤트 등록
        Button button = GameObject.Find("Create").GetComponent<Button>();
        button.onClick.AddListener(SpawnObjects);
    }

    public void SpawnObjects()
    {
        TMP_InputField longiNum = GameObject.Find("Longi_Number").GetComponent<TMP_InputField>();
        TMP_Dropdown longiType = GameObject.Find("Longi_Type").GetComponent<TMP_Dropdown>();
        TMP_Dropdown slotHoleType = GameObject.Find("Slot_hole_Type").GetComponent<TMP_Dropdown>();
        TMP_Dropdown rHole = GameObject.Find("R_Hole").GetComponent<TMP_Dropdown>();
        TMP_Dropdown plateType = GameObject.Find("Plate_Type").GetComponent<TMP_Dropdown>();
        TMP_Dropdown plateLocation = GameObject.Find("Plate_Location").GetComponent<TMP_Dropdown>();
        TMP_InputField longiDis = GameObject.Find("Longi_Distance").GetComponent<TMP_InputField>();


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

        int longiNumValue = 0;
        if (longiNum != null)
        {
            Debug.Log("TMP_InputField found!");

            // TMP_InputField의 텍스트 값을 가져옵니다.
            string inputText = longiNum.text;

            // 텍스트 값을 int 형으로 변환합니다.
            if (int.TryParse(inputText, out longiNumValue))
            {
                Debug.Log("Converted value: " + longiNumValue);
            }
            else
            {
                Debug.LogError("Failed to convert input text to int.");
            }
        }
        else
        {
            Debug.LogError("TMP_InputField not found.");
        }


        float longiDisValue = 0;
        if (longiDis != null)
        {
            Debug.Log("TMP_InputField found!");

            // TMP_InputField의 텍스트 값을 가져옵니다.
            string inputText = longiDis.text;

            // 텍스트 값을 float 형으로 변환합니다.
            if (float.TryParse(inputText, out longiDisValue))
            {
                Debug.Log("Converted value: " + longiDisValue);
            }
            else
            {
                Debug.LogError("Failed to convert input text to float.");
            }
        }
        else
        {
            Debug.LogError("TMP_InputField not found.");
        }

        if (longiDisValue >= 500f && longiDisValue <= 850f)
        {
            Debug.Log("Converted value: " + longiDisValue);
        }
        else
        {
            Debug.LogError("Value out of range. Value should be between 500 and 850.");
        }
        longiDisValue = longiDisValue * 0.0005F;

        // int randomIndex = Random.Range(1, 27);
        // float randomDis = Random.Range(500, 851) * 0.0005F;

        string[] longiSuffixes = { "LF", "LA", "LT" };
        // string randomLongiSuffix = longiSuffixes[Random.Range(0, longiSuffixes.Length)];
        string randomLongiSuffix = longiType.options[longiType.value].text;
        string longiFileName = $"longi_{longiNumValue}{randomLongiSuffix}";

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

        // string randomSlotHoleSuffix = selectedSlotHoleSuffixes[Random.Range(0, selectedSlotHoleSuffixes.Length)];
        string randomSlotHoleSuffix = slotHoleType.options[slotHoleType.value].text;
        string slotHoleFileName = $"slot_hole_{longiNumValue}{randomSlotHoleSuffix}";

        string[] plateSuffixes = { "CP01" };
        // string randomPlateSuffix = plateSuffixes[Random.Range(0, plateSuffixes.Length)];
        string randomPlateSuffix = plateType.options[plateType.value].text;
        string plateFileName = $"plate_18_{randomPlateSuffix}";

        GameObject longiModel = Resources.Load<GameObject>($"longi/{longiFileName}");
        GameObject slotHoleModel = Resources.Load<GameObject>($"slot_hole/{slotHoleFileName}");
        GameObject plateModel = Resources.Load<GameObject>($"plate/{plateFileName}");
        GameObject floorModel = Resources.Load<GameObject>("floor/floor");
        GameObject realFloorModel = Resources.Load<GameObject>("floor/real_floor");

        int selectedValue = 0;
        if (rHole != null)
        {
            Debug.Log("TMP_Dropdown found!");

            // 선택된 옵션의 값을 가져옵니다.
            string selectedOption = rHole.options[rHole.value].text;

            // 선택된 옵션의 값에 따라 정수 값을 할당합니다.
            switch (selectedOption)
            {
                case "None":
                    selectedValue = 0;
                    break;
                case "Right":
                    selectedValue = 1;
                    break;
                case "Left":
                    selectedValue = 2;
                    break;
                default:
                    Debug.LogError("Unknown option selected.");
                    selectedValue = 0; // 알 수 없는 옵션의 경우
                    break;
            }

            Debug.Log("Selected option: " + selectedOption + ", Assigned value: " + selectedValue);
        }
        else
        {
            Debug.LogError("TMP_Dropdown not found.");
        }
    

        makeRHole(longiNumValue, randomSlotHoleSuffix, longiDisValue, selectedValue);

        Vector3 spawnLongi1 = new Vector3(-1, 0, -longiDisValue);
        Vector3 spawnLongi2 = new Vector3(1, 0, longiDisValue);

        Vector3 spawnSlotHole1 = new Vector3(0, 0, -longiDisValue);
        Vector3 spawnSlotHole2 = new Vector3(0, 0, longiDisValue);

        Vector3 spawnPlate1 = new Vector3(0.01f, 0, -longiDisValue);
        Vector3 spawnPlate2 = new Vector3(0.01f, 0, longiDisValue);

        Vector3 spawnFloor = new Vector3(0, 0, 0);
        Vector3 spawnRealFloor = new Vector3(0, 0, 0);

        GameObject longiInstance1 = Instantiate(longiModel, spawnLongi1, Quaternion.identity);
        GameObject longiInstance2 = Instantiate(longiModel, spawnLongi2, Quaternion.Euler(0, 180, 0));
        Material longiMaterial = new Material(longiShader);
        longiMaterial.mainTexture = longiTexture;
        ApplyMaterial(longiInstance1, longiMaterial);
        ApplyMaterial(longiInstance2, longiMaterial);
        SetLayer(longiInstance1, 6);
        SetLayer(longiInstance2, 6);

        GameObject slotHoleInstance1 = Instantiate(slotHoleModel, spawnSlotHole1, Quaternion.identity);
        GameObject slotHoleInstance2 = Instantiate(slotHoleModel, spawnSlotHole2, Quaternion.Euler(0, 180, 0));
        Material slotHoleMaterial = new Material(sloteHoleShader);
        slotHoleMaterial.mainTexture = sloteHoleTexture;
        slotHoleMaterial.SetInt("_StencilID", 1);
        ApplyMaterial(slotHoleInstance1, slotHoleMaterial);
        ApplyMaterial(slotHoleInstance2, slotHoleMaterial);
        SetLayer(slotHoleInstance1, 6);
        SetLayer(slotHoleInstance2, 6);

        GameObject floorInstance = Instantiate(floorModel, spawnFloor, Quaternion.identity);
        GameObject realFloorInstance = Instantiate(realFloorModel, spawnRealFloor, Quaternion.identity);
        Material floorMaterial = new Material(floorShader);
        floorMaterial.mainTexture = floorTexture;
        ApplyMaterial(floorInstance, floorMaterial);
        ApplyMaterial(realFloorInstance, floorMaterial);
        SetLayer(floorInstance, 0);
        SetLayer(realFloorInstance, 6);

        //int plateChoice = Random.Range(0, 3);
        int plateChoice = 0;
        if (plateLocation != null)
        {
            Debug.Log("TMP_Dropdown found!");

            // 선택된 옵션의 값을 가져옵니다.
            string selectedOption = plateLocation.options[plateLocation.value].text;

            // 선택된 옵션의 값에 따라 정수 값을 할당합니다.
            switch (selectedOption)
            {
                case "None":
                    plateChoice = 0;
                    break;
                case "Right":
                    plateChoice = 1;
                    break;
                case "Left":
                    plateChoice = 2;
                    break;
                default:
                    Debug.LogError("Unknown option selected.");
                    plateChoice = 0; // 알 수 없는 옵션의 경우
                    break;
            }

            Debug.Log("Selected option: " + selectedOption + ", Assigned value: " + plateChoice);
        }
        else
        {
            Debug.LogError("TMP_Dropdown not found.");
        }

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

        // Canvas 오브젝트를 찾아 비활성화 시킵니다.
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

    void makeRHole(int index, string sloteHoleSuffix, float dis, int selectedValue)
    {
        int r_rand = 0;
        int radius = 0;
        int height = getLongiHeight(index);

        if (sloteHoleSuffix == "AA" || sloteHoleSuffix == "TG")
        {
            // 0~2
            r_rand = selectedValue;
            radius = getRadius(height);
        }
        if (sloteHoleSuffix == "AJ")
        {
            // 0~3
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
