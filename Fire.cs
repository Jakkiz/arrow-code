using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{

    public GameObject aimStick;
    public float bulletSpeed = 200f;
    public AudioClip shootArrow;
    float bulletMultiplier = 1f;
    float slowMoSpeedMultiplier = 5f; //value that multiply that speed in order to account for the slowMO speed of the physics engine
    // Use this for initialization
    void Start()
    {

    }

    public GameObject DoFire(float weaponCharge)
    {
        if (weaponCharge >= 1 && weaponCharge < 2)
        {
            bulletMultiplier = 1f;
        }
        if (weaponCharge >= 2 && weaponCharge < 3)
        {
            bulletMultiplier = 1.5f;
        }
        if (weaponCharge >= 3)
        {
            bulletMultiplier = 2.5f;
        }
        GameObject myArrow = Fire1();
        return myArrow;
    }


    GameObject Fire1()
    {
        float speed = bulletSpeed * bulletMultiplier;
        Vector3 hr = aimStick.transform.rotation.eulerAngles;
        hr = new Vector3(hr.x, hr.y, hr.z);
        GameObject arrowClone = (GameObject)Instantiate(Resources.Load("arrow"), aimStick.transform.position, Quaternion.Euler(hr));
        arrowClone.GetComponentInChildren<triggerScript>().ControllerNumber = GetComponentInParent<PlayerController>().ControllerNumber;
        arrowClone.GetComponentInChildren<SpriteRenderer>().color = GetComponentInParent<PlayerController>().color;
        arrowClone.GetComponentInChildren<TrailRenderer>().material.SetColor("_Color", GetComponentInParent<PlayerController>().color);
        if(Time.timeScale == 1)
        {
            arrowClone.GetComponent<Rigidbody2D>().AddForce(arrowClone.transform.right * speed);
        }
        else
        {
            arrowClone.GetComponent<Rigidbody2D>().AddForce(arrowClone.transform.right * speed * slowMoSpeedMultiplier);
        }
        return arrowClone;
    }

    public void FirePowerUp(int arrowNumber)
    {
        float speed = 1800f;
        Vector3 hr = aimStick.transform.rotation.eulerAngles;
        GameObject powerArrow = (GameObject)Instantiate(Resources.Load("powerArrow"), aimStick.transform.position, Quaternion.Euler(hr));
        powerArrow.GetComponentInChildren<triggerScript>().ControllerNumber = GetComponentInParent<PlayerController>().ControllerNumber;
        powerArrow.GetComponentInChildren<triggerScript>().arrowNumber = arrowNumber + 1;
        powerArrow.GetComponentInChildren<SpriteRenderer>().color = GetComponentInParent<PlayerController>().color;
        if (Time.timeScale == 1)
        {
            powerArrow.GetComponent<Rigidbody2D>().AddForce(powerArrow.transform.right * speed);
        }
        else
        {
            powerArrow.GetComponent<Rigidbody2D>().AddForce(powerArrow.transform.right * speed * slowMoSpeedMultiplier);
        }
        Destroy(powerArrow, 2f);
    }

    public GameObject FireGravityArrow()
    {
        float speed = 900f;
        Vector3 hr = aimStick.transform.rotation.eulerAngles;
        Vector3 position = new Vector3(aimStick.transform.position.x, aimStick.transform.position.y, aimStick.transform.position.z + 2);
        GameObject gravityArrow = (GameObject)Instantiate(Resources.Load("gravityArrow"), position, Quaternion.Euler(hr));
        gravityArrow.GetComponentInChildren<triggerScript>().ControllerNumber = GetComponentInParent<PlayerController>().ControllerNumber;
        gravityArrow.GetComponent<Rigidbody2D>().AddForce(gravityArrow.transform.right * speed);
        if (Time.timeScale == 1)
        {
            gravityArrow.GetComponent<Rigidbody2D>().AddForce(gravityArrow.transform.right * speed);
        }
        else
        {
            gravityArrow.GetComponent<Rigidbody2D>().AddForce(gravityArrow.transform.right * speed * slowMoSpeedMultiplier);
        }
        Destroy(gravityArrow, 2f);
        return gravityArrow;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
