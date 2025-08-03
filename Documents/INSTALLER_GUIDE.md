# RemoteMergeUtility Installer Guide

## Overview

This document provides comprehensive information about the RemoteMergeUtility installer creation and deployment process.

## Prerequisites

### Required Software
- **WiX Toolset v3.11 or later**
  - Download: https://wixtoolset.org/releases/
  - Required for building MSI installers
- **.NET 8.0 SDK**
  - Required for building the application
- **Visual Studio 2022** (recommended)
  - For development and debugging

## Installer Features

### Installation Capabilities
- **Custom Installation Directory**: Users can specify installation location
- **Automatic Registry Configuration**: URL scheme registration during installation
- **Start Menu Integration**: Application shortcut creation
- **Dependency Management**: All required DLLs and configuration files
- **Clean Uninstallation**: Complete removal including registry entries

### URL Scheme Registration
The installer automatically registers the custom URL scheme:
- **Main Protocol**: `RemoteMergeUtility` protocol handler
- **URL Redirect**: `mergeutil://` redirects to RemoteMergeUtility protocol
- **Registry Locations**:
  - `HKEY_CLASSES_ROOT\RemoteMergeUtility`
  - `HKEY_CLASSES_ROOT\mergeutil`
  - `HKEY_LOCAL_MACHINE\SOFTWARE\RemoteMergeUtility`

## Building the Installer

### Quick Build
```batch
# Run the build script
BuildInstaller.bat
```

### Manual Build Process
```batch
# 1. Build application in Release mode
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln -c Release

# 2. Navigate to installer directory
cd Installer

# 3. Compile WiX source
candle -dSolutionDir="..\\" Product.wxs

# 4. Generate MSI
light -ext WixUIExtension -ext WixUtilExtension Product.wixobj -o RemoteMergeUtility.Installer.msi
```

## Installer Components

### File Structure
```
Installer/
├── RemoteMergeUtility.Installer.wixproj    # WiX project file
├── Product.wxs                             # Main installer definition
├── License.rtf                             # License agreement text
└── RemoteMergeUtility.Installer.msi        # Generated installer (after build)
```

### Installed Components
1. **Main Application**
   - RemoteMergeUtility.exe
   - All dependency DLLs
   - Configuration files

2. **Scripts**
   - RegisterUrlScheme.bat
   - UnregisterUrlScheme.bat
   - TestUrlScheme.bat

3. **Registry Entries**
   - URL scheme registration
   - Application information
   - Installation metadata

4. **Start Menu**
   - Application shortcut
   - Proper working directory

## Installation Process

### User Experience
1. **Welcome Screen**: Introduction and overview
2. **License Agreement**: MIT-style license acceptance
3. **Installation Directory**: Customizable install location (default: Program Files)
4. **Installation Progress**: Real-time progress with component details
5. **Completion**: Success confirmation with launch option

### Silent Installation
```batch
# Silent install to default location
msiexec /i RemoteMergeUtility.Installer.msi /quiet

# Silent install to custom location
msiexec /i RemoteMergeUtility.Installer.msi /quiet INSTALLFOLDER="C:\MyApps\RemoteMergeUtility"
```

## Registry Configuration Details

### URL Scheme Registration
```registry
[HKEY_CLASSES_ROOT\RemoteMergeUtility]
@="URL:RemoteMergeUtility Protocol"
"URL Protocol"=""

[HKEY_CLASSES_ROOT\RemoteMergeUtility\DefaultIcon]
@="\"C:\Program Files\RemoteMergeUtility\RemoteMergeUtility.exe\",1"

[HKEY_CLASSES_ROOT\RemoteMergeUtility\shell\open\command]
@="\"C:\Program Files\RemoteMergeUtility\RemoteMergeUtility.exe\" \"%1\""

[HKEY_CLASSES_ROOT\mergeutil]
@="RemoteMergeUtility"
```

### Application Information
```registry
[HKEY_LOCAL_MACHINE\SOFTWARE\RemoteMergeUtility]
"InstallPath"="C:\Program Files\RemoteMergeUtility\"
"Version"="1.0.0.0"
"DisplayName"="RemoteMergeUtility"
```

## Uninstallation

### Automatic Cleanup
The installer provides complete uninstallation:
- Removes all installed files
- Cleans up registry entries
- Removes Start Menu shortcuts
- Executes UnregisterUrlScheme.bat during uninstall

### Manual Uninstallation
```batch
# Via Control Panel
# Programs and Features > RemoteMergeUtility > Uninstall

# Via Command Line
msiexec /x RemoteMergeUtility.Installer.msi

# Silent uninstall
msiexec /x RemoteMergeUtility.Installer.msi /quiet
```

## Troubleshooting

### Build Issues
1. **WiX Toolset Not Found**
   - Install WiX Toolset v3.11+
   - Ensure PATH includes WiX bin directory

2. **Missing Dependencies**
   - Build application in Release mode first
   - Verify all DLL files exist in bin\Release\net8.0-windows\

3. **Registry Access Issues**
   - Run installer as Administrator
   - Check Windows UAC settings

### Installation Issues
1. **Permission Denied**
   - Run installer as Administrator
   - Check target directory permissions

2. **URL Scheme Not Working**
   - Verify registry entries were created
   - Test with Scripts\TestUrlScheme.bat

## Customization

### Modifying Installation Directory
Edit Product.wxs:
```xml
<Property Id="WIXUI_INSTALLDIR" Value="INSTALLFOLDER" />
```

### Adding Custom Actions
```xml
<CustomAction Id="MyCustomAction" 
              ExeCommand="MyCommand.exe"
              Directory="INSTALLFOLDER"
              Execute="deferred"
              Impersonate="no" />
```

### Changing Product Information
Update Product.wxs header:
```xml
<Product Id="*" 
         Name="RemoteMergeUtility" 
         Version="1.0.0.0" 
         Manufacturer="Your Company Name" 
         UpgradeCode="YOUR-UPGRADE-CODE">
```

## Security Considerations

### URL Scheme Security
- Custom URL schemes can be security vectors
- Validate all incoming parameters
- Consider implementing user confirmation for sensitive operations

### Installation Security
- Installer runs with elevated privileges
- Registry modifications require Administrator access
- File system permissions are set appropriately

## Deployment

### Distribution
- MSI file can be distributed directly
- Consider code signing for production release
- Test on clean Windows installations

### Group Policy Deployment
- MSI supports enterprise deployment via Group Policy
- Silent installation parameters available
- Centralized configuration possible

---

**Generated with [Claude Code](https://claude.ai/code)**