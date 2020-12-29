using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableController : MonoBehaviour
{
  TowerObject tower;
  public Vector3 offset;
  public GameObject towerPrefab;
  public LayerMask placeableLayer;
  public LineRenderer rangeDisplay;

  private void Awake()
  {
    rangeDisplay = GetComponent<LineRenderer>();
  }

  private void SetupRange(float range)
  {
    rangeDisplay.positionCount = 32;
    float angle = 11.25f;
    rangeDisplay.useWorldSpace = false;

    for (int segment = 0; segment < rangeDisplay.positionCount; segment++)
    {
      float x = Mathf.Sin(Mathf.Deg2Rad * angle * segment) * range;
      float y = Mathf.Cos(Mathf.Deg2Rad * angle * segment) * range * 0.5f;

      rangeDisplay.SetPosition(segment, new Vector3(x, y, 0));
    }
  }

  public void SetTower(TowerObject newTower)
  {
    tower = newTower;
    GetComponent<SpriteRenderer>().sprite = newTower.levels[0].sprite;
    SetupRange(tower.levels[0].range);
  }

  public void UnsetTower()
  {
    tower = null;
    GetComponent<SpriteRenderer>().sprite = null;
    SetupRange(0);
    rangeDisplay.enabled = false;
  }

  private void Update()
  {
    Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(new Vector2(mouseRay.origin.x, mouseRay.origin.y), Camera.main.transform.forward, 50f, placeableLayer);

    if (hit.transform != null)
    {
      transform.position = hit.transform.position;
      rangeDisplay.enabled = true;
    }

    if (Input.GetButtonDown("Fire1"))
    {
      if (hit.transform != null) Place(hit.transform, tower);

      UnsetTower();
    }


    if (hit.transform == null)
    {
      Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousePosition.z = 20f;
      transform.position = mousePosition + offset;
      rangeDisplay.enabled = false;
    }
  }

  private void Place(Transform targetPlace, TowerObject tower)
  {
    if (tower == null) return;
    GameObject newTower = Instantiate(towerPrefab, targetPlace.position, Quaternion.identity);
    newTower.GetComponent<TowerController>().tower = tower;
    newTower.GetComponent<TowerController>().place = targetPlace.gameObject;
    GameManager.instance.RemoveFunds(tower.buyValue);
    targetPlace.gameObject.SetActive(false);
    rangeDisplay.enabled = false;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.layer == placeableLayer)
    {

    }
  }
}
