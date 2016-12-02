using UnityEngine;
using System.Collections;

public class GravitySnitch : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if(!coll.gameObject.GetComponent<PlayerController>().isPoweredUp)
            {
                coll.gameObject.GetComponent<PlayerController>().hasGravityArrow = true;
                GetComponentInParent<SnitchMovements>().DestroySnitch();
            }
        }
    }
}
