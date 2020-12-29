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
  public void SetTower(TowerObject newTower)
  {
    tower = newTower;
    GetComponent<SpriteRenderer>().sprite = newTower.levels[0].sprite;
  }
  public void UnsetTower()
  {
    tower = null;
    GetComponent<SpriteRenderer>().sprite = null;
  }

  private void Update()
  {
    Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(new Vector2(mouseRay.origin.x, mouseRay.origin.y), Camera.main.transform.forward, 50f, placeableLayer);

    if (hit.transform != null)
    {
      transform.position = hit.transform.position;
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
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.layer == placeableLayer)
    {

    }
  }
}
