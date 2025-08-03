# RemoteMergeUtility

A WPF desktop application for managing project keys and paths with custom URL scheme integration for shell command execution.

## Features

- **Project Management**: Manage key-to-path mappings for development projects
- **Editable Grid Interface**: Real-time editing of project configurations via DataGrid
- **Persistent Storage**: Automatic save/load functionality with JSON persistence
- **MVVM Architecture**: Clean separation of concerns with strict MVVM implementation
- **Modern UI**: Material Design components for contemporary user experience

## Technology Stack

- **.NET 8.0** - Modern cross-platform framework
- **WPF** - Rich desktop application framework
- **MVVM Pattern** - Clean architecture with data binding
- **LivetCask** - Japanese-made MVVM messaging framework
- **CommunityToolkit.Mvvm** - Microsoft's modern MVVM toolkit
- **MaterialDesignThemes** - Material Design UI components
- **Serilog** - Structured logging framework

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Windows OS (WPF requirement)
- Visual Studio 2022 or VS Code (recommended)

### Building the Application

```bash
# Clone the repository
git clone https://github.com/yourusername/RemoteMergeUtility.git
cd RemoteMergeUtility

# Build the solution
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln

# Run the application
dotnet run --project RemoteMergeUtility/RemoteMergeUtility/RemoteMergeUtility.csproj
```

### Development Commands

```bash
# Debug build
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln -c Debug

# Release build
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln -c Release

# Clean build artifacts
dotnet clean RemoteMergeUtility/RemoteMergeUtility.sln
```

## Project Structure

```
RemoteMergeUtility/
├── Documents/					# Project documentation
├── RemoteMergeUtility/		  # Main solution
━E  └── RemoteMergeUtility/	  # Application project
━E	  ├── Models/			  # Data models
━E	  ├── ViewModels/		  # MVVM view models
━E	  ├── Views/			   # XAML user interfaces
━E	  └── Services/			# Business services
├── .gitignore				   # Git ignore configuration
├── CLAUDE.md					# Development guidance (Japanese)
└── README.md					# This file
```

## Usage

1. **Launch the application**
2. **Add projects** using the "追加" (Add) button
3. **Edit project details** directly in the DataGrid cells:
   - **Key**: Unique identifier for the project
   - **Path**: File system path to the project directory
4. **Remove projects** by selecting a row and clicking "削除" (Delete)
5. **Data persistence**: Projects are automatically saved on application exit

## Configuration

Project data is stored in:
```
%AppData%/RemoteMergeUtility/projects.json
```

## Architecture

This application follows strict **MVVM (Model-View-ViewModel)** principles:

- **Models**: Data structures with change notification (`ProjectInfo`)
- **ViewModels**: Presentation logic and commands (`ProjectManagerViewModel`)
- **Views**: XAML-based user interfaces (`ProjectManagerView`)
- **Services**: Data access and external integrations (`JsonProjectDataService`)

### Key Design Patterns

- **Command Pattern**: All UI interactions use `ICommand` implementations
- **Observer Pattern**: `INotifyPropertyChanged` for data binding
- **Repository Pattern**: Service interfaces for data access abstraction
- **Dependency Injection**: Service lifetime management

## Roadmap

### Next Milestones

- [ ] **URL Scheme Handler**: Register and process custom URL schemes
- [ ] **Shell Command Execution**: Execute commands with project path resolution
- [ ] **Enhanced Validation**: Path existence checking and validation
- [ ] **Import/Export**: Configuration backup and sharing
- [ ] **Logging Integration**: Comprehensive error tracking with Serilog

### Future Features

- [ ] Project grouping and categorization
- [ ] Git integration for repository management
- [ ] Visual Studio integration
- [ ] Multi-language UI support
- [ ] Themes and customization options

## Contributing

This project follows Japanese coding standards as documented in `wpf_mvvm_coding_standards.md`. Key conventions:

- **Classes/Properties/Methods**: `PascalCase`
- **Private fields**: `_UpperCamelCase`
- **Readonly/Constants**: `_UPPER_SNAKE_CASE`
- **Strict MVVM**: No business logic in Views
- **Command binding**: No event handlers in code-behind

## License

This project is developed with Claude Code assistance and follows modern .NET development practices.

## Documentation

For detailed development information, see:
- [`Documents/DEVELOPMENT_STATUS.md`](Documents/DEVELOPMENT_STATUS.md) - Comprehensive development status
- [`CLAUDE.md`](CLAUDE.md) - Claude Code integration guidance (Japanese)
- [`wpf_mvvm_coding_standards.md`](wpf_mvvm_coding_standards.md) - Coding standards (Japanese)

---

**Generated with [Claude Code](https://claude.ai/code)**