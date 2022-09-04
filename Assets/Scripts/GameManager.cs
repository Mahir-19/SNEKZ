using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public Snake TheP;
    public Text GameStartText;
    public Text WaveText;
    bool GameStarted;
    float Countdown = 3;
    public bool IsGameOver;
    public GameObject GameOverHud;
    public GameObject InGameHUD;
    public GameObject WonHUD;
    public GameObject LostHUD;
    public int WaveCount;
    public AudioSource HitSound;
    public AudioSource CollectSound;
    public AudioSource WaveSound;
    public bool Failed;
    public bool IsPerformanceTest;
    public Toggle PerformanceToggle;
    public GameObject PerformanceWalls;
    public GameObject ClassicWalls;
    public Vector2 MaxSpawnerBound;
    public Text EnemyCount;
    public int TheEnemyCount;
    public GameObject MapHud;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("PerformanceMode") == 1)
        {
            PerformanceToggle.isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPerformanceTest)
        {
            MaxSpawnerBound.x = 45;
            MaxSpawnerBound.y = 45;
            PlayerPrefs.SetInt("PerformanceMode", 1);

        }
        else
        {
            MaxSpawnerBound.x = 12;
            MaxSpawnerBound.y = 6;
            PlayerPrefs.SetInt("PerformanceMode", 0);

        }
        MapHud.SetActive(IsPerformanceTest);
        ClassicWalls.SetActive(!IsPerformanceTest);
        PerformanceWalls.SetActive(IsPerformanceTest);
        IsPerformanceTest = PerformanceToggle.isOn;
        if (InGameHUD.activeSelf)
        {
            EnemyCount.text = TheEnemyCount.ToString() + " Enemies";
            WaveText.text = "WAVE " + WaveCount.ToString() + "/7";
            if (GameStarted == false)
            {
                Countdown -= Time.deltaTime;
                GameStartText.text = Mathf.RoundToInt(Countdown).ToString();
                if (Countdown <= 1)
                {
                    GameStarted = true;
                    GameStartText.gameObject.SetActive(false);
                }
            }
        }
        GameOverHud.SetActive(IsGameOver);
        if (IsGameOver)
        {
            InGameHUD.SetActive(false);
            if (TheP.TheParts.Count > 0&&Failed==false)
            {
                WonHUD.SetActive(true);
                LostHUD.SetActive(false);
            }
            else
            {
                WonHUD.SetActive(false);
                LostHUD.SetActive(true);
            }
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void PlayHitSound()
    {
        if (HitSound.isPlaying)
        {
            HitSound.Stop();
        }
        HitSound.Play();
    }
    public void PlayCollectSound()
    {
        CollectSound.Play();
    }
    public void PlayWaveSound()
    {
       WaveSound.Play();
    }
}
