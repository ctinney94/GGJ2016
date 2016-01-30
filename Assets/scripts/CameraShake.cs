using UnityEngine;
using System.Collections;
public class CameraShake : MonoBehaviour
{
    public GameObject CameraParent;//attached parent object of camera
    /// <summary>
    /// To be called by anything that wishes to shakeadat camera
    /// </summary>
    /// 
    public float shakeSpeed = 0.1f;
    public Vector3 shakeRange = new Vector3(-0.5f, 0.5f, 0);
    float shakeTimer = 0;
    const float shakeTime = 0.1f;
    Vector3 originalPos;
    public bool shake;

    void Update()
    {
        #region perlin noise

        if (shake)
        {
            if (shakeTimer > shakeTime * Time.timeScale)
            {
                shakeTimer = 0;
                shake = false;
                Camera.main.transform.localPosition = originalPos;
            }
            else
            {
                shakeTimer += Time.deltaTime;
                Camera.main.transform.localPosition = originalPos + Vector3.Scale(SmoothRandom.GetVector2(shakeSpeed--), shakeRange);

                shakeSpeed *= -1;
                shakeRange = new Vector3(shakeRange.x * -1, shakeRange.y);
            }
        }
        #endregion
    }

    public void shakeCamNew()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1,1)*10, Random.Range(-1,1))*10, ForceMode2D.Impulse);
    }

    public void ShakeCamera(float x, float y)
    {
        iTween.ShakePosition(CameraParent, iTween.Hash("x", x, "y", y, "isLocal", false, "time", 0.6f));
    }
}