using UnityEngine;
using UnityEngine.InputSystem;
using System.Net.Sockets;
using System.Text;
using TMPro;
using System;
using System.Linq.Expressions;


public class TabletControllerJimi : MonoBehaviour
{
    [Header("Network Settings")]
    public string _pcIPAddress = "10.154.155.170"; //10.227.4.197 <- the other ip
    public int _port = 5000;

    [Header("UI")]
    public TextMeshProUGUI _debugText; // drag the TabletDebugText here

    private SubmarineControllers _controls;
    private UdpClient _udpClient;

    private void Awake()
    {
        _controls = new SubmarineControllers();
        _udpClient = new UdpClient();
       // _udpClient.EnableBroadcast = true; // Allow tablet to shout to the whole network
    }

    private void OnEnable()
    {
        _controls.Player.Enable();

    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    private void Update()
    {
        Vector2 moveInput = _controls.Player.Move.ReadValue<Vector2>();

        //moveInput = _controls.Player.Move.ReadValue<Vector2>();

        string message = $"{moveInput.x}, {moveInput.y}";
        byte[] data = Encoding.UTF8.GetBytes(message);

        try
        {
            // Try to send the control data
            _udpClient.Send(data, data.Length, _pcIPAddress, _port);

            // If successful, update the UI
            if (_debugText != null)
            {
                _debugText.text = $"Broadcasting...\nSending: X:{moveInput.x:F2} Y:{moveInput.y:F2}";
            }
        }
        catch (Exception e)
        {
            // If Android blocks it or the network fails, print the error
            if (_debugText != null)
            {
                _debugText.text = $"Error:\n{e.Message}";
            }
        }
        
        _udpClient.Send(data, data.Length, _pcIPAddress, _port);
    }

    private void OnApplicationQuit()
    {
        _udpClient?.Close();
    }
}
