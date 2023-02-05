using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterManager : MonoBehaviour
{
    private PlayerInput playerInput;
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
        playerInput = GetComponent<PlayerInput>();
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

    //Find out the mouse position and jump to it (FLJ, 2/4/2023)
    public void OnFire()
    {
        Vector3 posicion = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        RaycastHit gotcha;
        Ray pointy=Camera.main.ViewportPointToRay(posicion);
        if(Physics.Raycast(pointy,out gotcha))
        {
            Vector3 dest = gotcha.point;
            dest.y = transform.position.y;
            controller.Move(dest - transform.position);
        }

    }

    public void OnInteract()
    {
        if(interactableText.activeSelf == true) {
            highlightedItem.Interact();
        }
        else //AHA! no interaction? Just move (FLJ,2/4/2023)
        {
            OnFire();
        }
    }

    void Update()
    {
        if (playerInput.currentActionMap.name == "UI") {
            highlightedItem = null;
            if (interactableText.activeSelf)
                interactableText.SetActive(false);
            return;
        }
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange);
        if (colliders.Length > 0) {
            int interactablesFound = 0;
            foreach (Collider collider in colliders) {
                InteractableItem itemInReach = collider.gameObject.GetComponent<InteractableItem>();
                if (itemInReach != null) {
                    if (!itemInReach.active)
                        continue;

                    interactablesFound++;

                    if (highlightedItem != null && highlightedItem != itemInReach) {
                        //highlightedItem.HighlightOff();
                    } else if (highlightedItem != itemInReach) {
                        //itemInAim.HighlightOn();
                        highlightedItem = itemInReach;
                    }
                    
                    if (!interactableText.activeSelf)
                        interactableText.SetActive(true);
                } 
            } 
            if (interactablesFound == 0) {
                highlightedItem = null;
                if (interactableText.activeSelf)
                    interactableText.SetActive(false);
            }
        }

        // Changes the height position of the player..
        if (!groundedPlayer) {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
