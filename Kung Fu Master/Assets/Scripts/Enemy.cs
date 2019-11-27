using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    [SerializeField] bool finalenemy = false;
 
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-1, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arms")
        {
            if (finalenemy)
            {
                SceneManager.LoadScene(1);
            }
            Destroy(gameObject);
        }
    }
}
