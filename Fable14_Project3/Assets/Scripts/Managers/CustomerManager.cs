using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private Customer customerPrefab = null;

    [SerializeField] private int maxCustomerAmount = 4;

    [Header("Spacing fields")]
    [SerializeField] private float startX = -10.0f;
    [SerializeField] private float spaceBetweenCustomers = 10.0f;

    [Header("Time fields")]
    [SerializeField] private float minTimeBetweenCustomers = 10.0f;
    [SerializeField] private float maxTimeBetweenCustomers = 20.0f;

    private List<Customer> customers;

    private PotionTableManager potionTableManager;

    private Coroutine customerEnqueingCoroutine;

    public event Action OnMaxCustomersReached;
    public event Action OnMaxCustomersLeft;
    public event Action<Customer> OnEnqueuedCustomer;
    public event Action<Customer> OnDequeueCustomer;

    private void Awake()
    {
        customers = new List<Customer>();
        potionTableManager = FindObjectOfType<PotionTableManager>();
    }

    private void Start()
    {
        StartEnqueingCustomers();
    }

    private void OnEnable()
    {
        OnMaxCustomersReached += StopEnqueingCustomers;
        OnMaxCustomersLeft += StartEnqueingCustomers;
    }

    private void OnDisable()
    {
        OnMaxCustomersReached = null;
        OnMaxCustomersLeft = null;
        OnEnqueuedCustomer = null;
        OnDequeueCustomer = null;
    }

    public void StartEnqueingCustomers()
    {
        if (customerEnqueingCoroutine != null)
            StopCoroutine(customerEnqueingCoroutine);
        customerEnqueingCoroutine = StartCoroutine(HandleCustomerEnqueing());
    }

    public void StopEnqueingCustomers()
    {
        StopCoroutine(customerEnqueingCoroutine);
    }

    public void RemoveCustomer(Customer customer)
    {
        if (customers.Count >= maxCustomerAmount)
            OnMaxCustomersLeft?.Invoke();
        customers.Remove(customer);

        OnDequeueCustomer?.Invoke(customer);

        PositionCustomers();
    }

    private IEnumerator HandleCustomerEnqueing()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeBetweenCustomers, maxTimeBetweenCustomers));
            if (customers.Count < maxCustomerAmount)
                EnqueueCustomer();
        }
    }

    private void EnqueueCustomer()
    {
        Customer newCustomer = Instantiate(customerPrefab);
        newCustomer.OnPatienceDepleted += () => RemoveCustomer(newCustomer);
        // Set a random potion type when the customer is created
        newCustomer.potionRequested = potionTableManager.FetchRandomPotionType();
        customers.Add(newCustomer);
        if (customers.Count >= maxCustomerAmount)
            OnMaxCustomersReached?.Invoke();
        PositionCustomers();

        OnEnqueuedCustomer?.Invoke(newCustomer);
    }

    private void PositionCustomers()
    {
        for (int i = 0; i < customers.Count; i++)
        {
            customers[i].transform.position = new Vector3(startX + i * spaceBetweenCustomers, 0);
        }
    }
}
