using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO.Ports;
public class Player : MonoBehaviour
{
    //This line will assignment a serial port to a variable, so you can access it later
    //Make sure "COM5" is the same port in the ArduinoIDE
    //9600 is the data rate in bits per second (baud), this should match the rate you send it at, default to 9600 i believe
    SerialPort ardIn = new SerialPort("COM8", 115200);

    //Variables that are used for an example.
    //The example being to make a cube jump when you press the button with a jump force given by the potentiometer
    public float jumpforce = 200.0f;
    private float zIn;
    public bool onGround = true;

    private float attackTimer = 0;
    void Start()
    {
        //Open the serial port
        ardIn.Open();
        //Sets the number of milliseconds before timeout occurs when a read operation does not finish
        //If something goes wrong in your read operations, it will timeout after ___ amount of milliseconds
        ardIn.ReadTimeout = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if the port is open
        if (ardIn.IsOpen)
        {
            // Try and run this code 
            try
            {
                //String to store our inputs from the arduino
                string input = ardIn.ReadLine();
                //printing out the input, just checking
                //print(input);
                //we are spliting our input string when we see a ','
                string[] valueString = input.Split(',');

                jumpforce = int.Parse(valueString[4]);
                zIn = float.Parse(valueString[3]);

                print(zIn);
                if (zIn < -15 && gameObject.GetComponent<Rigidbody>().velocity.magnitude < 10)
                {
                    gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(15, 0, 0), ForceMode.Force);
                }
                else if (zIn > 15 && gameObject.GetComponent<Rigidbody>().velocity.magnitude < 10)
                {
                    gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-15, 0, 0), ForceMode.Force);
                }

                //Setting a max jumoforce of 500, if its more we are going to clamp it to 500
                if (jumpforce > 800.0f)
                {
                    jumpforce = 800.0f;
                }
                //If it is too low we clamp it to 200
                else if (jumpforce < 100.0f)
                {
                    jumpforce = 100.0f;
                }
                //We are checking if out second value in the string is confirming we are pressing the button
                if (valueString[5] == "1")
                {
                    Attack();
                }
                attackTimer -= Time.deltaTime;
                if (attackTimer < 0)
                {
                    GameObject.Find("PlayerArms").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("PlayerArms").GetComponent<BoxCollider>().enabled = false;
                }
            }
            catch (System.Exception)
            {
            }
        }
    }


    //A simple jump funtion
    void Attack()
    {

        GameObject.Find("PlayerArms").GetComponent<MeshRenderer>().enabled = true;
        GameObject.Find("PlayerArms").GetComponent<BoxCollider>().enabled = true;

        attackTimer = 0.2f;
        //if (!onGround && gameObject.GetComponent<Rigidbody>().velocity.y == 0)
        {
        //    onGround = true;
        }
        //if (onGround)
        {
          //  gameObject.GetComponent<Rigidbody>().AddForce(transform.up * jumpforce, ForceMode.Force);
            //onGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            EditorSceneManager.LoadScene(2);
        }
    }
}
