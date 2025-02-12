using System;
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
  private int currentFloorIndex = 0;
  public EnemyObject enemyData;
  public UnityEvent<Transform> OnDie = new UnityEvent<Transform>();
  public TextMeshProUGUI nameField;
  public Slider healthBar;
  [SerializeField] private float currentSpeed;
  public float currentHealth;
  public AudioSource audioSource;
  public ParticleSystem dieEffect;
  public bool walking = true;
  [SerializeField] private Animator animator;

  private void Awake()
  {
    animator = GetComponent<Animator>();
    audioSource = GetComponent<AudioSource>();
    MoveToFloor(0);
  }

  private void OnEnable() => WaveManager.instance.enemies.Add(this);
  private void OnDisable() => WaveManager.instance.enemies.Remove(this);

  private void Start()
  {
    GetComponent<Animator>().runtimeAnimatorController = enemyData.animation;
    currentSpeed = enemyData.speed;
    currentHealth = enemyData.health;
  }

  private void MoveToFirstWaypoint()
  {
    currentWaypointIndex = 0;
    transform.position = waypoints[currentWaypointIndex].transform.position;
  }

  private void MoveToFloor(int floorIndex)
  {
    if (floorIndex >= WaveManager.instance.floor.Count) return;
    waypoints = WaveManager.instance.floor[floorIndex].waypoints;
    MoveToFirstWaypoint();
  }

  private void MoveToNextFloor()
  {
    currentFloorIndex++;

    if (currentFloorIndex > WaveManager.instance.floor.Count)
    {
      GameManager.instance.ReduceLife(1f);
      Destroy(gameObject);
    }

    MoveToFloor(currentFloorIndex);
  }

  public void SetName(string name)
  {
    nameField.text = name;
  }

  private void FixedUpdate()
  {
    if (!walking)
    {
      return;
    }
    if (currentWaypointIndex == waypoints.Count) MoveToNextFloor();
    if (currentWaypointIndex >= waypoints.Count) return;
    animator.SetInteger("Direction", GetDirection(waypoints[currentWaypointIndex].transform.position));

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

  private int GetDirection(Vector3 nextWaypoint)
  {
    Vector3 direction = (nextWaypoint - transform.position).normalized;
    if (direction.x > 0 && direction.y > 0) return 0;
    if (direction.x > 0 && direction.y < 0) return 1;
    if (direction.x < 0 && direction.y < 0) return 2;

    return 3;
  }

  public void TakeDamage(float amount)
  {
    currentHealth -= amount;

    healthBar.value = currentHealth / enemyData.health;

    if (currentHealth <= 0f)
    {
      OnDie.Invoke(transform);
      StartCoroutine(Die());
      GameManager.instance.AddFunds(enemyData.value);
      return;
    }
  }

  private IEnumerator Die()
  {
    walking = false;
    healthBar.gameObject.SetActive(false);
    nameField.gameObject.SetActive(false);
    animator.runtimeAnimatorController = null;
    audioSource.Stop();
    dieEffect.Play();
    yield return new WaitForSeconds(2f);
    Destroy(gameObject);
  }

  public void PlayStep()
  {
    audioSource.volume = .05f;
    audioSource.clip = enemyData.stepSound;
    audioSource.Play();
  }
}
