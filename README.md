# Valheim Stamina Control

スタミナ消費と自然回復を0～20倍に調整するValheim用BepInEx Modです。
ConfigurationManagerから操作でき、マルチプレイではConditional Config Sync（CCS）でサーバー設定を同期します。

## 必要環境

- PC版Valheim（1.0.12でビルド・起動検証）
- [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/)（検証版5.4.2350）
- [Conditional Config Sync](https://thunderstore.io/c/valheim/p/shudnal/ConditionalConfigSync/) 1.0.5以上
- 設定画面を使用するPC：[ConfigurationManager](https://thunderstore.io/c/valheim/p/shudnal/ConfigurationManager/)（shudnal版1.1.18で検証）とその依存Mod

専用サーバーにConfigurationManagerは不要です。

## ビルド・導入

.NET SDKで以下を実行します。GamePathは自身のValheimインストール先に置き換えてください。

```powershell
dotnet build ./Source/StaminaControl.csproj -c Release -p:GamePath="E:\SteamLibrary\steamapps\common\Valheim"
```

CCSが標準と異なるフォルダにある場合は `-p:SyncPath="CCSのDLLがあるフォルダ"` も指定します。
ターゲットは.NET Framework 4.7.2です。ゲーム本体と依存ModのDLLはローカル参照で、このリポジトリには含みません。

生成した `Source/bin/Release/net472/StaminaControl.dll` をゲーム終了中に
`BepInEx/plugins/StaminaControl/StaminaControl.dll` へ配置します。
Vortex利用時は旧版との二重導入を避け、管理対象ファイルの手動変更が再Deployで戻らないよう注意してください。

## 操作

ConfigurationManager（標準F1）で「Stamina Control」を開きます。

| 設定 | 初期値 | 内容 |
| --- | --- | --- |
| Enabled | true | Modの有効／無効 |
| ConsumptionMultiplier | 1 | スタミナ消費倍率（0～20） |
| RecoveryMultiplier | 1 | 自然回復倍率（0～20） |

消費0.5・回復2なら消費半分／自然回復2倍です。0は消費なし／自然回復なし。
小数を指定でき、変更は再起動せず反映されます。
負数は0、20超は20、NaN・Infinityは1へ補正します。

設定ファイル：`BepInEx/config/jp.custom.valheim.staminacontrol.cfg`

自然回復は休息・状態異常・ワールド設定などの既存補正に乗算します。
最大スタミナ、自然回復開始までの待ち時間、ポーションの直接回復量は変更しません。
消費RPC受信側に一度だけ倍率を適用し、行動開始に必要なスタミナ判定も調整します。
他Modの独自処理によるスタミナフィールドの直接変更は対象外です。

## マルチプレイ

**ホスト／専用サーバーと参加者全員に、同じバージョンの本ModとCCSが必要です。**
自分だけ導入して未導入の参加者へ効果を与えることはできません。
CCSの必須Modチェックで未導入・非互換の接続を拒否する設定です。

- ホストまたはサーバー管理者がConfigurationManagerで編集すると、サーバー経由で全員へ同期します。
- フレンドのサーバーで自分が編集するには、サーバー側の `adminlist.txt` による管理者登録が必要です。
- 一般参加者はサーバーの値を受信し、編集は制限されます。
- 専用サーバーでは管理者クライアントから編集するか、停止中にサーバーのcfgを編集します。
- 切断後のローカル値への復帰はCCSが管理します。

各設定は `AlwaysServerControlled`、`ModRequired = true` として登録しています。
非表示設定 `Server/LockConfiguration` は通常trueのまま使用します。

## 検証状況

実ゲームをヘッドレス起動した30項目の検証が通過しています。
ConfigurationManagerの項目取得・編集処理、実ゲームメソッドへのHarmonyパッチを確認しています。
**実際の2台のPCでの接続・同期配送・途中参加・切断復帰・クロスプレイは未検証です。**
詳細とマルチプレイの確認手順は [検証記録](docs/validation.md) を参照してください。

## ライセンス

[MIT](LICENSE)。ゲーム本体・BepInEx・CCS等の依存DLLはそれぞれの提供元から取得してください。
