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

## ZIPの作成

```powershell
./scripts/package.ps1 -Version 1.0.1
```

`artifacts/StaminaControl-1.0.1.zip` とSHA-256ファイルが生成されます。
DLLのバージョンと指定値が違う場合はエラーになります。

## GitHub Release

GitHubの **Actions → Release → Run workflow** で、`version` に `1.0.2` のような番号を入力します。
成功すると `v1.0.2` タグとReleaseが作られ、ビルドしたZIPとSHA-256ファイルが添付されます。
`v1.0.2` のようなタグをpushした場合も同じ処理が走ります。

バージョンはビルド時にDLL・BepInExのMod情報・同期バージョンへ共通で適用されます。
通常のローカルビルドはcsprojのVersionを使います。別の番号でビルドする場合は `-p:Version=1.0.2` を指定してください。

CIはSteamCMDの匿名ログインでValheim Dedicated Serverを取得し、ゲームDLLをコンパイル参照として使います。
BepInExPack 5.4.2350とCCS 1.0.5は配布元から取得してSHA-256を照合します。
ゲームDLLや依存ModのDLLはRelease Assetには含まれません。Steamの認証情報や追加のGitHubシークレットは不要です。

公開済みReleaseは上書きしません。アップロード途中で失敗してDraftが残った場合は、DraftとAssetを確認してから復旧してください。
