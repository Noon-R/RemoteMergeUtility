# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

**重要**: 日本語での返答を行ってください。

## プロジェクト概要

RemoteMergeUtilityは、MVVMアーキテクチャパターンを使用した.NET 8.0ベースのWPFアプリケーションです。厳格なMVVMコーディング規約に従うWindows専用のデスクトップユーティリティアプリケーションです。

このツールでは、カスタムURLスキーマで渡される引数からshellコマンドの実行を行います。
渡されるパラメータは、対象projectのKeyとRevisionの値、その他オプションとしての引数です。
アプリはKeyと対象のProjectPathのセットを保持します。
このデータは揮発しないようにファイルに保存します。

## 詳細プロジェクト進捗

### 📊 詳細進捗レポート
**[Documents/DEVELOPMENT_STATUS.md](Documents/DEVELOPMENT_STATUS.md)** を必ず確認してください。

このファイルには以下の詳細情報が含まれています：
- 完了した機能の詳細リスト
- 技術スタック構成
- プロジェクト構造の全体像
- URL スキーマ処理フローの詳細
- Build構成による動作の違い
- 次に実装すべき機能
- 開発・テストコマンド一覧

### 🔄 現在の実装状況
- ✅ 基本MVVM構造完成
- ✅ ProjectInfo管理機能完成  
- ✅ URLスキーマハンドラー完成
- ✅ 外部ツール連携機能完成
- ✅ Debug/Release環境切り替え完成
- 🔲 エラーハンドリング強化
- 🔲 設定管理機能

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

### URLスキーマテスト
```bash
# URLスキーマ登録（管理者権限で実行）
Scripts/RegisterUrlScheme.bat

# URLスキーマテスト
Scripts/TestUrlScheme.bat

# URLスキーマ削除（管理者権限で実行）
Scripts/UnregisterUrlScheme.bat
```

## アーキテクチャとMVVM規約

このプロジェクトは`wpf_mvvm_coding_standards.md`で定義された厳格なWPF MVVM コーディング規約に従います。主要なアーキテクチャ原則：

### フォルダ構造（実装済み）
- `Models/` - データモデルとビジネスロジック
- `ViewModels/` - INotifyPropertyChangedを実装するビューモデル
- `Views/` - 最小限のコードビハインドを持つXAMLビュー
- `Services/` - 外部サービスとデータアクセス

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

1. **MVVM準拠**: 新機能はすべて厳格なMVVM則に従う必要があります
2. **非同期操作**: ブロッキングの可能性がある操作にはasync/awaitを使用
3. **エラーハンドリング**: ユーザーフレンドリーなエラーメッセージでtry-catchブロックを実装
4. **バリデーション**: モデルバリデーションにはIDataErrorInfoを使用
5. **依存性注入**: テスタビリティのためにサービスをインターフェースで抽象化

## 外部ツール連携仕様

### 仮想外部ツールコマンド
- `launch -project-list` - ProjectPathと名前のセット取得
- `launch -name {name}` - Projectの起動状態取得
- `launch -project {projectName}` - Projectに命令実行

### Build構成による動作切り替え
- **Debug Build**: MockLaunchToolServiceを使用（Process実行なし）
- **Release Build**: LaunchToolServiceを使用（実際の外部ツール連携）

## プロジェクトステータス

現在はURL Scheme Handler & External Tool Integrationまで完了しています。

### 次の実装予定
1. エラーハンドリング強化
2. 設定管理機能
3. UI機能拡張

詳細な進捗状況は **[Documents/DEVELOPMENT_STATUS.md](Documents/DEVELOPMENT_STATUS.md)** を参照してください。