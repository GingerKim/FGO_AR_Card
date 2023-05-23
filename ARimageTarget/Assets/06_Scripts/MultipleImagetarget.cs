using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImagetarget : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    [SerializeField] //������ ����Ƽ�� �����
    private GameObject[] plaveablePrefab;  //�������� ������Ʈ�� ���� �� �ֵ��� ����Ʈ�� ����

    private Dictionary<string, GameObject> spawnedObject;  //�����͸� ��üȭ �ؼ� ���� ������Ʈ�� �ٷ�� ������ ���� ��üȭ�� ������Ʈ���� ����Ǿ� �ִ� ������ ����

    private void Awake() // ���ӿ�����Ʈ�� ó�� ������ �Ǿ��� �� ���� ó������ �Ҹ��� �Լ� �����鿡 ���� �ʱ�ȭ ����
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

    private void OnEnable() //trackedImageManager ����

    {
        trackedImageManager.trackedImagesChanged += OntrackedImageChaned;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged += OntrackedImageChaned;

    }

    void OntrackedImageChaned(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //Ʈ��ŷ�Ǵ� ��� �̹���
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
            spawnedObject[trackedImage.name].SetActive(false); //������� �� ������ 3D ������Ʈ �����

        }



    }


    // Update is called once per frame
    void UpdateSpawnObject(ARTrackedImage trackedImage)
    {
        string referenceImageName = trackedImage.referenceImage.name;
        //�̹��� �ν�
        spawnedObject[referenceImageName].transform.position = trackedImage.transform.position;
        spawnedObject[referenceImageName].transform.rotation = trackedImage.transform.rotation;

        spawnedObject[referenceImageName].SetActive(true);

    }

    void Update()
    {
        Debug.Log($"There are {trackedImageManager.trackables.count} image being tracked"); //��� ��ǥ�� ������Ʈ�� �ö󰡴��� Ȯ�ο�

        foreach (var trackedImage in trackedImageManager.trackables)
        {
            Debug.Log($"Image; {trackedImage.referenceImage.name} is at" + $"{trackedImage.transform.position}");


        }

    }

}