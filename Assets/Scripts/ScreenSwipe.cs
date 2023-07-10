using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class ScreenSwipe : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Player playerController;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        playerController.doLook = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        playerController.doLook = false;
    }
}
