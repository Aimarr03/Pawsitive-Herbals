using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace FadlanWork
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set;}

        public float InteractDistance = 1f;

        private NavMeshAgent agent;
        private Camera mainCamera;
        private Vector3 target;
        private BaseInteractableObject targetObject = null;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            mainCamera = Camera.main;

            if (Instance != null)
                throw new System.Exception("More than one instance of PlayerController");

            Instance = this;
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
                Vector3 targetPosition = targetObject.transform.position + new Vector3(targetObject.StandPositionOffset.x, targetObject.StandPositionOffset.y, 0);
                float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
                if (distanceToTarget <= InteractDistance)
                {
                    targetObject.Interact();
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
