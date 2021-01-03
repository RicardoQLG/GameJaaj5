using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{
  public TowerObject tower;
  public Image image;
  public TextMeshProUGUI btName;
  public TextMeshProUGUI value;

  private void Start()
  {
    image.sprite = tower.levels[0].sprite;
    btName.text = tower.levels[0].name;
    value.text = $"R$ {tower.buyValue}";
  }
}
