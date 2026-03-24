using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
public enum UpgradeType
{BiggerSaw, FasterSaw, MoreDamage, NewSaw}

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    [Header("Prefabs & Sockets")]
    public GameObject sawPrefab;
    public List<Transform> sawSockets; //positions for saws to be attached

    [Header("Current Stats")]
    public float globalScale = 1f;
    public float globalRotationSpeed = 100f;
    public float globalDamageBonus = 0f;

    private bool _isSlectingSocket = false;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update() {
        if(_isSlectingSocket)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleSocketSelection();        
            }
        }
    }
    public bool HasEmptySocket()
    {
        foreach (var socket in sawSockets)
        {
            if (socket.childCount == 0)
                return true;
        }
        return false;
    }
    public void ApplyUpgrade(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.BiggerSaw:
                globalScale += 0.2f;
                break;
            case UpgradeType.FasterSaw:
                globalRotationSpeed += 20f;
                break;
            case UpgradeType.MoreDamage:
                globalDamageBonus += 10f;
                break;
            case UpgradeType.NewSaw:
                StartSocketSelection();
                break;
        }
        UpdateAllExistingSaws();
    }
    private void StartSocketSelection()
    {
        _isSlectingSocket = true;
        // Step 1: Hide panel and pause game
        XPManager.Instance.HideUpgradePanel();
        
        // Step 2: Check empty sockets and apply dotween
        ApplyDotweenToEmptySockets();
    }
    
    private void ApplyDotweenToEmptySockets()
    {
        // Start pulsing animation only on empty sockets with unique ID
        foreach(var s in sawSockets)
        {
            if (s.childCount == 0) // Only animate empty sockets
                s.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true).SetId("SocketPulse_" + s.GetInstanceID());
        }
    }
    private void HandleSocketSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform selectedSocket = hit.transform;
            if (sawSockets.Contains(selectedSocket) && selectedSocket.childCount == 0)
            {
                PlaceSawAt(selectedSocket);
            }
        }
    }
    private void PlaceSawAt(Transform socket)
    {
        _isSlectingSocket = false;
        
        // Step 3: Kill all socket animations and reset scale
        KillAllSocketAnimations();
        
        // Step 4: Instantiate new saw
        GameObject newSaw = Instantiate(sawPrefab, socket);
        newSaw.transform.localPosition = Vector3.zero;
        newSaw.transform.localRotation = Quaternion.identity;

        UpdateAllExistingSaws();
        
        // Step 5: Resume game
        XPManager.Instance.FinishUpgrade();
    }
    
    private void KillAllSocketAnimations()
    {
        foreach(var s in sawSockets)
        {
            DOTween.Kill("SocketPulse_" + s.GetInstanceID());
            s.localScale = Vector3.one;
        }
    }
    private void UpdateAllExistingSaws()
    {
        Saw[] allSaws = FindObjectsOfType<Saw>();
        foreach (var saw in allSaws)
        {
            saw.transform.localScale = Vector3.one * globalScale;
            saw.transform.rotation = Quaternion.identity; // Reset rotation (0,0,0)
            saw.SetAnimatorSpeed(globalRotationSpeed); // Assuming base speed is 100
        }
    }
   
}