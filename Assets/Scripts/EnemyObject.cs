using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "TowerDefense/Enemy", order = 1)]
public class EnemyObject : ScriptableObject
{
  public Sprite sprite;
  public float health = 100f;
  public float speed = .8f;
  public float value = 0f;
}
