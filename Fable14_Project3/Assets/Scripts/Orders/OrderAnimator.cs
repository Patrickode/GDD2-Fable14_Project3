using UnityEngine;

public class OrderAnimator : MonoBehaviour
{
    Order order;

    private void Awake()
    {
        order = transform.parent.GetComponent<Order>();
    }

    public void TriggerDelete()
    {
        order.TriggerDelete();
    }
}
