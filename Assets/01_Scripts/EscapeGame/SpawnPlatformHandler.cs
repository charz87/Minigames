using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatformHandler : MonoBehaviour {

    public GameObject[] stageCollection;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SpawnTrigger")
        {
            GameObject stage = other.gameObject;
            Transform spawnLocation = stage.transform.parent.Find("SpawnLocation");
            GameObject newPlatform = Instantiate(stageCollection[Random.Range(0, stageCollection.Length)], spawnLocation.transform.position, Quaternion.identity);

            newPlatform.name = "SpawnedPlatform";
        }

        if(other.gameObject.name == "SpawnLocation")
        {
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }

}
