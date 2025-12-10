@echo off
title LaLa Store
echo ========================================
echo    Starting LaLa Store...
echo ========================================
echo.

cd /d "%~dp0"

REM Stop any running instance
echo Stopping any running instances...
taskkill /F /IM LaLaStore.exe 2>nul
timeout /t 2 /nobreak >nul

echo.
echo Building and starting the application...
echo.
dotnet run

pause

