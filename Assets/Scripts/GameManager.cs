using System;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  [SerializeField] private float currentFunds = 200f;
  public TextMeshProUGUI funds;
  public GameObject carrying;

  private void Awake()
  {
    if (instance == null) instance = this;
    if (instance != this) DestroyImmediate(gameObject);

    UpdateFundCanvas();
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
