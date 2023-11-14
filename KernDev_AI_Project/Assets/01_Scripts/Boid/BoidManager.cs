using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [Header("Spawning")]
    public Transform CenterSpawn;
    public float SpawnRange = 30.0f;

    [Header("Boids")]
    public GameObject BoidPrefab;
    public int AmountBoids = 3;
    public Transform Center;
    public float NeighboursRadius = 2.5f;


    private List<GameObject> BoidList = new List<GameObject>();

    private Vector3 c = new Vector3(0, 0, 0); //CenterOfMass
    private Vector3 totalBoidsPos = new Vector3(0, 0, 0); //CenterOfMass



    // Start is called before the first frame update
    void Start()
    {
        // meerdere spwanen
        for (int i = 1; i <= AmountBoids; i++)
        {
            Vector3 SpawnPos = CalculateRandomSpawn();
            GameObject Boid = Instantiate(BoidPrefab, SpawnPos, Quaternion.identity);
            BoidList.Add(Boid);
        }

        foreach (GameObject Boid in BoidList)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateAverageMass();
        foreach (GameObject boid in BoidList)
        {
            List<Transform> neighbours = GetNearbyNeighbours();
        }
    }

    private Vector3 CalculateRandomSpawn()
    {
        float randomX = Random.Range((CenterSpawn.position.x - SpawnRange), (CenterSpawn.position.x + SpawnRange));
        float randomY = Random.Range((CenterSpawn.position.y - SpawnRange), (CenterSpawn.position.y + SpawnRange));
        float randomZ = Random.Range((CenterSpawn.position.z - SpawnRange), (CenterSpawn.position.z + SpawnRange));
        Vector3 spawnPos = new Vector3(randomX, randomY, randomZ);

        return spawnPos;
    }

    private Vector3 CalculateAverageDirection()
    {
        Vector3 averageDirection = new Vector3(0,0,0);

        return averageDirection;
    }
    private void CalculateAverageMass()
    {
        Vector3 totalBoidsPos = new Vector3(0, 0, 0);
        for (int i = 0; i < BoidList.Count; i++)
        {
            GameObject boid = BoidList[i];
            Vector3 BoidPos = boid.transform.position;
            totalBoidsPos = totalBoidsPos + BoidPos;
            Debug.Log(boid.transform.name + " positie = " + BoidPos);
        }

        c = totalBoidsPos / BoidList.Count;
        Center.position = c;

        Debug.Log("CenterOfMass = " + c);
    }

    //private List<Transform> GetNearbyNeighbours(boid _boid)
    //{
    //    List<Transform> neighbours = new List<Transform>();
    //    Collider[] neighboursColliders= Physics.OverlapSphere(_boid.transform.position, NeighboursRadius);

    //    foreach (Collider collider in neighboursColliders)
    //    {
    //        if (collider != _boid.collider)
    //        {
    //            neighbours.Add(collider.transform);
    //        }
    //    }
    //    return neighbours;

    //}



}
