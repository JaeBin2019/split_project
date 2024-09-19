using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;  // StreamWriter를 사용하기 위해 추가


// string[] longiSuffixes = { "LF", "LA", "LT" };
// string[] plateSuffixes = { "CP01", "CP02", "CP03", "CP04", "CP05", "CP06" };
// string[] slotHoleSuffixesA = { "AH", "AA", "AG", "AJ" };
// string[] slotHoleSuffixesT = { "TE", "TG" };

public class ObjectSpawner : MonoBehaviour
{
    public ObjectLabeler objectLabeler;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private List<GameObject> objectsToLabel = new List<GameObject>();

    void Start()
    {
        // 초기화 코드
    }

    public void SpawnObjects(int longi1, int longi2, float longiDistance, string longiSuffix1, string longiSuffix2,
        int slotHoleStatus, string slotHoleSuffix1, string slotHoleSuffix2,
        int plateStatus, string plateSuffix, Quaternion plateRotation,
        int r_holeStatus, float cameraDis, float cameraHeight, Vector3 cameraPosition, Vector3 cameraRotation)
    {
        // 오브젝트 생성
        var longi1Instance = new Longi(longi1, longiSuffix1, new Vector3(-100, 0, -longiDistance), Quaternion.identity);
        var longi2Instance = new Longi(longi2, longiSuffix2, new Vector3(100, 0, longiDistance), Quaternion.Euler(0, 180, 0));

        AddObjectToLists(longi1Instance.GetGameObject());
        AddObjectToLists(longi2Instance.GetGameObject());


        if (slotHoleStatus == 1)
        {
            var slotHole1Instance = new SlotHole(longi1, slotHoleSuffix1, new Vector3(0, 0, -longiDistance), Quaternion.identity);
            var slotHole2Instance = new SlotHole(longi2, slotHoleSuffix2, new Vector3(0, 0, longiDistance), Quaternion.Euler(0, 180, 0));

            AddObjectToLists(slotHole1Instance.GetGameObject());
            AddObjectToLists(slotHole2Instance.GetGameObject());
        }

        var floorInstance = new Floor("floor", new Vector3(0, 0, 0), 0, "floor");
        var realFloorInstance = new Floor("real_floor", new Vector3(0, 0, 0), 6, "realFloor");

        AddObjectToLists(floorInstance.GetGameObject());
        AddObjectToLists(realFloorInstance.GetGameObject());

        float thick_w1 = 0.1f * Longi.getLongiThick_w(longi1);


        if (plateStatus == 1)
        {
            Vector3 spawnPlate = new Vector3(1f, 0, -longiDistance + thick_w1);
            var plateInstance = new Plate(longi1, plateSuffix, spawnPlate, plateRotation);
            AddObjectToLists(plateInstance.GetGameObject());
        }

        if (plateStatus == 2)
        {
            Vector3 spawnPlate = new Vector3(1f, 0, longiDistance - thick_w1);
            var plateInstance = new Plate(longi2, plateSuffix, spawnPlate, plateRotation);
            AddObjectToLists(plateInstance.GetGameObject());
        }



        if (r_holeStatus == 1)
        {
            Vector3 spawnRHole1 = new Vector3(0, 0, -longiDistance + thick_w1);
            var rotation1 = Quaternion.Euler(0, 0, 0);
            var r_holeInstance1 = new RHole(longi1, longiDistance, slotHoleSuffix1, spawnRHole1, rotation1);
            AddObjectToLists(r_holeInstance1.GetGameObject());
        }

        if (r_holeStatus == 2)
        {
            Vector3 spawnRHole2 = new Vector3(0, 0, longiDistance - thick_w1);
            var rotation2 = Quaternion.Euler(0, 180, 0);
            var r_holeInstance2 = new RHole(longi2, longiDistance, slotHoleSuffix2, spawnRHole2, rotation2);
            AddObjectToLists(r_holeInstance2.GetGameObject());
        }

        if (r_holeStatus == 3)
        {
            Vector3 spawnRHole1 = new Vector3(0, 0, -longiDistance + thick_w1);
            Vector3 spawnRHole2 = new Vector3(0, 0, longiDistance - thick_w1);
            var rotation1 = Quaternion.Euler(0, 0, 0);
            var rotation2 = Quaternion.Euler(0, 180, 0);

            var r_holeInstance1 = new RHole(longi1, longiDistance, slotHoleSuffix1, spawnRHole1, rotation1);
            var r_holeInstance2 = new RHole(longi2, longiDistance, slotHoleSuffix2, spawnRHole2, rotation2);
            AddObjectToLists(r_holeInstance1.GetGameObject());
            AddObjectToLists(r_holeInstance2.GetGameObject());
        }

        // ObjectLabeler에 객체 리스트 전달
        if (objectLabeler != null)
        {
            objectLabeler.objectsToLabel = objectsToLabel;
        }
        else
        {
            Debug.LogError("ObjectLabeler reference not set in ObjectSpawner!");
        }

        //  텍스트 파일에 데이터를 기록
        using (StreamWriter writer = new StreamWriter("GeneratedData.txt", true))
        {
            MakeTxt(writer, longi1, longi2, longiDistance, plateStatus, plateSuffix,
                cameraDis, cameraHeight, cameraPosition, cameraRotation);
        }
    }

    public void MakeTxt(StreamWriter writer, int longi1, int longi2, float longiDistance, int plateStatus, string plateSuffix,
        float cameraDis, float cameraHeight, Vector3 cameraPosition, Vector3 cameraRotation)
    {
        // 론지 거리에서 20을 곱해주어야 실제 값이 나옴
        longiDistance = longiDistance * 20;

        // Longi (론지 관련 변수)
        int longiHeight_L = Longi.getLongiHeight(longi1); // 론지L 의 높이
        int longiHeight_R = Longi.getLongiHeight(longi2); // 론지R 의 높이
        int plateHeight_L = plateStatus == 1 ? Plate.getPlateHeight(longi1) : 0; // 론지1에 대한 Plate 높이
        int plateHeight_R = plateStatus == 2 ? Plate.getPlateHeight(longi2) : 0; // 론지2에 대한 Plate 높이
        int longi_yong_jeop_jang_L = plateHeight_L; // 용접장 L
        int longi_yong_jeop_jang_R = plateHeight_R; // 용접장 R
        int gak_jang_L = 9; // 각장 L
        int gak_jang_R = 9; // 각장 R

        // 필렛 (필렛 관련 변수)
        float plateWidth_L = plateStatus == 1 ? Plate.getPlateWidth(longi1, plateSuffix) : 0;
        float plateWidth_R = plateStatus == 2 ? Plate.getPlateWidth(longi2, plateSuffix) : 0;
        float fillet_yong_jeop_jang_L = longiDistance; // 필렛 좌측 용접장
        float fillet_yong_jeop_jang_R = 0; // 필렛 우측 용접장
        int holeRadius = 0; // 홀 반지름 

        if (plateStatus == 0)
        {
            fillet_yong_jeop_jang_L = longiDistance;
            fillet_yong_jeop_jang_R = 0;
        }
        if (plateStatus == 1)
        {
            fillet_yong_jeop_jang_L = plateWidth_L;
            fillet_yong_jeop_jang_R = longiDistance - plateWidth_L;
            holeRadius = Longi.getRadius(longiHeight_L);

            if (plateSuffix == "CP02" || plateSuffix == "CP04" || plateSuffix == "CP06")
            {
                fillet_yong_jeop_jang_L -= Plate.getPlateHoleSize(longi1, plateSuffix);
            }
        }
        if (plateStatus == 2)
        {
            fillet_yong_jeop_jang_L = longiDistance - plateWidth_R;
            fillet_yong_jeop_jang_R = plateWidth_R;
            holeRadius = Longi.getRadius(longiHeight_R);

            if (plateSuffix == "CP02" || plateSuffix == "CP04" || plateSuffix == "CP06")
            {
                fillet_yong_jeop_jang_R -= Plate.getPlateHoleSize(longi2, plateSuffix);
            }
        }

        int gyeong_gye_yong_jeop_jang = 0; // 경계 용접장

        // 좌측 Collar
        float collarY_L = 0; // Y 위치
        int collarHeight_L = 0; // 높이
        int collarType_L = 0; // 상단 타입
        float collarSize_L = 0; // 형상 크기

        // 우측 Collar
        float collarY_R = 0; // Y 위치
        int collarHeight_R = 0; // 높이
        int collarType_R = 0; // 상단 타입
        float collarSize_R = 0; // 형상 크기

        if (plateStatus == 1)
        {
            // 좌측 Collar
            collarY_L = longiDistance / 2 - plateWidth_L; // Y 위치
            collarHeight_L = plateHeight_L; // 높이
            collarType_L = Plate.getCollarType(plateSuffix); // 상단 타입

            if (longi1 == 1 || longi1 == 2)
                collarSize_L = 20; // 형상 크기

            else
                collarSize_L = 30; // 형상 크기
        }

        if (plateStatus == 2)
        {
            // 우측 Collar
            collarY_R = plateWidth_R - longiDistance / 2; // Y 위치
            collarHeight_R = plateHeight_R; // 높이
            collarType_R = Plate.getCollarType(plateSuffix); // 상단 타입

            if (longi2 == 1 || longi2 == 2)
                collarSize_R = 20; // 형상 크기

            else
                collarSize_R = 30; // 형상 크기
        }

        // 데이터를 텍스트 파일에 작성
        writer.WriteLine($"Camera Distance : {cameraDis * 10}");
        writer.WriteLine($"Camera Height : {cameraHeight * 10}");
        writer.WriteLine($"Camera Rotation : {cameraRotation.ToString()}");
        writer.WriteLine($"Camera Position : {cameraPosition.ToString()}");

        writer.WriteLine($"Longi Height Left: {longiHeight_L}");
        writer.WriteLine($"Longi Height Right: {longiHeight_R}");

        writer.WriteLine($"Longi Distance: {longiDistance}");
        writer.WriteLine($"Longi Yong Jeop Jang Left: {longi_yong_jeop_jang_L}");
        writer.WriteLine($"Longi Yong Jeop Jang Right: {longi_yong_jeop_jang_R}");
        // writer.WriteLine($"Gak Jang Left: {gak_jang_L}");
        // writer.WriteLine($"Gak Jang Right: {gak_jang_R}");

        writer.WriteLine($"Fillet Yong Jeop Jang Left: {fillet_yong_jeop_jang_L}");
        writer.WriteLine($"Fillet Yong Jeop Jang Right: {fillet_yong_jeop_jang_R}");
        writer.WriteLine($"Hole Radius: {holeRadius}");
        writer.WriteLine($"Gyeong Gye Yong Jeop Jang: {gyeong_gye_yong_jeop_jang}");

        writer.WriteLine($"Collar Y Left: {collarY_L}");
        writer.WriteLine($"Collar Height Left: {collarHeight_L}");
        writer.WriteLine($"Collar Type Left: {collarType_L}");
        writer.WriteLine($"Collar Size Left: {collarSize_L}");

        writer.WriteLine($"Collar Y Right: {collarY_R}");
        writer.WriteLine($"Collar Height Right: {collarHeight_R}");
        writer.WriteLine($"Collar Type Right: {collarType_R}");
        writer.WriteLine($"Collar Size Right: {collarSize_R}");
    }



    private void AddObjectToLists(GameObject obj)
    {
        objectsToLabel.Add(obj);
        spawnedObjects.Add(obj);
    }

    public void DeleteSpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear();
    }

    private void AddObjectsToLists(List<GameObject> objs)
    {
        objectsToLabel.AddRange(objs);
        spawnedObjects.AddRange(objs);
    }
}

public abstract class BaseObject
{
    protected GameObject instance;

    public GameObject GetGameObject()
    {
        return instance;
    }

    //텍스처 타일링, 오프셋 값 적용함수
    public void SetTiling(Material mat, float tilingX, float tilingY, float offsetX, float offsetY)
    {
        if (mat.shader.name.Contains("Universal Render Pipeline"))
        {
            mat.SetVector("_BaseMap_ST", new Vector4(tilingX, tilingY, offsetX, offsetY));
        }
        else
        {
            mat.mainTextureScale = new Vector2(tilingX, tilingY);
            mat.mainTextureOffset = new Vector2(offsetX, offsetY);
        }
    }

    protected void ApplyMaterial(GameObject obj, Material mat)
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

    protected void SetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayer(child.gameObject, layer);
        }
    }
}

public class Longi : BaseObject
{
    public Longi(int index, string suffix, Vector3 position, Quaternion rotation)
    {
        // 텍스처 및 쉐이더 로드
        int longiTextureRand = UnityEngine.Random.Range(1, 5); //텍스처마다 오프셋 수치가 달라서 스위치문에 가져다쓸것
        Texture2D longiTexture = Resources.Load<Texture2D>($"Textures/longi{longiTextureRand}");
        Shader longiShader = Shader.Find("Custom/FloorShader");

        // 모델 로드
        string fileName = $"longi/longi_{index}{suffix}";
        GameObject model = Resources.Load<GameObject>(fileName);

        if (model != null)
        {
            instance = UnityEngine.Object.Instantiate(model, position, rotation);
            instance.transform.localScale *= 100;
            Material longiMaterial = new Material(longiShader) { mainTexture = longiTexture }; //아래 스위치문때문에 따로 변수선언
            //텍스처 크기, 위치조정하는부분
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
            ApplyMaterial(instance, longiMaterial);
            SetLayer(instance, 6);
            instance.tag = "longi";
        }
    }

    // Longi 두께 가져오는 함수
    public static float getLongiThick_w(int index)
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

    // Longi 높이 가져오는 함수
    public static int getLongiHeight(int index)
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

    public static int getLongiWidth(int index)
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
            case 9: return 90;
            case 10: return 90;
            case 11: return 90;
            case 12: return 90;
            case 13: return 90;
            case 14: return 90;
            case 15: return 100;
            case 16: return 100;
            case 17: return 100;
            case 18: return 125;
            case 19: return 150;
            case 20: return 75;
            case 21: return 75;
            case 22: return 75;
            case 23: return 75;
            case 24: return 75;
            case 25: return 90;
            case 26: return 90;
            default: return 0; // 기본값으로 0 반환
        }
    }


    public static int getRadius(int height)
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

    public static int getRadius1(int height)
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
}

public class SlotHole : BaseObject
{
    public SlotHole(int index, string suffix, Vector3 position, Quaternion rotation)
    {
        // 텍스처 및 쉐이더 로드
        Texture2D slotHoleTexture = Resources.Load<Texture2D>("Textures/myTexture");
        Shader slotHoleShader = Shader.Find("Custom/StencilMask");

        // 모델 로드
        string fileName = $"slot_hole/slot_hole_{index}{suffix}";
        GameObject model = Resources.Load<GameObject>(fileName);

        if (model != null)
        {
            instance = UnityEngine.Object.Instantiate(model, position, rotation);
            instance.transform.localScale *= 100;

            Material slotHoleMaterial = new Material(slotHoleShader);
            slotHoleMaterial.mainTexture = slotHoleTexture;
            slotHoleMaterial.SetInt("_StencilID", 1); // StencilID 설정

            ApplyMaterial(instance, slotHoleMaterial);
            SetLayer(instance, 6);
            instance.tag = "slotHole";
        }
    }
}

public class Floor : BaseObject
{
    public Floor(string floorType, Vector3 position, int layer, string tag)
    {
        // 텍스처 및 쉐이더 로드
        int floorTextureRand = UnityEngine.Random.Range(1, 5); //텍스처마다 오프셋 수치가 달라서 스위치문에 가져다쓸것
        Texture2D floorTexture = Resources.Load<Texture2D>($"Textures/floor{floorTextureRand}");
        Shader floorShader = Shader.Find("Custom/FloorShader");

        // 모델 로드
        GameObject model = Resources.Load<GameObject>($"floor/{floorType}");

        if (model != null)
        {
            instance = UnityEngine.Object.Instantiate(model, position, Quaternion.identity);
            instance.transform.localScale *= 100;
            Material floorMaterial = new Material(floorShader) { mainTexture = floorTexture }; //아래 스위치문때문에 따로 변수선언
            //텍스처 크기, 위치조정하는부분
            switch (floorTextureRand)
            {
                case 1:
                    SetTiling(floorMaterial, 0.4f, 0.4f, 0.1f, 0.0f);
                    break;
                case 2:
                    SetTiling(floorMaterial, 0.23f, 0.225f, 0.5f, 0.5f);
                    break;
                case 3:
                    SetTiling(floorMaterial, 0.228f, 0.229f, 0.5f, 0.5f);
                    break;
                case 4:
                    SetTiling(floorMaterial, 0.229f, 0.229f, 0.5f, 0.5f);
                    break;
            }
            ApplyMaterial(instance, floorMaterial);
            SetLayer(instance, layer);
            instance.tag = tag;
        }
    }
}

public class Plate : BaseObject
{
    public Plate(int index, string suffix, Vector3 position, Quaternion rotation)
    {
        // 텍스처 및 쉐이더 로드
        int plateTextureRand = UnityEngine.Random.Range(1, 5); //텍스처마다 오프셋 수치가 달라서 스위치문에 가져다쓸것
        Texture2D plateTexture = Resources.Load<Texture2D>($"Textures/plate{plateTextureRand}");
        Shader plateShader = Shader.Find("Custom/FloorShader");

        // 모델 로드
        string fileName = $"plate/plate{index}_{suffix}";
        GameObject model = Resources.Load<GameObject>(fileName);

        if (model != null)
        {
            instance = UnityEngine.Object.Instantiate(model, position, rotation);
            instance.transform.localScale *= 100;
            Material plateMaterial = new Material(plateShader) { mainTexture = plateTexture }; //아래 스위치문때문에 따로 변수선언
            //텍스처 크기, 위치조정하는부분
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
            ApplyMaterial(instance, plateMaterial);
            SetLayer(instance, 6);
            instance.tag = "plate";
        }
    }

    public static int getCollarType(string suffix)
    {
        switch (suffix)
        {
            case "CP01":
            case "CP02":
                return 0;

            case "CP03":
            case "CP04":
                return 1;

            case "CP05":
            case "CP06":
                return 2;

            default:
                return -1;
        }
    }

    public static int getPlateHoleSize(int index, string suffix)
    {
        switch (suffix)
        {
            case "CP01":
            case "CP03":
            case "CP05":
                return 0;

            case "CP02":
            case "CP04":
            case "CP06":
                return getPlateHeight(index) <= 250 ? 50 : 70;

            default:
                return -1;
        }
    }

    public static int getPlateHeight(int index)
    {
        switch (index)
        {
            case 1: return 20;
            case 2: return 25;
            case 3: return 50;
            case 4: return 50;
            case 5: return 80;
            case 6: return 100;
            case 7: return 100;
            case 8: return 150;
            case 9: return 150;
            case 10: return 150;
            case 11: return 200;
            case 12: return 200;
            case 13: return 230;
            case 14: return 230;
            case 15: return 280;
            case 16: return 330;
            case 17: return 330;
            case 18: return 380;
            case 19: return 380;
            case 20: return 50;
            case 21: return 50;
            case 22: return 75;
            case 23: return 75;
            case 24: return 75;
            case 25: return 100;
            case 26: return 100;
            default: return 0;
        }
    }

    public static float getPlateWidth(int index, string longiType)
    {
        int longiWidth = Longi.getLongiWidth(index);  // Longi Width 값을 가져옵니다.
        float thickW = Longi.getLongiThick_w(index);  // ThickW 값을 가져옵니다.
        int collarA = 50;  // Collar a는 고정 값 50

        int holeC = 0;

        if (longiType == "LA" || longiType == "LF")
        {
            if (longiWidth <= 150)
            {
                holeC = 0;
            }
            else if (longiWidth <= 300)
            {
                holeC = 50;
            }
            else if (longiWidth <= 450)
            {
                holeC = 75;
            }
            else
            {
                holeC = 100;
            }
        }
        else if (longiType == "LT")
        {
            if (longiWidth <= 250)
            {
                holeC = 0;
            }
            else if (longiWidth <= 350)
            {
                holeC = 50;
            }
            else if (longiWidth <= 450)
            {
                holeC = 75;
            }
            else
            {
                holeC = 100;
            }
        }

        // Plate Width 계산
        float plateWidth = longiWidth - thickW + holeC + collarA;

        return plateWidth;
    }
}

public class RHole : BaseObject
{
    public RHole(int index, float dis, string slotHoleSuffix, Vector3 position, Quaternion rotation)
    {
        // 텍스처 및 쉐이더 로드
        Texture2D rHoleTexture = Resources.Load<Texture2D>("Textures/myTexture");
        Shader rHoleShader = Shader.Find("Custom/StencilMask");

        int radius = 0;
        int height = Longi.getLongiHeight(index);
        float thick_w = 0.1f * Longi.getLongiThick_w(index);

        // if (slotHoleSuffix == "AA" || slotHoleSuffix == "TG")
        radius = Longi.getRadius(height);

        if (slotHoleSuffix == "AJ")
        {
            radius = Longi.getRadius1(height);
        }

        // 모델 로드
        GameObject model = Resources.Load<GameObject>($"r_hole/r_{radius}");

        if (model != null)
        {
            instance = UnityEngine.Object.Instantiate(model, position, rotation);

            instance.transform.localScale *= 100;

            Material rHoleMaterial = new Material(rHoleShader) { mainTexture = rHoleTexture };
            rHoleMaterial.SetInt("_StencilID", 1); // StencilID 설정

            ApplyMaterial(instance, rHoleMaterial);
            SetLayer(instance, 6);

            instance.tag = "r_hole";
        }
    }
}
