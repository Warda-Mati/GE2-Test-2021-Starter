using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harmonics : MonoBehaviour
{
    public float frequencyFactor = 1;

    public float amplitude = 40;

    public float theta = 0;

    private Boid dog;

    private float frequency;
    // Start is called before the first frame update
    void Start()
    {
        
        dog = transform.parent.GetComponent<Boid>();
        frequency = 1;
    }

    // Update is called once per frame
    void Update()
    {
        frequency = Mathf.Abs(dog.velocity.z * frequencyFactor);
        float angle = Mathf.Sin(theta) * amplitude;
        Quaternion q = Quaternion.AngleAxis(angle,Vector3.up);
        transform.localRotation = q;
        theta += Mathf.PI * 2.0f * Time.deltaTime * frequency;
    }
}
