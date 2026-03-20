using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using Unity.Collections;
using UnityEngine.Rendering.Universal;


public class UDPReciever : MonoBehaviour
{
    [Header("Network Settings")]
    public int _listenPort = 5000; //Tablet port number
    public float _sendRate = 0.1f; //Sends map data 10 times per second

    [Header("UI")]
    public TextMeshProUGUI _debugtext; //UI Text slot
    public Transform _submarineTransform; //Submarine object goes here!

    // Just one pipe
    private UdpClient _udpServer;
    //private UdpClient _udpSender;

    private IPEndPoint _remoteEndPoint;
    private float _nextSendTime;

    public Vector2 NetworkInput { get; private set; }

    private void Start()
    {
        // Bolted to 5000
        _udpServer = new UdpClient(_listenPort);

        // Sender uses a random outbound port
        //_udpSender = new UdpClient();

        //_remoteEndPoint = new IPEndPoint(IPAddress.Any, _listenPort);

        //if (_debugtext != null) This was for debugging
        //{
           // _debugtext.text = "Server started. Listening on port" + _port;
        //}
    }

    private void Update()
    {
        // RECIEVE input from the tablet
        while (_udpServer.Available > 0)
        {
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes = _udpServer.Receive(ref sender);
            string message = Encoding.UTF8.GetString(receiveBytes);

            string[] parts = message.Split(',');
            if (parts.Length == 2)
            {
                if (float.TryParse(parts[0], out float x) && float.TryParse(parts[1], out float y))
                {
                    NetworkInput = new Vector2(x, y);

                    _remoteEndPoint = sender;

                    // Update the UI with what we received
                    if (_debugtext != null)
                    {
                        _debugtext.text = $"Received Input:\nX (Turn): {x:F2}\nY (Move): {y:F2}";
                    }
                }
            }
        }

        // SEND position back to the tablet -> we only send if _remoteEndPoint has caught an IP from the tablet, so we know where to send it!
        if (_remoteEndPoint != null && Time.time >= _nextSendTime)
        {
            _nextSendTime = Time.time + _sendRate;

            // Format posX, posZ, rotationY
            string posData = $"{_submarineTransform.position.x},{_submarineTransform.position.z},{_submarineTransform.eulerAngles.y}";
            byte[] sendBytes = Encoding.UTF8.GetBytes(posData);

            //Grab the tablet's IP address, but force the port to 5001
            //IPEndPoint tabletReturnAddress = new IPEndPoint(_remoteEndPoint.Address, _replyPort);

            _udpServer.Send(sendBytes, sendBytes.Length, _remoteEndPoint);

            // We reply to the exact IP that just spoke to us, specifically targeting their receive port (5001)
            //_udpSender.Send(sendBytes, sendBytes.Length, tabletReturnAddress);

            if (_debugtext != null)
            {
                _debugtext.text = $"Received Input: X: {NetworkInput.x:F2} Y: {NetworkInput.y:F2}\nReplying to: { _remoteEndPoint.Address}:{ _remoteEndPoint.Port}";
            }
        }
    }

    private void OnApplicationQuit()
    {
        _udpServer?.Close();
    }

}
