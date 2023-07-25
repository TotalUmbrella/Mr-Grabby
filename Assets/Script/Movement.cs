using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour

{
    // Start is called before the first frame update
    public float speed = 10f;
    private CharacterController characterController;
    private float sensitivity = 1.8f;
    private float xRotation = 0f;
    public Camera camera;
    public float jumpHeight = 900f;
    public float gravity = -1800f;
    private float yvelocity = 0f;
    private float jumpBuffer = 0f;
    private bool land = false;
    public AudioSource landSound;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        camera.transform.localRotation = Quaternion.Euler(xRotation, 10, 0);
        if (Input.GetKey(KeyCode.LeftShift)){
            speed = 15f;
        }
        else {
            speed = 10f;
        }
        transform.Rotate(Vector3.up * mouseX);
        Vector3 move = ((forward * Input.GetAxis("Vertical")) * speed + (right * Input.GetAxis("Horizontal")) * speed);

        if (Input.GetKey(KeyCode.Space)){
            jumpBuffer = 0.01f;
        }
        jumpBuffer = jumpBuffer - Time.deltaTime;
        if (characterController.isGrounded){
            if (jumpBuffer > 0) {
                yvelocity = jumpHeight;
                Debug.Log("balls");
            }
        }        
        if (characterController.isGrounded){
            if (yvelocity <= 0 ){
                yvelocity = 0;
            }
        }

        yvelocity += gravity * Time.deltaTime;

        move = move + Vector3.up * yvelocity * Time.deltaTime;

        

        characterController.Move(move * Time.deltaTime);
    }
}
