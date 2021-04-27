using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject ballPrefab;

    public int force;

    public bool hasThrown = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ThrowBall());
        GetComponent<AudioSource>().Play(0);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator ThrowBall()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.Space) && !hasThrown)
            {
                GameObject ball = Instantiate(ballPrefab, transform.position, transform.rotation);
                ball.GetComponent<Rigidbody>().velocity = Vector3.forward * force;
                hasThrown = true;
                yield return new WaitForSeconds(2);
            }

            yield return null;
        }
    }
}
