Monster Hunter Online - Server
===
Server emulator for the game DMonster Hunter Online.

# Disclaimer
The project is intended for educational purpose only.

# Client
All information is based on Version: `2.0.11.942`  
Launch the client with the following args:  
`"MHOClient.exe -qos_id=food -q -loginqq=1234567890123456789 -nosplash`

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

# Memory Maps

OpenFileMapping
```
HANDLE OpenFileMappingA(
[in] DWORD  dwDesiredAccess,
[in] BOOL   bInheritHandle,
[in] LPCSTR lpName
);
```

CreateFileMapping
```
HANDLE CreateFileMappingA(
  [in]           HANDLE                hFile,
  [in, optional] LPSECURITY_ATTRIBUTES lpFileMappingAttributes,
  [in]           DWORD                 flProtect,
  [in]           DWORD                 dwMaximumSizeHigh,
  [in]           DWORD                 dwMaximumSizeLow,
  [in, optional] LPCSTR                lpName
);
```

MapViewOfFile
```
LPVOID MapViewOfFile(
  [in] HANDLE hFileMappingObject,
  [in] DWORD  dwDesiredAccess,
  [in] DWORD  dwFileOffsetHigh,
  [in] DWORD  dwFileOffsetLow,
  [in] SIZE_T dwNumberOfBytesToMap
);
```

## mhfcclient
-> Created via `CreateFileMapping`  
Name: `mhfcclient`  
RVA: `crygame.dll+0xF9C83`

## mhfcclient
-> Opened via `OpenFileMappingA`  
Name: `mhfcclient`  
RVA: `crygame.dll+0x1072BB`

## GloablVarBuffer_PID_XXX
-> Created via `CreateFileMapping`  
Name: `Local\GloablVarBuffer_PID_12120`  
RVA: `MHOClient.exe+0x1657F6`

## Global\ReadBitMapInfos_XXX
-> Opened via `OpenFileMapping`  
Name:`Global\ReadBitMapInfos_053FA2C44ECDBDEDA543AD3C1266D886`  
RVA: `iips.dll+0x10422`