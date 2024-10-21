using AimarWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI.Table;

namespace FadlanWork
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set;}
        public float InteractDistance = 1.5f;
        public PlayerInventory inventory;


        private NavMeshAgent agent;
        private Camera mainCamera;
        private Vector3 target;
        private BaseInteractableObject targetObject = null;
        private Animator playerAnimator;
        private Vector3 TargetPosition;
        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            inventory = GetComponent<PlayerInventory>();
            playerAnimator = GetComponent<Animator>();
            mainCamera = Camera.main;

            if (Instance != null)
                throw new System.Exception("More than one instance of PlayerController");

            Instance = this;
            TargetPosition = transform.position;
        }

        void Start()
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        void Update()
        {
            CheckTargetObjectInteraction();
            CheckMove();
        }

        void CheckMove()
        {
            if (Vector3.Distance(transform.position, TargetPosition) < 0.01)
            {
                playerAnimator.SetBool("Moving", false);
            }
            if (!Input.GetMouseButtonDown(0))
                return;
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (StoreMinigameManager.Instance != null)
            {
                if (StoreMinigameManager.Instance.IsMinigameActive) return;
            }
                

            Move();
        }

        void CheckTargetObjectInteraction()
        {
            if (targetObject != null)
            {
                Vector3 targetPosition = targetObject.transform.position + new Vector3(targetObject.StandPositionOffset.x, targetObject.StandPositionOffset.y, 0);
                float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
                if (distanceToTarget <= InteractDistance)
                {
                    targetObject.Interact(this);
                    
                    targetObject = null;
                }
            }
        }

        void Move()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<BaseInteractableObject>(out var interactable))
                {
                    targetObject = interactable;
                    Vector3 targetPosition = targetObject.transform.position + new Vector3(targetObject.StandPositionOffset.x, targetObject.StandPositionOffset.y, 0);
                    MoveToTarget(targetPosition);
                    playerAnimator.SetBool("Moving", true);
                }
            }
            else
            {
                targetObject = null;
                MoveToTarget(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                playerAnimator.SetBool("Moving", true);
            }
        }

        void MoveToTarget(Vector3 targetPosition)
        {
            targetPosition.z = transform.position.z;
            TargetPosition = targetPosition;
            agent.SetDestination(targetPosition);
        }
    }
}
