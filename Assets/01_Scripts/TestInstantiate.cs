using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInstantiate : MonoBehaviour {

    public GameObject targetToMove;
    public GameObject targetToReach;
    public float offsetValue;
    public float offsetDistance;

    private void Awake()
    {
        SpawnTargetToReach();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            MoveShipToFront();
        if (Input.GetKeyDown(KeyCode.A))
            MoveShipToBack();
    }

    void SpawnTargetToReach()
    {
        Vector3 lastPosition = transform.position;
        Vector3 offsetVector = new Vector3(offsetDistance, 0, 0);
        GameObject goal = Instantiate(targetToReach, lastPosition + offsetVector, Quaternion.identity);
        goal.name = "Goal";
        
    }

    public void MoveShipToFront() // Move Ship To Front
    {
        GameObject goal = GameObject.Find("Goal");

        if (transform.position.x < goal.transform.position.x)//Front Limit in x Position
        {
            Vector3 lastPosition = transform.position;
            Vector3 offsetVector = new Vector3(offsetValue, 0, 0);
            GameObject target = Instantiate(targetToMove, lastPosition + offsetVector, Quaternion.identity);
            target.name = "SpawnedTarget";
            StartCoroutine(MoveOverSeconds(this.gameObject, target.transform.position, 2));
            Destroy(target.gameObject, 2);
        }
        else if (transform.position.x >= goal.transform.position.x)
            print("Reached Goal");

    }

    public void MoveShipToBack() // Move Ship To Back
    {
        Vector3 lastPosition = transform.position;
        Vector3 offsetVector = new Vector3(offsetValue, 0, 0);
        GameObject target = Instantiate(targetToMove, lastPosition - offsetVector, Quaternion.identity);
        target.name = "SpawnedTarget";
        StartCoroutine(MoveOverSeconds(this.gameObject, target.transform.position, 2));
        Destroy(target.gameObject, 2);

    }

    IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds) // Move the bike to the position that will be selected based on answer
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;

        while (elapsedTime < seconds)
        {
            transform.position = Vector3.LerpUnclamped(startingPos, end, (elapsedTime / seconds));
            /*particlesStopMotion.SetActive(false);

            if (!isMovingBack)
                particlesMoveMotionFront.SetActive(true);
            else
                particlesMoveMotionBack.SetActive(true);*/

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = end;
        /*
        particlesMoveMotionFront.SetActive(false);
        particlesMoveMotionBack.SetActive(false);
        particlesStopMotion.SetActive(true);
        */

    }





}
