@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

echo ===========================================
echo RemoteMergeUtility URL Schema Unregistration
echo ===========================================
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

REM 既存のレジストリエントリ確認
reg query "HKEY_CLASSES_ROOT\mergeutil" >nul 2>&1
if %errorLevel% neq 0 (
    echo 情報: 'mergeutil://' URLスキーマは登録されていません。
    echo.
    pause
    exit /b 0
)

echo URLスキーマ 'mergeutil://' の登録を解除中...

REM レジストリエントリの削除
reg delete "HKEY_CLASSES_ROOT\mergeutil" /f >nul 2>&1

if %errorLevel% equ 0 (
    echo.
    echo ✓ URLスキーマの登録解除が完了しました！
    echo.
    echo 'mergeutil://' URLは今後動作しなくなります。
) else (
    echo.
    echo ✗ URLスキーマの登録解除に失敗しました。
    echo 手動でレジストリから削除してください。
    echo レジストリパス: HKEY_CLASSES_ROOT\mergeutil
)

echo.
pause