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
<<<<<<< Updated upstream
=======
        public bool wantToOrder = false;
        public SO_Jamu jamu_inginDibeli = null;
        public CustomerState CurrentState;
>>>>>>> Stashed changes

        private int lastQueueNumber;
        private Vector3 queuePosition;
        private bool orderAsked;
        private bool impatient = false;

        private NavMeshAgent agent;
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

            CurrentState = CustomerState.WaitingInQueue;

<<<<<<< Updated upstream
            CustomersQueueManager.Instance.OnQueueChanged += QueueChanged;
=======
            CustomersQueueManager.Instance.OnAddedQueue += QueueChanged;
            CustomersQueueManager.Instance.OnRemovedQueue -= QueueChanged;
>>>>>>> Stashed changes
            QueueChanged();
        }

        void OnDestroy()
        {
<<<<<<< Updated upstream
            CustomersQueueManager.Instance.OnQueueChanged -= QueueChanged;

=======
            CustomersQueueManager.Instance.OnAddedQueue -= QueueChanged;
            CustomersQueueManager.Instance.OnRemovedQueue -= QueueChanged;
            PergiDariToko?.Invoke();
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            switch (currentState)
=======
            if (Manager_Game.instance.IsPaused) return;
            if (StoreMinigameManager.Instance.IsMinigameActive) return;
            customerAnimator.SetBool("Moving", Vector2.Distance(transform.position, targetPosition) > 0.1f);
            /*Debug.Log(transform.position);
            Debug.Log(targetPosition);*/
            switch (CurrentState)
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
                currentState = CustomerState.Leaving;
=======
                CurrentState = CustomerState.Leaving;
                customerAnimator.SetTrigger("Angry");
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
                currentState = CustomerState.Leaving;
=======
                CurrentState = CustomerState.Leaving;
                customerAnimator.SetTrigger("Angry");
>>>>>>> Stashed changes
                CustomersQueueManager.Instance.DequeueCustomer();
            }
        }

        public void HandleLeaving()
        {
<<<<<<< Updated upstream
            agent.SetDestination(CustomersQueueManager.Instance.QueueSpawnTransform.position);
=======
            CurrentState = CustomerState.Leaving;
            Vector3 targetPos = CustomersQueueManager.Instance.QueueSpawnTransform.position;
            agent.SetDestination(targetPos);
            targetPosition = targetPos;
>>>>>>> Stashed changes
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

            if (CurrentState == CustomerState.WaitingInQueue && queueNumber == 0)
            {
                CurrentState = CustomerState.Ordering;
                currentPatience = OrderPatience;
                impatient = false;
            }

            visualCustomer.sortingOrder = 10 - queueNumber;

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
<<<<<<< Updated upstream
=======
        public void GettingDeliveredRightJamu()
        {
            Debug.Log("Customer Bahagia!");
            CurrentState = CustomerState.Leaving;
            CustomersQueueManager.Instance.DequeueCustomer();
            DihidangkanBenar?.Invoke(true);
            customerAnimator.SetTrigger("Happy");
            StopAllCoroutines();
        }
        public void GettingDeliveredWrongJamu()
        {
            Debug.Log("Customer Sedih!");
            CurrentState = CustomerState.Leaving;
            CustomersQueueManager.Instance.DequeueCustomer();
            customerAnimator.SetTrigger("Angry");
            DihidangkanBenar?.Invoke(false);
            StopAllCoroutines();
        }

        public void SetUpData(SO_Customer data)
        {
            visualCustomer.sprite = data.tipeKarakter;
        }
>>>>>>> Stashed changes
    }
}