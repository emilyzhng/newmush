using UnityEngine;
using System.Collections.Generic;

public class LootSpawner : MonoBehaviour
{
    public GameObject[] mushroomPrefabs;
    public Transform[] spawnPoints;

    public int numgoal1 = 0;
    public int numgoal2 = 0;
    public int mushindex1 = 0;
    public int mushindex2 = 2;
    public int mushindex3;
    public int numgoal3;

    // every poisonous type that can appear today gets at least this many spawns
    public int minPoisonPerType = 2;

    private List<Transform> availableSpawns;

    void Start()
    {
        goalAssignPerDay();
        Debug.Log("Day " + ButtonManager.Day);

        FilterSpawnPoints();

        // goal mushrooms take spawn points first so the objective can always be met,
        // then the guaranteed poisonous ones, then random fill
        GoalSpawn();
        PoisonSpawn();
        SpawnMushroom();
    }

    public void goalAssignPerDay()
    {
        if (ButtonManager.Day == 1)
        {
            mushindex1 = 0;
            numgoal1 = 2;

            mushindex2 = 2;
            numgoal2 = 2;
        }
        else if (ButtonManager.Day == 2)
        {
            mushindex1 = 2;
            numgoal1 = 3;

            mushindex2 = 4;
            numgoal2 = 3;
        }
        else if (ButtonManager.Day == 3)
        {
            mushindex1 = 2;
            numgoal1 = 2;

            mushindex2 = 4;
            numgoal2 = 2;

            mushindex3 = 6;
            numgoal3 = 3;
        }
    }

    public void FilterSpawnPoints()
    {
        availableSpawns = new List<Transform>();

        foreach (Transform point in spawnPoints)
        {
            SpawnPoint spawn = point.GetComponent<SpawnPoint>();

            if (spawn != null && spawn.region == ButtonManager.Day)
            {
                availableSpawns.Add(point);
            }
        }
    }

    public void GoalSpawn()
    {
        SpawnAtRandomPoints(mushindex1, numgoal1);
        SpawnAtRandomPoints(mushindex2, numgoal2);
        SpawnAtRandomPoints(mushindex3, numgoal3);
    }

    // places count of one prefab on random unused spawn points
    void SpawnAtRandomPoints(int prefabIndex, int count)
    {
        for (int i = 0; i < count && availableSpawns.Count > 0; i++)
        {
            int randomSpawn = Random.Range(0, availableSpawns.Count);

            Instantiate(
                mushroomPrefabs[prefabIndex],
                availableSpawns[randomSpawn].position,
                Quaternion.identity
            );

            availableSpawns.RemoveAt(randomSpawn);
        }
    }

    // prefab index range (min inclusive, max exclusive) that can spawn today
    void DayMushroomRange(out int min, out int max)
    {
        if (ButtonManager.Day == 1)
        {
            // Bellcap and Coral
            min = 0;
            max = 4;
        }
        else if (ButtonManager.Day == 2)
        {
            // Coral and Lantern
            min = 2;
            max = 6;
        }
        else
        {
            min = 2;
            max = mushroomPrefabs.Length;
        }
    }

    public void PoisonSpawn()
    {
        DayMushroomRange(out int min, out int max);

        for (int index = min; index < max; index++)
        {
            if (!mushroomPrefabs[index].GetComponent<MushCategorization>().mushData.isPoisonous)
                continue;

            SpawnAtRandomPoints(index, minPoisonPerType);
        }
    }

    public void SpawnMushroom()
{
    DayMushroomRange(out int min, out int max);

    foreach (Transform spawnPoint in availableSpawns)
    {
        int randomMushroomIndex = Random.Range(min, max);

        Instantiate(
            mushroomPrefabs[randomMushroomIndex],
            spawnPoint.position,
            Quaternion.identity
        );
    }
}
}