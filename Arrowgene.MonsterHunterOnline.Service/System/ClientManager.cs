using System.Collections.Generic;

namespace Arrowgene.MonsterHunterOnline.Service.System;

public class ClientManager
{
    private readonly List<Client> _clients;

    private readonly object _lock = new object();

    public ClientManager()
    {
        _clients = new List<Client>();
    }

    /// <summary>
    /// Returns all Clients.
    /// </summary>
    public List<Client> GetAll()
    {
        lock (_lock)
        {
            return new List<Client>(_clients);
        }
    }

    /// <summary>
    /// Adds a Client.
    /// </summary>
    public void Add(Client client)
    {
        if (client == null)
        {
            return;
        }

        lock (_lock)
        {
            _clients.Add(client);
        }
    }

    /// <summary>
    /// Removes the Client from all lists and lookup tables.
    /// </summary>
    public void Remove(Client client)
    {
        lock (_lock)
        {
            _clients.Remove(client);
        }
    }

    public Client GetClientByCharacterName(string characterName)
    {
        List<Client> clients = GetAll();
        foreach (Client client in clients)
        {
            Character character = client.Character;
            if (character == null)
            {
                continue;
            }

            if (character.Name == characterName)
            {
                return client;
            }
        }

        return null;
    }
}