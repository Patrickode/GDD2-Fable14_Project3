using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private Customer customerPrefab = null;

    // Spacing fields
    [SerializeField] private float startX = -10.0f;
    [SerializeField] private float spaceBetweenCustomers = 10.0f;

    // Time fields
    [SerializeField] private float minTimeBetweenCustomers = 10.0f;
    [SerializeField] private float maxTimeBetweenCustomers = 20.0f;

    [SerializeField] private int maxCustomerAmount = 4;

    private List<Customer> customers;

    private Coroutine customerEnqueingCoroutine;

    private void Awake()
    {
        customers = new List<Customer>();
    }

    private void Start()
    {
        StartEnqueingCustomers();
    }

    public void StartEnqueingCustomers()
    {
        customerEnqueingCoroutine = StartCoroutine(HandleCustomerEnqueing());
    }

    public void StopEnqueingCustomers()
    {
        StopCoroutine(customerEnqueingCoroutine);
    }
    
    private IEnumerator HandleCustomerEnqueing()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenCustomers, maxTimeBetweenCustomers));
            if (customers.Count < maxCustomerAmount)
                EnqueueCustomer();
        }
    }

    private void EnqueueCustomer()
    {
        Customer newCustomer = Instantiate(customerPrefab);
        customers.Add(newCustomer);
        PositionCustomers();
    }

    private void DequeueCustomer(Customer customer)
    {
        customers.Remove(customer);
        PositionCustomers();
    }

    private void PositionCustomers()
    {
        for (int i = 0; i < customers.Count; i++)
        {
            customers[i].transform.position = new Vector3(startX + i * spaceBetweenCustomers, 0, 1);
        }
    }
}
