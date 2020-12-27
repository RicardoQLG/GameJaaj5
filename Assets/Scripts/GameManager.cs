using System;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  [SerializeField] private float currentFunds = 200f;
  public TextMeshProUGUI funds;

  private void Awake()
  {
    if (instance == null) instance = this;
    if (instance != this) DestroyImmediate(this);
    UpdateFundCanvas();
  }

  private bool HasFunds(float amout)
  {
    return amout <= currentFunds;
  }

  public void AddFunds(float amount)
  {
    currentFunds += amount;
    UpdateFundCanvas();
  }

  private void UpdateFundCanvas()
  {
    funds.text = $"Fundos R$ {currentFunds.ToString("F2")}";
  }
}
