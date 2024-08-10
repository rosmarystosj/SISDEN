/*using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

public class NotificationService
{
    private HubConnection _hubConnection;
    private readonly NavigationManager _navigationManager;

    public event Action<string> OnNotificationReceived;

    public NotificationService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        InitializeHubConnection();
    }

    private void InitializeHubConnection()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/notificationHub"))
            .Build();

        // Configurar el manejo de eventos recibidos
        _hubConnection.On<string>("ReceiveNotification", (message) =>
        {
            // Asegúrate de modificar la UI de Blazor desde el hilo de la UI
            OnNotificationReceived?.Invoke(message);
        });
    }

    public async Task StartConnection()
    {
        if (_hubConnection.State == HubConnectionState.Disconnected)
        {
            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                // Manejar la excepción, posiblemente reintentar o registrar el error
                Console.Error.WriteLine($"Error starting SignalR connection: {ex.Message}");
            }
        }
    }

    public async Task StopConnection()
    {
        if (_hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
        }
    }
}*/
