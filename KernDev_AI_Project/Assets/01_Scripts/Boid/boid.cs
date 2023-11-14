using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boid : MonoBehaviour
{
    public Collider collider;

    [SerializeField] private int Force = 1;
    private int speed = 5;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //rb.AddRelativeForce(Vector3.fwd * 1);
    }
}
