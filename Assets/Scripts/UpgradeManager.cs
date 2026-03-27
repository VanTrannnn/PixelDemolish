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
    public float globalRotationSpeed = 1f;
    public float globalDamageBonus = 0f;

    private bool _isSlectingSocket = false;
    private int _sawsStillToPlace = 0;  
    private bool _mustPlaceSawOnFirstUpgrade = false;  

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

    private void Start()
    {
        // Check if there are any saws on the level
        if (!HasAnySaw())
        {
            _mustPlaceSawOnFirstUpgrade = true;
        }

        // Trigger initial saw placement at level start
        if (LevelManager.Instance != null)
        {
            int initialSawCount = LevelManager.Instance.GetInitialSawStart();
            if (initialSawCount > 0)
            {
                StartInitialSawPlacement(initialSawCount);
            }
        }
    }

    private bool HasAnySaw()
    {
        return GameObject.FindGameObjectsWithTag("Saw").Length > 0;
    }

    private void StartInitialSawPlacement(int sawCount)
    {
        _sawsStillToPlace = sawCount;
        _isSlectingSocket = true;
        Time.timeScale = 0f; 
        ApplyDotweenToEmptySockets();
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

    public bool MustPlaceSawOnFirstUpgrade()
    {
        return _mustPlaceSawOnFirstUpgrade;
    }

    public void ResetMustPlaceSawFlag()
    {
        _mustPlaceSawOnFirstUpgrade = false;
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
        XPManager.Instance.HideUpgradePanel();
        ApplyDotweenToEmptySockets();
    }
    
    private void ApplyDotweenToEmptySockets()
    {
        foreach(var s in sawSockets)
        {
            if (s.childCount == 0) 
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

    private bool IsInitialPlacement()
    {
        return _sawsStillToPlace > 0;
    }
    private void PlaceSawAt(Transform socket)
    {
        // Kill all socket animations and reset scale
        KillAllSocketAnimations();
        
        // Instantiate new saw
        GameObject newSaw = Instantiate(sawPrefab, socket);
        newSaw.transform.localPosition = Vector3.zero;
        newSaw.transform.localRotation = Quaternion.identity;

        UpdateAllExistingSaws();
        
        // Check if this is initial placement
        if (IsInitialPlacement())
        {
            _sawsStillToPlace--;
            if (_sawsStillToPlace <= 0)
            {
                // All initial saws placed, start the game
                _isSlectingSocket = false;
                Time.timeScale = 1f;
            }
            else
            {
                ApplyDotweenToEmptySockets();
            }
        }
        else
        {
            // Normal NewSaw upgrade flow
            _isSlectingSocket = false;
            XPManager.Instance.FinishUpgrade();
        }
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
            saw.transform.rotation = Quaternion.identity; 
            saw.SetAnimatorSpeed(globalRotationSpeed); 
        }
    }
   
}