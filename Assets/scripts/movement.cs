using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class movement : MonoBehaviour {

    public int HP = 100;

    AudioSource audio;
    public AudioClip missileSound, dead;
    bool alive=true;
    public float moveSpeed, fireSpeedA, fireSpeedB;
    public GameObject missiles, bullet, epxlosion, comboUI;
    int missileCount = 25;
    bool allowMissile = true, allowShoot = true;
    float jetpackJuice = 250;

    public float hotness;
    public Slider jetpackBar;
    public Text missileCountText;

    public int combo = 3;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        comboUI.GetComponentsInChildren<Image>()[1].fillAmount -= 0.005f;
        if (comboUI.GetComponentsInChildren<Image>()[1].fillAmount == 0 && combo > 1)
        {
            combo--;
            comboUI.GetComponentsInChildren<Image>()[1].fillAmount = 1;
            comboUI.GetComponentInChildren<Text>().text = "x"+combo;
        }

        if (HP > 0)
            inputThings();

        if (transform.rotation.eulerAngles.z != 0)
            transform.rotation = Quaternion.Euler(Vector3.zero);

        if (hotness > 0)
            hotness--;
        else
            allowShoot = true;

        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, hotness / 150);

        if (HP < 1 && alive)
        {
            alive = false;
            //we're dead :(
            GameObject explo = Instantiate(epxlosion) as GameObject;
            explo.transform.position = transform.position;
            explo.GetComponent<exploder>().sound = dead;
        }
    }

    void missileCD() { allowMissile = true; }

    void shootCD() { allowShoot = true; }
    
    void inputThings()
    {
        if (Input.GetMouseButton(0))
        {
            if (allowShoot)
            {
                Vector3 pos = Input.mousePosition;
                pos.z = transform.position.z - Camera.main.transform.position.z;
                pos = Camera.main.ScreenToWorldPoint(pos);
                Vector3 shootDir = pos - transform.position;
                Quaternion q = Quaternion.FromToRotation(Vector3.up, pos - transform.position);
                var dir = Quaternion.AngleAxis(q.eulerAngles.z, Vector3.forward) * Vector3.up * 2;

                GameObject go = Instantiate(bullet, transform.position + dir, q) as GameObject;
                //GameObject go = Instantiate(bullet, transform.position + (pos * 0.1f), q) as GameObject;

                go.GetComponent<Rigidbody2D>().AddForce(go.transform.up * 400);

                allowShoot = false;

                hotness += 10;
                if (hotness < 150)
                {
                    Invoke("shootCD", fireSpeedA);
                }

                //Shells
                GetComponentsInChildren<ParticleSystem>()[0].Emit(1);
            }
        }

        if (Input.GetKey(KeyCode.D))
            GetComponent<Rigidbody2D>().AddForce(new Vector2(1 * moveSpeed, 0), ForceMode2D.Impulse);


        if (Input.GetKey(KeyCode.A))
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-1 * moveSpeed, 0), ForceMode2D.Impulse);


        if (Input.GetKey(KeyCode.W))
        {
            if (jetpackJuice > 0)
            {
                jetpackBar.value = jetpackJuice;
                jetpackBar.GetComponentsInChildren<Image>()[1].color = Color.Lerp(Color.red, Color.green, jetpackJuice/250);
                jetpackJuice--;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1 * moveSpeed), ForceMode2D.Impulse);

                //Fire
                GetComponentsInChildren<ParticleSystem>()[1].Emit(40);
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (allowMissile)
            {
                    allowMissile = false;
                    Invoke("missileCD", fireSpeedB);
                if (missileCount > 0)
                {
                    missileCount--;
                    missileCountText.text = "x" + missileCount;
                    audio.clip = missileSound;
                    audio.Play();
                    GameObject rocket = Instantiate(missiles) as GameObject;
                    rocket.transform.position = transform.position + new Vector3(0, 3f, 0);

                    if (GetComponentInChildren<targetFinder>().targets.Count > 0)
                    {
                        rocket.GetComponent<rocket>().target = GetComponentInChildren<targetFinder>().targets[0].transform;
                    }
                    rocket.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }
    }
}