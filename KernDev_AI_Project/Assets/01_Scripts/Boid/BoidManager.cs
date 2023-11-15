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
    public boid BoidPrefab;
    public int AmountBoids = 3;
    public GameObject CenterPrefab;
    public float NeighboursRadius = 5f;


    private List<boid> BoidList = new List<boid>();

    private Vector3 c = new Vector3(0, 0, 0); //CenterOfMass
    private Vector3 totalBoidsPos = new Vector3(0, 0, 0); //CenterOfMass



    // Start is called before the first frame update
    void Start()
    {
        // meerdere spwanen
        for (int i = 1; i <= AmountBoids; i++)
        {
            Vector3 SpawnPos = CalculateRandomSpawn();
            boid Boid = Instantiate(BoidPrefab, SpawnPos, Quaternion.identity);
            BoidList.Add(Boid);
        }

        foreach (boid Boid in BoidList)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (boid boid in BoidList)
        {
            List<Transform> neighbours = GetNearbyNeighbours(boid);
            Vector3 centerOfMass = CalculateAverageMass(neighbours);
            foreach (Transform _transform in neighbours)
            {
                _transform.position = centerOfMass;
            }

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
        Vector3 averageDirection = new Vector3(0, 0, 0);

        return averageDirection;
    }
    //private void CalculateAverageMass()
    //{
    //    Vector3 totalBoidsPos = new Vector3(0, 0, 0);
    //    for (int i = 0; i < BoidList.Count; i++)
    //    {
    //        boid boid = BoidList[i];
    //        Vector3 BoidPos = boid.transform.position;
    //        totalBoidsPos = totalBoidsPos + BoidPos;
    //        //Debug.Log(boid.transform.name + " positie = " + BoidPos);
    //    }

    //    c = totalBoidsPos / BoidList.Count;
    //    Center.position = c;

    //    //Debug.Log("CenterOfMass = " + c);
    //}

    private Vector3 CalculateAverageMass(List<Transform> _neigboursList)
    {
        Debug.Log(_neigboursList.Count);
        Vector3 totalBoidsPos = new Vector3(0, 0, 0);
        for (int i = 0; i < _neigboursList.Count; i++)
        {
            Transform neiboursTransforms = _neigboursList[i];
            Vector3 neigbourPos = neiboursTransforms.position;
            totalBoidsPos = totalBoidsPos + neigbourPos;
            //Debug.Log(boid.transform.name + " positie = " + BoidPos);
        }

        
        Vector3 c = totalBoidsPos / _neigboursList.Count;
        Debug.Log("the Total vector : " + totalBoidsPos + " | Center of the mass : " + c + "neigbourcount : " + _neigboursList.Count);

        //Debug.Log(c);
        return c;
        //GameObject center;

        //center = Instantiate(CenterPrefab, c, Quaternion.identity);

        //center.transform.position = c;

        //Debug.Log("CenterOfMass = " + c);
    }

    private List<Transform> GetNearbyNeighbours(boid _boid)
    {
        List<Transform> neighbours = new List<Transform>();
        Collider[] neighboursColliders = Physics.OverlapSphere(_boid.transform.position, NeighboursRadius);

        foreach (Collider collider in neighboursColliders)
        {
            if (collider != _boid.collider)
            {
                neighbours.Add(collider.transform);
            }
        }
        Debug.Log(neighbours.Count);
        return neighbours;

    }



}
