# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

**重要E*: 日本語での返答を行ってください、E

## プロジェクト概要E

RemoteMergeUtilityは、MVVMアーキチE��チャパターンを使用した.NET 8.0ベ�EスのWPFアプリケーションです。厳格なMVVMコーチE��ング規紁E��従うWindows専用のチE��クトップユーチE��リチE��アプリケーションです、E

こ�EチE�Eルでは、カスタムURLスキーマで渡される引数からshellコマンド�E実行を行います、E
渡されるパラメータは、対象projectのKeyとRevisionの持E��E そ�E他�Eとしての引数
アプリ冁E��Keyと対象のProjectPathのセチE��を保持します、E
こ�EチE�Eタは揮発しなぁE��ぁE��ファイルに保存します、E

## ビルドと開発コマンチE

### ビルドと実衁E
```bash
# ソリューションをビルチE
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln

# チE��チE��モードでアプリケーションを実衁E
dotnet run --project RemoteMergeUtility/RemoteMergeUtility/RemoteMergeUtility.csproj

# リリース版をビルチE
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln -c Release
```

### プロジェクト構造
```
RemoteMergeUtility/
├── RemoteMergeUtility.sln		  # ソリューションファイル
└── RemoteMergeUtility/
	├── RemoteMergeUtility.csproj   # メインプロジェクトファイル
	├── App.xaml					# アプリケーションエントリーポインチE
	├── MainWindow.xaml/.cs		 # メインウィンドウ
	└── [今後実裁E��定�EMVVM構造]
```

## アーキチE��チャとMVVM規紁E

こ�Eプロジェクト�E`wpf_mvvm_coding_standards.md`で定義された厳格なWPF MVVM コーチE��ング規紁E��従います。主要なアーキチE��チャ原則�E�E

### フォルダ構造�E�実裁E��定！E
- `Models/` - チE�EタモチE��とビジネスロジチE��
- `ViewModels/` - INotifyPropertyChangedを実裁E��るビューモチE��
- `Views/` - 最小限のコードビハインドを持つXAMLビュー
- `Services/` - 外部サービスとチE�Eタアクセス
- `Converters/` - チE�EタバインチE��ング用の値コンバ�Eター
- `Commands/` - カスタムコマンド実裁E
- `Helpers/` - ヘルパ�EクラスとユーチE��リチE��
- `Resources/` - UIリソース�E�画像、スタイルなど�E�E

### 主要なMVVM要件
- **View** はビジネスロジチE��を含んではならなぁE- チE�EタバインチE��ングと最小限のUIロジチE��のみ
- **ViewModel** はViewを直接参�EしてはならなぁE- チE�EタバインチE��ングとコマンドを使用
- **Model** はUI更新のためにINotifyPropertyChangedを実裁E
- クラス、�Eロパティ、メソチE��にはPascalCaseを使用
- プライベ�Eトフィールドには`_`プレフィチE��ス付きUpperCamelCaseを使用
- **readonly フィールドと定数**には`_UPPER_SNAKE_CASE`を使用�E�Erivateの場合�E`_`プレフィチE��ス付き�E�E
- すべてのUI操作�EイベントハンドラーではなくCommandバインチE��ングを使用

### インスト�Eル済みライブラリ
- **LivetCask (4.0.2)** - Messaging、Behavior、MVVM支援�E�日本製�E�E
- **CommunityToolkit.Mvvm (8.4.0)** - モダンなMVVMフレームワーク�E�Eicrosoft公式！E
- **Microsoft.Extensions.DependencyInjection (9.0.7)** - 依存性注入コンチE��
- **Serilog.Extensions.Hosting (9.0.0)** - 構造化ログ出劁E
- **MaterialDesignThemes (5.2.1)** - Material Design UIコンポ�EネンチE
- **System.Text.Json** - JSON シリアライゼーション�E�ENET標準！E

### 追加検討ライブラリ
- **FluentValidation** - 流暢なAPIによるモチE��バリチE�Eション
- **Serilog.Sinks.File** - ファイル出力用Sink�E�忁E��に応じて�E�E

### チE�EタバインチE��ングパターン
```xml
<!-- 適刁E��双方向バインチE��ング -->
<TextBox Text="{Binding PropertyName, UpdateSourceTrigger=PropertyChanged}" />

<!-- コマンドバインチE��ング -->
<Button Command="{Binding SaveCommand}" />

<!-- コンバ�Eター使用 -->
<TextBlock Visibility="{Binding IsVisible, Converter={StaticResource BoolToVisibilityConverter}}" />
```

## 開発ガイドライン

1. **MVVM準拠**: 新機�Eはすべて厳格なMVVM刁E��に従う忁E��がありまぁE
2. **非同期操佁E*: ブロチE��ングの可能性がある操作にはasync/awaitを使用
3. **エラーハンドリング**: ユーザーフレンドリーなエラーメチE��ージでtry-catchブロチE��を実裁E
4. **バリチE�Eション**: モチE��バリチE�EションにはIDataErrorInfoを使用
5. **依存性注入**: チE��タビリチE��のためにサービスをインターフェースで抽象匁E

## プロジェクトスチE�Eタス

現在は最小限のWPFプロジェクトテンプレートです。確立されたコーチE��ング規紁E��従ってMVVM構造とビジネスロジチE��の実裁E��忁E��です�