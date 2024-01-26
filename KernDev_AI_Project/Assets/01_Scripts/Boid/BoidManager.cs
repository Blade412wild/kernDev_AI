using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [Header("WORLD Boundry")]
    public int MinX = -40;
    public int MaxX = 40;

    public int MinY = -40;
    public int MaxY = 40;

    public int MinZ = -40;
    public int MaxZ = 40;

    public int BoundryStrenght;


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
            Vector3 averageVelocity = CalculateMovementTowardsCenterOfNeigboursAndAverageVolicity(neighbours, centerOfMass, boid);

            // regel Seperation
            CalculateSerpartion(neighbours, boid);

            // regel allignment (werkt niet)
            MatchSameVelocityFromNeigbours(neighbours, averageVelocity);

            // keep them in a sertain boundary
            //KeepboidInBoundary(boid);


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

    private Vector3 CalculateMovementTowardsCenterOfNeigboursAndAverageVolicity(List<Transform> _neigboursList, Vector3 _centerOfMass, boid _boid)
    {
        Vector3 totalVelocity = new Vector3(0, 0, 0);
        if (_neigboursList.Count != 0)
        {
            foreach (Transform _transform in _neigboursList)
            {
                Vector3 moveDirection = _centerOfMass - _transform.position;
                Vector3 currentVelocity = _transform.position + moveDirection.normalized * Time.deltaTime;

                totalVelocity = totalVelocity + currentVelocity;
                _transform.position = currentVelocity;

            }
            // bereken hier de velocity van elke boid en zet hem in een nieuwe lijst en return die en geef hem door aan regel 3
        }
        else
        {
            //// sommige boid hebben geen buren dus bewegen ze niet, nu gaan ze naar het midden toe om weer een groep te vormen
            //Vector3 moveDirection = CenterSpawn.position - _boid.transform.position;
            //Vector3 currentVelocity = _boid.transform.position + moveDirection.normalized * Time.deltaTime * 15;
            //_boid.transform.position = currentVelocity;
        }
        Vector3 averageVelocity = totalVelocity / _neigboursList.Count;

        
        return averageVelocity;

    }

    private void CalculateSerpartion(List<Transform> _neigboursList, boid _boid)
    {

        for (int j = 0; j < _neigboursList.Count; j++)
        {
            Transform neigbour = _neigboursList[j];

            //float DistanceBetweenEachNeigbour = Vector3.Distance(_boid.transform.position, neigbour.position);
            float DistanceBetweenEachNeigbour = (_boid.transform.position - neigbour.position).sqrMagnitude;


            //Debug.Log("Distance between boid : " + _boid + " and his neigbour : " + j + " = " + DistanceBetweenEachNeigbour);

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

    private void MatchSameVelocityFromNeigbours(List<Transform> _neigbourList, Vector3 _averageVelocityNeigbours)
    {
        Vector3 perceivedVector;

        foreach(Transform _transform in _neigbourList)
        {
            _transform.position =  _transform.position + _averageVelocityNeigbours .normalized * Time.deltaTime;
        }
    }

    private void KeepboidInBoundary(boid _boid)
    {
        Vector3 boidPos = _boid.transform.position;
        Vector3 boidVector = new Vector3(0,0,0);
        if (boidPos.x < MinX)
        {
            boidVector = boidPos - (Vector3.left * BoundryStrenght);
        }
        else if(boidPos.x > MaxX)
        {
            boidVector = boidPos - (Vector3.right * BoundryStrenght);

        }

        if (boidPos.y < MinY)
        {
            boidVector = boidPos - (Vector3.down * BoundryStrenght);
        }
        else if (boidPos.y > MaxY)
        {
            boidVector = boidPos - (Vector3.up * BoundryStrenght);

        }

        if (boidPos.z < MinZ)
        {
            boidVector = boidPos - (Vector3.back * BoundryStrenght);
        }
        else if (boidPos.z > MaxZ)
        {
            boidVector = boidPos - (Vector3.forward * BoundryStrenght);
        }

        _boid.transform.position = _boid.transform.position + boidVector * Time.deltaTime;
        Debug.Log(boidVector);

    }



}
