# KittyToken MCP Server

An [MCP](https://modelcontextprotocol.io/) server that lets AI assistants like Claude Code get Azure AD (Microsoft Entra ID) access tokens and manage saved Azure AD configurations.

## About

The server runs over stdio and uses the same token logic as the KittyToken desktop app. It is a self-contained single-file exe, so the target machine does not need .NET installed.

Configurations are stored in the same database as the desktop app (`%LOCALAPPDATA%\KittyToken.sqlite3`), so a configuration created by the AI shows up in the app and the other way round. The app loads configurations only when it starts, so restart it to see changes made by the AI.

## Tools

### Access tokens

| Tool | Description | Parameters |
|------|-------------|------------|
| `get_access_token` | Gets an access token with the OAuth 2.0 client credentials flow | `instance`, `tenantId`, `clientId`, `clientSecret`, `scope` |
| `get_access_token_by_configuration` | Gets an access token using a saved configuration | `configurationId` |
| `decode_jwt` | Decodes a JWT into its header and payload, without validating the signature | `jwtToken` |

### Configurations

| Tool | Description | Parameters |
|------|-------------|------------|
| `get_configurations` | Lists all saved configurations, including their IDs | – |
| `create_configuration` | Creates a configuration and saves it to the database. Returns it with its new ID | `name`, `instance`, `tenantId`, `clientId`, `clientSecret`, `scope` |
| `update_configuration` | Overwrites all values of a saved configuration (not its name) | `configurationId`, `instance`, `tenantId`, `clientId`, `clientSecret`, `scope` |
| `rename_configuration` | Renames a saved configuration | `configurationId`, `name` |
| `delete_configuration` | Deletes a saved configuration | `configurationId` |

You don't need to look up IDs yourself. Refer to a configuration by its name, and the AI looks up the ID with `get_configurations`.

## Prompts

Prompts appear in Claude Code as slash commands.

| Prompt | Slash command | Description | Arguments |
|--------|---------------|-------------|-----------|
| `get_encoded_token` | `/mcp__kittytoken__get_encoded_token` | Gets a token and prints only the encoded token, ready to copy into an API call. If you then ask, it shows the decoded header and payload | `instance`, `tenantId`, `clientId`, `clientSecret`, `scope` |
| `get_encoded_token_by_configuration` | `/mcp__kittytoken__get_encoded_token_by_configuration` | Same as `get_encoded_token`, using a saved configuration | `configurationName` |
| `get_encoded_token_from_project` | `/mcp__kittytoken__get_encoded_token_from_project` | Same as `get_encoded_token`, using the Azure AD config found in the current project (`appsettings*.json`, user secrets) | – |

Examples:

- `/mcp__kittytoken__get_encoded_token_by_configuration "<configuration-name>"`
- `/mcp__kittytoken__get_encoded_token https://login.microsoftonline.com/ <tenant-id> <client-id> <secret> api://<app-id>/.default`

> [!WARNING]
> Client secrets become part of the AI conversation when you pass them to a tool or the `get_encoded_token` prompt, when `get_configurations` returns them, and when `get_encoded_token_from_project` reads them from the project. Use the server only with secrets you are allowed to share with your AI provider.

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

To skip the approval prompt on every tool call, add `mcp__kittytoken__*` to `permissions.allow` in your Claude Code settings.

To remove the server:

```powershell
claude mcp remove kittytoken --scope user
```

### Updating

Close Claude Code (it keeps the exe locked while running), publish again and overwrite the exe in the same location.s

## Run from source

For development, register the project with `dotnet run` instead of the published exe:

```powershell
claude mcp add kittytoken-dev -- dotnet run --project "<PATH TO REPO>\src\KittyToken.McpServer"
```

The server logs to stderr, because stdout carries the MCP messages.
