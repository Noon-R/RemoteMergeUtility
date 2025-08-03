# WPF MVVM コーチE��ング規紁E
## 🎯 基本方釁E- **MVVMパターンの厳格な遵宁E*
- **保守性とチE��タビリチE��の向丁E*
- **個人開発での効玁E��を重要E*

---

## 📁 プロジェクト構�E

### フォルダ構造
```
RemoteMer/
├── Models/		   # チE�EタモチE��・ビジネスロジチE��
├── ViewModels/	   # ビューモチE��
├── Views/		   # XAML画面
├── Services/		# 外部サービス・チE�Eタアクセス
├── Converters/	  # 値コンバ�Eター
├── Commands/		# カスタムコマンチE├── Helpers/		 # ヘルパ�Eクラス
└── Resources/	   # リソース�E�画像�E辞書等！E```

### ✁EチェチE��ポインチE- [ ] 吁E��イヤーが適刁E��フォルダに配置されてぁE��
- [ ] Viewに直接ビジネスロジチE��が書かれてぁE��ぁE- [ ] ViewModelがViewに依存してぁE��ぁE
---

## 🏗�E�EMVVM実裁E��紁E
### Model
```csharp
// ✁EGood
public class User : INotifyPropertyChanged
{
	private string _Name;
	public string Name
	{
		get => _Name;
		set
		{
			_Name = value;
			OnPropertyChanged();
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
```

### ViewModel
```csharp
// ✁EGood
public class UserViewModel : ViewModelBase
{
	private User _User;
	private ICommand _SaveCommand;

	public User User
	{
		get => _User;
		set => SetProperty(ref _User, value);
	}

	public ICommand SaveCommand => 
		_SaveCommand ??= new RelayCommand(ExecuteSave, CanExecuteSave);

	private void ExecuteSave()
	{
		// 保存�E琁E	}

	private bool CanExecuteSave()
	{
		return User?.Name?.Length > 0;
	}
}
```

### View (XAML)
```xml
<!-- ✁EGood -->
<TextBox Text="{Binding User.Name, UpdateSourceTrigger=PropertyChanged}" />
<Button Content="保孁E Command="{Binding SaveCommand}" />

<!-- ❁EBad: コードビハインドでのイベント�E琁E-->
<Button Content="保孁E Click="SaveButton_Click" />
```

---

## 📝 命名規紁E
### クラス・プロパティ・メソチE��
- **PascalCase**を使用
- 意味のある名前を付けめE- ViewModelには`ViewModel`サフィチE��ス
- Viewには画面の目皁E��表す名剁E
```csharp
// ✁EGood
public class UserRegistrationViewModel { }
public class UserRegistrationView { }
public string FirstName { get; set; }
public void SaveUserData() { }

// ❁EBad
public class UserVM { }
public string fName { get; set; }
public void Save() { }
```

### フィールド�E変数
- **UpperCamelCase**を使用
- プライベ�Eトフィールド�E`_`プレフィチE��ス

```csharp
// ✁EGood
private string _UserName;
private readonly IUserService _UserService;

// ❁EBad
private string UserName;
private string _userName;
private string username;
```

---

## 🔗 チE�EタバインチE��ング

### 推奨パターン
```xml
<!-- ✁EGood: 双方向バインチE��ング -->
<TextBox Text="{Binding UserName, UpdateSourceTrigger=PropertyChanged}" />

<!-- ✁EGood: コマンドバインチE��ング -->
<Button Command="{Binding SaveCommand}" />

<!-- ✁EGood: コンバ�Eター使用 -->
<TextBlock Visibility="{Binding IsVisible, Converter={StaticResource BoolToVisibilityConverter}}" />
```

### ✁EチェチE��ポインチE- [ ] すべてのUI要素がViewModelにバインドされてぁE��
- [ ] UpdateSourceTriggerが適刁E��設定されてぁE��
- [ ] 型変換にはコンバ�Eターを使用してぁE��

---

## 🎨 XAML規紁E
### レイアウチE```xml
<!-- ✁EGood: 構造化されたレイアウチE-->
<Grid>
	<Grid.RowDefinitions>
		<RowDefinition Height="Auto"/>
		<RowDefinition Height="*"/>
		<RowDefinition Height="Auto"/>
	</Grid.RowDefinitions>
	
	<StackPanel Grid.Row="0" Orientation="Horizontal" Margin="10">
		<TextBlock Text="ユーザー吁E" VerticalAlignment="Center"/>
		<TextBox Text="{Binding UserName}" Width="200" Margin="5,0"/>
	</StackPanel>
</Grid>
```

### スタイル・リソース
```xml
<!-- ✁EGood: リソース辞書の活用 -->
<Window.Resources>
	<Style x:Key="PrimaryButtonStyle" TargetType="Button">
		<Setter Property="Background" Value="#007ACC"/>
		<Setter Property="Foreground" Value="White"/>
		<Setter Property="Padding" Value="10,5"/>
	</Style>
</Window.Resources>
```

---

## 🛠�E�E依存性注入・サービス

### サービスインターフェース
```csharp
// ✁EGood
public interface IUserService
{
	Task<User> GetUserAsync(int id);
	Task SaveUserAsync(User user);
}

public class UserService : IUserService
{
	// 実裁E}
```

### ViewModelでの使用
```csharp
// ✁EGood
public class UserViewModel : ViewModelBase
{
	private readonly IUserService _UserService;

	public UserViewModel(IUserService userService)
	{
		_UserService = userService ?? throw new ArgumentNullException(nameof(userService));
	}
}
```

---

## 🚨 エラーハンドリング

### 非同期�E琁E```csharp
// ✁EGood
private async void ExecuteLoadUser()
{
	try
	{
		IsLoading = true;
		User = await _UserService.GetUserAsync(UserId);
	}
	catch (Exception ex)
	{
		ErrorMessage = $"ユーザー読み込みエラー: {ex.Message}";
		// ログ出劁E	}
	finally
	{
		IsLoading = false;
	}
}
```

### バリチE�Eション
```csharp
// ✁EGood: IDataErrorInfoの実裁Epublic class User : INotifyPropertyChanged, IDataErrorInfo
{
	public string this[string columnName]
	{
		get
		{
			switch (columnName)
			{
				case nameof(Name):
					return string.IsNullOrEmpty(Name) ? "名前は忁E��でぁE : null;
				default:
					return null;
			}
		}
	}

	public string Error => null;
}
```

---

## 📋 コードレビューチェチE��リスチE
### MVVM遵宁E- [ ] ViewにビジネスロジチE��がなぁE- [ ] ViewModelがViewに依存してぁE��ぁE- [ ] ModelにUI関連のコードがなぁE
### パフォーマンス
- [ ] 重い処琁E�E非同期で実裁E- [ ] メモリリークの原因となるイベントハンドラーが適刁E��解除されてぁE��
- [ ] 不要なPropertyChangedイベントが発生してぁE��ぁE
### 保守性
- [ ] クラスの責任が単一である
- [ ] メソチE��が適刁E��長さ！E0行以冁E��奨�E�E- [ ] マジチE��ナンバ�Eが定数化されてぁE��

### チE��タビリチE��
- [ ] ViewModelが単体テスト可能
- [ ] 依存関係がインターフェースで抽象化されてぁE��
- [ ] コマンド�E実行条件が�E確

---

## 🔧 推奨チE�Eル・ライブラリ

### 忁E��E- **CommunityToolkit.Mvvm** (旧Microsoft.Toolkit.Mvvm)
- **System.ComponentModel.DataAnnotations** (バリチE�Eション)

### 推奨
- **Prism** (大規模アプリケーションの場吁E
- **MaterialDesignInXamlToolkit** (モダンUI)
- **Serilog** (ログ出劁E

### 設定侁E```xml
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.0" />
<PackageReference Include="MaterialDesignThemes" Version="4.9.0" />
```

---

## 📚 参老E��ンク

- [Microsoft WPF ガイドライン](https://docs.microsoft.com/ja-jp/dotnet/desktop/wpf/)
- [MVVM パターン詳細](https://docs.microsoft.com/ja-jp/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern)
- [CommunityToolkit.Mvvm ドキュメンチE(https://docs.microsoft.com/ja-jp/dotnet/communitytoolkit/mvvm/)

---

*最終更新: 2025年8朁E