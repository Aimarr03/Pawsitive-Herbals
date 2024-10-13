using AimarWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace FadlanWork
{
    public class PlayerController : MonoBehaviour
    {
        public float InteractDistance = 1.5f;
        public PlayerInventory inventory;


        private NavMeshAgent agent;
        private Camera mainCamera;
        private Vector3 target;
        private BaseInteractableObject targetObject = null;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            inventory = GetComponent<PlayerInventory>();
            mainCamera = Camera.main;
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
            if (!Input.GetMouseButtonDown(0))
                return;
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (StoreMinigameManager.Instance.IsMinigameActive)
                return;

            Move();
        }

        void CheckTargetObjectInteraction()
        {
            if (targetObject != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetObject.transform.position);
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
                    MoveToTarget(interactable.transform.position);
                }
            }
            else
            {
                targetObject = null;
                MoveToTarget(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            }
        }

        void MoveToTarget(Vector3 targetPosition)
        {
            targetPosition.z = transform.position.z;
            agent.SetDestination(targetPosition);
        }
    }
}
