using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyShipPrefab;

    [SerializeField]
    private GameObject[] powerups;

    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }

    public void StartSpawnRoutines()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }

    // create a coroutine to spawn the enemy every 5 seconds
    IEnumerator EnemySpawnRoutine()
    {
        while(_gameManager.gameOver == false)
        {
            Instantiate(enemyShipPrefab, new Vector3(Random.Range(-7.8F, 7.8F), 6.37F, 0), Quaternion.identity);
            yield return new WaitForSeconds(5.0F);
        }
    }

    IEnumerator PowerUpSpawnRoutine()
    {
        while(_gameManager.gameOver == false)
        {
            int randomPowerup = Random.Range(0, 3);
            Instantiate(powerups[randomPowerup], new Vector3(Random.Range(-7.8F, 7.8F), 6.37F, 0), Quaternion.identity);
            yield return new WaitForSeconds(5.0F);
        }
    }



}
