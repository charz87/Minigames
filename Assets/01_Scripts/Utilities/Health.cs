using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public Slider healthBar;// The slider UI representing the health bar

    public float healthAmount = 100f; // The max value of the health

    private float initialHealth; // the initial health i got

    private void Awake()
    {
        initialHealth = healthAmount; // set initial health to whatever i got in health amount
    }

    private void Update()
    {
        GetCurrentHealth(); //Get my current health
        
        healthBar.GetComponent<Slider>().value = healthAmount; // change slider value based on healthamount
    }

    public void ReduceHealth()// Reduce the Health 
    {
        healthAmount = healthAmount - (initialHealth * 0.1f);//Loses 10% of the initial health
    }

    public float GetCurrentHealth()// method to get the current health and pass it over to other scripts
    {
        return healthAmount; 
        
    }

}
