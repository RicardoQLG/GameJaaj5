using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  [SerializeField] private float currentFunds = 200f;
  public TextMeshProUGUI funds;
  public GameObject carrying;
  public TowerController selectedTower;
  public LayerMask towerLayer;
  public float lifes = 10f;
  public GameObject winCanvas;
  public GameObject defeatCanvas;
  public List<Image> lifeDisplay;

  private void Awake()
  {
    if (instance == null) instance = this;
    if (instance != this) DestroyImmediate(gameObject);

    Application.runInBackground = true;
    UpdateFundCanvas();
  }

  private void Update()
  {
    Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(new Vector2(mouseRay.origin.x, mouseRay.origin.y), Camera.main.transform.forward, 50f, towerLayer);
    RaycastHit2D[] hitAll = Physics2D.RaycastAll(new Vector2(mouseRay.origin.x, mouseRay.origin.y), Camera.main.transform.forward, 50f, towerLayer);

    for (int i = 0; i < lifeDisplay.Count; i++)
    {
      if (i < lifes)
      {
        lifeDisplay[i].color = Color.white;
      }
      else
      {
        lifeDisplay[i].color = Color.black;
      }
    }

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

  public void ReloadScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  public void ReduceLife(float amount)
  {
    lifes -= amount;

    if (lifes <= 0)
    {
      defeatCanvas.SetActive(true);
    }
  }

  public void WinGame()
  {
    winCanvas.SetActive(true);
  }
}
