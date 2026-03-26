using DG.Tweening;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private bool _detouched;

    public int Id { get; set; }

    private void Update()
    {
        // click for pc
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.timeScale == 0)
                return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                Detouch();
            }
        }

        // touch for mobile
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Time.timeScale == 0)
                return;
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                Detouch();
            }
        }
    }
    [ContextMenu("Detouch cube")]
    public void Detouch()
    {
        if (_detouched)
            return;

        _detouched = true;
        GetComponentInParent<Entity>().DetouchCube(this);
        GetComponent<ColorCube>().ApplyDetouchColor();
    }

    public void Destroy()
    {
        Detouch();

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        transform.DOScale(0, 0.5f).OnComplete(() => Destroy(gameObject));
    }

}