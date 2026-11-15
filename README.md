# KittyToken

Get Azure AD (Microsoft Entra ID) access tokens and inspect JWTs, either in a desktop app or from an AI assistant such as Claude Code.

## About

KittyToken gets access tokens with the OAuth 2.0 client credentials flow. It comes in two parts:

| Part | What it does |
|------|--------------|
| **Desktop app** | Save your app registrations as configurations, click Run and see the decoded token right away |
| **MCP server** | Lets Claude Code and other MCP clients get access tokens and manage saved configurations for you. See the [MCP server README](src/KittyToken.McpServer/README.md) |

## Features

### Desktop app

- Get access tokens with the client credentials flow (Instance, Tenant Id, Client Id, Client Secret, Scope)
- Save multiple configurations, then rename or delete them
- Decode any JWT and view its header, payload and signature
- Switch between the encoded and decoded token view and copy the token

### MCP server

- Get access tokens with the client credentials flow, directly or from a saved configuration
- Create, update, rename and delete configurations, shared with the desktop app
- Decode JWTs
- Single self-contained exe, no .NET install needed
- Registered in Claude Code with one `claude mcp add` command

## Requirements

- Windows 10 version 1809 or later (x64)
- To build: .NET 10 SDK, and the .NET MAUI workload for the desktop app (`dotnet workload install maui`)

## Tech Stack

- **Desktop UI:** .NET MAUI with CommunityToolkit.Maui
- **MCP server:** [ModelContextProtocol C# SDK](https://csharp.sdk.modelcontextprotocol.io/) over stdio
- **Framework:** .NET 10
- **Storage:** SQLite via Entity Framework Core

## Project Structure

| Project | Description |
|---------|-------------|
| `src/KittyToken.Application` | Shared logic: token requests, JWT decoding, storage |
| `src/KittyToken.Maui` | Desktop app |
| `src/KittyToken.McpServer` | MCP server |

## Build

### Desktop app: portable release (x64)

```powershell
dotnet publish src/KittyToken.Maui/KittyToken.Maui.csproj -f net10.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win-x64 -p:WindowsPackageType=None -p:WindowsAppSDKSelfContained=true -p:PublishSingleFile=true -p:DebugType=None -p:DebugSymbols=false
```

Output is in `src/KittyToken.Maui/bin/Release/net10.0-windows10.0.19041.0/win-x64/publish`.

### MCP server

```powershell
dotnet publish src/KittyToken.McpServer/KittyToken.McpServer.csproj -c Release -r win-x64 -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false
```

Output is a single `KittyToken.McpServer.exe` in `src/KittyToken.McpServer/bin/Release/net10.0/win-x64/publish`. Copy it to a permanent location and register it:

```powershell
claude mcp add --scope user kittytoken -- "C:\Tools\KittyToken\KittyToken.McpServer.exe"
```

The [MCP server README](src/KittyToken.McpServer/README.md) covers scopes, checking the connection, updating and running from source.

## Data

Configurations are stored locally in `%LOCALAPPDATA%\KittyToken.sqlite3`.

> [!IMPORTANT]
> Client secrets are saved unencrypted, so keep this file private.

> [!WARNING]
> With the MCP server, client secrets become part of the AI conversation: when you pass them to a tool, and when the server returns saved configurations.
