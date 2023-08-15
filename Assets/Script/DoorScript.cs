using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour

{
    public Transform door;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    //clamp rotation between 30 and 80
    {
        float rotation = Mathf.Clamp(door.rotation.x, 30, 80);
        door.rotation = Quaternion.Euler(rotation, 0, 0);
    }
}
