using UnityEngine;

public class CameraController : MonoBehaviour
{
  public static CameraController instance;
  public int currentFloor = 0;
  public float transitionSpeed;
  public int floorCount = -1;
  public bool animating
  {
    get { return Mathf.Abs(cameraDestionation - transform.position.y) > 0.1f; }
    private set { }
  }
  public float cameraDestionation
  {
    get { return currentFloor * 13; }
    private set { }
  }

  private void Awake()
  {
    if (instance == null) instance = this;
    if (instance != this) Destroy(this);

    floorCount = WaveManager.instance.floor.Count - 1;
  }

  public void Up()
  {
    if (currentFloor == 0) return;
    if (animating) return;
    currentFloor++;
  }

  public void Down()
  {
    if (currentFloor == -floorCount) return;
    if (animating) return;
    currentFloor--;
  }

  private void Update()
  {
    Vector3 finalPosition = transform.position;
    finalPosition.y = cameraDestionation;
    transform.position = Vector3.Lerp(transform.position, finalPosition, Time.deltaTime * transitionSpeed);
  }
}
