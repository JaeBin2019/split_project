using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public ObjectLabeler objectLabeler;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private List<GameObject> objectsToLabel = new List<GameObject>();

    void Start()
    {
        // 초기화 코드
    }

    public void SpawnObjects(int longi1, string longiSuffix1, string slotHoleSuffix1, string slotHoleSuffix2, string plateSuffix1, string plateSuffix2)
    {
        float randomDis = Random.Range(500, 851) * 0.05F;

        // 오브젝트 생성
        var longi1Instance = new Longi(longi1, longiSuffix1, new Vector3(-100, 0, -randomDis), Quaternion.identity);
        var longi2Instance = new Longi(longi1, longiSuffix1, new Vector3(100, 0, randomDis), Quaternion.Euler(0, 180, 0));

        AddObjectToLists(longi1Instance.GetGameObject());
        AddObjectToLists(longi2Instance.GetGameObject());

        var slotHole1Instance = new SlotHole(longi1, slotHoleSuffix1, new Vector3(0, 0, -randomDis), Quaternion.identity);
        var slotHole2Instance = new SlotHole(longi1, slotHoleSuffix2, new Vector3(0, 0, randomDis), Quaternion.Euler(0, 180, 0));

        AddObjectToLists(slotHole1Instance.GetGameObject());
        AddObjectToLists(slotHole2Instance.GetGameObject());

        var floorInstance = new Floor("floor", new Vector3(0, 0, 0), 0, "floor");
        var realFloorInstance = new Floor("real_floor", new Vector3(0, 0, 0), 6, "realFloor");

        AddObjectToLists(floorInstance.GetGameObject());
        AddObjectToLists(realFloorInstance.GetGameObject());

        float thick_w1 = 0.1f * getLongiThick_w(longi1);
        Vector3 spawnPlate1 = new Vector3(1f, 0, -randomDis + thick_w1);
        Vector3 spawnPlate2 = new Vector3(1f, 0, randomDis - thick_w1);

        var plate1Instance = new Plate(longi1, plateSuffix1, spawnPlate1, Quaternion.identity);
        var plate2Instance = new Plate(longi1, plateSuffix2, spawnPlate2, Quaternion.Euler(0, 180, 0));

        AddObjectToLists(plate1Instance.GetGameObject());
        AddObjectToLists(plate2Instance.GetGameObject());

        makeRHole(longi1, slotHoleSuffix1, randomDis);

        // ObjectLabeler에 객체 리스트 전달
        if (objectLabeler != null)
        {
            objectLabeler.objectsToLabel = objectsToLabel;
        }
        else
        {
            Debug.LogError("ObjectLabeler reference not set in ObjectSpawner!");
        }
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

    void makeRHole(int index, string slotHoleSuffix, float dis)
    {
        int r_rand = 0;
        int radius = 0;
        int height = getLongiHeight(index);
        float thick_w = 0.1f * getLongiThick_w(index);

        if (slotHoleSuffix == "AA" || slotHoleSuffix == "TG")
        {
            r_rand = Random.Range(0, 2);
            radius = getRadius(height);
        }
        if (slotHoleSuffix == "AJ")
        {
            r_rand = Random.Range(0, 3);
            radius = getRadius1(height);
        }

        if (r_rand == 0 || radius == 0)
        {
            return;
        }

        Vector3 spawnRHole1 = new Vector3(0, 0, dis);
        Vector3 spawnRHole2 = new Vector3(0, 0, -dis);
        Vector3 spawnRHole3 = new Vector3(0, 0, -dis + thick_w);
        Vector3 spawnRHole4 = new Vector3(0, 0, dis - thick_w);

        var rHoleInstance = new RHole(radius, r_rand == 1 ? spawnRHole1 : spawnRHole3, r_rand == 1 ? spawnRHole2 : spawnRHole4, Quaternion.identity, Quaternion.Euler(0, 180, 0));

        AddObjectsToLists(rHoleInstance.GetInstances());
    }

    private void AddObjectsToLists(List<GameObject> objs)
    {
        objectsToLabel.AddRange(objs);
        spawnedObjects.AddRange(objs);
    }

    // Longi 두께 가져오는 함수
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

    // Longi 높이 가져오는 함수
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
}

public abstract class BaseObject
{
    protected GameObject instance;

    public GameObject GetGameObject()
    {
        return instance;
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
        Texture2D longiTexture = Resources.Load<Texture2D>($"Textures/longi{Random.Range(1, 5)}");
        Shader longiShader = Shader.Find("Custom/FloorShader");

        // 모델 로드
        string fileName = $"longi/longi_{index}{suffix}";
        GameObject model = Resources.Load<GameObject>(fileName);

        if (model != null)
        {
            instance = Object.Instantiate(model, position, rotation);
            instance.transform.localScale *= 100;
            ApplyMaterial(instance, new Material(longiShader) { mainTexture = longiTexture });
            SetLayer(instance, 6);
            instance.tag = "longi";
        }
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
            instance = Object.Instantiate(model, position, rotation);
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
        Texture2D floorTexture = Resources.Load<Texture2D>($"Textures/floor{Random.Range(1, 5)}");
        Shader floorShader = Shader.Find("Custom/FloorShader");

        // 모델 로드
        GameObject model = Resources.Load<GameObject>($"floor/{floorType}");

        if (model != null)
        {
            instance = Object.Instantiate(model, position, Quaternion.identity);
            instance.transform.localScale *= 100;
            ApplyMaterial(instance, new Material(floorShader) { mainTexture = floorTexture });
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
        Texture2D plateTexture = Resources.Load<Texture2D>($"Textures/plate{Random.Range(1, 5)}");
        Shader plateShader = Shader.Find("Custom/FloorShader");

        // 모델 로드
        string fileName = $"plate/plate{index}_{suffix}";
        GameObject model = Resources.Load<GameObject>(fileName);

        if (model != null)
        {
            instance = Object.Instantiate(model, position, rotation);
            instance.transform.localScale *= 100;
            ApplyMaterial(instance, new Material(plateShader) { mainTexture = plateTexture });
            SetLayer(instance, 6);
            instance.tag = "plate";
        }
    }
}

public class RHole : BaseObject
{
    private List<GameObject> instances = new List<GameObject>();

    public RHole(int radius, Vector3 position1, Vector3 position2, Quaternion rotation1, Quaternion rotation2)
    {
        // 텍스처 및 쉐이더 로드
        Texture2D rHoleTexture = Resources.Load<Texture2D>("Textures/myTexture");
        Shader rHoleShader = Shader.Find("Custom/StencilMask");

        // 모델 로드
        GameObject model = Resources.Load<GameObject>($"r_hole/r_{radius}");

        if (model != null)
        {
            GameObject instance1 = Object.Instantiate(model, position1, rotation1);
            GameObject instance2 = Object.Instantiate(model, position2, rotation2);

            instance1.transform.localScale *= 100;
            instance2.transform.localScale *= 100;

            Material rHoleMaterial = new Material(rHoleShader) { mainTexture = rHoleTexture };
            rHoleMaterial.SetInt("_StencilID", 1); // StencilID 설정

            ApplyMaterial(instance1, rHoleMaterial);
            ApplyMaterial(instance2, rHoleMaterial);
            SetLayer(instance1, 6);
            SetLayer(instance2, 6);

            instance1.tag = "r_hole";
            instance2.tag = "r_hole";

            instances.Add(instance1);
            instances.Add(instance2);
        }
    }

    public List<GameObject> GetInstances()
    {
        return instances;
    }
}
