using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using Unity.Collections;


public class UDPReciever : MonoBehaviour
{
    [Header("Network Settings")]
    public int _port = 5000; //Tablet port number
    public float _sendRate = 0.1f; //Sends map data 10 times per second

    [Header("UI")]
    public TextMeshProUGUI _debugtext; //UI Text slot
    public Transform _submarineTransform; //Submarine object goes here!

    private UdpClient _udpClient;
    private IPEndPoint _remoteEndPoint;
    private float _nextSendTime;

    public Vector2 NetworkInput { get; private set; }

    private void Start()
    {
        _udpClient = new UdpClient(_port);
        _remoteEndPoint = new IPEndPoint(IPAddress.Any, _port);

        if (_debugtext != null)
        {
            _debugtext.text = "Server started. Listening on port" + _port;
        }
    }

    private void Update()
    {
        // RECIEVE input from the tablet
        while (_udpClient.Available > 0)
        {
            byte[] receiveBytes = _udpClient.Receive(ref _remoteEndPoint);
            string message = Encoding.UTF8.GetString(receiveBytes);

            string[] parts = message.Split(',');
            if (parts.Length == 2)
            {
                if (float.TryParse(parts[0], out float x) && float.TryParse(parts[1], out float y))
                {
                    NetworkInput = new Vector2(x, y);

                    // Update the UI with what we received
                    if (_debugtext != null)
                    {
                        _debugtext.text = $"Received Input:\nX (Turn): {x:F2}\nY (Move): {y:F2}";
                    }
                }
            }
        }

        // SEND position back to the tablet -> we only send if _remoteEndPoint has caught an IP from the tablet, so we know where to send it!
        if (_remoteEndPoint.Address != IPAddress.Any && Time.time >= _nextSendTime)
        {
            _nextSendTime = Time.time + _sendRate;

            // Format posX, posZ, rotationY
            string posData = $"{_submarineTransform.position.x},{_submarineTransform.position.z},{_submarineTransform.eulerAngles.y}";
            byte[] sendBytes = Encoding.UTF8.GetBytes(posData);

            // Send back to the exact IP and port the tablet just used
            _udpClient.Send(sendBytes, sendBytes.Length, _remoteEndPoint);
        }
    }

    private void OnApplicationQuit()
    {
        _udpClient?.Close();
    }

}
