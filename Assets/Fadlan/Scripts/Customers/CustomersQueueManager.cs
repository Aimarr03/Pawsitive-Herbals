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
<<<<<<< Updated upstream
        public event Action OnQueueChanged;
=======
        public List<SO_Customer> List_Data_Customer;

        public event Action OnAddedQueue;
        public event Action OnRemovedQueue;
        public float durationToSpawn;
        public int maxQueue = 5;
        public float currentDuration;
>>>>>>> Stashed changes

        void Awake()
        {
            if (Instance != null)
                throw new Exception("More than one instance of CustomersQueueManager");

            Instance = this;
        }

        void Start()
        {
<<<<<<< Updated upstream
            for (int i = 0; i < 5; i++)
=======

        }
        private void Update()
        {
            if (Manager_Game.instance.IsPaused) return;
            if (CustomersQueue.Count >= maxQueue) return;

            if (!Manager_TokoJamu.instance.CekTokoBuka()) {
                ClearQueueExceptFirst();
                return;
            };

            currentDuration += Time.deltaTime;
            if (currentDuration > durationToSpawn)
>>>>>>> Stashed changes
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
            if (!CustomersQueue.Contains(customer)) return -1;

            return CustomersQueue.IndexOf(customer);
        }

        public void DequeueCustomer()
        {
            RemoveCustomer(CustomersQueue.First());
        }

        public void RemoveCustomer(Customer customer)
        {
            customer.HandleLeaving();
            CustomersQueue.Remove(customer);
<<<<<<< Updated upstream
            OnQueueChanged?.Invoke();
            Destroy(customer.gameObject, 3);
=======
            Destroy(customer.gameObject, 1.8f);
            OnRemovedQueue?.Invoke();
>>>>>>> Stashed changes
        }

        public Customer GetFirst()
        {
            return CustomersQueue.First();
        }

        public void ClearQueueExceptFirst()
        {
            if (CustomersQueue.Count <= 1)
                return;

            for (int i = CustomersQueue.Count - 1; i > 0; i--)
            {
                RemoveCustomer(CustomersQueue[i]);
            }
        }
    }
}