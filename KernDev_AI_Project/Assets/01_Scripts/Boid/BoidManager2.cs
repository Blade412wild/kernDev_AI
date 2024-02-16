using JetBrains.Annotations;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoidManager2 : MonoBehaviour
{
    [Header("WORLD Boundry")]
    public int MinX = -40;
    public int MaxX = 40;

    public int MinY = -40;
    public int MaxY = 40;

    public int MinZ = -40;
    public int MaxZ = 40;

    public int BoundryStrenght;
    public float turnStrenght;
    public int changeDirectionDistance = 15;


    [Header("Spawning")]
    public Transform CenterSpawn;
    public float SpawnRange = 30.0f;

    [Header("Boids")]
    public boid BoidPrefab;
    public int AmountBoids = 3;
    public GameObject CenterPrefab;

    [SerializeField] private float cohesion = 100;
    [SerializeField] private float seperation = 100;
    [SerializeField] private float SeperationStrenght = 10f;
    [SerializeField] private float allignment = 8;






    [SerializeField] private Transform PerceivedMassOfCenterPrefab;
    [SerializeField] private Transform MassOfCenterPrefab;



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
        //if (Input.GetKey(KeyCode.Space))
        //{
        Vector3 v1;
        Vector3 v2;
        Vector3 v3;
        Vector3 v4;




        foreach (boid boid in BoidList)
        {
            Vector3 centerOfMass = CalculateAverageMass(boid, BoidList);
            Vector3 perceivedCenterOfMass = CalculatePerceivedAverageMass(boid, BoidList);

            v1 = perceivedCenterOfMass;
            v2 = rule2(boid, BoidList);
            v3 = rule3(boid, BoidList);
            v4 = KeepboidInBoundary(boid);
            avoidObstable(boid);
            //transform.position += transform.TransformDirection(perceivedCenterOfMass) * 5 * Time.deltaTime;

            //boid.transform.Rotate(0.0f, turnStrenght, 0.0f, Space.Self);


            //boid.Velocity = boid.Velocity + v1 + v2+ v3;

            //boid.transform.position = boid.transform.position + (v1);
            //boid.transform.position = boid.transform.position + (v1 + v2);
            //boid.transform.position = boid.transform.position + (v1 + v2 + v3);
            Vector3 newDirection = v1 + v2 + v3 + v4;
            //Vector3 lookDirection = newDirection - boid.transform.position;
            //Quaternion rotation = Quaternion.LookRotation(lookDirection);
            //boid.transform.rotation = rotation;
            boid.transform.position = boid.transform.position + newDirection;


            //boid.Velocity = boid.Velocity + v1;
            //boid.transform.position = boid.transform.position + boid.Velocity;
        }

        //}

    }
    private Vector3 rule1(boid _boidj, List<boid> _boids, Vector3 _centerOfMass)
    {
        Vector3 totalVelocity = new Vector3(0, 0, 0);

        if (_boids.Count != 0)
        {
            Vector3 moveDirection = Vector3.zero;
            foreach (boid _boid in _boids)
            {
                moveDirection = _centerOfMass - _boid.transform.position;
                Vector3 currentVelocity = _boid.transform.position + moveDirection.normalized; //* Time.deltaTime;

                totalVelocity = totalVelocity + currentVelocity;
                _boid.transform.position = currentVelocity;
                moveDirection = totalVelocity;

                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(_boid.transform.position, _boid.transform.position - currentVelocity, out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(_boid.transform.position, _boid.transform.position - currentVelocity * hit.distance, Color.yellow);
                    //Debug.Log("Did Hit");
                }
                else
                {
                    Debug.DrawRay(_boid.transform.position, _boid.transform.position - currentVelocity * 1000, Color.white);
                    //Debug.Log("Did not Hit");
                }
            }
            //return moveDirection;
        }
        else
        {
            return Vector3.zero;
        }

        Vector3 averageVelocity = totalVelocity / _boids.Count;

        return averageVelocity;

    }

    private Vector3 rule2(boid _boidj, List<boid> _boids)
    {
        Vector3 c = Vector3.zero;

        foreach (boid _boid in _boids)
        {
            if (_boid != _boidj)
            {
                float DistanceBetweenEachNeigbour = (_boid.transform.position - _boidj.transform.position).sqrMagnitude;
                if (DistanceBetweenEachNeigbour < seperation)
                {
                    //MoveBoidAwayFromNeigbour(_boid.transform, neigbour);
                    c = c - (_boid.transform.position - _boidj.transform.position);
                }
            }
        }

        return c / SeperationStrenght;
    }
    private Vector3 rule3(boid _boidj, List<boid> _boids)
    {
        Vector3 perceivedVelocity = new Vector3(0, 0, 0);

        foreach (boid boid in _boids)
        {
            if (boid != _boidj)
            {
                perceivedVelocity = perceivedVelocity + boid.Velocity;
            }

        }
        perceivedVelocity = perceivedVelocity / (_boids.Count - 1);

        //return perceivedCenterOfMass;
        return (perceivedVelocity - _boidj.Velocity) / allignment;
    }



    private Vector3 CalculateRandomSpawn()
    {
        float randomX = Random.Range((CenterSpawn.position.x - SpawnRange), (CenterSpawn.position.x + SpawnRange));
        float randomY = Random.Range((CenterSpawn.position.y - SpawnRange), (CenterSpawn.position.y + SpawnRange));
        float randomZ = Random.Range((CenterSpawn.position.z - SpawnRange), (CenterSpawn.position.z + SpawnRange));
        Vector3 spawnPos = new Vector3(randomX, randomY, randomZ);

        return spawnPos;
    }

    private Vector3 CalculateAverageMass(boid _boid, List<boid> boidList)
    {
        Vector3 totalBoidsPos = new Vector3(0, 0, 0);
        for (int i = 0; i < boidList.Count; i++)
        {
            Transform neiboursTransforms = boidList[i].transform;
            Vector3 neigbourPos = neiboursTransforms.position;
            totalBoidsPos = totalBoidsPos + neigbourPos;
        }

        Vector3 c = totalBoidsPos / (boidList.Count);
        MassOfCenterPrefab.position = c;

        return c;
    }
    private Vector3 CalculatePerceivedAverageMass(boid _boidJ, List<boid> boidList)
    {
        Vector3 perceivedCenterOfMass = new Vector3(0, 0, 0);

        foreach (boid boid in boidList)
        {
            if (boid != _boidJ)
            {
                perceivedCenterOfMass = perceivedCenterOfMass + boid.transform.position;
            }

        }
        perceivedCenterOfMass = perceivedCenterOfMass / (boidList.Count - 1);
        PerceivedMassOfCenterPrefab.position = perceivedCenterOfMass;
        //Debug.Log(perceivedCenterOfMass);

        //return perceivedCenterOfMass;
        return (perceivedCenterOfMass - _boidJ.transform.position) / cohesion;
    }

    private Vector3 KeepboidInBoundary(boid _boid)
    {
        Vector3 boidPos = _boid.transform.position;
        Vector3 vector = new Vector3(0, 0, 0);

        if (boidPos.x < MinX)
        {
            _boid.transform.Rotate(0.0f, turnStrenght, 0.0f, Space.World);
            vector.x = BoundryStrenght * 1;
        }
        else if (boidPos.x > MaxX)
        {
            _boid.transform.Rotate(0.0f, turnStrenght, 0.0f, Space.World);
            vector.x = BoundryStrenght * -1;

        }

        if (boidPos.y < MinY)
        {
            vector.y = BoundryStrenght * 1;
        }
        else if (boidPos.y > MaxY)
        {
            vector.y = BoundryStrenght * -1;
        }

        if (boidPos.z < MinZ)
        {
            _boid.transform.Rotate(0.0f, turnStrenght, 0.0f, Space.World);
            vector.z = BoundryStrenght * 1;
        }
        else if (boidPos.z > MaxZ)
        {
            _boid.transform.Rotate(0.0f, turnStrenght, 0.0f, Space.World);
            vector.z = BoundryStrenght * -1;
        }

        return vector;

    }

    private void avoidObstable(boid _boid)
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(_boid.transform.position, _boid.transform.TransformDirection(Vector3.forward), out hit, changeDirectionDistance))
        {
            Debug.DrawRay(_boid.transform.position, _boid.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            _boid.transform.Rotate(0.0f, turnStrenght, 0.0f, Space.World);

            Debug.Log("Did Hit");

        }
        else
        {
            Debug.DrawRay(_boid.transform.position, _boid.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }

}
