@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

echo ======================================
echo RemoteMergeUtility URL Schema Test
echo ======================================
echo.

REM URLスキーマ登録確認
reg query "HKEY_CLASSES_ROOT\RemoteMergeUtility" >nul 2>&1
set "MAIN_EXISTS=%errorLevel%"
reg query "HKEY_CLASSES_ROOT\mergeutil" >nul 2>&1
set "REDIRECT_EXISTS=%errorLevel%"

if %MAIN_EXISTS% neq 0 if %REDIRECT_EXISTS% neq 0 (
    echo ⚠️  警告: 'mergeutil://' URLスキーマが登録されていません。
    echo 先に RegisterUrlScheme.bat を管理者権限で実行してください。
    echo.
    pause
    exit /b 1
)

echo ✓ URLスキーマが登録されています。
echo.

:MENU
echo [テストメニュー]
echo 1. 基本テスト (Default, revision=1)
echo 2. プロジェクト指定テスト (MyProject, revision=123)
echo 3. 引数付きテスト (Default, revision=5, args=debug-mode)
echo 4. 複雑な引数テスト (TestProj, revision=999, args=--verbose --output=test.log)
echo 5. エラーテスト (無効なrevision)
echo 6. カスタムURL入力
echo 7. 全パターン一括テスト
echo 0. 終了
echo.

set /p choice="選択してください (0-7): "

if "%choice%"=="1" goto TEST1
if "%choice%"=="2" goto TEST2
if "%choice%"=="3" goto TEST3
if "%choice%"=="4" goto TEST4
if "%choice%"=="5" goto TEST_ERROR
if "%choice%"=="6" goto CUSTOM
if "%choice%"=="7" goto ALL_TESTS
if "%choice%"=="0" goto END
goto MENU

:TEST1
echo.
echo [テスト1] 基本テスト
set "TEST_URL=mergeutil://?target=Default&revision=1"
echo URL: !TEST_URL!
echo.
start "" "!TEST_URL!"
goto WAIT_AND_MENU

:TEST2
echo.
echo [テスト2] プロジェクト指定テスト
set "TEST_URL=mergeutil://?target=MyProject&revision=123"
echo URL: !TEST_URL!
echo.
start "" "!TEST_URL!"
goto WAIT_AND_MENU

:TEST3
echo.
echo [テスト3] 引数付きテスト
set "TEST_URL=mergeutil://?target=Default&revision=5&args=debug-mode"
echo URL: !TEST_URL!
echo.
start "" "!TEST_URL!"
goto WAIT_AND_MENU

:TEST4
echo.
echo [テスト4] 複雑な引数テスト
set "TEST_URL=mergeutil://?target=TestProj&revision=999&args=--verbose --output=test.log"
echo URL: !TEST_URL!
echo.
start "" "!TEST_URL!"
goto WAIT_AND_MENU

:TEST_ERROR
echo.
echo [エラーテスト] 無効なパラメータ
echo.
echo a) revision=0 (無効)
set "TEST_URL=mergeutil://?target=Default&revision=0"
echo URL: !TEST_URL!
start "" "!TEST_URL!"
timeout /t 2 /nobreak >nul

echo.
echo b) revision=-5 (無効)
set "TEST_URL=mergeutil://?target=Default&revision=-5"
echo URL: !TEST_URL!
start "" "!TEST_URL!"
timeout /t 2 /nobreak >nul

echo.
echo c) revision=abc (無効)
set "TEST_URL=mergeutil://?target=Default&revision=abc"
echo URL: !TEST_URL!
start "" "!TEST_URL!"
goto WAIT_AND_MENU

:CUSTOM
echo.
echo [カスタムURL入力]
echo 形式: mergeutil://?target=<ProjectKey>&revision=<Number>&args=<OptionalString>
echo 例: mergeutil://?target=MyProj&revision=42&args=custom-test
echo.
set /p "CUSTOM_URL=URLを入力してください: "

if "!CUSTOM_URL!"=="" (
    echo エラー: URLが入力されていません。
    goto WAIT_AND_MENU
)

echo.
echo 実行URL: !CUSTOM_URL!
start "" "!CUSTOM_URL!"
goto WAIT_AND_MENU

:ALL_TESTS
echo.
echo [全パターン一括テスト]
echo 複数のテストを順番に実行します...
echo.

echo 1/4: 基本テスト
start "" "mergeutil://?target=Default&revision=1"
timeout /t 3 /nobreak >nul

echo 2/4: プロジェクト指定テスト
start "" "mergeutil://?target=MyProject&revision=123"
timeout /t 3 /nobreak >nul

echo 3/4: 引数付きテスト
start "" "mergeutil://?target=Default&revision=5&args=debug-mode"
timeout /t 3 /nobreak >nul

echo 4/4: 複雑なテスト
start "" "mergeutil://?target=TestProj&revision=999&args=--verbose --output=test.log"

echo.
echo 全テスト完了！
goto WAIT_AND_MENU

:WAIT_AND_MENU
echo.
echo 3秒後にメニューに戻ります...
timeout /t 3 /nobreak >nul
echo.
goto MENU

:END
echo.
echo テストを終了します。
pause
exit /b 0