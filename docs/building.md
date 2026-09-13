# ビルド

.NET SDKと、BepInEx・Conditional Config Syncを導入済みのValheimが必要です。
ターゲットフレームワークは.NET Framework 4.7.2です。

リポジトリのルートで実行してください。`GamePath` は自身のインストール先に置き換えます。

```powershell
dotnet build ./Source/StaminaControl.csproj -c Release -p:GamePath="E:\SteamLibrary\steamapps\common\Valheim"
```

CCSのDLLが `BepInEx/plugins/ConditionalConfigSync.Plugin/` 以外にある場合は、
`-p:SyncPath="CCSのDLLがあるフォルダ"` も指定してください。

生成された `Source/bin/Release/net472/StaminaControl.dll` を
`BepInEx/plugins/StaminaControl/` にコピーします。

ゲーム本体・依存ModのDLLはローカル参照です。リポジトリや配布用ZIPには含めないでください。

既存の動作確認については [検証記録](validation.md) を参照してください。
