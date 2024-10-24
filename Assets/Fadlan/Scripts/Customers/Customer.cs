using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AimarWork;
using AimarWork.GameManagerLogic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

        public bool wantToOrder = false;
        public SO_Jamu jamu_inginDibeli = null;
        public CustomerState CurrentState;

        private int lastQueueNumber;
        private Vector3 queuePosition;
        private bool orderAsked;
        private bool impatient = false;

        private NavMeshAgent agent;
        private Coroutine impatientCoroutine;
        private Coroutine thinkingCoroutine;

        private Animator customerAnimator;
        private Vector3 targetPosition;
        [SerializeField] private SpriteRenderer visualCustomer;

        public Image SpriteRenderer_JamuVIsual;

        public static event Action<bool> DihidangkanBenar;
        public static event Action PergiDariToko;
        void Awake()
        {
            currentPatience = QueuePatience;
            agent = GetComponent<NavMeshAgent>();
            customerAnimator = GetComponent<Animator>();
            targetPosition = transform.position;
        }

        void Start()
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            CurrentState = CustomerState.WaitingInQueue;

            CustomersQueueManager.Instance.OnQueueChanged += QueueChanged;
            CustomersQueueManager.Instance.OnAddedQueue += QueueChanged;
            CustomersQueueManager.Instance.OnRemovedQueue -= QueueChanged;
            QueueChanged();
        }

        void OnDestroy()
        {
            CustomersQueueManager.Instance.OnQueueChanged -= QueueChanged;
            CustomersQueueManager.Instance.OnAddedQueue -= QueueChanged;
            CustomersQueueManager.Instance.OnRemovedQueue -= QueueChanged;
            PergiDariToko?.Invoke();
            if (impatientCoroutine != null)
                StopCoroutine(impatientCoroutine);

            if (thinkingCoroutine != null)
                StopCoroutine(thinkingCoroutine);
        }


        void Update()
        {
            if (Manager_Game.instance.IsPaused) return;
            if (StoreMinigameManager.Instance.IsMinigameActive) return;
            customerAnimator.SetBool("Moving", Vector2.Distance(transform.position, targetPosition) > 0.1f);
            /*Debug.Log(transform.position);
            Debug.Log(targetPosition);*/
            switch (CurrentState)
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
            targetPosition = queuePosition;
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

                CurrentState = CustomerState.Leaving;
                customerAnimator.SetTrigger("Angry");
                customerAnimator.SetBool("Waiting Order", false);
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
                CurrentState = CustomerState.Leaving;
                customerAnimator.SetTrigger("Angry");
                CustomersQueueManager.Instance.DequeueCustomer();
                customerAnimator.SetBool("Waiting Order", false);
                DihidangkanBenar?.Invoke(false);
            }
        }
        private IEnumerator MakingAnOrder()
        {
            Debug.Log("Customer sedang menuju ke kasir");
            while (Vector2.Distance(transform.position, targetPosition) > 0.2f)
            {
                yield return null;
            }
            Debug.Log("Customer sedang berpikir");
            customerAnimator.SetBool("Ordering", true);
            wantToOrder = true;
            yield return new WaitForSeconds(1.5f);
            customerAnimator.SetBool("Ordering", false);
            wantToOrder = false;
            
            Debug.Log("Customer siap memesan");
            jamu_inginDibeli = Manager_TokoJamu.instance.MencariPemesanan();
            customerAnimator.SetBool("Waiting Order", true);
            SpriteRenderer_JamuVIsual.sprite = jamu_inginDibeli.ikon;
        }

        public void HandleLeaving()
        {
            CurrentState = CustomerState.Leaving;
            Vector3 targetPos = CustomersQueueManager.Instance.QueueSpawnTransform.position;
            agent.SetDestination(targetPos);
            targetPosition = targetPos;
        }

        void QueueChanged()
        {
            queueNumber = CustomersQueueManager.Instance.GetQueueNumber(this);
            queuePosition = new Vector3(CustomersQueueManager.Instance.QueueLineTransform.position.x + (queueNumber * CustomersQueueManager.Instance.QueueSpacing),
                                        CustomersQueueManager.Instance.QueueLineTransform.position.y,
                                        CustomersQueueManager.Instance.QueueLineTransform.position.z);
            queuePosition.z = 0;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            targetPosition = queuePosition;
            targetPosition.z = 0;
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
                //MakingAnOrder();
                impatient = false;
                StartCoroutine(MakingAnOrder());
            }

            visualCustomer.sortingOrder = 10 - queueNumber;

            lastQueueNumber = queueNumber;
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
        public void GettingDeliveredRightJamu()
        {
            Debug.Log("Customer Bahagia!");
            customerAnimator.SetBool("Waiting Order", false);
            CurrentState = CustomerState.Leaving;
            CustomersQueueManager.Instance.DequeueCustomer();
            DihidangkanBenar?.Invoke(true);
            customerAnimator.SetTrigger("Happy");
            StopAllCoroutines();
        }
        public void GettingDeliveredWrongJamu()
        {
            Debug.Log("Customer Sedih!");
            customerAnimator.SetBool("Waiting Order", false);
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
    }
}