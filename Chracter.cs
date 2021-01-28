using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chracter : MonoBehaviour
{
    public Rigidbody Rb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Rb.AddForce(0, 0, 10);
        }
      
        if (Input.GetKey(KeyCode.S))
        {
            Rb.AddForce(0, 0, -10);
        }
   
        if (Input.GetKey(KeyCode.A))
        {
            Rb.AddForce(-10, 0, 0);
        }
      
        if (Input.GetKey(KeyCode.D))
        {
            Rb.AddForce(10, 0, 0);
        }

       

    }

    private void OnCollisionStay(Collision collision)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (collision.gameObject.tag == "Floor")
            {
                float velocity = 0.7f * Mathf.Abs(Physics.gravity.y) * 0.7f;
                velocity = Mathf.Sqrt(velocity);
                Rb.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
            }
        }
    }

    
}
