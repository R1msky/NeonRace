using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float acceleration = 2f;
    private Transform target;
    private Rigidbody2D rb;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 direction = target.transform.position - transform.position;
        
        rb.AddForce(direction * moveSpeed);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 10f);

        if (hit.transform && hit.transform.tag == "Player")
        {

             rb.AddForce(hit.transform.position * moveSpeed * acceleration);
            
        }

        else if (hit.transform && hit.transform.tag != "Player")
        {
            Vector2 newDirection = Random.insideUnitCircle.normalized;
            rb.AddForce(newDirection * moveSpeed * acceleration);
        }

       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Trail")
        {
            Destroy(gameObject);
        }
    }

}
