using System;
using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
  public float stopDistance = 0.5f;
  private float speed = 0f;
  public float acceleration = 0.1f;
  public float damage = 100f;
  public TowerController tower;
  public Transform target;
  Vector3 targetOffset = Vector3.up * 0.5f;
  bool hitted = false;

  public void SetTower(TowerController tower)
  {
    this.tower = tower;
    if (tower.targets.Count == 0)
    {
      StartCoroutine(Explode());
      return;
    }
    target = tower.targets[0];
    target.GetComponent<EnemyController>().OnDie.AddListener(UpdateTarget);
  }

  private void UpdateTarget(Transform oldTarget)
  {
    oldTarget.GetComponent<EnemyController>().OnDie.RemoveListener(UpdateTarget);

    if (tower.targets.Count > 0)
    {
      target = tower.targets[0];
      Debug.Break();
      return;
    }

    StartCoroutine(Explode());
  }

  private void FixedUpdate()
  {
    if (target == null) return;
    if (DistanceFromTarget() > stopDistance)
    {
      speed += acceleration;
      Vector3 nextPosition = Direction() * speed * Time.deltaTime;
      transform.position += nextPosition;
      return;
    }

    Hit();
  }

  private IEnumerator Explode()
  {
    GetComponent<SpriteRenderer>().sprite = null;
    GetComponent<ParticleSystem>().Play();
    hitted = true;
    yield return new WaitForSeconds(.25f);

    Destroy(gameObject);
  }

  private void Hit()
  {
    if (hitted) return;
    target.GetComponent<EnemyController>().TakeDamage(damage);
    StartCoroutine(Explode());
  }

  private Vector3 Direction()
  {
    return (target.position - transform.position + targetOffset).normalized;
  }

  private float DistanceFromTarget()
  {
    return Vector3.Distance(transform.position, target.position + targetOffset);
  }
}
