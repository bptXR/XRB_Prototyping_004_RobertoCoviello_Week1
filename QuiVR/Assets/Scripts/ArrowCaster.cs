using UnityEngine;

public class ArrowCaster : MonoBehaviour
{
    [SerializeField] private Transform tip;
    [SerializeField] private LayerMask layerMask = ~0;

    private Vector3 _lastPosition = Vector3.zero;

    public bool CheckForCollision(out RaycastHit hit)
    {
        if (_lastPosition == Vector3.zero)
            _lastPosition = tip.position;

        bool collided = Physics.Linecast(_lastPosition, tip.position, out hit, layerMask);
        _lastPosition = collided ? _lastPosition : tip.position;

        return collided;
    }
}
