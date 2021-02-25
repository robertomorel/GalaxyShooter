using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManage : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyShip;

    [SerializeField]
    private GameObject[] _powerUps;

    private GameManager _gameManager;

    // Use this for initialization
    void Start()
    {

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(CreateEnemyRoutine());
        StartCoroutine(PowerUpSpawnRoutine());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StarSpawnRoutines()
    {
        StartCoroutine(CreateEnemyRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }

    IEnumerator CreateEnemyRoutine()
    {
        while (!_gameManager.gameOver)
        {
            float randomX = Random.Range(-7.7f, 7.7f);
            Instantiate(_enemyShip, transform.position + new Vector3(randomX, 6.6f, 0), Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator PowerUpSpawnRoutine()
    {
        while (!_gameManager.gameOver)
        {
            int randomPowerUp = Random.Range(0, 3);
            float randomX = Random.Range(-7.7f, 7.7f);
            Instantiate(_powerUps[randomPowerUp], new Vector3(randomX, 7, 0), Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }
}
