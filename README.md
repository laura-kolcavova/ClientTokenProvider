# ClientTokenProvider

## Publish on Windows

Run the following command to publish the project on Windows:

```bash
dotnet publish ./src/ClientTokenProvider -f net8.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win-x64 -p:WindowsPackageType=None
```

or run run the following command to publish the self-contained project on Windows:

```bash
dotnet publish ./src/ClientTokenProvider -f net8.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win-x64 -p:WindowsPackageType=None -p:WindowsAppSDKSelfContained=true
```

Publishing builds the app, copying the executable to the _src\ClientTokenProvider\bin\Release\net8.0-windows10.0.19041.0\win10-x64\publish_ folder.

## TODO List:

- Localization
- Import Configuration
- Export Configuration
- Duplicate Configuration
- Drag&Drop Configuration List Items
- Tabs
