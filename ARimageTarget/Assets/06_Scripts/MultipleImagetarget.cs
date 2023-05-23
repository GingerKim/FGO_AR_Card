using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImagetarget : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    [SerializeField] //변수가 유니티에 노출됨
    private GameObject[] plaveablePrefab;  //여러개의 오브젝트를 받을 수 있도록 리스트로 선언

    private Dictionary<string, GameObject> spawnedObject;  //데이터를 실체화 해서 게임 오브젝트로 다루는 데이터 저장 실체화된 오브젝트들이 저장되어 있는 데이터 구조

    private void Awake() // 게임오브젝트가 처음 생성이 되었을 때 가장 처음으로 불리는 함수 변수들에 대한 초기화 진행
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        spawnedObject = new Dictionary<string, GameObject>();

        foreach (GameObject obj in plaveablePrefab)
        {
            GameObject newObject = Instantiate(obj);

            newObject.name = obj.name;
            newObject.SetActive(false);

            spawnedObject.Add(newObject.name, newObject);


        }


    }

    private void OnEnable() //trackedImageManager 연결

    {
        trackedImageManager.trackedImagesChanged += OntrackedImageChaned;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged += OntrackedImageChaned;

    }

    void OntrackedImageChaned(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //트래킹되는 모든 이미지
        foreach (ARTrackedImage trackedImage in eventArgs.added)

        {
            UpdateSpawnObject(trackedImage);

        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateSpawnObject(trackedImage);

        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnedObject[trackedImage.name].SetActive(false); //사라졌을 때 생성된 3D 오브젝트 사라짐

        }



    }


    // Update is called once per frame
    void UpdateSpawnObject(ARTrackedImage trackedImage)
    {
        string referenceImageName = trackedImage.referenceImage.name;
        //이미지 인식
        spawnedObject[referenceImageName].transform.position = trackedImage.transform.position;
        spawnedObject[referenceImageName].transform.rotation = trackedImage.transform.rotation;

        spawnedObject[referenceImageName].SetActive(true);

    }

    void Update()
    {
        Debug.Log($"There are {trackedImageManager.trackables.count} image being tracked"); //어느 좌표에 오브젝트가 올라가는지 확인용

        foreach (var trackedImage in trackedImageManager.trackables)
        {
            Debug.Log($"Image; {trackedImage.referenceImage.name} is at" + $"{trackedImage.transform.position}");


        }

    }

}