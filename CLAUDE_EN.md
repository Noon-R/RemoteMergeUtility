# CLAUDE.md (English Version)

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

**IMPORTANT**: Please respond in Japanese language for development guidance.

## Project Overview

RemoteMergeUtility is a .NET 8.0-based WPF application using strict MVVM architectural patterns. This is a Windows-specific desktop utility application that follows rigorous MVVM coding standards.

This tool executes shell commands based on arguments passed through custom URL schemes.
The parameters include the target project Key, Revision value, and optional additional arguments.
The application maintains Key-to-ProjectPath mappings.
This data is persisted to files to prevent data loss.

## Detailed Project Progress

### Project Progress Report
**[Documents/DEVELOPMENT_STATUS.md](Documents/DEVELOPMENT_STATUS.md)** MUST be referenced for comprehensive details.

This file contains detailed information including:
- Detailed list of completed features
- Technology stack configuration
- Complete project structure overview
- URL scheme processing flow details
- Build configuration behavioral differences
- Next features to implement
- Development and testing command reference

### Current Implementation Status
- Complete: Basic MVVM structure
- Complete: ProjectInfo management functionality
- Complete: URL scheme handler
- Complete: External tool integration
- Complete: Debug/Release environment switching
- Pending: Enhanced error handling
- Pending: Configuration management

## Build and Development Commands

### Build and Execution
```bash
# Build solution
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln

# Run application in debug mode
dotnet run --project RemoteMergeUtility/RemoteMergeUtility/RemoteMergeUtility.csproj

# Build release version
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln -c Release
```

### URL Scheme Testing
```bash
# Register URL scheme (run as Administrator)
Scripts/RegisterUrlScheme.bat

# Test URL scheme
Scripts/TestUrlScheme.bat

# Unregister URL scheme (run as Administrator)
Scripts/UnregisterUrlScheme.bat
```

## Architecture and MVVM Standards

This project follows strict WPF MVVM coding standards defined in `wpf_mvvm_coding_standards.md`. Key architectural principles:

### Folder Structure (Implemented)
- `Models/` - Data models and business logic
- `ViewModels/` - ViewModels implementing INotifyPropertyChanged
- `Views/` - XAML views with minimal code-behind
- `Services/` - External services and data access

### Core MVVM Requirements
- **View** must not contain business logic - data binding and minimal UI logic only
- **ViewModel** must not directly reference Views - use data binding and commands
- **Model** implements INotifyPropertyChanged for UI updates
- Use PascalCase for classes, properties, and methods
- Use `_` prefixed UpperCamelCase for private fields
- **readonly fields and constants** use `_UPPER_SNAKE_CASE` (with `_` prefix for private)
- All UI operations use Command binding instead of event handlers

### Installed Libraries
- **LivetCask (4.0.2)** - Messaging, Behavior, MVVM support (Japanese-made)
- **CommunityToolkit.Mvvm (8.4.0)** - Modern MVVM framework (Microsoft official)
- **Microsoft.Extensions.DependencyInjection (9.0.7)** - Dependency injection container
- **Serilog.Extensions.Hosting (9.0.0)** - Structured logging
- **MaterialDesignThemes (5.2.1)** - Material Design UI components
- **System.Text.Json** - JSON serialization (.NET standard)

### Data Binding Patterns
```xml
<!-- Proper two-way binding -->
<TextBox Text="{Binding PropertyName, UpdateSourceTrigger=PropertyChanged}" />

<!-- Command binding -->
<Button Command="{Binding SaveCommand}" />

<!-- Converter usage -->
<TextBlock Visibility="{Binding IsVisible, Converter={StaticResource BoolToVisibilityConverter}}" />
```

## Development Guidelines

1. **MVVM Compliance**: All new features must follow strict MVVM principles
2. **Async Operations**: Use async/await for potentially blocking operations
3. **Error Handling**: Implement try-catch blocks with user-friendly error messages
4. **Validation**: Use IDataErrorInfo for model validation
5. **Dependency Injection**: Abstract services through interfaces for testability

## External Tool Integration Specifications

### Virtual External Tool Commands
- `launch -project-list` - Get ProjectPath and name mappings
- `launch -name {name}` - Get project launch status
- `launch -project {projectName}` - Execute project commands

### Build Configuration Switching
- **Debug Build**: Uses MockLaunchToolService (no Process execution)
- **Release Build**: Uses LaunchToolService (actual external tool integration)

## Project Status

Currently completed through URL Scheme Handler & External Tool Integration.

### Next Implementation Plans
1. Enhanced error handling
2. Configuration management functionality
3. UI feature expansion

For detailed progress status, refer to **[Documents/DEVELOPMENT_STATUS.md](Documents/DEVELOPMENT_STATUS.md)**.