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
        public List<Customer> SeatCostumers = new();
        public List<Transform> SeatList = new();
        public event Action OnQueueChanged;
        public List<SO_Customer> List_Data_Customer;
        public event Action OnAddedQueue;
        public event Action OnRemovedQueue;
        public float durationToSpawn;
        public int maxQueue = 5;
        public float currentDuration;

        public AudioClip OpenDoor;
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
            if (CustomersQueue.Count >= maxQueue) return;

            if (!Manager_TokoJamu.instance.CekTokoBuka()) return;

            currentDuration += Time.deltaTime;
            if (currentDuration > durationToSpawn)
            {
                currentDuration = 0;
                NewCustomer();
            }
        }
        public void NewCustomer()
        {
            Customer customer = Instantiate(customerPrefab).GetComponent<Customer>();
            Manager_Audio.instance.PlaySFX(OpenDoor);
            SO_Customer randomizeData = List_Data_Customer[UnityEngine.Random.Range(0, List_Data_Customer.Count)];
            customer.SetUpData(randomizeData);

            customer.transform.SetParent(QueueSpawnTransform);
            customer.transform.position = QueueSpawnTransform.position;

            CustomersQueue.Add(customer);
            OnQueueChanged?.Invoke();
            OnAddedQueue?.Invoke();
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
            CustomersQueue.Remove(customer);
            OnQueueChanged?.Invoke();
            OnRemovedQueue?.Invoke();
        }

        public Customer GetFirst()
        {
            if (CustomersQueue.Count == 0) return null;
            return CustomersQueue.First();
        }

        public bool IsSeatAvailable()
        {
            return SeatCostumers.Count < SeatList.Count;
        }

        public void AssignSeat(Customer customer)
        {
            if (!IsSeatAvailable()) return;

            SeatCostumers.Add(customer);

            int seatNumber = 0;
            for (int i=0; i<SeatList.Count; i++)
            {
                bool found = true;
                foreach (Customer cs in SeatCostumers)
                {
                    if (cs.SeatNumber == seatNumber)
                        found = false;
                }

                if (found)
                {
                    break;
                } else {
                    seatNumber++;
                }
            }

            customer.SeatNumber = seatNumber;
        }

        public Vector3 GetSeatPosition(Customer customer)
        {
            if (!SeatCostumers.Contains(customer)) return Vector3.zero;
            return SeatList[customer.SeatNumber].position;
        }

        public void RemoveSeat(Customer customer)
        {
            SeatCostumers.Remove(customer);
        }
    }
}