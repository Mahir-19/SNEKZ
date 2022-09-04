using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //TheTypes 0=MoveTowardsPlayer 1=Enemy with turrent 2=Fixed Waypoints 3=ExploadingEnemy 4=DieSpawnEnemies
    public int TheType;
    public Transform Target;
    public float MoveSpeed = 5;
    public GameObject[] TheTypesObj;
    public Transform Turrent;
    public Transform TurrentLookHelper;
    public Transform Muzzle;
    public Transform LookHelper;
    public GameObject TheBullet;
    public float BulletForce=500;
    public float FireRate = 0.3f;
    float FireTimestamp;
    Vector3 RandTarget1;
    Vector3 RandTarget2;
    Vector3 RandTarget;
    bool IsDead;
    public GameObject ExplosionObj;
    bool Initialized;
    public Transform[] SpawnPos;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < TheTypesObj.Length; i++)
        {
            TheTypesObj[i].SetActive(false);
        }
        if (TheTypesObj.Length < TheType)
        {
            TheType = 0;
        }
        
        TheTypesObj[TheType].SetActive(true);
        if (TheType == 2)
        {
            RandTarget1 = new Vector3(Random.Range(-12, 12), Random.Range(-7, 7), 0);
            RandTarget2 = new Vector3(Random.Range(-12, 12), Random.Range(-7, 7), 0);
            while (Vector3.Distance(RandTarget1, RandTarget2) < 5)
            {
                RandTarget2 = new Vector3(Random.Range(-12, 12), Random.Range(-6, 6), 0);
            }
            RandTarget = RandTarget1;
        }
        Initialized = true;

    }

    // Update is called once per frame
    public void ManualUpdate()
    {
        if (IsDead||!Initialized)
            return;
        CollisionMan(1);
        if (TheType == 1)
        {
            TurrentLookHelper.LookAt(Target);
            Turrent.localEulerAngles= new Vector3(TurrentLookHelper.localEulerAngles.x, TurrentLookHelper.localEulerAngles.y, 0);
            if (FireTimestamp < Time.time)
            {
                // GameObject Go = Instantiate(TheBullet, Muzzle.position, Quaternion.identity);
                GameObject Go = GetComponentInParent<Spawner>().GetBullet();
                Go.SetActive(true);
                Go.GetComponent<Rigidbody>().isKinematic = false;
                Go.transform.position = Muzzle.position;
                Vector3 Dir = Muzzle.forward;
                Go.GetComponent<Rigidbody>().AddForce(Dir * BulletForce);
               // Destroy(Go, 10f);
                FireTimestamp = Time.time + FireRate;
            }
        }
        else if (TheType == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, RandTarget, MoveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, RandTarget) < 1)
            {
                if (RandTarget == RandTarget1)
                {
                    RandTarget = RandTarget2;
                }
                else
                {
                    RandTarget = RandTarget1;
                }
            }
          
        }
        else
        {
            LookHelper.LookAt(Target);
            TheTypesObj[TheType].transform.localEulerAngles = new Vector3(LookHelper.localEulerAngles.x, 90, 0);
           // transform.LookAt(Target);
            transform.position = Vector3.MoveTowards(transform.position, Target.position, MoveSpeed * Time.deltaTime);
           
        }
      
    }
    void CollisionMan(float Dist)
    {
        RaycastHit Hit;
        for (int i = 0; i < 4; i++)
        {
            float YDir = 0;
            if (i == 1)
            {
                YDir = 90;
            }
            else if (i == 2)
            {
                YDir = 180;
            }
            if (i == 3)
            {
                YDir = -90;
            }
            //Debug.Log(YDir);
            Vector3 Dir = new Vector3(0, YDir, 0);
            if (Physics.Raycast(transform.position, Dir, out Hit, Dist))
            {
                //Debug.Log(Hit.transform.name);
                if (Hit.transform.GetComponentInParent<Part>() != null)
                {
                    IsDead = true;
                    GetComponentInParent<Spawner>().TheMan.PlayHitSound();
                    Destroy(Hit.transform.GetComponentInParent<Part>().gameObject);
                    if (TheType == 3)
                    {
                        ExplosionObj.SetActive(true);
                        TheTypesObj[TheType].SetActive(false);
                        Destroy(gameObject, 2f);
                    }
                    else if (TheType == 4)
                    {
                        GetComponentInParent<Spawner>().SpawnSubEnemies(SpawnPos);
                        Destroy(gameObject);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }

            }

        }
    }
}
