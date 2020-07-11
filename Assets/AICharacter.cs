using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    public GameObject ragdoll;
    Animator a => GetComponent<Animator>();
    float runSpeed = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool running = false;
    public void RunAway()
    {
        a.SetInteger("LocomotionState", 2);
        running = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {

            a.SetInteger("LocomotionState", 0);
        } else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            a.SetInteger("LocomotionState", 1);
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            a.SetInteger("LocomotionState", 2);
            running = true;
        }
        if (running)
        {
            transform.position += transform.forward * Time.deltaTime * runSpeed;
        }
    }
}
