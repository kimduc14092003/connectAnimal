using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinPanelController : MonoBehaviour
{
    public GameObject Effect;

    private void OnEnable()
    {
        Effect.SetActive(true);
    }
}
