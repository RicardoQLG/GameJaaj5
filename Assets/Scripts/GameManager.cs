using System;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  [SerializeField] private float currentFunds = 200f;
  public TextMeshProUGUI funds;
  public GameObject carrying;
  public TowerController selectedTower;
  public LayerMask towerLayer;

  private void Awake()
  {
    if (instance == null) instance = this;
    if (instance != this) DestroyImmediate(gameObject);

    UpdateFundCanvas();
  }

  private void Update()
  {
    Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(new Vector2(mouseRay.origin.x, mouseRay.origin.y), Camera.main.transform.forward, 50f, towerLayer);
    RaycastHit2D[] hitAll = Physics2D.RaycastAll(new Vector2(mouseRay.origin.x, mouseRay.origin.y), Camera.main.transform.forward, 50f, towerLayer);

    if (Input.GetButtonDown("Fire1"))
    {
      if (selectedTower != null)
      {
        selectedTower.OnDeselect();
        selectedTower = null;
      }

      foreach (var singleHit in hitAll)
      {
        if (singleHit.transform != null && singleHit.collider.GetType() != typeof(PolygonCollider2D))
        {
          selectedTower = singleHit.transform.GetComponent<TowerController>();
          selectedTower.OnSelect();
          break;
        }
      }
    }
  }

  private bool HasFunds(float amout)
  {
    return amout <= currentFunds;
  }

  private void UpdateFundCanvas()
  {
    funds.text = $"Fundos R$ {currentFunds.ToString("F2")}";
  }

  public void ChoseTower(TowerObject tower)
  {
    if (tower.buyValue > currentFunds) return;
    carrying.GetComponent<PlaceableController>().SetTower(tower);
  }

  public void AddFunds(float amount)
  {
    currentFunds += amount;
    UpdateFundCanvas();
  }

  public void RemoveFunds(float amount)
  {
    currentFunds -= amount;
    UpdateFundCanvas();
  }
}
