using System;
using System.Collections;
using System.Collections.Generic;
using AimarWork;
using AimarWork.GameManagerLogic;
using UnityEngine;
using UnityEngine.AI;

namespace FadlanWork
{
    public class Customer : MonoBehaviour
    {
        public enum CustomerState
        {
            WaitingInQueue,
            Ordering,
            Leaving
        }

        [Header("Customer Configuration")]
        public float QueuePatience = 35;
        public float MovePatienceIncrease = 3;
        public float OrderPatience = 25;
        public float TurnImpatientPatience = 15;
        public int ImpatientSeconds = 5;
        public GameObject ImpatientObject;

        [Header("Customer Info")]
        public float currentPatience;
        public int queueNumber = -1;

        private int lastQueueNumber;
        private Vector3 queuePosition;
        private bool orderAsked;
        private bool impatient = false;

        private NavMeshAgent agent;
        private CustomerState currentState;
        private Coroutine impatientCoroutine;
        private Coroutine thinkingCoroutine;

        void Awake()
        {
            currentPatience = QueuePatience;
            agent = GetComponent<NavMeshAgent>();
        }

        void Start()
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            currentState = CustomerState.WaitingInQueue;

            CustomersQueueManager.Instance.OnQueueChanged += QueueChanged;
            QueueChanged();
        }

        void OnDestroy()
        {
            CustomersQueueManager.Instance.OnQueueChanged -= QueueChanged;

            if (impatientCoroutine != null)
                StopCoroutine(impatientCoroutine);

            if (thinkingCoroutine != null)
                StopCoroutine(thinkingCoroutine);
        }


    void Update()
    {
        if (Manager_Waktu.instance.IsPaused) return;
        switch (currentState)
        {
            switch (currentState)
            {
                case CustomerState.WaitingInQueue:
                    HandleWaitingInQueue();
                    break;
                case CustomerState.Ordering:
                    HandleOrdering();
                    break;
                case CustomerState.Leaving:
                    HandleLeaving();
                    break;
            }
        }

        private void HandleWaitingInQueue()
        {
            agent.SetDestination(queuePosition);

            currentPatience -= Time.deltaTime;

            if (!impatient && currentPatience <= TurnImpatientPatience)
            {
                impatient = true;

                if (impatientCoroutine != null)
                    StopCoroutine(impatientCoroutine);

                impatientCoroutine = StartCoroutine(ImpatienceEmotion());
            }

            if (currentPatience <= 0)
            {
                currentState = CustomerState.Leaving;
                CustomersQueueManager.Instance.RemoveCustomer(this);
            }
        }

        private void HandleOrdering()
        {
            agent.SetDestination(queuePosition);

            currentPatience -= Time.deltaTime;

            if (!impatient && currentPatience <= TurnImpatientPatience)
            {
                impatient = true;

                if (impatientCoroutine != null)
                    StopCoroutine(impatientCoroutine);
                
                impatientCoroutine = StartCoroutine(ImpatienceEmotion());
            }

            if (currentPatience <= 0)
            {
                currentState = CustomerState.Leaving;
                CustomersQueueManager.Instance.DequeueCustomer();
            }
        }

        private void HandleLeaving()
        {
            agent.SetDestination(CustomersQueueManager.Instance.QueueSpawnTransform.position);
        }

        void QueueChanged()
        {
            queueNumber = CustomersQueueManager.Instance.GetQueueNumber(this);
            queuePosition = new Vector3(CustomersQueueManager.Instance.QueueLineTransform.position.x + (queueNumber * CustomersQueueManager.Instance.QueueSpacing),
                                        CustomersQueueManager.Instance.QueueLineTransform.position.y,
                                        CustomersQueueManager.Instance.QueueLineTransform.position.z);

            if (queueNumber < lastQueueNumber)
            {
                currentPatience += MovePatienceIncrease;
                if (currentPatience > TurnImpatientPatience)
                    impatient = false;
            }

            if (currentState == CustomerState.WaitingInQueue && queueNumber == 0)
            {
                currentState = CustomerState.Ordering;
                currentPatience = OrderPatience;
                impatient = false;
            }

            lastQueueNumber = queueNumber;
        }

        public void AskOrder()
        {
            if (orderAsked)
                return;

            orderAsked = true;

            Manager_Jamu.instance.PemesananJamu();

            if (thinkingCoroutine != null)
                StopCoroutine(thinkingCoroutine);

            thinkingCoroutine = StartCoroutine(ThinkingForOrder());
        }

        IEnumerator ThinkingForOrder()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2, 3));
            Debug.Log("Order: " + Manager_Jamu.instance.jamu_difoksukan.nama);
        }

        IEnumerator ImpatienceEmotion()
        {
            int tickCount = ImpatientSeconds * 2;
            for (int i = 0; i < tickCount; i++)
            {
                if (!impatient)
                    break;

                ImpatientObject.SetActive(!ImpatientObject.activeSelf);
                yield return new WaitForSeconds(0.5f);
            }
            ImpatientObject.SetActive(false);
        }
    }
}