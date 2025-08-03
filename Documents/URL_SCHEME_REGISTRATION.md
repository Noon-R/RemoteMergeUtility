# URL Scheme Registration Guide

## Overview
This document provides instructions for registering the custom URL scheme `mergeutil://` to enable the RemoteMergeUtility application to handle URLs from browsers and other applications.

## URL Scheme Format
```
mergeutil://?target=<ProjectKey>&revision=<Number>&args=<OptionalString>
```

### Parameters
- **target**: ProjectInfo key that specifies which project to use (required)
- **revision**: Natural number for version/revision specification (required, > 0)
- **args**: Optional string parameter for additional arguments

### Example URLs
```
mergeutil://?target=Default&revision=1
mergeutil://?target=MyProject&revision=123&args=--verbose
mergeutil://?target=Default&revision=5&args=debug-mode
```

## Windows Registry Registration

### Manual Registration
Create a `.reg` file with the following content and execute it as Administrator:

```registry
Windows Registry Editor Version 5.00

[HKEY_CLASSES_ROOT\mergeutil]
@="URL:Remote Merge Utility Protocol"
"URL Protocol"=""

[HKEY_CLASSES_ROOT\mergeutil\DefaultIcon]
@="\"C:\\Path\\To\\RemoteMergeUtility.exe\",1"

[HKEY_CLASSES_ROOT\mergeutil\shell]

[HKEY_CLASSES_ROOT\mergeutil\shell\open]

[HKEY_CLASSES_ROOT\mergeutil\shell\open\command]
@="\"C:\\Path\\To\\RemoteMergeUtility.exe\" \"%1\""
```

**Important**: Replace `C:\\Path\\To\\RemoteMergeUtility.exe` with the actual installation path of your application.

### PowerShell Registration Script
```powershell
# Run as Administrator
$exePath = "C:\Path\To\RemoteMergeUtility.exe"
$protocolName = "mergeutil"

# Create registry keys
New-Item -Path "HKCR:\$protocolName" -Force
Set-ItemProperty -Path "HKCR:\$protocolName" -Name "(Default)" -Value "URL:Remote Merge Utility Protocol"
Set-ItemProperty -Path "HKCR:\$protocolName" -Name "URL Protocol" -Value ""

New-Item -Path "HKCR:\$protocolName\DefaultIcon" -Force
Set-ItemProperty -Path "HKCR:\$protocolName\DefaultIcon" -Name "(Default)" -Value "`"$exePath`",1"

New-Item -Path "HKCR:\$protocolName\shell\open\command" -Force
Set-ItemProperty -Path "HKCR:\$protocolName\shell\open\command" -Name "(Default)" -Value "`"$exePath`" `"%1`""

Write-Host "URL scheme registered successfully!"
```

## Testing the Registration

### Test from Command Line
```cmd
start mergeutil://?target=Default&revision=1
```

### Test from Browser
Navigate to any of these URLs in your browser:
- `mergeutil://?target=Default&revision=1`
- `mergeutil://?target=MyProject&revision=123&args=test`

### Test from PowerShell
```powershell
Start-Process "mergeutil://?target=Default&revision=1"
```

## Application Behavior
When a `mergeutil://` URL is clicked:

1. Windows launches RemoteMergeUtility.exe with the URL as a command line argument
2. The application parses the URL parameters using `UrlSchemeService`
3. The application locates the ProjectInfo with the matching `target` key
4. The application processes the `revision` and `args` parameters as needed
5. A debug message box is currently shown (will be replaced with actual functionality)

## Troubleshooting

### Registration Issues
- Ensure you run the registration script as Administrator
- Verify the executable path is correct and accessible
- Check that the registry entries were created under `HKEY_CLASSES_ROOT\mergeutil`

### URL Not Working
- Test with a simple URL first: `mergeutil://?target=Default&revision=1`
- Check Windows Event Viewer for application errors
- Verify the application starts correctly from command line with the URL as an argument

### Debugging
- The application currently shows a debug message box when receiving URL scheme requests
- Check the message box content to verify parameter parsing
- Use Task Manager to confirm the application is starting when URLs are clicked

## Security Considerations
- Only accept URLs with the expected `mergeutil://` scheme
- Validate all parameters before processing
- Ensure the `target` parameter matches an existing ProjectInfo key
- Sanitize the `args` parameter if used for shell command execution