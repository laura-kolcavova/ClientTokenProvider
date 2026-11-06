# KittyToken

A desktop app for getting Azure AD (Microsoft Entra ID) access tokens and inspecting JWTs.

## About

KittyToken gets access tokens with the OAuth 2.0 client credentials flow. Save your app registrations as configurations, click Run and see the decoded token right away.

## Features

- Get access tokens with the client credentials flow (Instance, Tenant Id, Client Id, Client Secret, Scope)
- Save multiple configurations, then rename or delete them
- Decode any JWT and view its header, payload and signature
- Switch between the encoded and decoded token view and copy the token

## Requirements

- Windows 10 version 1809 or later

## Tech Stack

- **UI:** .NET MAUI with CommunityToolkit.Maui
- **Framework:** .NET 10
- **Storage:** SQLite via Entity Framework Core

## Data

Configurations are stored locally in `%LOCALAPPDATA%\KittyToken.sqlite3`.

> [!IMPORTANT]
> Client secrets are saved unencrypted, so keep this file private.
