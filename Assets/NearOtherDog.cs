using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearOtherDog : MonoBehaviour
{
    public int distance;
    public int rotationSpeed;
    public GameObject otherDog;

    private bool barking = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Bark());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, otherDog.transform.position) < distance)
        {
            GetComponent<FollowPath>().enabled = false;
            GetComponent<Boid>().velocity = Vector3.zero;
            GetComponent<Boid>().acceleration = Vector3.zero;
            GetComponent<Boid>().force = Vector3.zero;
            Vector3 toDog= otherDog.transform.position - transform.position;
            Quaternion lookAtDog = Quaternion.LookRotation(toDog);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, lookAtDog, rotationSpeed * Time.deltaTime);
            barking = true;
        }
        else
        {
            GetComponent<FollowPath>().enabled = true;
            barking = false;
        }
    }

    IEnumerator Bark()
    {
        while (true)
        {
            if (barking)
            {
                GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(2);
            }

            yield return null;
        }
       
    }
}
