@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

echo ==========================================
echo RemoteMergeUtility Installer Build Script
echo ==========================================
echo.

REM Release ビルドの実行
echo アプリケーションをReleaseモードでビルド中...
dotnet build RemoteMergeUtility/RemoteMergeUtility.sln -c Release

if %errorLevel% neq 0 (
    echo エラー: Releaseビルドに失敗しました。
    pause
    exit /b 1
)

echo ✓ Releaseビルドが完了しました。
echo.

REM WiX Toolsetの確認
where candle >nul 2>&1
if %errorLevel% neq 0 (
    echo エラー: WiX Toolsetがインストールされていません。
    echo WiX Toolset v3.11以降をインストールしてください。
    echo ダウンロード: https://wixtoolset.org/releases/
    echo.
    pause
    exit /b 1
)

echo WiX Toolsetが見つかりました。
echo.

REM インストーラーのビルド
echo インストーラーをビルド中...
cd Installer

REM WiXオブジェクトファイルのコンパイル
candle -dSolutionDir="..\\" Product.wxs

if %errorLevel% neq 0 (
    echo エラー: WiXコンパイルに失敗しました。
    cd ..
    pause
    exit /b 1
)

REM MSIファイルの生成
light -ext WixUIExtension -ext WixUtilExtension Product.wixobj -o RemoteMergeUtility.Installer.msi

if %errorLevel% neq 0 (
    echo エラー: MSI生成に失敗しました。
    cd ..
    pause
    exit /b 1
)

cd ..

echo.
echo ✓ インストーラーのビルドが完了しました！
echo.
echo 生成されたファイル:
echo   Installer\RemoteMergeUtility.Installer.msi
echo.
echo インストーラーの機能:
echo   - カスタムインストール先の指定
echo   - URLスキーマの自動登録
echo   - スタートメニューショートカット作成
echo   - アンインストール時の自動クリーンアップ
echo.
pause