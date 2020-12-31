using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "TowerDefense/Tower", order = 0)]
public class TowerObject : ScriptableObject
{
  public float buyValue = 0f;
  public List<Tower> levels = new List<Tower>();
}

[Serializable]
public class Tower
{
  [Header("Text")]
  public string name;
  [TextArea(10, 10)]
  public string description;
  [Header("Images")]
  public Sprite sprite;
  public GameObject projectile;
  [Header("Attributes")]
  public float speed;
  public float damage;
  public float range;
}