using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    
    private int totalScorePoint = 0;
    private int minScore = 1500;
    public TextMeshProUGUI text;

    public GameObject leftSpawner;
    public GameObject rightSpawner;
    public GameObject leftCoinSpawner;
    public GameObject rightCoinSpawner;
    private EnemyRandomSpawner randomSpawnerLeft;
    private EnemyRandomSpawner randomSpawnerRight;

    private RandomSpawner coinRandomSpawnerLeft;
    private RandomSpawner coinRandomSpawnerRight;

    private Timer timer;
    public GameObject timerText;

    public void AddScore(int scorePoint)
    {
        this.totalScorePoint += scorePoint;

        if(totalScorePoint > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", totalScorePoint);
        }
       

        timer = timerText.GetComponent<Timer>();

        if (timer != null )
        {
            timer.remainingTime += 3;
        }


        if (totalScorePoint >= minScore)
        {
            randomSpawnerLeft = leftSpawner.GetComponent<EnemyRandomSpawner>();
            randomSpawnerRight = rightSpawner.gameObject.GetComponent<EnemyRandomSpawner>();
            randomSpawnerLeft.AddEnemy();
            randomSpawnerRight.AddEnemy();


            coinRandomSpawnerLeft = leftCoinSpawner.GetComponent<RandomSpawner>();
            coinRandomSpawnerRight = rightCoinSpawner.GetComponent<RandomSpawner>();
            coinRandomSpawnerLeft.IncreaseCoinSpawnCooldown();
            coinRandomSpawnerRight.IncreaseCoinSpawnCooldown();
            minScore += 1500;
            
        }
        text.text = totalScorePoint.ToString();


    }

    

}
