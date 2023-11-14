using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CenterOfMass : MonoBehaviour
{
    public List<GameObject> BoidList;

    public Transform Center;

    // Angular speed in radians per sec.
    public float speed = 2.0f;

    private Vector3 c = new Vector3(0, 0, 0); //CenterOfMass
    private Vector3 totalBoidsPos = new Vector3(0, 0, 0); //CenterOfMass



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < BoidList.Count; i++)
        {
            GameObject boid = BoidList[i];
            Vector3 BoidPos = boid.transform.position;
            totalBoidsPos = totalBoidsPos + BoidPos;
            Debug.Log(boid.transform.name + " positie = " + BoidPos);
            Debug.Log(c);
        }
        
        c = totalBoidsPos / BoidList.Count;

        Center.position = c;
        Debug.Log( "CenterOfMass = " + c);
    }

    // Update is called once per frame
    void Update()
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


        foreach (GameObject boid in BoidList)
        {
            boid.transform.position = (c - boid.transform.position) / 10000;
        }

        Debug.Log("CenterOfMass = " + c);
    }
}
