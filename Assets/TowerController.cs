using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
  public TowerObject tower;
  public List<Transform> targets;
  public int currentLevel = 0;

  private void Start()
  {
    UpdateTower();
    StartCoroutine(Shoot());
  }

  public void Upgrade()
  {
    currentLevel++;
    UpdateTower();
  }

  private void UpdateTower()
  {
    GetComponent<SpriteRenderer>().sprite = tower.levels[currentLevel].sprite;
    GetComponent<CircleCollider2D>().radius = tower.levels[currentLevel].range;

  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.layer == 7)
    {
      targets.Add(other.transform);
      other.transform.GetComponent<EnemyController>().OnDie.AddListener(RemoveTarget);
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (targets.Contains(other.transform)) RemoveTarget(other.transform);
  }

  private void RemoveTarget(Transform target)
  {
    targets.Remove(target);
  }

  private IEnumerator Shoot()
  {
    while (true)
    {
      if (targets.Count > 0)
      {
        targets[0].GetComponent<EnemyController>().TakeDamage(tower.levels[currentLevel].damage);
      }
      yield return new WaitForSeconds(tower.levels[currentLevel].speed);
    }
  }
}
