using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public GameObject explosionFXParticles;
    public GameObject waterSplashFXParticles;

    private void OnTriggerEnter(Collider other)
    {
        GameObject spawnFX;
        if(other.gameObject.tag == "Water")
        {
            spawnFX = Instantiate(waterSplashFXParticles, transform.position, Quaternion.identity);
        }
        else
        {
            spawnFX = Instantiate(explosionFXParticles, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
            
        Destroy(spawnFX, 3);
    }

}
