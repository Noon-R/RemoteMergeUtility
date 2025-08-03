@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

echo ==========================================
echo RemoteMergeUtility URL Schema Registration
echo ==========================================
echo.

REM 管理者権限チェック
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo エラー: 管理者権限で実行してください。
    echo 右クリックして「管理者として実行」を選択してください。
    echo.
    pause
    exit /b 1
)

REM 実行ファイルのパスを取得
set "EXE_PATH=D:\0_Data\Developer\C#_works\RemoteMergeUtility\RemoteMergeUtility\RemoteMergeUtility\bin\Debug\net8.0-windows\RemoteMergeUtility.exe"

REM 実行ファイルの存在確認
if not exist "%EXE_PATH%" (
    echo エラー: 実行ファイルが見つかりません。
    echo パス: %EXE_PATH%
    echo.
    echo プロジェクトをビルドしてから再実行してください。
    echo.
    pause
    exit /b 1
)

echo 実行ファイル: %EXE_PATH%
echo.

REM レジストリ登録
echo URLスキーマ 'mergeutil://' を登録中...

reg add "HKEY_CLASSES_ROOT\mergeutil" /ve /d "URL:Remote Merge Utility Protocol" /f >nul
if %errorLevel% neq 0 (
    echo エラー: レジストリの書き込みに失敗しました。
    pause
    exit /b 1
)

reg add "HKEY_CLASSES_ROOT\mergeutil" /v "URL Protocol" /d "" /f >nul
reg add "HKEY_CLASSES_ROOT\mergeutil\DefaultIcon" /ve /d "\"%EXE_PATH%\",1" /f >nul
reg add "HKEY_CLASSES_ROOT\mergeutil\shell\open\command" /ve /d "\"%EXE_PATH%\" \"%%1\"" /f >nul

if %errorLevel% equ 0 (
    echo.
    echo ✓ URLスキーマの登録が完了しました！
    echo.
    echo テスト用URL:
    echo   mergeutil://?target=Default^&revision=1
    echo   mergeutil://?target=MyProject^&revision=123^&args=test
    echo.
    echo ブラウザまたはコマンドラインから上記URLを実行してテストしてください。
) else (
    echo.
    echo ✗ URLスキーマの登録に失敗しました。
)

echo.
pause