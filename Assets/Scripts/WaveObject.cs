using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "TowerDefense/Wave", order = 3)]
public class WaveObject : ScriptableObject
{
  public List<EnemyObject> enemies;
}
