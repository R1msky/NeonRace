using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float smoothnes = 0.25f;
    private float startSize;
    private float maxSize;
    private Vector2 movePos;
    private Rigidbody2D rb;
    private Camera camera;
    private readonly float xScale = 0.2f;
    private readonly float min_xScale = 0.3f;
    private readonly float yScale = 0.2f;
    private readonly float max_yScale = 0.18f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        camera = Camera.main.GetComponent<Camera>();
        startSize = camera.orthographicSize;
        maxSize = startSize * 2f;
        movePos = transform.position;
    }
    private void FixedUpdate()
    {

        if (rb.velocity != Vector2.zero)
        {
           
            Scaling();
  
            camera.orthographicSize = Mathf.Lerp(startSize, rb.velocity.magnitude, smoothnes);
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, startSize, maxSize);
        }
        //  rb.AddForce(joystick.Direction * moveSpeed);


        // mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //dir = (mousePos - transform.position).normalized;
      //  rb.velocity = movePos * moveSpeed; 
        rb.AddForce(movePos * moveSpeed);

    }

    private void Scaling()
    {
        var vel = Mathf.Sqrt(Mathf.Abs(rb.velocity.y + rb.velocity.x) / 100f);
        // float velX = Mathf.Abs(rb.velocity.x);
        // float velY = Mathf.Abs(rb.velocity.y);
        var x = Mathf.Lerp(xScale, min_xScale, vel);
        var y = Mathf.Lerp(yScale, max_yScale, vel);

        transform.localScale = new Vector3(
        x,
        y,
        transform.localScale.z);
    }

    internal void SetColor(Color32 color)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.SetColor("_EmissionColor", color);
    }

   public void SetMovePosition(Vector3 newPosition)
    {
         movePos = newPosition;
    }




}
