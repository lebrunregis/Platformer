using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.PlayerInput;

[RequireComponent(typeof(Collider2D))]
public class CollisionEmitter2D : MonoBehaviour
{
    public UnityEvent<Collider2D> m_OnTriggerEnterEvents = new();
    public UnityEvent<Collider2D> m_OnTriggerStayEvents = new();
    public UnityEvent<Collider2D> m_OnTriggerExitEvents = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Sensor collided");
        m_OnTriggerEnterEvents.Invoke(other);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        m_OnTriggerEnterEvents.Invoke(other);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        m_OnTriggerEnterEvents.Invoke(other);
    }
}