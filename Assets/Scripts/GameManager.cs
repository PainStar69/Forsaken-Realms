using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Input's")]
    public PlayerControls _inputAction;

    [Header("Panels")]
    [SerializeField] private GameObject _InventoryPanel;

    private bool _inventoryPanelActive = false;

    private void Awake()
    {
        _inputAction = new PlayerControls();
    }

    private void OnEnable()
    {
        _inputAction.Enable();
        _inputAction.Player.StockInventory.performed += InventoryOpenButtonClicked;
    }

    private void OnDisable()
    {
        _inputAction.Player.StockInventory.performed -= InventoryOpenButtonClicked;
        _inputAction.Disable();
    }

    private void InventoryOpenButtonClicked(InputAction.CallbackContext _ctx)
    {
        Debug.Log("Button pressed!");

        _inventoryPanelActive = !_inventoryPanelActive;
        _InventoryPanel.SetActive(_inventoryPanelActive);
    }
}
