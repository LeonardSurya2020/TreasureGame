using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomSpawner : MonoBehaviour
{
    public GameObject[] randomObject;
    public float radius = 1f;

    public float firstSecondSpawn = 5f;
    public float secondSpawn = 5f;
    public float minTrans;
    public float maxTrans;



    public int enemyTypeAmount = 1;

    private enemyPatrol enemyPatrol;

    private void Start()
    {
        StartCoroutine(RandomSpawn());
    }

    public IEnumerator RandomSpawn()
    {
        while (true)
        {

            Debug.Log("type" + enemyTypeAmount);
            yield return new WaitForSeconds(firstSecondSpawn);
            int randomNumber = Random.Range(0, randomObject.Length);
            var wanted = Random.Range(minTrans, maxTrans);
            var position = new Vector3(wanted, transform.position.y);
            GameObject gameObj = Instantiate(randomObject[randomNumber], position, Quaternion.identity);
            yield return new WaitForSeconds(secondSpawn);
            
        }
    }

    public void IncreaseCoinSpawnCooldown()
    {
        firstSecondSpawn += 1;
        secondSpawn += 1;
    }
}
