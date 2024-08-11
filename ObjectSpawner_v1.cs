using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    void Start()
    {
        // 랜덤으로 1부터 26 사이의 숫자 선택
        // int randomIndex = Random.Range(1, 27);
        int randomIndex = Random.Range(18, 19);

        // 론지 사이 랜덤 거리 생성 / mm 로 변환
        float randomDis = Random.Range(250, 426) * 0.001F;

        // longi 파일 이름 생성
        // string[] longiSuffixes = { "AA", "LA" };
        string[] longiSuffixes = { "LA" };
        string randomLongiSuffix = longiSuffixes[Random.Range(0, longiSuffixes.Length)];
        string longiFileName = $"longi_{randomIndex}{randomLongiSuffix}";

        // slot_hole 파일 이름 생성
        // string[] slotHoleSuffixes = { "AA", "AG", "AH", "AJ" };
        string[] slotHoleSuffixes = { "AA", "AH" };
        string randomSlotHoleSuffix = slotHoleSuffixes[Random.Range(0, slotHoleSuffixes.Length)];
        string slotHoleFileName = $"slot_hole_{randomIndex}{randomSlotHoleSuffix}";

        // plate 파일 이름 생성
        string[] plateSuffixes = { "CP01" };
        string randomPlateSuffix = plateSuffixes[Random.Range(0, plateSuffixes.Length)];
        string plateFileName = $"plate{randomIndex}_{randomPlateSuffix}";

        // Resources 폴더에서 모델을 로드합니다.
        GameObject longiModel = Resources.Load<GameObject>($"longi/{longiFileName}");
        GameObject slotHoleModel = Resources.Load<GameObject>($"slot_hole/{slotHoleFileName}");
        GameObject plateModel = Resources.Load<GameObject>($"plate/{plateFileName}");
        GameObject floorModel = Resources.Load<GameObject>("floor/floor");


        // 원하는 좌표에 오브젝트를 생성합니다.
        // 1 => 1m, 1m 만큼 앞 뒤로 이동
        Vector3 spawnLongi1 = new Vector3(-1, 0, -randomDis);
        Vector3 spawnLongi2 = new Vector3(1, 0, randomDis);

        Vector3 spawnSlotHole1 = new Vector3(0, 0, -randomDis);
        // 좌우 반전으로 인해 두께 5mm 만큼 뒤로 이동하여, 앞으로 당겨주었다
        Vector3 spawnSlotHole2 = new Vector3(0.005F, 0, randomDis);

        Vector3 spawnPlate1 = new Vector3(-0.1F, 0, -randomDis);
        Vector3 spawnPlate2 = new Vector3(-0.1F, 0, randomDis);

        Vector3 spawnFloor = new Vector3(0, 0, 0);

        // longi 오브젝트 생성
        // y축 180도 회전 : Quaternion.Euler(0, 180, 0)
        GameObject longiInstance1 = Instantiate(longiModel, spawnLongi1, Quaternion.identity);
        GameObject longiInstance2 = Instantiate(longiModel, spawnLongi2, Quaternion.Euler(0, 180, 0));

        // slot_hole 오브젝트 생성
        GameObject slotHoleInstance1 = Instantiate(slotHoleModel, spawnSlotHole1, Quaternion.identity);
        GameObject slotHoleInstance2 = Instantiate(slotHoleModel, spawnSlotHole2, Quaternion.Euler(0, 180, 0));

        // plate 오브젝트 생성
        GameObject plateInstance1 = Instantiate(plateModel, spawnPlate1, Quaternion.identity);
        GameObject plateInstance2 = Instantiate(plateModel, spawnPlate2, Quaternion.Euler(0, 180, 0));

        // floor 오브젝트 생성
        GameObject floorInstance = Instantiate(floorModel, spawnFloor, Quaternion.identity);

    }
}
