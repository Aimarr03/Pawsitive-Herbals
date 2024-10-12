using System.Collections;
using System.Collections.Generic;
using FadlanWork;
using UnityEngine;
using UnityEngine.AI;

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

    [Header("Customer Info")]
    public float currentPatience;
    public int queueNumber = -1;
    private int lastQueueNumber;
    private Vector3 queuePosition;

    private NavMeshAgent agent;
    private CustomerState currentState;

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
    }

    void Update()
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
        }

        if (currentState == CustomerState.WaitingInQueue && queueNumber == 0)
        {
            currentState = CustomerState.Ordering;
            currentPatience = OrderPatience;
        }

        lastQueueNumber = queueNumber;
    }
}
