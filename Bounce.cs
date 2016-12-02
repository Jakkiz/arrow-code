using UnityEngine;
using System.Collections;
using Thor;

public class Bounce : MonoBehaviour
{
    public GameObject hitParticle;
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Arrow" || coll.gameObject.tag == "GravityArrow" || coll.gameObject.tag == "TrapArrow")
        {
           // (coll.gameObject.GetComponentInChildren<TrapArrowTrigger>().arrowNumber > 1)
            float velocityColl = coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
            if (coll.gameObject.tag == "Arrow" || coll.gameObject.tag == "GravityArrow")
            {
                if ((coll.gameObject.GetComponentInChildren<triggerScript>().arrowNumber > 1) && gameObject.tag != "ArenaWall")
                {
                    coll.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
                    coll.gameObject.GetComponent<Rigidbody2D>().AddForce(coll.transform.right * (velocityColl * 80));
                    coll.gameObject.GetComponentInChildren<triggerScript>().arrowNumber = 3;
                    //Sprite Blaster
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<Collider2D>().enabled = false;
                    SpriteBlaster.instance.Blast(spriteRenderer.sprite, spriteRenderer.transform.localToWorldMatrix);
                }
                else
                {
                    GameObject hitParticlex = (GameObject)Instantiate(hitParticle, coll.transform.position, Quaternion.Euler(0, 0, 0));
                    Destroy(hitParticlex, 1f);
                    Quaternion newRot = new Quaternion();
                    Vector2 reflect = coll.gameObject.GetComponentInParent<Rigidbody2D>().velocity.normalized;
                    float angle = Mathf.Atan2(reflect.y, reflect.x) * Mathf.Rad2Deg;
                    newRot = Quaternion.AngleAxis(angle, Vector3.forward);
                    coll.gameObject.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
            if (coll.gameObject.tag == "TrapArrow" && gameObject.tag != "ArenaWall")
            {
                if(coll.gameObject.GetComponentInChildren<TrapArrowTrigger>().arrowNumber > 1)
                {
                    coll.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
                    coll.gameObject.GetComponent<Rigidbody2D>().AddForce(coll.transform.right * (velocityColl * 80));
                    coll.gameObject.GetComponentInChildren<TrapArrowTrigger>().arrowNumber = 3;
                    //Sprite Blaster
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<Collider2D>().enabled = false;
                    SpriteBlaster.instance.Blast(spriteRenderer.sprite, spriteRenderer.transform.localToWorldMatrix);
                }
                else
                {
                    GameObject hitParticlex = (GameObject)Instantiate(hitParticle, coll.transform.position, Quaternion.Euler(0, 0, 0));
                    Destroy(hitParticlex, 1f);
                    Quaternion newRot = new Quaternion();
                    Vector2 reflect = coll.gameObject.GetComponentInParent<Rigidbody2D>().velocity.normalized;
                    float angle = Mathf.Atan2(reflect.y, reflect.x) * Mathf.Rad2Deg;
                    newRot = Quaternion.AngleAxis(angle, Vector3.forward);
                    coll.gameObject.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }

        }
        if (coll.gameObject.tag == "Player")
        {
            if (coll.gameObject.GetComponent<PlayerController>().isPushed == true)
            {
                coll.gameObject.GetComponent<PlayerController>().isStunned = true;
                coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
        if(coll.gameObject.tag == "TrapArrow")
        {
            GameObject hitParticlex = (GameObject)Instantiate(hitParticle, coll.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(hitParticlex, 1f);
            Quaternion newRot = new Quaternion();
            Vector2 reflect = coll.gameObject.GetComponentInParent<Rigidbody2D>().velocity.normalized;
            float angle = Mathf.Atan2(reflect.y, reflect.x) * Mathf.Rad2Deg;
            newRot = Quaternion.AngleAxis(angle, Vector3.forward);
            coll.gameObject.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (coll.gameObject.GetComponent<PlayerController>().isPushed == true)
            {
                coll.gameObject.GetComponent<PlayerController>().isStunned = true;
                coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

            }

        }
    }
}

