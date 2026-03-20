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
    public string _pcIPAddress = "10.154.73.170"; //10.227.4.197 <- the other ip
    public int _pcPort = 5000;
    //public int _receivePort = 5001;

    [Header("UI")]
    public TextMeshProUGUI _debugText; // drag the TabletDebugText here
    public RectTransform _playerIcon; // The submarine icon here
    public float _mapscaleMultiplier = 15f; // To adjust if the icon moves too fast

    private SubmarineControllers _controls;

    //Two "pipes"
    //private UdpClient _udpSender;
    //private UdpClient _udpReceiver;

    // Just one pipe
    private UdpClient _udpClient;

    private Vector2 _moveInput;

    // Track what we receive
    private float _lastPosX = 0f;
    private float _lastPosZ = 0f;

    //Timer for our firewall-bypassing ping
    //private float _nextPingTime;

    private void Awake()
    {
        _controls = new SubmarineControllers();

        //Sender has no assigned port (uses rand temp one to push data out)
        _udpClient = new UdpClient();

        _udpClient.Connect(_pcIPAddress, _pcPort);

        //Receiver is permanently bolted to port 5001 to listen
        //_udpReceiver = new UdpClient(_receivePort);

        // Bind the UdpClient to port 5001
        //_udpClient = new UdpClient(_receivePort);
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
        // UDP HOLE PUNCH
        // Force the receiving pipe to send a heartbeat out so Android leaves the door open for incoming replies
        //if (Time.time >= _nextPingTime)
        //{
            //_nextPingTime = Time.time + 1f; //Ping once per second
            //byte[] pingData = Encoding.UTF8.GetBytes("PING");

            //try 
            //{ _udpReceiver.Send(pingData, pingData.Length, _pcIPAddress, _sendPort); } 
            //catch { }
        //}

        // SEND control input to the PC
        Vector2 moveInput = _controls.Player.Move.ReadValue<Vector2>();

        //moveInput = _controls.Player.Move.ReadValue<Vector2>();

        string message = $"{moveInput.x}, {moveInput.y}";
        byte[] sendData = Encoding.UTF8.GetBytes(message);

        try
        {
            // Try to send the control data
            _udpClient.Send(sendData, sendData.Length);

            // If successful, update the UI
            if (_debugText != null)
            {
                _debugText.text = $"Broadcasting...\nSending: X:{moveInput.x:F2} Y:{moveInput.y:F2}";
            }
        }
        catch //(Exception e)
        {
            // If Android blocks it or the network fails, print the error
            //if (_debugText != null)
            //{
               // _debugText.text = $"Error:\n{e.Message}";
            //}
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

                // Save the numbers so we can print them
                _lastPosX = posX;
                _lastPosZ = posZ;

                if (_playerIcon != null)
                {
                    // Map the 3D World X/Z to the 2D UI X/Y
                    _playerIcon.anchoredPosition = new Vector2(posX * _mapscaleMultiplier, posZ * _mapscaleMultiplier);

                    // Rotate the icon (negative because 2D UI rotation is inverted from 3D Y-axis)
                    _playerIcon.localRotation = Quaternion.Euler(0, 0, -rotY);
                }
            }
        }

        // UPDATE DEBUG TEXT
        if (_debugText != null)
        {
            _debugText.text = $"SENDING\nX: {_moveInput.x:F2} | Y: {_moveInput.y:F2}\n\nRECEIVING\nX: {_lastPosX:F2} | Z: {_lastPosZ:F2}";
        }
        
        //_udpClient.Send(data, data.Length, _pcIPAddress, _receivePort);
    }

    private void OnApplicationQuit()
    {
        _udpClient?.Close();
    }
}
