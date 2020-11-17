using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class Order : MonoBehaviour
{
    private Customer customer;

    // From 0 (don't follow) to 1 (instantaneous)
    private float followSpeed = 0.7f;

    private float maxPatience;

    public Customer Customer
    {
        get => customer;
        set
        {
            customer = value;
            potionLiquidRenderer.color = Customer.potionRequested.color;
            maxPatience = Customer.Patience;
            potionName.text = Customer.potionRequested.potionName;
        }
    }

    public event Action<Order> OnDelete;

    private SpriteRenderer potionLiquidRenderer;
    private TextMeshPro potionName;
    private PatienceBar patienceBar;

    private void Awake()
    {
        potionLiquidRenderer = FindObjectsOfType<SpriteRenderer>().First(renderer => renderer.name == "Liquid");
        potionName = GetComponentInChildren<TextMeshPro>();
        patienceBar = GetComponentInChildren<PatienceBar>();
    }

    private void Update()
    {
        FollowCustomerX();
        UpdatePatienceBar();
    }

    private void OnDisable()
    {
        OnDelete = null;
    }

    private void FollowCustomerX()
    {
        if (Customer)
        {
            // Follow customer (only in the x direction)
            Vector3 newPosition = Vector3.Lerp(transform.position, customer.transform.position, followSpeed);
            newPosition = new Vector3(newPosition.x, transform.position.y);
            transform.position = newPosition;
        }
    }

    private void UpdatePatienceBar()
    {
        if (Customer)
        {
            float percent = Customer.Patience / maxPatience;
            patienceBar.SetPatience(percent);
        }
    }

    public void TriggerDelete()
    {
        OnDelete?.Invoke(this);
    }
}