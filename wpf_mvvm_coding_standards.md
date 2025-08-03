# WPF MVVM コーディング規約

## 🎯 基本方針
- **MVVMパターンの厳格な遵守**
- **保守性とテスタビリティの向上**
- **個人開発での効率性を重視**

---

## 📁 プロジェクト構成

### フォルダ構造
```
RemoteMer/
├── Models/           # データモデル・ビジネスロジック
├── ViewModels/       # ビューモデル
├── Views/           # XAML画面
├── Services/        # 外部サービス・データアクセス
├── Converters/      # 値コンバーター
├── Commands/        # カスタムコマンド
├── Helpers/         # ヘルパークラス
└── Resources/       # リソース（画像・辞書等）
```

### ✅ チェックポイント
- [ ] 各レイヤーが適切なフォルダに配置されている
- [ ] Viewに直接ビジネスロジックが書かれていない
- [ ] ViewModelがViewに依存していない

---

## 🏗️ MVVM実装規約

### Model
```csharp
// ✅ Good
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
// ✅ Good
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
        // 保存処理
    }

    private bool CanExecuteSave()
    {
        return User?.Name?.Length > 0;
    }
}
```

### View (XAML)
```xml
<!-- ✅ Good -->
<TextBox Text="{Binding User.Name, UpdateSourceTrigger=PropertyChanged}" />
<Button Content="保存" Command="{Binding SaveCommand}" />

<!-- ❌ Bad: コードビハインドでのイベント処理 -->
<Button Content="保存" Click="SaveButton_Click" />
```

---

## 📝 命名規約

### クラス・プロパティ・メソッド
- **PascalCase**を使用
- 意味のある名前を付ける
- ViewModelには`ViewModel`サフィックス
- Viewには画面の目的を表す名前

```csharp
// ✅ Good
public class UserRegistrationViewModel { }
public class UserRegistrationView { }
public string FirstName { get; set; }
public void SaveUserData() { }

// ❌ Bad
public class UserVM { }
public string fName { get; set; }
public void Save() { }
```

### フィールド・変数
- **UpperCamelCase**を使用
- プライベートフィールドは`_`プレフィックス

```csharp
// ✅ Good
private string _UserName;
private readonly IUserService _UserService;

// ❌ Bad
private string UserName;
private string _userName;
private string username;
```

---

## 🔗 データバインディング

### 推奨パターン
```xml
<!-- ✅ Good: 双方向バインディング -->
<TextBox Text="{Binding UserName, UpdateSourceTrigger=PropertyChanged}" />

<!-- ✅ Good: コマンドバインディング -->
<Button Command="{Binding SaveCommand}" />

<!-- ✅ Good: コンバーター使用 -->
<TextBlock Visibility="{Binding IsVisible, Converter={StaticResource BoolToVisibilityConverter}}" />
```

### ✅ チェックポイント
- [ ] すべてのUI要素がViewModelにバインドされている
- [ ] UpdateSourceTriggerが適切に設定されている
- [ ] 型変換にはコンバーターを使用している

---

## 🎨 XAML規約

### レイアウト
```xml
<!-- ✅ Good: 構造化されたレイアウト -->
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="*"/>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>
    
    <StackPanel Grid.Row="0" Orientation="Horizontal" Margin="10">
        <TextBlock Text="ユーザー名:" VerticalAlignment="Center"/>
        <TextBox Text="{Binding UserName}" Width="200" Margin="5,0"/>
    </StackPanel>
</Grid>
```

### スタイル・リソース
```xml
<!-- ✅ Good: リソース辞書の活用 -->
<Window.Resources>
    <Style x:Key="PrimaryButtonStyle" TargetType="Button">
        <Setter Property="Background" Value="#007ACC"/>
        <Setter Property="Foreground" Value="White"/>
        <Setter Property="Padding" Value="10,5"/>
    </Style>
</Window.Resources>
```

---

## 🛠️ 依存性注入・サービス

### サービスインターフェース
```csharp
// ✅ Good
public interface IUserService
{
    Task<User> GetUserAsync(int id);
    Task SaveUserAsync(User user);
}

public class UserService : IUserService
{
    // 実装
}
```

### ViewModelでの使用
```csharp
// ✅ Good
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

### 非同期処理
```csharp
// ✅ Good
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
        // ログ出力
    }
    finally
    {
        IsLoading = false;
    }
}
```

### バリデーション
```csharp
// ✅ Good: IDataErrorInfoの実装
public class User : INotifyPropertyChanged, IDataErrorInfo
{
    public string this[string columnName]
    {
        get
        {
            switch (columnName)
            {
                case nameof(Name):
                    return string.IsNullOrEmpty(Name) ? "名前は必須です" : null;
                default:
                    return null;
            }
        }
    }

    public string Error => null;
}
```

---

## 📋 コードレビューチェックリスト

### MVVM遵守
- [ ] Viewにビジネスロジックがない
- [ ] ViewModelがViewに依存していない
- [ ] ModelにUI関連のコードがない

### パフォーマンス
- [ ] 重い処理は非同期で実装
- [ ] メモリリークの原因となるイベントハンドラーが適切に解除されている
- [ ] 不要なPropertyChangedイベントが発生していない

### 保守性
- [ ] クラスの責任が単一である
- [ ] メソッドが適切な長さ（20行以内推奨）
- [ ] マジックナンバーが定数化されている

### テスタビリティ
- [ ] ViewModelが単体テスト可能
- [ ] 依存関係がインターフェースで抽象化されている
- [ ] コマンドの実行条件が明確

---

## 🔧 推奨ツール・ライブラリ

### 必須
- **CommunityToolkit.Mvvm** (旧Microsoft.Toolkit.Mvvm)
- **System.ComponentModel.DataAnnotations** (バリデーション)

### 推奨
- **Prism** (大規模アプリケーションの場合)
- **MaterialDesignInXamlToolkit** (モダンUI)
- **Serilog** (ログ出力)

### 設定例
```xml
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.0" />
<PackageReference Include="MaterialDesignThemes" Version="4.9.0" />
```

---

## 📚 参考リンク

- [Microsoft WPF ガイドライン](https://docs.microsoft.com/ja-jp/dotnet/desktop/wpf/)
- [MVVM パターン詳細](https://docs.microsoft.com/ja-jp/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern)
- [CommunityToolkit.Mvvm ドキュメント](https://docs.microsoft.com/ja-jp/dotnet/communitytoolkit/mvvm/)

---

*最終更新: 2025年8月*