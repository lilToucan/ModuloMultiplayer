using Mirror;
using UnityEngine;

[RequireComponent
    (typeof(NetworkIdentity),
    typeof(Rigidbody2D),
    typeof(NetworkTransformReliable))
]
public class BallMovement : NetworkBehaviour
{
    [SerializeField]
    float startingSpeed = 12f;

    [SerializeField]
    float bounceSpeedUp = 2;

    [SerializeField, Range(0f,1f)]
    float hitVelpercentage = 0.2f;

    float currentSpeed;

    Rigidbody2D rb;

    #region Server

    [Server]
    public void StartGame(Vector2 direction)
    {
        currentSpeed = startingSpeed;
        rb.linearVelocity = direction * currentSpeed;
    }

    [Command]
    void CmdBounce(Vector2 normal, Vector2 hitVelocity)
    {
        currentSpeed += bounceSpeedUp;

        Vector2 vel = Vector2.Reflect(rb.linearVelocity, normal) * currentSpeed;
        vel += hitVelocity * hitVelpercentage;

        rb.linearVelocity = vel;
    }

    #endregion

    #region Client

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = Vector2.zero;
        Vector2 hitVel = Vector2.zero;

        foreach (var contact in collision.contacts)
        {
            normal += contact.normal;
        }
        normal = normal.normalized;

        if (collision.transform.TryGetComponent(out RacketMovement racket))
            hitVel = racket.currentVelocity;

        CmdBounce(normal, hitVel);
    }

    #endregion
}
