using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeMover : MonoBehaviour
{

    public GameObject targetToMove; // Where i´m going to move

    private bool isReachedEnemy = false; // Have i reached the enemy position

    public Transform enemyPosition; // The position of the enemy pursuer

    public float offsetValue;// This is the Distance that we would like to advance and reduce with each answer selection

    public GameObject particleGroundSmoke; // The particle of the Bike

    public AudioClip bikeMotor; //Audio Idle of Bike

    public AudioClip bikeMotorAccel; // Audio Accelerate of Bike

    public AudioClip bikeMotorSlow; // Audio SlowDown of Bike

    [SerializeField]
    private AudioSource myMainAudioSource; //Audio source tha reproduces the Idle Bike Audio

    [SerializeField]
    private AudioSource mySecondaryAudioSource; //Audio source tha reproduces the Accelerate and Slowdown Bike Audio

    private bool isMovingBack = false;// To check if bike is moving back or front

    public void MoveBikeToFront() // Move the bike to the front position
    {
        isMovingBack = false;
        GameObject goal = GameObject.Find("Goal");

        if (transform.position.x < goal.transform.position.x)//Front Limit in x Position - TODO remove magic number
        {
            Vector3 lastPosition = transform.position;
            Vector3 offsetVector = new Vector3(offsetValue, 0, 0);
            GameObject target = Instantiate(targetToMove, lastPosition + offsetVector, Quaternion.identity);
            target.name = "SpawnedTarget";

            StartCoroutine(MoveOverSeconds(this.gameObject, target.transform.position, 2));
            Destroy(target.gameObject, 2);

            mySecondaryAudioSource.PlayOneShot(bikeMotorAccel);
        }
    }
       
    public void MoveBikeToBack()// Move the bike to the back position
    {
        isMovingBack = true;
        
        if (transform.position.x > enemyPosition.position.x)//Back Limit in x Position - TODO remove magic number
        {
            Vector3 lastPosition = transform.position;
            Vector3 offsetVector = new Vector3(offsetValue, 0, 0);
            GameObject target = Instantiate(targetToMove, lastPosition - offsetVector, Quaternion.identity);
            target.name = "SpawnedTarget";

            StartCoroutine(MoveOverSeconds(this.gameObject, target.transform.position, 2));
            Destroy(target.gameObject, 2);

            mySecondaryAudioSource.PlayOneShot(bikeMotorSlow);
        }
        else if(transform.position.x <= enemyPosition.position.x) // Have i reached where the position x is 
        {
            isReachedEnemy = true;
            particleGroundSmoke.SetActive(false);
        }
    }

    IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds) // Move the bike to the position that will be selected based on answer
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;

        ParticleSystem.VelocityOverLifetimeModule groundVelocity = particleGroundSmoke.GetComponent<ParticleSystem>().velocityOverLifetime;
        ParticleSystem.MinMaxCurve rate = new ParticleSystem.MinMaxCurve();

        while (elapsedTime < seconds)
        {
            transform.position = Vector3.LerpUnclamped(startingPos, end, (elapsedTime / seconds));
            mySecondaryAudioSource.volume = 0.6f;

            if (isMovingBack)//Just when moving front we accel
            {
                rate.constantMax = 1.61f;//
                groundVelocity.x = rate;
            }

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = end;
        rate.constantMax = -2.61f;//
        groundVelocity.x = rate;
        mySecondaryAudioSource.volume = 0.0f;


    }
    public bool ReachedEnemy() // Get if we have reached the enemy x position
    {
        return isReachedEnemy;
    }

}
