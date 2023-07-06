using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls pActions;

    public float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 desiredDirection;

    private void OnEnable()
    {
        pActions = new PlayerControls();
        pActions.Enable();
    }

    private void OnDisable()
    {
        pActions.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        desiredDirection.x = pActions.PlayerActions.Movement.ReadValue<Vector2>().x;
        //desiredDirection.y = pActions.PlayerActions.Movement.ReadValue<Vector2>().y;
        desiredDirection = desiredDirection.normalized;

        rb.MovePosition(rb.position + desiredDirection * moveSpeed * Time.fixedDeltaTime);
    }


}
