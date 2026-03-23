using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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
                SpawnNewSaw();
                break;
        }
        UpdateAllExistingSaws();
    }
    private void UpdateAllExistingSaws()
    {
        Saw[] allSaws = FindObjectsOfType<Saw>();
        foreach (var saw in allSaws)
        {
            saw.transform.localScale = Vector3.one * globalScale;
            saw.SetAnimatorSpeed(globalRotationSpeed); // Assuming base speed is 100
        }
    }
    private void SpawnNewSaw()
    {
        foreach (var socket in sawSockets)
        {
            if (socket.childCount == 0)
            {
                GameObject newSaw = Instantiate(sawPrefab, socket);
                newSaw.transform.localPosition = Vector3.zero;
                newSaw.transform.localRotation = Quaternion.identity;
                newSaw.transform.localScale = Vector3.one * globalScale;
                return;
            }
        }
    }
}