using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AimarWork;
using UnityEngine;

namespace FadlanWork
{
    public class CustomersQueueManager : MonoBehaviour
    {
        public static CustomersQueueManager Instance;

        public Transform QueueLineTransform;
        public Transform QueueSpawnTransform;
        public float QueueSpacing;
        public GameObject customerPrefab;

        public List<Customer> CustomersQueue = new();
        public event Action OnQueueChanged;

        void Awake()
        {
            if (Instance != null)
                throw new Exception("More than one instance of CustomersQueueManager");

            Instance = this;
        }

        void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                NewCustomer();
            }
        }

        public void NewCustomer()
        {
            Customer customer = Instantiate(customerPrefab).GetComponent<Customer>();
            customer.transform.SetParent(QueueSpawnTransform);
            customer.transform.position = QueueSpawnTransform.position;

            CustomersQueue.Add(customer);
            OnQueueChanged?.Invoke();
        }

        public int GetQueueNumber(Customer customer)
        {
            return CustomersQueue.IndexOf(customer);
        }

        public void DequeueCustomer()
        {
            RemoveCustomer(CustomersQueue.First());
        }

        public void RemoveCustomer(Customer customer)
        {
            CustomersQueue.Remove(customer);
            OnQueueChanged?.Invoke();
            Destroy(customer.gameObject, 3);
        }

        public Customer GetFirst()
        {
            return CustomersQueue.First();
        }
    }
}