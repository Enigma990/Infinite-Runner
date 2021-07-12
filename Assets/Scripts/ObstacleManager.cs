using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles = null;
    private List<GameObject> obstaclesList = new List<GameObject>();

    private Transform playerTransform = null;

    //Spawn Inf0
    private float spawnDistance = 100f;
    private float spawnTime = 2f;
    private float nextSpawnTime = 3f;
    private float minSpawnTime = 0.1f;
    private float maxSpawnTime = 2.0f;

    private int nextScoreLevel = 10;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = PlayerController.Instance.transform;

        //Initializing Obstacles
        for (int i = 0; i < obstacles.Length; i++)
        {
            GameObject obstacle = Instantiate(obstacles[i], transform) as GameObject;
            obstacle.SetActive(false);
            obstaclesList.Add(obstacle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Calculating next Spawn time
        spawnTime += Time.deltaTime;

        if(spawnTime > nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            spawnTime = 0f;
        }

        //Decreasing Spawn time with Score
        if (PlayerController.Instance.Score > nextScoreLevel && maxSpawnTime > 1.0f)
        {
            maxSpawnTime -= 0.2f;
            PlayerController.Instance.UpdateSpeed();
            nextScoreLevel += 10;
        }
    }

    void SpawnObstacle()
    {
        int nextObstacleIndex = Random.Range(0, obstaclesList.Count);
        //Check if obstacle is active in scene or not
        while (obstaclesList[nextObstacleIndex].activeInHierarchy)
        {
            nextObstacleIndex = Random.Range(0, obstaclesList.Count);
        }

        //Spawn Obstacle
        obstaclesList[nextObstacleIndex].transform.position = new Vector3(Random.Range(-1, 2) * 5, 1.0f, 1f * playerTransform.position.z + spawnDistance);
        obstaclesList[nextObstacleIndex].SetActive(true);
    }
}
