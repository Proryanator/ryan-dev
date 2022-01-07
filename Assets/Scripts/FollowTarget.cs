using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public float damping = 5.0f;

    // Update is called once per frame
    void Update()
    {

        Vector3 wantedPosition;
        wantedPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);
    }
}
