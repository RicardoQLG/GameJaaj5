using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class EnemyController : MonoBehaviour
{
  public List<GameObject> waypoints;
  private int currentWaypointIndex = 0;
  public EnemyObject enemyData;
  public UnityEvent<Transform> OnDie = new UnityEvent<Transform>();
  public TextMeshProUGUI nameField;
  public Slider healthBar;

  [SerializeField] private float currentSpeed;
  [SerializeField] public float currentHealth;

  private void Awake()
  {
    waypoints = WaveManager.instance.waypoints;
  }

  private void OnEnable() => WaveManager.instance.enemies.Add(this);
  private void OnDisable() => WaveManager.instance.enemies.Remove(this);

  private void Start()
  {
    currentWaypointIndex = 0;
    GetComponent<SpriteRenderer>().sprite = enemyData.sprite;
    currentSpeed = enemyData.speed;
    currentHealth = enemyData.health;
    transform.position = waypoints[currentWaypointIndex].transform.position;
  }

  public void SetName(string name)
  {
    nameField.text = name;
  }

  private void FixedUpdate()
  {
    if (currentWaypointIndex == waypoints.Count) return;

    Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].transform.position;

    float distance = Vector3.Distance(transform.position, currentWaypointPosition);

    if (distance <= .05f)
    {
      transform.position = waypoints[currentWaypointIndex].transform.position;
      currentWaypointIndex++;
      return;
    }

    Vector3 direction = currentWaypointPosition - transform.position;
    Vector3 directionNormalized = direction.normalized;

    transform.position += directionNormalized * Time.deltaTime * currentSpeed;
  }

  public void TakeDamage(float amount)
  {
    currentHealth -= amount;

    healthBar.value = currentHealth / enemyData.health;

    if (currentHealth <= 0f)
    {
      OnDie.Invoke(transform);
      DestroyImmediate(gameObject);
      return;
    }
  }
}
