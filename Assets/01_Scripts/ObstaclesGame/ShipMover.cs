using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMover : MonoBehaviour {

    public GameObject targetToMove; // Where i´m going to move
    private bool isShipReachedGoal = false; // Have i reached the Goal position
    public GameObject particlesShipWaves;
    public GameObject obstacleToAppear;
    public AudioClip rowing;
    public float offsetValue;
    private bool isMovingBack = false;
    private AudioSource myAudioSource;

    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();

    }

    public void MoveShipToFront() // Move Ship To Front
    {
        isMovingBack = false;

        GameObject goal = GameObject.Find("Goal");

        if (transform.position.x < goal.transform.position.x)//Front Limit in x Position
        {
            Vector3 lastPosition = transform.position;
            Vector3 offsetVector = new Vector3(offsetValue, 0, 0);
            GameObject target = Instantiate(targetToMove, lastPosition + offsetVector, Quaternion.identity);
            target.name = "SpawnedTarget";

            /* KEEP UNTIL IT'S SAVE TO REMOVE
            Vector3 moveDirection = new Vector3(transform.position.x + 10f, transform.position.y, transform.position.z);
            Transform spawnLocation = transform.FindChild("SpawnFrontShip");
            GameObject target = Instantiate(targetToMove, spawnLocation.position, Quaternion.identity);
            */
            StartCoroutine(MoveOverSeconds(this.gameObject, target.transform.position, 2));
            Destroy(target.gameObject, 2);
            myAudioSource.PlayOneShot(rowing);

        }
        else if (transform.position.x >= goal.transform.position.x)
            isShipReachedGoal = true;

    }

    public void MoveShipToBack()// Move the Ship to the back position
    {
        isMovingBack = true;

        GameObject goal = GameObject.Find("Goal");

        if (transform.position.x > -11.0f)//Back Limit in x Position - TODO remove magic number
        {
            /*KEEP UNTIL IT'S SAVE TO REMOVE*/
            /*Transform spawnLocation = transform.FindChild("SpawnBackShip");
            Transform spawnObstacleLocation = transform.FindChild("SpawnFrontShip");
            GameObject target = Instantiate(targetToMove, spawnLocation.position, Quaternion.identity);
            */
            Vector3 lastPosition = transform.position;
            Vector3 offsetVector = new Vector3(offsetValue, 0, 0);
            GameObject target = Instantiate(targetToMove, lastPosition - offsetVector, Quaternion.identity);
            target.name = "SpawnedTarget";
            GameObject obstacle = Instantiate(obstacleToAppear, lastPosition + offsetVector, Quaternion.Euler(0, -90, 0));

            StartCoroutine(MoveOverSeconds(this.gameObject, target.transform.position, 3));
            Destroy(target.gameObject, 3);
            Destroy(obstacle.gameObject, 3.5f);


        }
    
    }

    IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds) // Move the bike to the position that will be selected based on answer
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;

        ParticleSystem.VelocityOverLifetimeModule groundVelocity = particlesShipWaves.GetComponent<ParticleSystem>().velocityOverLifetime;
        ParticleSystem.MinMaxCurve rate = new ParticleSystem.MinMaxCurve();

        while (elapsedTime < seconds)
        {
            transform.position = Vector3.LerpUnclamped(startingPos, end, (elapsedTime / seconds));
          

            if (!isMovingBack)
            {
                rate.constantMax = -2f;//Ship moves Front
                groundVelocity.x = rate;
            }
            else
            {
                rate.constantMax = 2f;//Ship moves Back
                groundVelocity.x = rate;
            }
                

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = end;
        rate.constantMax = 0f;//Ship Stop
        groundVelocity.x = rate;
        myAudioSource.Stop();


    }

    public bool ShipReachedGoal()
    {
        return isShipReachedGoal;
    }

}
