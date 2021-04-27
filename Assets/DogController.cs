using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public Transform player;
    public int playerDistance = 10;
    
    public bool isCarryingBall = false;
 
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<StateMachine>().ChangeState(new ArriveToPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

class ArriveToPlayer : State
{
    private DogController dog;
    public override void Enter()
    {
        dog = owner.GetComponent<DogController>();
        owner.GetComponent<Arrive>().enabled = true;
        
    }

    public override void Think()
    {
        // move to position
        Vector3 toPlayer = owner.transform.position - dog.player.position;
        toPlayer.Normalize();
        //Vector3 targetPos = new Vector3(dog.player.position.x + toPlayer.x * dog.playerDistance,0,dog.player.position.z + toPlayer.z * dog.playerDistance);
        Vector3 targetPos = dog.player.position + toPlayer * dog.playerDistance;
        targetPos.y = 0;
        owner.GetComponent<Arrive>().targetPosition = targetPos;
        if (Vector3.Distance(owner.transform.position, targetPos) < 2.0f)
        {
            // if dog is carrying ball, drop it and allow player to throw new one
            if (dog.isCarryingBall)
            {
                dog.isCarryingBall = false;
                GameObject ball = dog.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                ball.transform.parent = null;
                ball.GetComponent<Rigidbody>().useGravity = true;
                dog.player.gameObject.GetComponent<Player>().hasThrown = false;
            }
            // dog will look at player
            owner.ChangeState(new LookAtPlayer());
        }
        // if there is a ball, go fetch
        if (GameObject.FindWithTag("ball"))
        {
            owner.ChangeState(new FetchBall());
        }
    }

    public override void Exit()
    {
        // stop dog when close to player
        owner.GetComponent<Arrive>().enabled = false;
        owner.GetComponent<Boid>().velocity = Vector3.zero;
        owner.GetComponent<Boid>().force = Vector3.zero;
        owner.GetComponent<Boid>().acceleration = Vector3.zero;

    }
}

class LookAtPlayer : State
{
    private DogController dog;
    private GameObject dogHead;
    public override void Enter()
    {
        dog = owner.GetComponent<DogController>();
        dogHead = dog.gameObject.transform.Find("head").gameObject;
        
    }

    public override void Think()
    {
        // rotate dog's head to look at player by rotating it
        int rotationSpeed = 2;
        Vector3 toPlayer = dog.player.position - dogHead.transform.position;
        Quaternion lookAtPlayer = Quaternion.LookRotation(toPlayer);
        dogHead.transform.rotation =
        Quaternion.Slerp(dogHead.transform.rotation, lookAtPlayer, rotationSpeed * Time.deltaTime);
    
        if (Vector3.Distance(owner.transform.position, dog.player.position) > 20.0f)
        {
            owner.ChangeState(new ArriveToPlayer());
        }

        if (GameObject.FindWithTag("ball"))
        {
            owner.ChangeState(new FetchBall());
        }
    }

    public override void Exit()
    {
       
    }
}

class FetchBall : State
{
    private DogController dog;
    private GameObject ball;
    private GameObject dogAttachParent;
    public override void Enter()
    {
        // play barking noise
        owner.GetComponent<AudioSource>().Play();
        dog = owner.GetComponent<DogController>();
        owner.GetComponent<Arrive>().enabled = true;
        //owner.GetComponent<Arrive>().targetGameObject = GameObject.FindWithTag("ball");
        dogAttachParent = dog.gameObject.transform.Find("dog").gameObject;
        //ball = owner.GetComponent<Arrive>().targetGameObject;
        ball = GameObject.FindWithTag("ball");

    }

    public override void Think()
    {
        // seek and arrive to ball's position
        owner.GetComponent<Arrive>().targetPosition = ball.transform.position;
        owner.GetComponent<Arrive>().targetPosition.y = 0;
        if (Vector3.Distance(owner.transform.position, ball.transform.position) < 2f)
        {
           // set ball's parent and position to attach empty object
            Transform ballAttach = dogAttachParent.transform.GetChild(0);
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero; 
            ball.GetComponent<Rigidbody>().useGravity = false; 
            ball.transform.parent = ballAttach;
            ball.transform.localPosition = Vector3.zero;
            ball.gameObject.tag = "Untagged";
            
        }
        // if ball is caught return to player
        if(ball.tag == "Untagged")
            owner.ChangeState(new ArriveToPlayer());
    }

    public override void Exit()
    {
        dog.isCarryingBall = true;
        //owner.GetComponent<Arrive>().targetPosition = null;
        owner.GetComponent<Boid>().velocity = Vector3.zero;
    }
}

