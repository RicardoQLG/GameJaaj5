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
  public List<EnemyObject> variants;
  public List<string> deployables;
  public List<EnemyController> enemies;
  public List<WaveObject> waves;
  public int currentWaveIndex = 0;
  public UnityEvent OnWaveEnd = new UnityEvent();
  public UnityEvent OnLastWaveEnd = new UnityEvent();

  private void Awake()
  {
    if (instance == null) instance = this;
    if (instance != this) DestroyImmediate(gameObject);
  }

  [ContextMenu("Start Wave")]
  public void StartWave()
  {
    StartCoroutine(ProduceWave(deployables));
  }

  private IEnumerator ProduceWave(List<string> deployables)
  {
    int enemyIndex = 0;
    do
    {
      GameObject enemy = Instantiate(enemyPrefab, startPosition.position, Quaternion.identity);
      string name = NameManager.instance.GetRandomUniqueName();
      enemy.GetComponent<EnemyController>().SetName(name);
      enemy.GetComponent<EnemyController>().enemyData = waves[currentWaveIndex].enemies[enemyIndex];
      yield return new WaitForSeconds(interval);
    } while (++enemyIndex < waves[currentWaveIndex].enemies.Count);
  }

  private void Update()
  {
    if (enemies.Count == 0 && deployables.Count == 0)
    {
      OnWaveEnd.Invoke();
    }

    if (currentWaveIndex >= waves.Count)
    {
      OnLastWaveEnd.Invoke();
    }
  }

  public void Log()
  {
    Debug.Log("Log");
  }
}
