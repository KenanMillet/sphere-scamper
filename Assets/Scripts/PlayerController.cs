﻿using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour {

    public float acceleration;
    public float maxSpd;
    private Rigidbody rb;
    Vector3 movement;

    GameController gameController;
    public GameObject shockwave;

    public bool poweredUp;

    void Update() //most game code will go here
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        
        if(Input.GetButtonDown("Use Powerup") && poweredUp == true)
        {
            poweredUp = false;
            shockwave.SetActive(true);
        }
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

    }

    void FixedUpdate()// called before physics calcs
    {
        rb.AddForce(movement * acceleration);

        Vector2 velocity2D = new Vector2(rb.velocity.x, rb.velocity.z);
        if (velocity2D.magnitude > maxSpd)
        {
            velocity2D = velocity2D.normalized * maxSpd;
            Debug.Log(velocity2D);
            Vector3 velocity3D = rb.velocity;
            velocity3D.x = velocity2D.x;
            velocity3D.z = velocity2D.y;
            rb.velocity = velocity3D;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pick Up"))
        {
            Destroy(other.gameObject);
            gameController.AddScore(1);
        }
        else if (other.gameObject.CompareTag("Power Up") && poweredUp == false)
        {
            Destroy(other.gameObject);
            poweredUp = true;
            gameController.AddScore(1);
            
        }
    }

    void OnEnable()
    {
        shockwave.SetActive(false);
    }

}
