using UnityEngine;

public class StartGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Application.LoadLevel("main");
        }
    }
}
