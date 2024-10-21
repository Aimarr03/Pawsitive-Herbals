using System;
using System.Collections.Generic;
using System.Linq;
using AimarWork;
using AimarWork.GameManagerLogic;
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
        public List<SO_Customer> List_Data_Customer;
        
        public event Action OnAddedQueue;
        public float durationToSpawn;
        public int maxQueue = 5;
        public float currentDuration;

        void Awake()
        {
            if (Instance != null)
                throw new Exception("More than one instance of CustomersQueueManager");

            Instance = this;
        }

        void Start()
        {
            
        }
        private void Update()
        {
            if (Manager_Game.instance.IsPaused) return;
            
            if (!Manager_TokoJamu.instance.CekTokoBuka() || CustomersQueue.Count >= maxQueue) return;
            currentDuration += Time.deltaTime;
            if(currentDuration > durationToSpawn)
            {
                currentDuration = 0;
                NewCustomer();
            }
        }
        public void NewCustomer()
        {
            Customer customer = Instantiate(customerPrefab).GetComponent<Customer>();

            SO_Customer randomizeData = List_Data_Customer[UnityEngine.Random.Range(0, List_Data_Customer.Count)];
            customer.SetUpData(randomizeData);

            customer.transform.SetParent(QueueSpawnTransform);
            customer.transform.position = QueueSpawnTransform.position;

            CustomersQueue.Add(customer);
            OnAddedQueue?.Invoke();
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
            Destroy(customer.gameObject, 1.8f);
        }

        public Customer GetFirst()
        {
            return CustomersQueue.First();
        }
    }
}