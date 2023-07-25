using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Outline;

public class Grab : MonoBehaviour
{
    private bool assigned = false;
    GameObject currentObject = null;
    GameObject pastObject = null;
            GameObject grabbedObject = null;
    public float grabDistance = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, transform.forward, out hit, 3f);
        if (currentObject != null && didHit && currentObject != hit.collider.gameObject){
            if (grabbedObject == null){
                Destroy(currentObject.GetComponent<Outline>());
                currentObject = null;
            }
        }
        if (didHit && hit.collider.gameObject.GetComponentInParent<Rigidbody>() != null && hit.collider.gameObject.GetComponentInParent<Rigidbody>().gameObject.tag == "Grabbable") {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (!assigned){
                if (grabbedObject){
                    currentObject = grabbedObject;
                }
                else {
                    currentObject = hit.collider.gameObject;
                }
                assigned = true;
                //adds outline
                var outline = currentObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.green;
                outline.OutlineWidth = 10f;
            }
            if (currentObject != hit.collider.gameObject && grabbedObject == null){
                //this code runs when you look at a different object
                //the object you are looking at has no longer been assigned
                assigned = false;
            }
            Debug.Log(assigned);
        }
        //grab object
        if (Input.GetMouseButtonDown(0)){
            if (didHit && hit.collider.gameObject.GetComponentInParent<Rigidbody>() != null && hit.collider.gameObject.GetComponentInParent<Rigidbody>().gameObject.tag == "Grabbable"){
                grabbedObject = hit.collider.gameObject.GetComponentInParent<Rigidbody>().gameObject;
                grabDistance = Vector3.Distance(transform.position + transform.up * 0.575f, grabbedObject.transform.position);
                grabDistance -= 1f;
            }
        }
        //if you aren't looking at anything
        if (!didHit && currentObject != null && grabbedObject == null){
            Destroy(currentObject.GetComponent<Outline>());
            currentObject = null;
        }
    }
    void FixedUpdate(){
            if(grabbedObject) {
                grabDistance += Input.mouseScrollDelta.y * 1f;
                grabDistance = Mathf.Clamp(grabDistance, 1.5f, 3f);

                //set target position and camera vectors
                Vector3 targetPos = transform.position;
                Vector3 forward = transform.forward;
                Vector3 right = transform.right;
                Vector3 up = transform.up;

                //make sure targetpos is in front of you
                targetPos += forward * grabDistance;
                float grabForce = 100f;
                // I have no clue what this does (but I need it here - oscar)
                Renderer rend = grabbedObject.GetComponentInChildren<Renderer>();
                // doubles force if its closer why does this happen? idk (idk I wrote this ages ago ... I have no clue why it's there ... it probably did something - oscar)
                if (rend != null && Vector3.Distance(targetPos, rend.bounds.center) < 2f) {
                    grabForce *= 2f;
                }
                grabbedObject.GetComponent<Rigidbody>().AddForce((targetPos - rend.bounds.center) * grabForce * grabbedObject.GetComponent<Rigidbody>().mass);
                // damping
                grabbedObject.GetComponent<Rigidbody>().velocity *= 0.5f;
                // damping angular velocity
                grabbedObject.GetComponent<Rigidbody>().angularVelocity *= 0.5f;

                if (Input.GetKey(KeyCode.Q)){
                    grabbedObject.GetComponent<Rigidbody>().AddTorque(transform.up * -90 * grabbedObject.GetComponent<Rigidbody>().mass);
                }
                if (Input.GetKey(KeyCode.E)){
                    grabbedObject.GetComponent<Rigidbody>().AddTorque(transform.right * 90 * grabbedObject.GetComponent<Rigidbody>().mass);
                }
                if (!Input.GetMouseButton(0)){
                    grabbedObject.transform.parent = null;
                    grabbedObject = null;
                }
            }
    }
}
