using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public GameManagerMultipleChoice_Cannon gameManagerRef; // Reference to the Cannon Gamemanager
    public Transform[] missTargets; // Array of miss point Targets
    public Transform pointPlayerTarget; // the Point of the target i will shoot
    public Transform pointBossTarget; // The Point of he Boss Target i will shoot
    public Health objectHealth; // The Health of the object a will shoot
    public Health bossHealth; // The enemy Boss Health
    private Transform myMissTarget; // The Miss target that will be selected among all my miss targets
    public GameObject projectilePrefab; // The projectile i will shoot
    public float shootAngle; // The angle i will shoot
    public GameObject shootFXParticles; // The particles used whenever i shoot
    public GameObject spawnProjectilePosition; // The position from where the projectile will spawn
    public float timerBeforeShoot = 1f; // time taken before actually shoot

    private Transform myTarget; // The real target that will be selected among all my targets depending on setting of game selected
    public Health myTargetHealth; // The real health target

    private void Awake()
    {
        //What Setting are we having
        switch (gameManagerRef.gameSetting)
        {
            case GameManagerMultipleChoice_Cannon.GameSetting.VS:
                myTarget = pointPlayerTarget;
                myTargetHealth = objectHealth;
                break;
            case GameManagerMultipleChoice_Cannon.GameSetting.CO_OP:
                myTarget = pointBossTarget;
                myTargetHealth = bossHealth;
                break;
            case GameManagerMultipleChoice_Cannon.GameSetting.SINGLE:
                myTarget = pointBossTarget;
                myTargetHealth = bossHealth;
                break;
            default:
                break;
        }
       
    }

    //Makes calculation between distance to target and the angle from where we want to shoot 
    Vector3 BallisticVel(Transform target, float angle)
    {
        Vector3 dir = target.position - transform.position; //get Direction of Target
        float h = dir.y; // get Height difference 
        dir.y = 0; // retaining horizontal direction only
        float distance = dir.magnitude;// get horizontal distance
        float a = angle * Mathf.Deg2Rad;//convert angle to radians
        dir.y = distance * Mathf.Tan(a);//set dir to elevation angle
        distance += h / Mathf.Tan(a);//correct for small height differences
        //Calculate the velocity magnitude
        float vel = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));

        return vel * dir.normalized;
    }
    //Calls when we want to Shoot the cannon ball To a Target
    public void ShootProjectileToTarget()
    {
        StartCoroutine(PrepareToShootProjectileToTarget());

    }
    //Calls when we want to Shoot the cannon ball To a Miss Target
    public void ShootProjectileToMissTarget()
    {
        StartCoroutine(PrepareToShootProjectileToMissTarget());
        
    }

    //Time for the Cannon to prepare to really Shoot the projectile To the Target and then shoot
    IEnumerator PrepareToShootProjectileToTarget()
    {
        yield return new WaitForSeconds(timerBeforeShoot);

        GameObject ball = Instantiate(projectilePrefab, spawnProjectilePosition.transform.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().velocity = BallisticVel(myTarget, shootAngle);
        GameObject shootFX = Instantiate(shootFXParticles, spawnProjectilePosition.transform.position, Quaternion.identity);

        Destroy(ball, 5);
        Destroy(shootFX, 3);
    }
    void SetMissTarget() // Set the MISS Target
    {
        int randomMissTargetIndex = Random.Range(0, missTargets.Length);
        myMissTarget = missTargets[randomMissTargetIndex];

    }

    //Time for the Cannon to prepare to really Shoot the projectile To the a MISS Target and then shoot
    IEnumerator PrepareToShootProjectileToMissTarget()
    {
        SetMissTarget();
        yield return new WaitForSeconds(timerBeforeShoot);

        GameObject ball = Instantiate(projectilePrefab, spawnProjectilePosition.transform.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().velocity = BallisticVel(myMissTarget, shootAngle);
        GameObject shootFX = Instantiate(shootFXParticles, spawnProjectilePosition.transform.position, Quaternion.identity);

        Destroy(ball, 5);
        Destroy(shootFX, 3);


    }

}
