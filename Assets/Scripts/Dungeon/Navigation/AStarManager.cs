using System.Collections.Generic;
using UnityEngine;

public class AStarManager : MonoBehaviour

{
  public static AStarManager instance;

  void Awake()
  {
    instance = this;
  }

}