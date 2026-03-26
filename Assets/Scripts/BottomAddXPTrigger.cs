using UnityEngine;

public class BottomAddXPTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //if per cube
        if (other.TryGetComponent(out Cube cube))
        {
            XPManager.Instance.AddXP(1);
            LevelProgressManager.Instance.NotifyPixelDestroyed();
            Destroy(other.gameObject);
        }
        //if per cubes
        else if (other.TryGetComponent(out Entity entity))
        {
            int pixels = other.transform.childCount;
            XPManager.Instance.AddXP(pixels);
            LevelProgressManager.Instance.NotifyPixelDestroyed(pixels);
            Destroy(other.gameObject);
        }
    }
}