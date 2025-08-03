# RemoteMergeUtility - Development Status

## Project Overview

**RemoteMergeUtility** is a WPF desktop application built with strict MVVM architecture for managing project keys and paths. The application is designed to handle custom URL scheme arguments and execute shell commands based on project configurations.

### Core Purpose
- Manage project key-to-path mappings
- Accept parameters via custom URL schemes (project key, revision, additional arguments)
- Execute shell commands with resolved project paths
- Persistent data storage to maintain project configurations across sessions

## Current Development Status

### ✁ECompleted Features

#### 1. **Project Architecture & Setup**
- ✁E.NET 8.0 WPF application structure
- ✁EStrict MVVM pattern implementation
- ✁EComprehensive folder structure (Models, ViewModels, Views, Services)
- ✁EModern library integration and dependency management

#### 2. **Data Management System**
- ✁E`ProjectInfo` model with INotifyPropertyChanged implementation
- ✁EJSON-based persistence service (`JsonProjectDataService`)
- ✁EAutomatic data loading on application startup
- ✁EAutomatic data saving on application shutdown
- ✁EData stored in `%AppData%/RemoteMergeUtility/projects.json`

#### 3. **User Interface Implementation**
- ✁EMain project management window
- ✁EEditable DataGrid for project key-path pairs
- ✁EAdd/Remove project functionality
- ✁EReal-time cell editing capabilities
- ✁EProper data binding with UpdateSourceTrigger=PropertyChanged

#### 4. **MVVM Implementation**
- ✁E`ViewModelBase` with proper property change notifications
- ✁E`RelayCommand` implementation for command binding
- ✁E`ProjectManagerViewModel` with full data management
- ✁EComplete separation of concerns (View, ViewModel, Model)
- ✁ENo code-behind business logic

#### 5. **Development Environment**
- ✁EGit repository initialization with comprehensive .gitignore
- ✁EInitial commit with proper project structure
- ✁ECoding standards documentation (Japanese)
- ✁EClaude Code integration documentation

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

## Code Quality & Standards

### **Naming Conventions**
- **Classes, Properties, Methods**: PascalCase
- **Private fields**: `_UpperCamelCase`
- **Readonly fields and constants**: `_UPPER_SNAKE_CASE`
- **All UI interactions**: Command binding (no event handlers)

### **Architecture Compliance**
- ✁EViews contain no business logic
- ✁EViewModels never reference Views directly
- ✁EModels implement INotifyPropertyChanged for UI updates
- ✁EAsync operations for potentially blocking calls
- ✁EProper error handling with user-friendly messages

## Project Structure

```
RemoteMergeUtility/
├── Documents/					# Project documentation
├── RemoteMergeUtility/		  # Main solution folder
━E  ├── RemoteMergeUtility.sln   # Visual Studio solution
━E  └── RemoteMergeUtility/	  # Main project
━E	  ├── Models/			  # Data models and business logic
━E	  ━E  └── ProjectInfo.cs   # Project key-path model
━E	  ├── ViewModels/		  # MVVM view models
━E	  ━E  ├── ViewModelBase.cs		# Base class for ViewModels
━E	  ━E  ├── RelayCommand.cs		 # Command implementation
━E	  ━E  └── ProjectManagerViewModel.cs # Main VM
━E	  ├── Views/			   # XAML user interfaces
━E	  ━E  └── ProjectManagerView.xaml # Main project management UI
━E	  ├── Services/			# External services and data access
━E	  ━E  ├── IProjectDataService.cs	 # Data service interface
━E	  ━E  └── JsonProjectDataService.cs  # JSON persistence implementation
━E	  ├── App.xaml			 # Application entry point
━E	  ├── MainWindow.xaml	  # Main application window
━E	  └── RemoteMergeUtility.csproj # Project configuration
├── .gitignore				   # Git ignore rules for .NET
├── CLAUDE.md					# Claude Code development guidance (Japanese)
└── wpf_mvvm_coding_standards.md # Detailed coding standards (Japanese)
```

## Pending Development Areas

### 🔧 **Next Priority Features**

#### 1. **URL Scheme Handler Implementation**
- Register custom URL scheme for the application
- Parse incoming URL parameters (project key, revision, arguments)
- Command-line argument processing

#### 2. **Shell Command Execution**
- Command template system with parameter substitution
- Process execution with proper error handling
- Working directory management based on project paths

#### 3. **Enhanced UI Features**
- Project validation (path existence checking)
- Import/Export functionality for project configurations
- Search and filtering capabilities
- Keyboard shortcuts for common operations

#### 4. **Error Handling & Logging**
- Comprehensive error logging with Serilog
- User-friendly error dialogs
- Application crash recovery

#### 5. **Configuration Management**
- Application settings persistence
- Command template customization
- UI theme and appearance options

### 🎯 **Future Enhancements**

#### 1. **Advanced Features**
- Project grouping and categorization
- Recent projects tracking
- Backup and restore functionality
- Multi-language support expansion

#### 2. **Integration Features**
- Git repository integration
- Visual Studio integration
- External tool integration capabilities

#### 3. **User Experience**
- Tooltips and help system
- Keyboard accessibility
- Drag-and-drop support for path selection

## Development Notes

### **Performance Considerations**
- ObservableCollection usage for real-time UI updates
- Async/await pattern for file I/O operations
- Efficient data binding with proper change notifications

### **Testing Strategy**
- Unit testing capabilities built into MVVM structure
- Dependency injection allows for easy mocking
- Service interfaces enable testable architecture

### **Deployment Preparation**
- Self-contained .NET deployment ready
- Minimal external dependencies
- Windows-specific optimizations

## Build & Development Commands

```bash
# Build the solution
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln

# Run in debug mode
dotnet run --project RemoteMergeUtility/RemoteMergeUtility/RemoteMergeUtility.csproj

# Build for release
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln -c Release
```

---

**Last Updated**: August 2025  
**Development Stage**: Core Infrastructure Complete, Feature Implementation Phase  
**Next Milestone**: URL Scheme Handler & Shell Command Execution