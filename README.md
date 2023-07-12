Monster Hunter Online - Server
===
Server emulator for the game Monster Hunter Online.

# Disclaimer
The project is intended for educational purpose only.

# Dependencies
You will require dotnet 7.0 SDK
https://dotnet.microsoft.com/en-us/download/dotnet/7.0

you can check by typing `dotnet --list-sdks` into the console.
```
PS C:\Users\x> dotnet --list-sdks
6.0.405 [C:\Program Files\dotnet\sdk]
7.0.102 [C:\Program Files\dotnet\sdk]
7.0.305 [C:\Program Files\dotnet\sdk]
```

# Server
use the following parameters: `service start` to start the server

# Client
All information is based on Version: `2.0.11.942`  
To run the client please refer to:
https://github.com/sebastian-heinz/mho_launcher

# Hosts
It is not required to modify the hosts in order to run the client. 
But still suggested to prevent the client from automatically 
sending crash reports or other data.
As it is unknown who owns or what happens to the data.
```
127.0.0.1 tqos.gamesafe.qq.com
127.0.0.1 down.qq.com
127.0.0.1 stat.iips.qq.com
127.0.0.1 ied-tqos.qq.com
127.0.0.1 apps.game.qq.com
```

# Project
This explains the high level concepts of this server, 
to provide enough knowledge for contribution.

## Logging
When any data is transferred it will be logged to the console,
and additionally to a file in the `Logs/` folder.
It will only log the high level packet data

## Handling Packets
When the client sends data, the server will automatically parse the received data
and construct a `CsProtoPacket` this contains a Id and the data. 
Based on the Id the server will then call a `ICsProtoHandler` implementation.
The implementations can be found in the `/CsProto/Handler/` folder.

A newly created handler should be named after its Id with the `Handler` suffix.
```
`CS_CMD_ID.CS_CMD_MULTI_NET_IPINFO` -> `CsCmdMultiNetIpInfoHandler`
```

The handler implementation should extend `CsProtoStructureHandler<TStructure>`
where `TStructure` is the corresponding packet structure class.
```
public class CsCmdMultiNetIpInfoHandler : CsProtoStructureHandler<MultiNetIpInfo>
```

The handler needs to provide the Id which should trigger a call to the handler.
```
public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_MULTI_NET_IPINFO;
```

And it needs to override the `Handle` function, with the specified `TStructure` as parameter.
```
public override void Handle(Client client, MultiNetIpInfo req)
```
This allows the server to automatically provide a parsed class instance 
with all relevant data assigned to the structure class variables.


The last thing is that the handler needs to be registered with the `Server` class.
It contains a `LoadPacketHandler()` method, where newly created handler need to be added.

Now when a packet with the corresponding Id arrives 
then the code inside the `Handle` function will be executed.

## Parsing Structures
Before you can create a handler class, its data structure needs to be defined.
It is required to define the logic that parses the packet data into meaningful variables. 
All structures are located in the `/CsProto/Structures/` folder.

The Structure name should follow the name in the `csproto.xml` file, without the `Cs` prefix.

They need to extend the `CsStructure` class, as it provides convenience methods for parsing.
```
public class MultiNetIpInfo : CsStructure
```

All its properties, and array values should be initialized in the constructor:
```
    public MultiNetIpInfo()
    {
        SelectIp = "";
        DomainName = "";
        DomainAnalyseIp = "";
        PingDomainIp = 0;
        ConfigIp = InitArray(CsProtoConstant.CS_MAX_IP_STRING_COUNT, () => "");
        PingIp = new int[CsProtoConstant.CS_MAX_IP_STRING_COUNT];
        Port = 0;
        Signature = new List<byte>();
        Isp = 0;
        Mode = 0;
        HistoryPingWeight = new int[CsProtoConstant.CS_MAX_IP_STRING_COUNT];
    }
```
This helps to prevent raising exception when accessing the data.

Lastly a `Write` and `Read` method needs to be implemented:
```
public override void Write(IBuffer buffer)
    {
        WriteString(buffer, SelectIp);
        [...]
    }

    public override void Read(IBuffer buffer)
    {
        SelectIp = ReadString(buffer);
        [...]
    }
```
This defines how the packet data is parsed into the variables.


# Project Details
This section covers the protocol and workings in more depth.

## Protocol
The protocol to communicate is called "TDPU" (Transaction Protocol Data Unit).



# Connections
These are other observed connections, but not required to be implemented for a functioning service.
## port 7533/UDP and 7534/TCP
Client will send out UDP packets on port 7533 every second.
This packet contains a TCP port number (default 7543), on which the client listens.
Connecting to this port via TCP Socket causes the game client to send data packets.
From my investigation this looks to be used for a `Scaleform AMP` client, a performance anlysis tool.

## port 8081/UDP (tqos.gamesafe.qq.com)
Looks to be anti cheat based communication

## port 8000/UDP (ied-tqos.qq.com )
Looks to be system / log information based communication