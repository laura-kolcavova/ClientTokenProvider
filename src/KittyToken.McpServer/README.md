# KittyToken MCP Server

An [MCP](https://modelcontextprotocol.io/) server that lets AI assistants like Claude Code get Azure AD (Microsoft Entra ID) access tokens.

## About

The server runs over stdio and uses the same token logic as the KittyToken desktop app. It is a self-contained single-file exe, so the target machine does not need .NET installed.

## Tools

| Tool | Description | Parameters |
|------|-------------|------------|
| `get_access_token` | Gets an access token with the OAuth 2.0 client credentials flow | `instance`, `tenantId`, `clientId`, `clientSecret`, `scope` |

> [!WARNING]
> The client secret is passed as a tool argument, so it becomes part of the AI conversation. Use it only with secrets you are allowed to share with your AI provider.

## Requirements

- Windows 10 or later (x64)
- [Claude Code](https://docs.claude.com/en/docs/claude-code) or another MCP client

## Build

### Publish portable release (x64)

Requires the .NET 10 SDK.

```powershell
dotnet publish src/KittyToken.McpServer/KittyToken.McpServer.csproj -c Release -r win-x64 -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false
```

Output is a single `KittyToken.McpServer.exe` in `src/KittyToken.McpServer/bin/Release/net10.0/win-x64/publish`.

Copy it to a permanent location, for example `C:\Tools\KittyToken\KittyToken.McpServer.exe`. The MCP client starts the exe from that path, so it must not move afterwards.

> [!TIP]
> For other platforms, replace `win-x64` with one of the runtime identifiers listed in the `.csproj` (e.g. `win-arm64`, `osx-arm64`, `linux-x64`).

## Register in Claude Code

Add the server for your user, so it is available in every project:

```powershell
claude mcp add --scope user kittytoken -- "C:\Tools\KittyToken\KittyToken.McpServer.exe"
```

| Scope | Where it is available |
|-------|-----------------------|
| `local` (default) | Only you, only in the current project |
| `user` | Only you, in all projects |
| `project` | Everyone in the repo, through a `.mcp.json` file committed to it |

Check that it connected:

```powershell
claude mcp list
```

You should see `kittytoken: ... - ✓ Connected`. Inside a Claude Code session, `/mcp` shows the server and its tools.

Then just ask, for example:

> Get me an access token for tenant `<tenant-id>`, client `<client-id>`, secret `<secret>` and scope `api://<app-id>/.default`.

To remove the server:

```powershell
claude mcp remove kittytoken --scope user
```

### Updating

Close Claude Code (it keeps the exe locked while running), publish again and overwrite the exe in the same location.

## Run from source

For development, register the project with `dotnet run` instead of the published exe:

```powershell
claude mcp add kittytoken-dev -- dotnet run --project "<PATH TO REPO>\src\KittyToken.McpServer"
```

The server logs to stderr, because stdout carries the MCP messages.
