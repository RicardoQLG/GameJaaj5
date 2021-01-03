using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
  public static WaveManager instance { get; private set; }
  public float interval;
  public Transform startPosition;
  public GameObject enemyPrefab;
  public List<FloorController> floor;
  public List<EnemyController> enemies;
  public List<WaveObject> waves;
  public int currentWaveIndex = 0;
  public UnityEvent OnWaveEnd = new UnityEvent();
  public UnityEvent OnLastWaveEnd = new UnityEvent();
  public bool isProducing = false;
  public bool waitingNextWave = true;

  private void Awake()
  {
    if (instance == null) instance = this;
    if (instance != this) DestroyImmediate(gameObject);
  }

  [ContextMenu("Start Wave")]
  public void StartWave()
  {
    waitingNextWave = false;
    StartCoroutine(ProduceWave());
  }

  private IEnumerator ProduceWave()
  {
    int enemyIndex = 0;
    isProducing = true;
    do
    {
      GameObject enemy = Instantiate(enemyPrefab, startPosition.position, Quaternion.identity);
      string name = NameManager.instance.GetRandomUniqueName();
      enemy.GetComponent<EnemyController>().SetName(name);
      enemy.GetComponent<EnemyController>().enemyData = waves[currentWaveIndex].enemies[enemyIndex];
      yield return new WaitForSeconds(interval);
    } while (++enemyIndex < waves[currentWaveIndex].enemies.Count);
    isProducing = false;
  }

  private void Update()
  {
    if (enemies.Count == 0 && isProducing == false && waitingNextWave == false)
    {
      OnWaveEnd.Invoke();
      waitingNextWave = true;
      currentWaveIndex++;
    }

    if (currentWaveIndex >= waves.Count)
    {
      OnLastWaveEnd.Invoke();
      GameManager.instance.WinGame();
    }
  }
}
