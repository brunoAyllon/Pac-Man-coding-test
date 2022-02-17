using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypoint = 0;

    public float ghostMovementSpeed = 5.0f;
    private Vector3 originalPosition;
    private Animator ghostAnimator;
    private Collider2D ghostCollider;
    private Rigidbody2D ghostRigidbody;

    public float startMovementDelay = 5.0f;
    private float timeWaited = 0;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        ghostAnimator = gameObject.GetComponent<Animator>();
        ghostCollider = gameObject.GetComponent<Collider2D>();
        ghostRigidbody = gameObject.GetComponent<Rigidbody2D>();

        GameplayManager.instance.resetGame.AddListener(Reset);
    }

    private void Reset()
    {
        currentWaypoint = 0;
        timeWaited = 0;
        ghostAnimator.Rebind();
        gameObject.transform.position = originalPosition;
    }

    IEnumerator MovementStartDelay()
    {
        yield return new WaitForSeconds(startMovementDelay);
    }

    void FixedUpdate()
    {
        if (GameplayManager.instance.gameFinished)
        {
            return;
        }

        if (timeWaited < startMovementDelay)
        {
            timeWaited += Time.deltaTime;
            return;
        }

        if (Vector2.Distance(gameObject.transform.position, waypoints[currentWaypoint].position) < 0.1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

        Vector3 moveDir = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, ghostMovementSpeed);
        ghostRigidbody.MovePosition(moveDir);

        Debug.Log("Moving towards waypoint: " + currentWaypoint);
        float moveDirX = waypoints[currentWaypoint].position.x - transform.position.x;
        float moveDirY = waypoints[currentWaypoint].position.y - transform.position.y;

        // Avoid jittering between two states that would cause spinning
        ghostAnimator.SetFloat("MovementDirectionX", Mathf.Abs(moveDirX) >= Mathf.Abs(moveDirY)? moveDirX:0.0f );
        ghostAnimator.SetFloat("MovementDirectionY", Mathf.Abs(moveDirY) > Mathf.Abs(moveDirX) ? moveDirY:0.0f);
    }
}
