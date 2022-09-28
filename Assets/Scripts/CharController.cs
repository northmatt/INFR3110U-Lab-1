using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {
    public float moveForce = 0f;
    public float jumpForce = 0f;
    public float groundedDot = 0f;

    private Rigidbody rBody;
    private PlayerInput playerInput;
    private List<GameObject> groundedObjects = new List<GameObject>();
    private Vector2 inputs = Vector2.zero;
    private byte jump = 0;
    private bool grounded = false;

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }

    private void Awake() {
        playerInput = new PlayerInput();
    }

    void Start() {
        rBody = GetComponent<Rigidbody>();
    }

    void Update() {
        inputs = playerInput.Player.Move.ReadValue<Vector2>();
        Debug.Log(playerInput.Player.Move.triggered + ", " + playerInput.Player.Move.IsPressed() + ", " + playerInput.Player.Move.ReadValue<Vector2>());

        if (playerInput.Player.Jump.triggered && jump == 0 && grounded)
            jump = 4;
    }

    private void FixedUpdate() {
        grounded = groundedObjects.Count > 0;

        rBody.AddForce(inputs.x * moveForce * Time.fixedDeltaTime, 0f, inputs.y * moveForce * Time.fixedDeltaTime);
        inputs = Vector3.zero;

        if (jump == 4)
            rBody.AddForce(0f, jumpForce, 0f, ForceMode.VelocityChange);

        //reduce jump cooldown if grounded
        if (jump > 0 && grounded)
            --jump;
    }

    private void OnCollisionEnter(Collision collision) {
        //upto 64 contact points, unlikely to ever need >20. List has dynamic scaling
        //List<ContactPoint> contactPoints = new List<ContactPoint>();
        ContactPoint[] contactPoints = new ContactPoint[20];
        int contactPointsCount = collision.GetContacts(contactPoints);

        //loop through all contacts and compare contact normal to a specific range
        for (int i = 0; i < contactPointsCount; ++i)
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > groundedDot) {
                groundedObjects.Add(collision.gameObject);
                break;
            }
    }

    private void OnCollisionStay(Collision collision) {
        //upto 64 contact points, unlikely to ever need >20. List has dynamic scaling
        //List<ContactPoint> contactPoints = new List<ContactPoint>();
        ContactPoint[] contactPoints = new ContactPoint[20];
        int contactPointsCount = collision.GetContacts(contactPoints);

        //loop through all contacts and compare contact normal to a specific range
        for (int i = 0; i < contactPointsCount; ++i)
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > groundedDot) {
                if (!groundedObjects.Contains(collision.gameObject)) groundedObjects.Add(collision.gameObject);
                return;
            }

        groundedObjects.Remove(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision) {
        //Is late, takes a few frames to call OnColExit
        groundedObjects.Remove(collision.gameObject);
    }
}
