using UnityEngine;

public class CustomerAnimator : MonoBehaviour
{
    Customer customer;

    private void Awake()
    {
        customer = transform.parent.GetComponent<Customer>();
    }

    public void Destroy()
    {
        Destroy(customer.gameObject);
    }
}
