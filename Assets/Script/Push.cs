using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    float pushPower = 30f;
    private  CharacterController _characterController;
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var body = hit.collider.attachedRigidbody;
        // no rigidbody
        if (body == null || body.isKinematic)
            return;
        // Only push rigidbodies in the right layers
        //var bodyLayerMask = 1 << body.gameObject.layer;
        //if ((bodyLayerMask & pushLayers) == 0)
        //return;
 
        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.05)
            return;
        // Calculate push direction from move direction, we only push objects to the sides
        // never up and down
        var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
 
        // push with move speed but never more than walkspeed
        body.velocity = pushDir * pushPower * _characterController.velocity.magnitude * Time.fixedDeltaTime;
    }
}
