using UnityEngine;

namespace DefaultNamespace
{
  public class LifeRestorationConsumable : MonoBehaviour
  {
    [SerializeField] private int healthRestoration;

    public int HealthRestoration()
    {
      return  healthRestoration;
    } 
  }
}