using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLooking : MonoBehaviour
{

    [SerializeField] private Transform TargetCenter;
    Vector3 lookingDir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetCenter == null) return;
        //lookingDir = TargetCenter.position - transform.position;
        transform.LookAt(TargetCenter.position);
    }
}
