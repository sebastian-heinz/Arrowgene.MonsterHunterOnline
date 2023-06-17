Monster Hunter Online - Server
===
Server emulator for the game DMonster Hunter Online.

# Disclaimer
The project is intended for educational purpose only.

# Server
use this parameters: `service start` to start the server

# Client
All information is based on Version: `2.0.11.942`  
To run the client please refer to:
https://github.com/sebastian-heinz/mho_luncher

# Hosts
```
127.0.0.1 tqos.gamesafe.qq.com
127.0.0.1 down.qq.com
127.0.0.1 stat.iips.qq.com
127.0.0.1 ied-tqos.qq.com
127.0.0.1 apps.game.qq.com
```

# Connections
## port 7533/UDP and 7534/TCP
Client will send out UDP packets on port 7533 every second.
This packet contains a TCP port number (default 7543), on which the client listens.
Connecting to this port via TCP Socket causes the game client to send data packets.
From my investigation this looks to be used for a `Scaleform AMP` client, a performance anlysis tool.

## port 8081/UDP (tqos.gamesafe.qq.com)
Looks to be anti cheat based communication

## port 8000/UDP (ied-tqos.qq.com )
Looks to be system / log information based communication