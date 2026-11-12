using System.Collections;
using UnityEngine;

public class TransitionPanel : MonoBehaviour
{

  [SerializeField] private int leftXCoord;
  [SerializeField] private int rightXCoord;
  [SerializeField] private float transitionDuration;
  [SerializeField] private GameObject panel;

  private Vector3 _initPos;
  private Vector3 _centrePos;
  private Vector3 _endPos;
  
  private void awake()
  {
    _initPos = panel.transform.position;
    _initPos.x = leftXCoord;
    _centrePos = Vector3.zero;
    _endPos = panel.transform.position;
    _endPos.x = rightXCoord;
  }
  
  private void ShowPanel()
  {
    panel.transform.position = new Vector3(leftXCoord, panel.transform.position.y,  panel.transform.position.z);
    panel.SetActive(true);
    StartCoroutine(PanelEnter());
  }

  private void HidePanel()
  {
    StartCoroutine(PanelExit());
    panel.SetActive(false);
  }
  
  private IEnumerator PanelEnter()
  {
    float elapsed = 0f;

    while (elapsed < transitionDuration)
    {
      elapsed += Time.deltaTime;
      float t = elapsed / transitionDuration;
      transform.position = Vector3.Lerp(_initPos, _centrePos, t);
      yield return null;
    }

    transform.position = _centrePos;
  }
  
  private IEnumerator PanelExit()
  {
    float elapsed = 0f;

    while (elapsed < transitionDuration)
    {
      elapsed += Time.deltaTime;
      float t = elapsed / transitionDuration;
      panel.transform.position = Vector3.Lerp(_centrePos, _endPos, t);
      yield return null;
    }

    panel.transform.position = _endPos;
    
    panel.SetActive(false);
  }
}