using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UpdateController : MonoBehaviour
{
  public TowerController tower;
  public Image image;
  public TextMeshProUGUI towerName;
  public TextMeshProUGUI value;
  public Button updagradeButton;

  private void Awake()
  {
    gameObject.SetActive(false);
  }

  public void SetTower(TowerController tower)
  {
    if (tower == null)
    {
      gameObject.SetActive(false);
      return;
    }

    gameObject.SetActive(true);

    this.tower = tower;
  }

  private void Update()
  {
    if (tower != null) UpdateTowerData();
  }

  private void UpdateTowerData()
  {
    if (this.tower.currentLevel == tower.tower.levels.Count - 1) updagradeButton.enabled = false;
    else updagradeButton.enabled = true;

    int nextLevel = Mathf.Min((this.tower.currentLevel + 1), this.tower.tower.levels.Count - 1);
    float upgradeValue = tower.tower.levels[nextLevel].upgradeValue;
    if (!GameManager.instance.HasFunds(upgradeValue)) updagradeButton.enabled = false;

    towerName.text = tower.tower.levels[nextLevel].name;
    value.text = $"R$ {tower.tower.levels[nextLevel].upgradeValue}";
    image.sprite = tower.tower.levels[nextLevel].sprite;
  }

  public void UpgradeCurrentTower()
  {
    int nextLevel = Mathf.Min((this.tower.currentLevel + 1), this.tower.tower.levels.Count - 1);
    float upgradeValue = tower.tower.levels[nextLevel].upgradeValue;
    GameManager.instance.RemoveFunds(upgradeValue);
    tower.Upgrade();
  }
}
