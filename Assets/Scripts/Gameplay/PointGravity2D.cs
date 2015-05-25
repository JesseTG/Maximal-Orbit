using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PointGravity2D : MonoBehaviour
{
    public Vector2 Center;
    private float _gravity;
    private Rigidbody2D _body;

    void Start ()
    {
        this._gravity = Physics2D.gravity.magnitude;
        this._body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate ()
    {
        Vector2 force = this.Center - _body.position;
        force.Normalize (); // Direction

        force *= this._gravity * _body.mass;
        // Acceleration and mass

        _body.AddForce (force);
    }
}
