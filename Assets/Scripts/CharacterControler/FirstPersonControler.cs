using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControler : MonoBehaviour
{
    public float gravity = 9.5f;
    public float moveSpeed = 0.5f;
    public float mouseSensitivity = 1f;
    public float jumHeight = 1f;
  
    private CharacterController ch;
    //private bool canJump = true;
    private Transform cam;
    private float xRotation = 0f;
    private float gVelocity;

    private void Start()
    {
        ch = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //Rotainting players//

        float xMouse = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        float yMouse = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

        xRotation -= yMouse;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * xMouse);
        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //Basic Movment//

        float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 translation = transform.right * x + transform.forward * z;

        // Gravity Force //
        if (ch.isGrounded)
            gVelocity = -0.025f;
        else
            gVelocity -= gravity * Time.deltaTime * Time.deltaTime;

        //Jumping//

        if (ch.isGrounded && Input.GetButtonDown("Jump"))
        {
            gVelocity = Mathf.Sqrt(jumHeight * 2 * gravity) * Time.deltaTime;
        }

        translation.y = gVelocity;

       

        //Moving character
        ch.Move(translation);

    }
}
