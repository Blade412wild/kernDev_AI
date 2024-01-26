using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour
{
    float fieldofView;
    Transform target;
    Transform agent;

    private float range;
    private List<GameObject> sneseObjects = new List<GameObject>();
    private List<GameObject> allPossibleSenseAbleObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //allPossibleSenseAbleObjects = 
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool LineOfSight()
    {
        // is target in range
        Vector3 dirToTarget = target.position - agent.position;
        if (Vector3.SqrMagnitude(agent.position - target.position) > range * range) { return false; }

        
        float dot = Vector3.Dot(agent.forward, dirToTarget.normalized);
        if (dot < fieldofView) { return false; }

        if(Physics.Raycast(agent.position, dirToTarget, out RaycastHit hitInfo))
        {
            return true;
        }

        return false;

    }

}

    public interface IDetectable
    {

    }