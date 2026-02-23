using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class UDPReciever : MonoBehaviour
{
    [Header("Network Settings")]
    public int _port = 5000; //Tablet port number

    private UdpClient _udpClient;
    private IPEndPoint _remoteEndPoint;

    public Vector2 NetworkInput { get; private set; }

    private void Start()
    {
        _udpClient = new UdpClient(_port);
        _remoteEndPoint = new IPEndPoint(IPAddress.Any, _port);
    }

    private void Update()
    {
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
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        _udpClient?.Close();
    }

}
