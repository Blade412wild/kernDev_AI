using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.UIElements;
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
    public float SeperationsStrenght = 0.3f;
    public float SperationRadius = 1.0f;


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
            //finding neigbours
            List<Transform> neighbours = GetNearbyNeighbours(boid);
            Vector3 centerOfMass = CalculateAverageMass(neighbours);

            // regel Cohesion
            CalculateMovementTowardsCenterOfNeigbours(neighbours, centerOfMass);

            // regel Seperation
            CalculateSerpartion(neighbours, boid);


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

    private Vector3 CalculateAverageMass(List<Transform> _neigboursList)
    {
        Vector3 totalBoidsPos = new Vector3(0, 0, 0);
        for (int i = 0; i < _neigboursList.Count; i++)
        {
            Transform neiboursTransforms = _neigboursList[i];
            Vector3 neigbourPos = neiboursTransforms.position;
            totalBoidsPos = totalBoidsPos + neigbourPos;
            //Debug.Log(boid.transform.name + " positie = " + BoidPos);
        }

        Vector3 c = totalBoidsPos / _neigboursList.Count;
        // Debug.Log("the Total vector : " + totalBoidsPos + " | Center of the mass : " + c + "neigbourcount : " + _neigboursList.Count);

        return c;
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
        //Debug.Log(neighbours.Count);
        return neighbours;

    }

    private void CalculateMovementTowardsCenterOfNeigbours(List<Transform> _neigboursList, Vector3 _centerOfMass)
    {
        if (_neigboursList.Count != 0)
        {
            foreach (Transform _transform in _neigboursList)
            {
                Vector3 moveDirection = _centerOfMass - _transform.position;

                _transform.position = _transform.position + moveDirection.normalized * Time.deltaTime;

            }

        }

    }

    private void CalculateSerpartion(List<Transform> _neigboursList, boid _boid)
    {

        for (int j = 0; j < _neigboursList.Count; j++)
        {
            Transform neigbour = _neigboursList[j];

            //float DistanceBetweenEachNeigbour = Vector3.Distance(_boid.transform.position, neigbour.position);
            float DistanceBetweenEachNeigbour = (_boid.transform.position - neigbour.position).sqrMagnitude;


            Debug.Log("Distance between boid : " + _boid + " and his neigbour : " + j + " = " + DistanceBetweenEachNeigbour);

            if(DistanceBetweenEachNeigbour < SperationRadius)
            {
                MoveBoidAwayFromNeigbour(_boid.transform, neigbour);
            }

        }
    }

    private void MoveBoidAwayFromNeigbour(Transform _boid, Transform _neigbour)
    {
        Vector3 moveAwayDirection = new Vector3(0, 0, 0);

        moveAwayDirection += _boid.position - _neigbour.position;
        Debug.Log(moveAwayDirection);

        _boid.position = _boid.position + moveAwayDirection * SeperationsStrenght; 



    }



}
