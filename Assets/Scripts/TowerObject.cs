using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "TowerDefense/Tower", order = 0)]
public class TowerObject : ScriptableObject
{
  public List<Tower> levels = new List<Tower>();
}

[Serializable]
public class Tower
{
  public Sprite sprite;
  public float speed;
  public float damage;
  public float range;

}