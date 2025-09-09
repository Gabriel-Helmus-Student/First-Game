using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Calls the Rigidbody of the player
    public Rigidbody my_Rigidbody;
    //Calls the Rigidbody of the ball
    public Rigidbody ball_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        ball_Rigidbody.AddForce(-transform.forward * 1250f);
    }

    // When the player collides with sphere, print "You Lose!"
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Cylider")
        {
            print("You Lose!");
        }
    }

    // Update is called once per framerate of game
    // When player presses left or right arrow key, move player left or right
    void FixedUpdate()
    {
        if (Input.GetKey("left") || Input.GetKey("right"))
        {
            Vector3 my_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            my_Rigidbody.MovePosition(transform.position + my_Input * Time.deltaTime * 5f);
        }
    }
}
