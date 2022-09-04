using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Snake : MonoBehaviour
{
    public float fTurnRate = 90.0f;  // 90 degrees of turning per second
    public float fSpeed = 1.0f;  // Units per second of movement;
    public GameObject PartPref;
    public List<GameObject> TheParts=new List<GameObject>();
    public Vector3[] ThePos ;
    float timestamp;
    float HurtTimestamp;
    GameManager TheMan;
    public Vector2 MaxBounds;
    public Vector2 MaxBoundsPerformance;
    private void Start()
    {
        TheMan = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        int MaxParts = 50;
        if (TheMan.IsPerformanceTest)
        {
            MaxParts = 500;
        }
        SpawnParts(MaxParts);
    }

    void Update()
    {
        if (TheMan.InGameHUD.activeSelf&&!TheMan.IsGameOver)
        {
            InputMan();
            SnakeLogic();
            CollisionMan(1f);
            if (TheMan.IsPerformanceTest)
            {
                if (transform.position.x >= MaxBoundsPerformance.x || transform.position.x <= -MaxBoundsPerformance.x ||
                transform.position.y >= MaxBoundsPerformance.y || transform.position.y <= -MaxBoundsPerformance.y)
                {
                    if (TheParts.Count >= 5)
                    {
                        TheMan.PlayHitSound();
                        transform.position = TheParts[4].transform.position;
                        Destroy(TheParts[0]);
                    }
                    else
                    {
                        TheMan.Failed = true;
                        TheMan.IsGameOver = true;
                    }
                }
            }
            else
            {
                if (transform.position.x >= MaxBounds.x || transform.position.x <= -MaxBounds.x ||
                transform.position.y >= MaxBounds.y || transform.position.y <= -MaxBounds.y)
                {
                    if (TheParts.Count >= 5)
                    {
                        TheMan.PlayHitSound();
                        transform.position = TheParts[4].transform.position;
                        Destroy(TheParts[0]);
                    }
                    else
                    {
                        TheMan.Failed = true;
                        TheMan.IsGameOver = true;
                    }
                }
            }
           
        }
       
    }
    public void SpawnParts(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            GameObject go = Instantiate(PartPref, transform.position, transform.rotation);
            TheParts.Add(go);
        }
    }
    void InputMan()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.forward * fTurnRate * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(-Vector3.forward * fTurnRate * Time.deltaTime);
    }
    void SnakeLogic()
    {
        if (ThePos.Length != TheParts.Count)
        {
            Vector3[] tempPos = new Vector3[ThePos.Length];
            for (int i = 0; i < ThePos.Length; i++)
            {
                tempPos[i] = ThePos[i];
            }
            ThePos = new Vector3[TheParts.Count];
            for (int i = 0; i < ThePos.Length; i++)
            {
                if (tempPos.Length > i)
                {
                    ThePos[i] = tempPos[i];
                }
            }
            HurtTimestamp = Time.time +1f;
        }
        if (timestamp < Time.time)
        {
            Vector3[] tempPos = new Vector3[ThePos.Length];

            for (int i = 0; i < ThePos.Length; i++)
            {
                tempPos[i] = ThePos[i];
            }
            for (int i = 1; i < ThePos.Length; i++)
            {
                ThePos[i] = tempPos[i - 1];
            }
            if (ThePos.Length > 0)
            {
                ThePos[0] = transform.position;
            }
            timestamp = Time.time + 0.1f;
        }
        for (int i = 0; i < TheParts.Count; i++)
        {
            if (TheParts[i] == null)
            {
                TheParts.RemoveAt(i);
                return;
            }
            if (HurtTimestamp > Time.time)
            {
                TheParts[i].transform.position = Vector3.Lerp(TheParts[i].transform.position, ThePos[i], 5 * Time.deltaTime);

            }
            else
            {
                TheParts[i].transform.position = Vector3.Lerp(TheParts[i].transform.position, ThePos[i], 10 * Time.deltaTime);

            }

            if (i == 0)
            {
                TheParts[i].transform.LookAt(transform.position);
                //TheParts[i].GetComponent<Part>().goLeader = gameObject;
            }
            else
            {
                TheParts[i].transform.LookAt(ThePos[i - 1]);
                // TheParts[i].GetComponent<Part>().goLeader = TheParts[i-1];
            }
        }
        transform.localPosition = transform.localPosition + transform.up * fSpeed * Time.deltaTime;
        if (TheParts.Count == 0)
        {
            TheMan.IsGameOver = true;
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
                if (Hit.transform.tag == "Multiplier")
                {
                    TheMan.PlayCollectSound();
                    SpawnParts(10);
                    Destroy(Hit.transform.gameObject);
                }
               /* }else if (Hit.transform.tag == "Wall")
                {
                    if (TheParts.Count >= 5)
                    {
                        TheMan.PlayHitSound();
                        transform.position = TheParts[4].transform.position;
                        Destroy(TheParts[0]);
                    }
                    else
                    {
                        TheMan.Failed = true;
                        TheMan.IsGameOver = true;
                       
                    }
                }*/
                

            }

        }
    }
}
