using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {
    public float moveForce;
    public float jumpForce;

    private Rigidbody rBody;
    private List<Collision> collisions = new List<Collision>();
    private Vector3 inputs = Vector3.zero;

    // Start is called before the first frame update
    void Start() {
        rBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        inputs.Set(
            Input.GetAxisRaw("Horizontal"),
            Input.GetButtonDown("Jump") ? 1f : inputs.y,
            Input.GetAxisRaw("Vertical")
        );
    }

    private void FixedUpdate() {
        bool grounded = IsGrounded();

        rBody.AddForce(inputs.x * moveForce, 0f, inputs.z * moveForce);

        if (grounded && inputs.y > 0.01f)
            rBody.AddForce(0f, inputs.y * jumpForce, 0f, ForceMode.VelocityChange);

        inputs = Vector3.zero;
    }

    //loop through all collisions and get contact normal range
    private bool IsGrounded() {
        //upto 64 contact points, unlikely to ever need >20. List has dynamic scaling
        //List<ContactPoint> contactPoints = new List<ContactPoint>();
        ContactPoint[] contactPoints = new ContactPoint[20];
        int contactPointsCount = 0;

        foreach (Collision curCollision in collisions) {
            contactPointsCount = curCollision.GetContacts(contactPoints);

            for (int i = 0; i < contactPointsCount; ++i){
                //Debug.Log("Normal: " + contactPoints[i].normal);
            }
        }

        return true;
    }

    private void OnCollisionEnter(Collision collision) {
        collisions.Add(collision);
        Debug.Log("Enter: " + collision.GetHashCode());
        foreach (Collision curCollision in collisions) Debug.Log(curCollision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision) {
        collisions.Remove(collision);
        Debug.Log("Exit" + collision.GetHashCode());
        foreach (Collision curCollision in collisions) Debug.Log(curCollision.gameObject.name);
    }
}
