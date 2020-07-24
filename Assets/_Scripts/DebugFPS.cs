using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFPS : MonoBehaviour
{

    public delegate void OnMouseButtonHeld_0(DebugFPS fps);
    public OnMouseButtonHeld_0 onMouseButtonHelld_0;
    public delegate void OnMouseButtonUp_0(DebugFPS fps);
    public OnMouseButtonUp_0 onMouseButtonUp_0;
    public float speed = 3f;
    public float rotSpeed = 10f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        { 
            Cursor.lockState = CursorLockMode.Locked;
            onMouseButtonHelld_0?.Invoke(this);
        }
        if (Input.GetMouseButtonUp(0))
        {
            onMouseButtonUp_0?.Invoke(this);
        }

        var speed2 = Input.GetKey(KeyCode.LeftShift) ? speed * 2.5f : speed;

        transform.position += Input.GetAxis("Vertical") * transform.forward * speed2 * Time.deltaTime;
        transform.position += Input.GetAxis("Horizontal") * transform.right * speed2 * Time.deltaTime;

        float h = rotSpeed * Input.GetAxis("Mouse X");
        float v = rotSpeed * Input.GetAxis("Mouse Y");
        if (
            (Vector3.Angle(Vector3.up,transform.up) < 70 || h < 0)
            && Vector3.Angle(-Vector3.up,-transform.up) < 70 || h > 0) transform.Rotate(Vector3.right * -v);
        transform.Rotate(Vector3.up * h, Space.World);

    }

}
