using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour
{
    public bool active = true;
    bool activateTriggers = false;
    public int numberController = 0;
    private float speed = 500f;
    public Vector2 velocity;
    public Rigidbody2D rigidBody;
    public BlackHoleTrigger firstTrigger;
    public BlackHoleTrigger secondTrigger;

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();   
	}

	// Update is called once per frame
	void Update ()
    {
        if(!activateTriggers && active)
        {
            firstTrigger.SetInnerTrigger("Player", numberController);
            secondTrigger.SetOuterTrigger("Player", numberController);
            firstTrigger.Activate();
            secondTrigger.Activate();
            activateTriggers = true;
        }
        if (active && activateTriggers)
        {
            rigidBody.AddForce(velocity.normalized * speed * Time.deltaTime);
            Destroy(gameObject, 5f);
        }
	}

}
