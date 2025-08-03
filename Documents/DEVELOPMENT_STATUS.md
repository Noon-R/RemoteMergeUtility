# RemoteMergeUtility - Development Status

## Project Overview

**RemoteMergeUtility** is a WPF desktop application built with strict MVVM architecture for managing project keys and paths. The application is designed to handle custom URL scheme arguments and execute shell commands based on project configurations with external tool integration.

### Core Purpose
- Manage project key-to-path mappings
- Accept parameters via custom URL schemes (project key, revision, additional arguments)
- Execute shell commands with resolved project paths through external tool integration
- Persistent data storage to maintain project configurations across sessions
- Seamless integration with virtual external tools for project lifecycle management

## Current Development Status

### Completed Features

#### 1. **Project Architecture & Setup**
- .NET 8.0 WPF application structure
- Strict MVVM pattern implementation
- Comprehensive folder structure (Models, ViewModels, Views, Services)
- Modern library integration and dependency management

#### 2. **Data Management System**
- `ProjectInfo` model with INotifyPropertyChanged implementation
- JSON-based persistence service (`JsonProjectDataService`)
- Automatic data loading on application startup
- Automatic data saving on application shutdown
- Data stored in `%AppData%/RemoteMergeUtility/projects.json`
- Immutable model design with constructor initialization
- JSON serialization with proper deserialization support

#### 3. **User Interface Implementation**
- Main project management window
- Editable DataGrid for project key-path pairs
- Add/Remove project functionality with Default project protection
- Real-time cell editing capabilities
- Proper data binding with UpdateSourceTrigger=PropertyChanged
- Special styling for non-deletable Default project

#### 4. **MVVM Implementation**
- `ViewModelBase` with proper property change notifications
- `RelayCommand` implementation for command binding
- `ProjectManagerViewModel` with full data management
- Complete separation of concerns (View, ViewModel, Model)
- No code-behind business logic
- UPPER_SNAKE_CASE naming for readonly fields and constants
- Explicit private field declarations (no backing fields)
- Command properties implemented with dedicated private readonly fields

#### 5. **URL Scheme Handler System**
- Custom URL scheme registration (`mergeutil://`)
- URL parameter parsing service (`UrlSchemeService`)
- Command-line argument processing in App.xaml.cs
- Registry installation/uninstallation batch scripts
- URL validation and error handling
- Test suite for URL scheme verification
- Registry key organization under RemoteMergeUtility

#### 6. **External Tool Integration**
- Virtual external tool service interface (`ILaunchToolService`)
- Mock implementation for debug builds (`MockLaunchToolService`)
- Real implementation for release builds (`LaunchToolService`)
- Project lifecycle management (launch/status checking)
- HTTP request sending for running projects
- Build configuration-based service selection
- Project name resolution from path mappings

#### 7. **Application Infrastructure**
- Git repository initialization with comprehensive .gitignore
- Initial commit with proper project structure
- Coding standards documentation (Japanese)
- Claude Code integration documentation
- Code formatting standardization (tabs instead of spaces)
- Comprehensive URL scheme registration documentation
- Automated testing scripts for URL scheme validation
- Single instance application control with mutex
- System tray integration for background operation
- Structured logging system with LogService wrapper
- Named pipe server for inter-process communication

## Technology Stack

### **Core Framework**
- .NET 8.0 (Windows)
- WPF (Windows Presentation Foundation)
- C# with nullable reference types enabled

### **MVVM & Libraries**
- **LivetCask (4.0.2)** - Messaging and MVVM behaviors (Japanese-made)
- **CommunityToolkit.Mvvm (8.4.0)** - Modern MVVM framework (Microsoft official)
- **Microsoft.Extensions.DependencyInjection (9.0.7)** - Dependency injection container
- **MaterialDesignThemes (5.2.1)** - Material Design UI components
- **Serilog.Extensions.Hosting (9.0.0)** - Structured logging framework

### **Data & Persistence**
- System.Text.Json - JSON serialization (.NET standard)
- File-based persistence in application data folder

### **External Integration**
- System.Diagnostics.Process - External command execution
- System.Net.Http - HTTP client for REST API communication
- Windows Registry - URL scheme registration

## Code Quality & Standards

### **Naming Conventions**
- **Classes, Properties, Methods**: PascalCase
- **Private fields**: `_UpperCamelCase`
- **Readonly fields and constants**: `_UPPER_SNAKE_CASE`
- **All UI interactions**: Command binding (no event handlers)

### **Architecture Compliance**
- Views contain no business logic
- ViewModels never reference Views directly
- Models implement INotifyPropertyChanged for UI updates
- Async operations for potentially blocking calls
- Proper error handling with user-friendly messages
- Service abstraction for testability and build configuration switching

## Project Structure

```
RemoteMergeUtility/
├── Documents/					# Project documentation
│   ├── DEVELOPMENT_STATUS.md			# This file - project progress summary
│   ├── INSTALLER_GUIDE.md			# Windows installer creation guide
│   └── URL_SCHEME_REGISTRATION.md		# URL scheme setup documentation
├── Scripts/					# Automation scripts
│   ├── RegisterUrlScheme.bat			# URL scheme registration
│   ├── UnregisterUrlScheme.bat		# URL scheme removal
│   └── TestUrlScheme.bat			# URL scheme testing
├── Installer/					# Windows installer configuration
│   ├── RemoteMergeUtility.Installer.wixproj	# WiX project file
│   ├── Product.wxs				# WiX installer definition
│   └── License.rtf				# License agreement for installer
├── BuildInstaller.bat				# Automated installer build script
├── RemoteMergeUtility/				# Main solution folder
│   ├── RemoteMergeUtility.sln			# Visual Studio solution
│   └── RemoteMergeUtility/			# Main project
│       ├── Models/				# Data models and business logic
│       │   ├── ProjectInfo.cs			# Project key-path model
│       │   └── UrlSchemeRequest.cs		# URL scheme parameter model
│       ├── ViewModels/				# MVVM view models
│       │   ├── ViewModelBase.cs		# Base class for ViewModels
│       │   ├── RelayCommand.cs			# Command implementation
│       │   └── ProjectManagerViewModel.cs	# Main VM with explicit fields
│       ├── Views/				# XAML user interfaces
│       │   └── ProjectManagerView.xaml	# Main project management UI
│       ├── Services/				# External services and data access
│       │   ├── IProjectDataService.cs		# Data service interface
│       │   ├── JsonProjectDataService.cs	# JSON persistence implementation
│       │   ├── IUrlSchemeService.cs		# URL parsing interface
│       │   ├── UrlSchemeService.cs		# URL parsing implementation
│       │   ├── ILaunchToolService.cs		# External tool interface
│       │   ├── LaunchToolService.cs		# Real external tool service
│       │   ├── MockLaunchToolService.cs	# Debug mock service
│       │   └── LogService.cs			# Structured logging wrapper
│       ├── App.xaml				# Application entry point with tray support
│       ├── MainWindow.xaml			# Main application window
│       └── RemoteMergeUtility.csproj		# Project configuration
├── .gitignore					# Git ignore rules for .NET
├── CLAUDE.md					# Claude Code development guidance (Japanese)
├── wpf_mvvm_coding_standards.md		# Detailed coding standards (Japanese)
└── README.md					# Project overview and setup instructions
```

## Current Workflow Integration

### **URL Scheme Processing Flow**
1. **URL Reception**: `mergeutil://?target=ProjectKey&revision=123&args=optional`
2. **Single Instance Check**: Mutex ensures only one application instance
3. **Parameter Parsing**: `UrlSchemeService` extracts and validates parameters
4. **Project Resolution**: App.xaml.cs loads ProjectInfo and resolves target path
5. **External Tool Query**: `ILaunchToolService` checks project status via virtual tool
6. **Conditional Execution**:
   - **If not running**: Launch project via external tool command
   - **If running**: Send HTTP POST request with revision/args data
7. **System Tray Operation**: Application runs in background after processing

### **Application Lifecycle**
- **Startup**: Single instance enforcement with named mutex
- **Background Operation**: System tray icon with context menu
- **Inter-Process Communication**: Named pipe server for URL scheme handling
- **Graceful Shutdown**: Proper resource cleanup and data persistence

### **Build Configuration Behavior**
- **Debug Build**: Uses `MockLaunchToolService` with simulated responses
- **Release Build**: Uses `LaunchToolService` with actual external tool integration

## Pending Development Areas

### **Next Priority Features**

#### 1. **Windows Installer Package**
- WiX Toolset v3-based MSI installer
- Automatic URL scheme registration during installation
- Start menu shortcuts and desktop integration
- Uninstall cleanup with registry key removal

#### 2. **Enhanced Error Handling**
- Comprehensive error logging with Serilog integration
- User-friendly error dialogs with retry mechanisms
- Application crash recovery and diagnostics

#### 3. **Configuration Management**
- Application settings persistence beyond project data
- External tool endpoint configuration
- UI theme and appearance options
- Logging level and output configuration

#### 4. **Advanced UI Features**
- Project validation (path existence checking)
- Import/Export functionality for project configurations
- Search and filtering capabilities
- Keyboard shortcuts for common operations

### **Future Enhancements**

#### 1. **Advanced Features**
- Project grouping and categorization
- Recent projects tracking
- Backup and restore functionality
- Multi-language support expansion

#### 2. **Integration Features**
- Git repository integration
- Visual Studio integration
- Extended external tool integration capabilities

#### 3. **User Experience**
- Tooltips and help system
- Keyboard accessibility
- Drag-and-drop support for path selection

## Development Notes

### **Performance Considerations**
- ObservableCollection usage for real-time UI updates
- Async/await pattern for file I/O and HTTP operations
- Efficient data binding with proper change notifications
- Service lifetime management with proper disposal

### **Testing Strategy**
- Unit testing capabilities built into MVVM structure
- Dependency injection allows for easy mocking
- Service interfaces enable testable architecture
- Mock services for development and testing

### **Deployment Preparation**
- Self-contained .NET deployment ready
- Registry-based URL scheme registration
- Windows-specific optimizations
- Automated installation scripts

## Build & Development Commands

```bash
# Build the solution
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln

# Run in debug mode (uses MockLaunchToolService)
dotnet run --project RemoteMergeUtility/RemoteMergeUtility/RemoteMergeUtility.csproj

# Build for release (uses LaunchToolService)
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln -c Release

# Build Windows installer (requires WiX Toolset v3.11+)
BuildInstaller.bat

# Test URL scheme (after registration)
Scripts/TestUrlScheme.bat
```

## Installation & Setup

```bash
# 1. Register URL scheme (run as Administrator)
Scripts/RegisterUrlScheme.bat

# 2. Test URL scheme functionality
Scripts/TestUrlScheme.bat

# 3. Unregister if needed (run as Administrator)
Scripts/UnregisterUrlScheme.bat
```

---

**Last Updated**: August 2025  
**Development Stage**: Core Application Infrastructure Complete  
**Next Milestone**: Windows Installer Package & Enhanced Error Handling  
**Status**: Production-ready core functionality with system tray integration and logging