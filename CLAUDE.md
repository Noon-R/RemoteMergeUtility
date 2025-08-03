# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

**重要**: 日本語での返答を行ってください。

## プロジェクト概要

RemoteMergeUtilityは、MVVMアーキテクチャパターンを使用した.NET 8.0ベースのWPFアプリケーションです。厳格なMVVMコーディング規約に従うWindows専用のデスクトップユーティリティアプリケーションです。

このツールでは、カスタムURLスキーマで渡される引数からshellコマンドの実行を行います。
渡されるパラメータは、対象projectのKeyとRevisionの指定, その他のとしての引数
アプリ内でKeyと対象のProjectPathのセットを保持します。
このデータは揮発しないようにファイルに保存します。

## ビルドと開発コマンド

### ビルドと実行
```bash
# ソリューションをビルド
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln

# デバッグモードでアプリケーションを実行
dotnet run --project RemoteMergeUtility/RemoteMergeUtility/RemoteMergeUtility.csproj

# リリース版をビルド
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln -c Release
```

### プロジェクト構造
```
RemoteMergeUtility/
├── RemoteMergeUtility.sln          # ソリューションファイル
└── RemoteMergeUtility/
    ├── RemoteMergeUtility.csproj   # メインプロジェクトファイル
    ├── App.xaml                    # アプリケーションエントリーポイント
    ├── MainWindow.xaml/.cs         # メインウィンドウ
    └── [今後実装予定のMVVM構造]
```

## アーキテクチャとMVVM規約

このプロジェクトは`wpf_mvvm_coding_standards.md`で定義された厳格なWPF MVVM コーディング規約に従います。主要なアーキテクチャ原則：

### フォルダ構造（実装予定）
- `Models/` - データモデルとビジネスロジック
- `ViewModels/` - INotifyPropertyChangedを実装するビューモデル
- `Views/` - 最小限のコードビハインドを持つXAMLビュー
- `Services/` - 外部サービスとデータアクセス
- `Converters/` - データバインディング用の値コンバーター
- `Commands/` - カスタムコマンド実装
- `Helpers/` - ヘルパークラスとユーティリティ
- `Resources/` - UIリソース（画像、スタイルなど）

### 主要なMVVM要件
- **View** はビジネスロジックを含んではならない - データバインディングと最小限のUIロジックのみ
- **ViewModel** はViewを直接参照してはならない - データバインディングとコマンドを使用
- **Model** はUI更新のためにINotifyPropertyChangedを実装
- クラス、プロパティ、メソッドにはPascalCaseを使用
- プライベートフィールドには`_`プレフィックス付きUpperCamelCaseを使用
- **readonly フィールドと定数**には`_UPPER_SNAKE_CASE`を使用（privateの場合は`_`プレフィックス付き）
- すべてのUI操作はイベントハンドラーではなくCommandバインディングを使用

### インストール済みライブラリ
- **LivetCask (4.0.2)** - Messaging、Behavior、MVVM支援（日本製）
- **CommunityToolkit.Mvvm (8.4.0)** - モダンなMVVMフレームワーク（Microsoft公式）
- **Microsoft.Extensions.DependencyInjection (9.0.7)** - 依存性注入コンテナ
- **Serilog.Extensions.Hosting (9.0.0)** - 構造化ログ出力
- **MaterialDesignThemes (5.2.1)** - Material Design UIコンポーネント
- **System.Text.Json** - JSON シリアライゼーション（.NET標準）

### 追加検討ライブラリ
- **FluentValidation** - 流暢なAPIによるモデルバリデーション
- **Serilog.Sinks.File** - ファイル出力用Sink（必要に応じて）

### データバインディングパターン
```xml
<!-- 適切な双方向バインディング -->
<TextBox Text="{Binding PropertyName, UpdateSourceTrigger=PropertyChanged}" />

<!-- コマンドバインディング -->
<Button Command="{Binding SaveCommand}" />

<!-- コンバーター使用 -->
<TextBlock Visibility="{Binding IsVisible, Converter={StaticResource BoolToVisibilityConverter}}" />
```

## 開発ガイドライン

1. **MVVM準拠**: 新機能はすべて厳格なMVVM分離に従う必要があります
2. **非同期操作**: ブロッキングの可能性がある操作にはasync/awaitを使用
3. **エラーハンドリング**: ユーザーフレンドリーなエラーメッセージでtry-catchブロックを実装
4. **バリデーション**: モデルバリデーションにはIDataErrorInfoを使用
5. **依存性注入**: テスタビリティのためにサービスをインターフェースで抽象化

## プロジェクトステータス

現在は最小限のWPFプロジェクトテンプレートです。確立されたコーディング規約に従ってMVVM構造とビジネスロジックの実装が必要です。