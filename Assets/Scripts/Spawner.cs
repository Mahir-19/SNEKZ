using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform TheP;
    public GameObject EnemyPref;
    public List<GameObject> TheEnemies = new List<GameObject>();
    public float SpawnRate = 1;
    float SpawnTimestamp;
    public float WaveWaitTimestamp;
    public GameObject WaveIndicator;
    public float MaxEnemies = 5;
    public bool WaveStarted;
    float IndicatorTimestamp;
    public int WaveCount = 1;
    public GameManager TheMan;
    public bool TestingEnemyType;
    public int EnemyType;
    public Transform[] SpawnPos;
    public Transform[] SpawnPosExtra;
    public GameObject MultiplierPref;
    public GameObject[] BulletPool;
    public GameObject BulletPref;
    // Start is called before the first frame update
    void Start()
    {
        TheMan = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        TheP = GameObject.FindGameObjectWithTag("Player").transform;
        WaveWaitTimestamp = Time.time + 5;
        BulletPool = new GameObject[500];
        for(int i = 0; i < 500; i++)
        {
            GameObject go = Instantiate(BulletPref, transform);
            go.SetActive(false);
            BulletPool[i] = go;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!TheMan.InGameHUD.activeSelf||TheMan.IsGameOver)
        {
            WaveWaitTimestamp = Time.time + 5;
            return;
        }
        if (WaveWaitTimestamp < Time.time)
        {
            SpawnMan();
            WaveIndicator.SetActive(false);
            EnemyMan();
        }
        else
        {
            WaveStarted = false;
            if (IndicatorTimestamp < Time.time)
            {
                if (WaveIndicator.activeSelf)
                {
                    WaveIndicator.SetActive(false);

                }
                else
                {
                    WaveIndicator.SetActive(true);

                }
                IndicatorTimestamp = Time.time + 0.2f;

            }

        }
        TheMan.TheEnemyCount = TheEnemies.Count;
    }
    public GameObject GetBullet()
    {
        for(int i = 0; i < BulletPool.Length; i++)
        {
            if (!BulletPool[i].activeSelf)
            {
                BulletPool[i].GetComponent<Rigidbody>().isKinematic = true;
                return BulletPool[i];
            }
        }
        return null;
    }
    void EnemyMan()
    {
        for(int i = 0; i < TheEnemies.Count; i++)
        {
            if (TheEnemies[i] == null)
            {
                TheEnemies.RemoveAt(i);
            }
            else
            {
                TheEnemies[i].GetComponent<Enemy>().ManualUpdate();
            }
        }
        if (TheEnemies.Count >= MaxEnemies)
        {
            WaveStarted = true;
        }
        else if (TheEnemies.Count == 0)
        {
            if (TheMan.IsPerformanceTest)
            {
                TheMan.TheP.SpawnParts(200);
            }
            TheMan.WaveCount += 1;
            if (!TestingEnemyType)
            {
                if (TheMan.WaveCount < 4)
                {
                    EnemyType += 1;
                }
                else
                {
                    EnemyType = Random.Range(0, 5);
                }
            }
            GameObject Go = Instantiate(MultiplierPref, new Vector3(Random.Range(-TheMan.MaxSpawnerBound.x, TheMan.MaxSpawnerBound.x), Random.Range(-TheMan.MaxSpawnerBound.y, TheMan.MaxSpawnerBound.y), 0), Quaternion.identity);
            Destroy(Go, 5f);
            transform.position = new Vector3(Random.Range(-TheMan.MaxSpawnerBound.x, TheMan.MaxSpawnerBound.x), Random.Range(-TheMan.MaxSpawnerBound.y, TheMan.MaxSpawnerBound.y), 0);
            WaveWaitTimestamp = Time.time + 3;
            WaveCount += 1;
            if (TheMan.WaveCount >= 8)
            {
                TheMan.IsGameOver = true;
            }
            else
            {
                TheMan.PlayWaveSound();

            }
        }
    }
    void SpawnMan()
    {

        if (TheEnemies.Count<MaxEnemies&&!WaveStarted)
        {
            for(int i = 0; i < SpawnPos.Length; i++)
            {
                GameObject Go = Instantiate(EnemyPref, transform);
                Go.transform.position = SpawnPos[i].position;
                TheEnemies.Add(Go);
                Go.GetComponent<Enemy>().Target = TheP;
                Go.GetComponent<Enemy>().TheType = EnemyType;
            }
            if (TheMan.IsPerformanceTest)
            {
                int Count = 0;
                int MaxCount = 100;
                if (EnemyType == 1)
                {
                    MaxCount = 20;
                }
                while (Count < MaxCount)
                {
                    Vector3 RandPos =transform.position + Random.insideUnitSphere*5;
                    RandPos.z = 0;
                    GameObject Go = Instantiate(EnemyPref, transform);
                    Go.transform.position = RandPos;
                    TheEnemies.Add(Go);
                    Go.GetComponent<Enemy>().Target = TheP;
                    Go.GetComponent<Enemy>().TheType = EnemyType;
                    Count += 1;
                }
               
            }
           
            //Destroy(Go, 5f);
        }
    }
    public void SpawnSubEnemies(Transform[] SpawnPoints)
    {
        for(int i = 0; i < SpawnPoints.Length; i++)
        {
            GameObject Go = Instantiate(EnemyPref, transform);
            Go.transform.position = SpawnPoints[i].position;
            TheEnemies.Add(Go);
            Go.GetComponent<Enemy>().Target = TheP;
            Go.GetComponent<Enemy>().TheType = 0;
            Go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
    
}
