using UnityEngine;
using UnityEngine.InputSystem;
using System.Net.Sockets;
using System.Text;


public class TabletControllerJimi : MonoBehaviour
{
    [Header("Network Settings")]
    public string _pcIPAddress = "10.227.4.197";
    public int _port = 5000;

    private SubmarineControllers _controls;
    private UdpClient _udpClient;

    private void Awake()
    {
        _controls = new SubmarineControllers();
        _udpClient = new UdpClient();
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

        string message = $"{moveInput.x}, {moveInput.y}";
        byte[] data = Encoding.UTF8.GetBytes(message);

        _udpClient.Send(data, data.Length, _pcIPAddress, _port);
    }

    private void OnApplicationQuit()
    {
        _udpClient?.Close();
    }
}
