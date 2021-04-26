using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float velocity = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("up"))
        {
            pos.z += velocity * Time.deltaTime;
        }
        else if(Input.GetKey("down"))
        {
            pos.z -= velocity * Time.deltaTime;
        }

        if (Input.GetKey("left"))
        {
            pos.x -= velocity * Time.deltaTime;
        }
        else if(Input.GetKey("right"))
        {
            pos.x += velocity * Time.deltaTime;
        }

        transform.position = pos;
    }
}
