using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private Customer customerPrefab = null;
    [SerializeField] private List<Sprite> customerSprites = null;

    [SerializeField] private int maxCustomerAmount = 4;

    [Header("Spacing fields")]
    [SerializeField] private float spaceBetweenCustomers = 10.0f;

    [Header("Time fields")]
    [Tooltip("How long each customer will wait before leaving. X = Minimum, Y = Maximum. If either X or Y is " +
        "set to less than 0, sets patience equal to float.MaxValue, effectively making it infinite.")]
    [SerializeField] private Vector2 patienceRange = Vector2.one * 20;
    [SerializeField] private float minTimeBetweenCustomers = 10.0f;
    [SerializeField] private float maxTimeBetweenCustomers = 20.0f;

    private Queue<Customer> customers;
    public Customer CurrentCustomer => customers.Count > 0 ? customers.Peek() : null;
    private CustomerContainer customerContainer;

    private PotionTableManager potionTableManager;

    private Coroutine customerEnqueingCoroutine;
    private bool moreCustomersAllowed = true;

    public static Action OnMaxCustomersReached;
    public static Action OnMaxCustomersLeft;
    public static Action<Customer> OnEnqueuedCustomer;
    public static Action<Customer> OnDequeueCustomer;
    public static Action LastCustomerDequeued;

    private void Awake()
    {
        //If patience range is negative or zero, make patience infinite.
        if (patienceRange.x <= 0 || patienceRange.y <= 0)
        {
            patienceRange = Vector2.one * float.MaxValue;
        }
        //Otherwise, swap patience range's x and y if x is greater than y.
        else if (patienceRange.x > patienceRange.y)
        {
            float temp = patienceRange.x;
            patienceRange.x = patienceRange.y;
            patienceRange.y = temp;
        }

        customers = new Queue<Customer>();
        customerContainer = FindObjectOfType<CustomerContainer>();
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
        DayTimer.DayEnded += OnDayEnd;
    }

    private void OnDisable()
    {
        OnMaxCustomersReached = null;
        OnMaxCustomersLeft = null;
        OnEnqueuedCustomer = null;
        OnDequeueCustomer = null;
        DayTimer.DayEnded -= OnDayEnd;
    }

    private void Update()
    {
        if (CurrentCustomer)
            CurrentCustomer.Patience -= Time.deltaTime;
    }

    private void StartEnqueingCustomers()
    {
        if (customerEnqueingCoroutine != null)
            StopCoroutine(customerEnqueingCoroutine);
        customerEnqueingCoroutine = StartCoroutine(HandleCustomerEnqueing());
    }

    private void StopEnqueingCustomers()
    {
        StopCoroutine(customerEnqueingCoroutine);
    }
    private void OnDayEnd()
    {
        moreCustomersAllowed = false;
        StopEnqueingCustomers();
    }

    private void DequeueCustomer()
    {
        if (moreCustomersAllowed && customers.Count >= maxCustomerAmount)
        {
            OnMaxCustomersLeft?.Invoke();
        }
        else if (!moreCustomersAllowed && customers.Count <= 1)
        {
            LastCustomerDequeued?.Invoke();
        }

        OnDequeueCustomer?.Invoke(customers.Dequeue());
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
        // Parent them to the Customer Container
        newCustomer.transform.parent = customerContainer.transform;
        newCustomer.OnPatienceDepleted += () => DequeueCustomer();
        newCustomer.OnRequestComplete += (potion) => DequeueCustomer();
        newCustomer.OnWrongPotionSubmitted += () => DequeueCustomer();
        newCustomer.OnDestroyed += PositionCustomers;
        // Set a random potion type when the customer is created
        newCustomer.potionRequested = potionTableManager.FetchRandomPotionType();
        // Give the customer a random amount of patience within the desired range
        newCustomer.Patience = UnityEngine.Random.Range(patienceRange.x, patienceRange.y);
        // Set a random sprite
        Sprite newCustomerSprite = null;
        if (customerSprites.Count > 0)
        {
            do
            {
                // Keep setting the customer sprite until one is found that is not in the queue (avoid repeats)
                newCustomerSprite = customerSprites[UnityEngine.Random.Range(0, customerSprites.Count)];
            } while (customers.Any(c => c.SpriteRenderer.sprite == newCustomerSprite));
        }
        if (newCustomerSprite)
            newCustomer.SpriteRenderer.sprite = newCustomerSprite;

        customers.Enqueue(newCustomer);
        if (customers.Count >= maxCustomerAmount)
            OnMaxCustomersReached?.Invoke();
        PositionCustomers();

        OnEnqueuedCustomer?.Invoke(newCustomer);
    }

    private void PositionCustomers()
    {
        int i = 0;
        foreach (Customer c in customers)
        {
            if (c)
            {
                c.transform.position = new Vector3(i * spaceBetweenCustomers, 0);
            }

            i--;
        }
    }

    public void SubmitPotion(Potion potion)
    {
        CurrentCustomer.SubmitPotion(potion);
    }
}
