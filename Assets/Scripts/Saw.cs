using UnityEngine;

public class Saw : MonoBehaviour
{
    private Animator _anim;
    private void Awake() {
        _anim = GetComponent<Animator>();
    }
    public void SetAnimatorSpeed(float speed)
    {
        if (_anim != null)
        {
            _anim.speed = speed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cube cube))
        {
            cube.Detouch();
        }
    }
}