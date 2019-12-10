using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoundScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject moveParticles;
    [SerializeField] private GameObject explosionParticles;
    [SerializeField] private GlitchEffect glitchEffect;
    [SerializeField] private Joystick joystick;
    [SerializeField] private float smoothnes = 0.25f;
    [SerializeField] private Image healthBar;
    private float startHealth = 100f;
    private float health;
    private int cash;


    private readonly float xScale = 0.2f;
    private readonly float min_xScale = 0.3f;
    private readonly float yScale = 0.2f;
    private readonly float max_yScale = 0.18f;

    private Vector3 dir;
    private Vector3 mousePos;

    private bool isPressed = false;

    private float xVel = 0.2f;
    private float yVel = 0.2f;
    private float newScaleX;
    private float newScaleY;

    private Rigidbody2D rb;
    private LineRenderer lineRenderer;

    private float startSize;
    private float maxSize;
    private Camera camera;

    private void Start()
    {
        health = startHealth;
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        camera = Camera.main.GetComponent<Camera>();
        startSize = camera.orthographicSize;
        maxSize = startSize * 2f;
       

    }

    private void OnParticleCollision(GameObject other)
    {
        
        if (other.gameObject.tag == "ExplosionPS")
        {
            Debug.Log("hit");
            GetDamage(10f);
            StartCoroutine(StartGlitch());
            if (health < 0)
            {
                SelfDestroy();
            }

        }
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "GreenBall")
        {
            GetDamage(100f);
            if (health < 0)
            {
                SelfDestroy();
            }
        }
    }

    private void GetDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
    }

    public void SetHealth(float healthAmount)
    {   if (health < 100)
        {
            health += healthAmount;
            healthBar.fillAmount = health / startHealth;
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetCash(int cashAmount)
    {
        cash += cashAmount;
    }

    private void Update()
    {

       /* if (isPressed)
        {
            lineRenderer.enabled = true;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dir = transform.position - (mousePos + Vector3.forward * 10);

            DrawTrajectory(transform.position, transform.position + dir);

            Rotation();
        }

        if (rb.velocity != Vector2.zero)
        {
            Scaling();
            Rotation();
         //   camera.orthographicSize = Mathf.Lerp(startSize, maxSize, rb.velocity.magnitude*Time.deltaTime);
        }*/
    }

    private void FixedUpdate()
    {
        
        if (rb.velocity != Vector2.zero)
        {
            //Debug.Log(rb.velocity.magnitude);
            Scaling();
            Rotation();
           
            camera.orthographicSize = Mathf.Lerp(startSize, rb.velocity.magnitude, smoothnes);
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, startSize, maxSize);
        }
       rb.AddForce(joystick.Direction * moveSpeed);
        
        /*mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         dir = (mousePos - transform.position).normalized;
         rb.velocity = dir * moveSpeed; */


    }

    private void OnMouseDown ()
    {
        isPressed = true;
        
        Time.timeScale = 0.2f;
        
    }

    private void OnMouseUp ()
    {
        isPressed = false;
        Time.timeScale = 1f;
        var dirN = dir.normalized;
        rb.AddForce(dirN * dir.magnitude * moveSpeed);

        lineRenderer.enabled = false;
        Instantiate(moveParticles, transform.position, Quaternion.identity);

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

    private void Rotation()
    {
        var deltaX = transform.position.x - mousePos.x;
        var deltaY = transform.position.y - mousePos.y;
        var angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void DrawTrajectory(Vector3 from, Vector3 to)
    {
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
    }

    public IEnumerator StartGlitch()
    {
        glitchEffect.GlitchOn();
       yield return new WaitForSeconds(1f);
        glitchEffect.GlitchOff();
    }

    private void SelfDestroy()
    {
        
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
  
            Destroy(gameObject);
        
    }
}


