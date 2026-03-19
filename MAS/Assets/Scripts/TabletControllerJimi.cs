using UnityEngine;
using UnityEngine.InputSystem;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using System;
using System.Linq.Expressions;


public class TabletControllerJimi : MonoBehaviour
{
    [Header("Network Settings")]
    public string _pcIPAddress = "10.214.209.170"; //10.227.4.197 <- the other ip
    public int _port = 5000;

    [Header("UI")]
    public TextMeshProUGUI _debugText; // drag the TabletDebugText here
    public RectTransform _playerIcon; // The submarine icon here
    public float _mapscaleMultiplier = 1; // To adjust if the icon moves too fast

    private SubmarineControllers _controls;
    private UdpClient _udpClient;
    private Vector2 _moveInput;

    private void Awake()
    {
        _controls = new SubmarineControllers();
        _udpClient = new UdpClient();
        //_udpClient.EnableBroadcast = true; // Allow tablet to shout to the whole network
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
        // SEND control input to the PC
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

        // RECIEVE minimap position from the PC
        while (_udpClient.Available > 0)
        {
            IPEndPoint serverEnd = new IPEndPoint(IPAddress.Any, 0);
            byte[] recieveBytes = _udpClient.Receive(ref serverEnd);
            string mapData = Encoding.UTF8.GetString(recieveBytes);

            string[] parts = mapData.Split(',');

            // If we got X, Z and Rotation successfully
            if (parts.Length == 3 &&
                float.TryParse(parts[0], out float posX) &&
                float.TryParse(parts[1], out float posZ) &&
                float.TryParse(parts[2], out float rotY))
            {
                if (_playerIcon != null)
                {
                    // Map the 3D World X/Z to the 2D UI X/Y
                    _playerIcon.anchoredPosition = new Vector2(posX * _mapscaleMultiplier, posZ * _mapscaleMultiplier);

                    // Rotate the icon (negative because 2D UI rotation is inverted from 3D Y-axis)
                    _playerIcon.localRotation = Quaternion.Euler(0, 0, -rotY);
                }
            }
        }
        
        _udpClient.Send(data, data.Length, _pcIPAddress, _port);
    }

    private void OnApplicationQuit()
    {
        _udpClient?.Close();
    }
}
