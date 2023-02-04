using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterManager : MonoBehaviour
{
    private InputControl inputControl;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private Vector2 movement;
    private Transform camTransform;
    private bool jumped;
    private InteractableItem highlightedItem;
    public GameObject interactableText;
    private float interactionRange = 2;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        jumped = false;
        camTransform = Camera.main.transform;
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (!jumped) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            jumped = true;
        }
    }

    public void OnInteract()
    {
        if(interactableText.activeSelf == true) {
            highlightedItem.Interact();
        }
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && jumped) {
            jumped = false;
        }
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }
        Vector3 nForward = camTransform.forward;
        nForward.y = 0;
        nForward.Normalize();
        Vector3 nRight = camTransform.right;
        nRight.y = 0;
        nRight.Normalize();
        Vector3 move = nForward * movement.y + nRight * movement.x;
        if (move.magnitude > 0)
            transform.LookAt(transform.position + (nForward * 5));
        controller.Move(move * Time.deltaTime * playerSpeed);

        //Vector2 aim = inputControl.Look.ReadValue<Vector2>();
        Vector3 origin = transform.position + (transform.forward);
        Ray ray = new Ray(origin, transform.forward);
        Debug.DrawRay(origin, transform.forward, Color.white, 0, false);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5)) {
            InteractableItem itemInAim = hit.collider.gameObject.GetComponent<InteractableItem>();
            if(itemInAim != null) {

                if (!itemInAim.active)
                    return;
                if (highlightedItem != null && highlightedItem != itemInAim) {
                    //highlightedItem.HighlightOff();
                } else if (highlightedItem != itemInAim) {
                    //itemInAim.HighlightOn();
                    highlightedItem = itemInAim;
                }
                Debug.Log(highlightedItem.name);
                if (interactableText.activeSelf && hit.distance > interactionRange)
                    interactableText.SetActive(false);
                else if (!interactableText.activeSelf && hit.distance <= interactionRange)
                    interactableText.SetActive(true);
            } else if (highlightedItem != null) {
                //highlightedItem.HighlightOff();
                highlightedItem = null;
                if (interactableText.activeSelf)
                    interactableText.SetActive(false);
            }
        } else if (highlightedItem != null) {
            //highlightedItem.HighlightOff();
            highlightedItem = null;
            if (interactableText.activeSelf)
                interactableText.SetActive(false);
        }

        // Changes the height position of the player..
        if (!groundedPlayer) {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }
}