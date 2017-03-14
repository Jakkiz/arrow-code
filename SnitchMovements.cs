using UnityEngine;
using System.Collections;

public class SnitchMovements : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector3 dir;
    private float speed = 1500f; 
    private float secSpeed = 1500f; 
    private RaycastHit2D upHit;
    private RaycastHit2D fHit;
    private RaycastHit2D lHit;
    private RaycastHit2D rHit;
    private RaycastHit2D nupHit;
    private RaycastHit2D nfHit;
    private RaycastHit2D nlHit;
    private RaycastHit2D nrHit;
    private Vector2 newPosition = new Vector2(0f, 0f);
    private Vector2 oldPosition = new Vector2(0f, 0f);
    private bool positionFound = false;
    private float distance;
    private Vector2 leftVect = new Vector2(1, 0.3f);
    private Vector2 rightVect = new Vector2(1, -0.3f);
    public LayerMask arena;
    private float longRay = 1.2f;
    private float shortRay = 0.8f;
    private float timer = 0;
    private Vector2 lastPos;

    void Start()
    {
        lastPos = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Raycasting()
    {
        upHit = Physics2D.Raycast(transform.position, Vector2.up, longRay, arena);
        nupHit = Physics2D.Raycast(transform.position, -Vector2.up, longRay, arena);
        fHit = Physics2D.Raycast(transform.position, Vector2.right, longRay, arena);
        lHit = Physics2D.Raycast(transform.position, leftVect, shortRay, arena);
        rHit = Physics2D.Raycast(transform.position, rightVect, shortRay, arena);
        nfHit = Physics2D.Raycast(transform.position, -Vector2.right, longRay, arena);
        nlHit = Physics2D.Raycast(transform.position, -leftVect, shortRay, arena);
        nrHit = Physics2D.Raycast(transform.position, -rightVect, shortRay, arena);
    }

    private void GenerateNewPoint()
    {
        if (!positionFound)
        {
            do
            {
                float x = Random.Range(0, 10);
                float y = Random.Range(0, 5);
                float nx = Random.Range(0, 100);
                float ny = Random.Range(0, 100);
                if (nx > 50)
                {
                    x = -x;
                }
                if (ny > 50)
                {
                    y = -y;
                }
                newPosition = new Vector2(x, y);
                distance = Vector2.Distance(oldPosition, newPosition);
            } while (distance < 5);
            positionFound = true;
            //Debug.Log("created: " + newPosition);
            oldPosition = newPosition;
        }
    }

    private void Patrol()
    {
        if (fHit.collider != null)
        {
            dir = -Vector2.right;
            rigidBody.AddForce(dir * speed *Time.deltaTime);
        }
        if (rHit.collider != null)
        {
            dir = new Vector2(1, 0.6f);
            rigidBody.AddForce(dir * speed * Time.deltaTime);
        }
        if (lHit.collider != null)
        {
            dir = new Vector2(1, -0.6f);
            rigidBody.AddForce(dir * speed * Time.deltaTime);
        }
        if (nfHit.collider != null)
        {
            dir = Vector2.right;
            rigidBody.AddForce(dir * speed * Time.deltaTime);
        }
        if (nrHit.collider != null)
        {
            dir = new Vector2(-1, -0.6f);
            rigidBody.AddForce(dir * speed * Time.deltaTime);
        }
        if (nlHit.collider != null)
        {
            dir = new Vector2(-1, 0.6f);
            rigidBody.AddForce(dir * speed * Time.deltaTime);
        }
        if (nupHit.collider != null)
        {
            dir = Vector2.up;
            rigidBody.AddForce(dir * speed * Time.deltaTime);
        }
        if (upHit.collider != null)
        {
            dir = -Vector2.up;
            rigidBody.AddForce(dir * speed * Time.deltaTime);
        }

        rigidBody.AddForce((newPosition - (Vector2)transform.position) * 1500f * Time.deltaTime);

        Debug.DrawRay(transform.localPosition, Vector2.up * longRay, Color.green);
        Debug.DrawRay(transform.localPosition, -Vector2.up * longRay, Color.green);
        Debug.DrawRay(transform.localPosition, Vector2.right * longRay, Color.yellow);
        Debug.DrawRay(transform.localPosition, leftVect * shortRay, Color.red);
        Debug.DrawRay(transform.localPosition, rightVect * shortRay, Color.magenta);
        Debug.DrawRay(transform.localPosition, -Vector2.right * longRay, Color.yellow);
        Debug.DrawRay(transform.localPosition, -leftVect * shortRay, Color.red);
        Debug.DrawRay(transform.localPosition, -rightVect * shortRay, Color.magenta);
    }

    public void DestroySnitch()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        Raycasting();
        Patrol();
        if (Vector2.Distance(transform.position, newPosition) < 1f)
        {
            positionFound = false;
            GenerateNewPoint();
        }
        if (Vector2.Distance(transform.position, lastPos) < 0.01f)
        {
            timer = timer + Time.deltaTime;
        }
        if (timer > 0.4f)
        {
            positionFound = false;
            GenerateNewPoint();
            timer = 0;
        }
        lastPos = transform.position;
    }
}
