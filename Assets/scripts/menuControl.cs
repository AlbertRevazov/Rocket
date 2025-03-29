using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuControl : MonoBehaviour
{
  void Update()
  {
    if (Input.GetKey(KeyCode.Space))
    {
      SceneManager.LoadScene(1);
    }
  }
}