using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] platformPrefab = null;

    private Transform playerTransform = null;

    private float spawnLocation = 0.0f;
    [SerializeField] private float platformLength = 30.0f;
    private int maxPlatforms = 7;

    private List<GameObject> platformsList = new List<GameObject>();

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initializing Platforms
        playerTransform = PlayerController.Instance.transform;
        for (int i = 0; i < platformPrefab.Length; i++)
        {
            GameObject platform = Instantiate(platformPrefab[i], transform) as GameObject;
            platform.SetActive(false);
            platformsList.Add(platform);
        }

        //Creating Platforms in Scene
        for (int i = 0; i < maxPlatforms; i++)
        {
            int index = Random.Range(0, platformsList.Count);

            while (platformsList[index].activeInHierarchy) 
            {
                index = Random.Range(0, platformsList.Count);
            }

            platformsList[index].transform.position = Vector3.forward * spawnLocation;
            platformsList[index].SetActive(true);
            spawnLocation += platformLength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.z > spawnLocation - maxPlatforms * platformLength)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        foreach (GameObject platform in platformsList)
        {
            if (!platform.activeInHierarchy)
            {
                platform.transform.position = Vector3.forward * spawnLocation;
                spawnLocation += platformLength;
                platform.SetActive(true);
            }
        }
    }
}
