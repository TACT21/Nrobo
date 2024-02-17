using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using UnityEditor.MemoryProfiler;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class Conecter : IDisposable
{
    public readonly HubConnection  hubConnection;
    
    public Conecter(string SeverUrl)
    {
        hubConnection = new HubConnectionBuilder().WithUrl(SeverUrl).Build();

        hubConnection.Closed += async (error) =>
        {
            await Task.Delay(new System.Random().Next(0, 5) * 1000);
            await hubConnection.StartAsync();
        };
    }

    public void Dispose()
    {
        hubConnection.DisposeAsync();
    }
}
