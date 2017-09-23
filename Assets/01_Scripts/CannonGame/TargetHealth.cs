using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHealth : MonoBehaviour {

    public Health myHealth; //reference to the health of this object

    [SerializeField]
    private Animator animTarget;

    // if i get hit reduce my health
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Projectile")
        {
            myHealth.ReduceHealth();
            animTarget.SetTrigger("Hit");
            
        }

    }


}
