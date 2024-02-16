using System.Collections.Generic;
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


    [Header("Spawning")]
    public Transform CenterSpawn;
    public float SpawnRange = 30.0f;

    [Header("Boids")]
    public boid BoidPrefab;
    public int AmountBoids = 3;
    public GameObject CenterPrefab;
    public float NeighboursRadius = 5f;
    public float SeperationsStrenght = 0.3f;
    public float SperationRadius = 100.0f;

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
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 v1;
            Vector3 v2;
            Vector3 v3;

            foreach (boid boid in BoidList)
            {
                Vector3 centerOfMass = CalculateAverageMass(boid, BoidList);
                Vector3 perceivedCenterOfMass = CalculatePerceivedAverageMass(boid, BoidList);

                MassOfCenterPrefab.position = centerOfMass;
                PerceivedMassOfCenterPrefab.position = perceivedCenterOfMass;
                v1 = perceivedCenterOfMass;
                //v2 = 

                // regel Cohesion
                //CalculateMovementTowardsCenterOfNeigboursAndAverageVolicity(BoidList, perceivedCenterOfMass, boid);

                //v1 = rule1(boid, BoidList, centerOfMass);
                // regel Seperation
                //v2 = CalculateSerpartion(BoidList, boid);

                // regel allignment (werkt niet)
                //MatchSameVelocityFromNeigbours(BoidList, averageVelocity);

                // keep them in a sertain boundary
                //KeepboidInBoundary(boid);


                //boid.Velocity = boid.Velocity + v1 + v2;
                boid.transform.position = boid.transform.position + v1;

            }

        }

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
        return Vector3.zero;
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
        Vector3 perceivedCenterOfMass = new Vector3(0, 0, 0);

        for (int i = 0; i < boidList.Count; i++)
        {
            Transform neiboursTransforms = boidList[i].transform;
            Vector3 neigbourPos = neiboursTransforms.position;
            totalBoidsPos = totalBoidsPos + neigbourPos;

            //Debug.Log(boid.transform.name + " positie = " + BoidPos);
        }

        Vector3 c = totalBoidsPos / (boidList.Count);
        //c = c - _boid.transform.position;
        // Debug.Log("the Total vector : " + totalBoidsPos + " | Center of the mass : " + c + "neigbourcount : " + _neigboursList.Count);

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

        //return perceivedCenterOfMass;
        return (perceivedCenterOfMass - _boidJ.transform.position) / 100;
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

    private Vector3 CalculateMovementTowardsCenterOfNeigboursAndAverageVolicity(List<boid> _neigboursList, Vector3 _centerOfMass, boid _boid)
    {
        Vector3 totalVelocity = new Vector3(0, 0, 0);
        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (_neigboursList.Count != 0)
        {
            foreach (boid boid in _neigboursList)
            {
                moveDirection = _centerOfMass - _boid.transform.position;
                Vector3 currentVelocity = _boid.transform.position + moveDirection.normalized * Time.deltaTime;

                totalVelocity = totalVelocity + currentVelocity;
                _boid.transform.position = currentVelocity;




            }
            // bereken hier de velocity van elke boid en zet hem in een nieuwe lijst en return die en geef hem door aan regel 3
        }
        Vector3 averageVelocity = totalVelocity / _neigboursList.Count;

        Debug.Log("movedirection = " + moveDirection);
        Debug.Log("totalVelocity = " + totalVelocity);
        Debug.Log("averageVelocity = " + averageVelocity);
        return averageVelocity;

    }

    private Vector3 CalculateSerpartion(List<boid> _neigboursList, boid _boid)
    {
        Vector3 c = new Vector3(0, 0, 0);

        for (int j = 0; j < _neigboursList.Count; j++)
        {
            Transform neigbour = _neigboursList[j].transform;

            //float DistanceBetweenEachNeigbour = Vector3.Distance(_boid.transform.position, neigbour.position);
            float DistanceBetweenEachNeigbour = (_boid.transform.position - neigbour.position).sqrMagnitude;


            //Debug.Log("Distance between boid : " + _boid + " and his neigbour : " + j + " = " + DistanceBetweenEachNeigbour);

            if (DistanceBetweenEachNeigbour < SperationRadius)
            {
                //MoveBoidAwayFromNeigbour(_boid.transform, neigbour);
                c = c - (neigbour.transform.position - _boid.transform.position);
            }


        }
        return c;
    }

    private void MoveBoidAwayFromNeigbour(Transform _boid, Transform _neigbour)
    {
        Vector3 moveAwayDirection = new Vector3(0, 0, 0);

        moveAwayDirection += _boid.position - _neigbour.position;
        Debug.Log(moveAwayDirection);

        _boid.position = _boid.position + moveAwayDirection * SeperationsStrenght;

    }

    private void MatchSameVelocityFromNeigbours(List<boid> _neigbourList, Vector3 _averageVelocityNeigbours)
    {
        Vector3 perceivedVector;

        foreach (boid _boid in _neigbourList)
        {
            _boid.transform.position = _boid.transform.position + _averageVelocityNeigbours.normalized * Time.deltaTime;
        }
    }

    private void KeepboidInBoundary(boid _boid)
    {
        Vector3 boidPos = _boid.transform.position;
        Vector3 boidVector = new Vector3(0, 0, 0);
        if (boidPos.x < MinX)
        {
            boidVector = boidPos - (Vector3.left * BoundryStrenght);
        }
        else if (boidPos.x > MaxX)
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
