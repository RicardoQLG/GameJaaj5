﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
  public static MusicManager instance;
  private void Awake()
  {
    if (instance == null) instance = this;
    if (instance != this) Destroy(gameObject);

    DontDestroyOnLoad(gameObject);
  }
}
