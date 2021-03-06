﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private Order orderPrefab = null;

    private List<Order> orders;
    private OrderContainer orderContainer;

    private void Awake()
    {
        orders = new List<Order>();
        orderContainer = FindObjectOfType<OrderContainer>();
    }

    private void OnEnable()
    {
        CustomerManager.OnEnqueuedCustomer += CreateOrderFor;
        CustomerManager.OnDequeueCustomer += PlayOutAnimation;
    }

    private void OnDisable()
    {
        CustomerManager.OnEnqueuedCustomer -= CreateOrderFor;
    }

    private void CreateOrderFor(Customer customer)
    {
        Order order = Instantiate(orderPrefab, customer.transform.position, Quaternion.identity);
        order.transform.parent = orderContainer.transform;
        order.transform.position = new Vector3(order.transform.position.x, 4);
        order.Customer = customer;
        order.OnDelete += DeleteOrder;
        orders.Add(order);
    }

    private void PlayOutAnimation(Customer customer)
    {
        Order order = orders.First(o => o.Customer == customer);
        Animation animation = order.GetComponentInChildren<Animation>();
        animation.Play("OrderOut");
    }

    private void DeleteOrder(Order order)
    {
        orders.Remove(order);
        Destroy(order.gameObject);
    }
}
