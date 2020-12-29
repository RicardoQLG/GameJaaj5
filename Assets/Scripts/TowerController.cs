using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TowerController : MonoBehaviour
{
  public TowerObject tower;
  public List<Transform> targets;
  public int currentLevel = 0;
  public bool shooting = false;
  public GameObject place;
  int segments = 32;
  float angle;

  private void OnDrawGizmosSelected()
  {
    Handles.DrawWireDisc(transform.position, Vector3.forward, tower.levels[currentLevel].range);
  }

  private void Start()
  {
    angle = 360 / segments;
    UpdateTower();
    OnDeselect();
    float range = tower.levels[currentLevel].range;

    LineRenderer line = GetComponent<LineRenderer>();
    line.positionCount = segments + 1;
    line.useWorldSpace = false;

    for (int segment = 0; segment < line.positionCount; segment++)
    {
      float x = Mathf.Sin(Mathf.Deg2Rad * angle * segment) * range;
      float y = Mathf.Cos(Mathf.Deg2Rad * angle * segment) * range * 0.5f;

      line.SetPosition(segment, new Vector3(x, y, 0));
    }
  }

  public void Upgrade()
  {
    currentLevel++;
    UpdateTower();
  }

  private void UpdateTower()
  {
    GetComponent<SpriteRenderer>().sprite = tower.levels[currentLevel].sprite;
    // GetComponent<CircleCollider2D>().radius = tower.levels[currentLevel].range;
    float range = tower.levels[currentLevel].range;

    PolygonCollider2D polygon = GetComponent<PolygonCollider2D>();

    Vector2[] points = new Vector2[segments];

    for (int segment = 0; segment < segments; segment++)
    {
      float x = Mathf.Sin(Mathf.Deg2Rad * angle * segment) * range;
      float y = Mathf.Cos(Mathf.Deg2Rad * angle * segment) * range * 0.5f;

      points[segment] = new Vector2(x, y);
    }

    polygon.SetPath(0, points);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.layer == 7)
    {
      targets.Add(other.transform);
      other.transform.GetComponent<EnemyController>().OnDie.AddListener(RemoveTarget);
      if (!shooting) StartCoroutine(Shoot());
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (targets.Contains(other.transform)) RemoveTarget(other.transform);
  }

  private void RemoveTarget(Transform target)
  {
    targets.Remove(target);
    target.GetComponent<EnemyController>().OnDie.RemoveListener(RemoveTarget);
  }

  private IEnumerator Shoot()
  {
    shooting = true;
    while (true)
    {
      if (targets.Count > 0)
      {
        targets[0].GetComponent<EnemyController>().TakeDamage(tower.levels[currentLevel].damage);
      }
      yield return new WaitForSeconds(tower.levels[currentLevel].speed);
      if (targets.Count == 0)
      {
        StopCoroutine(Shoot());
        shooting = false;
      }
    }
  }

  public void OnSelect()
  {
    GetComponent<LineRenderer>().enabled = true;
  }

  public void OnDeselect()
  {
    GetComponent<LineRenderer>().enabled = false;
  }
}
