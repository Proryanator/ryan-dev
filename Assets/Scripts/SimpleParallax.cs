using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParallax : MonoBehaviour{
    
    private Transform cameraTransform;
    private Vector3 cameraLastPosition;

    [SerializeField]
    private float parallaxMultiplier = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    
    void LateUpdate(){
        Vector3 delta = cameraTransform.position - cameraLastPosition;
        
        // calculate how far the camera has moved
        transform.position += delta * parallaxMultiplier;
        
        // update the last position of the camera
        cameraLastPosition = cameraTransform.position;
    }
}
