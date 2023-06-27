@echo off

taskkill /f /im VBCSCompiler.exe
taskkill /f /im dotnet.exe
rd /s /q %TEMP%\VBCSCompiler\AnalyzerAssemblyLoader
rd /s /q %TEMP%\VS\AnalyzerAssemblyLoader

rd /s /q try\Kroki.Example\bin
rd /s /q try\Kroki.Example\obj\Debug

rd /s /q src\Kroki.Core\bin
rd /s /q src\Kroki.Core\obj\Debug

rd /s /q src\Kroki.Generator\bin
rd /s /q src\Kroki.Generator\obj\Debug

dotnet build
