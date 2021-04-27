using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public float frequencyFactor = 1;

    public float amplitude = 40;

    public float theta = 0;



    private float frequency;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        frequency = frequencyFactor;
        float angle = Mathf.Sin(theta) * amplitude;
        Quaternion q = Quaternion.AngleAxis(angle,Vector3.forward);
        transform.localRotation = q;
        theta += Mathf.PI * 2.0f * Time.deltaTime * frequency;
    }
}
