# Stamina Control

Valheimのスタミナ消費量と自然回復速度を変更するModです。
消費を半分にしたり、回復を2倍にしたり、それぞれ個別に設定できます。

サーバーと参加者全員にインストールしてください。設定はサーバー側から同期されます。

## インストール

必要なMod：

- [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/)
- [Conditional Config Sync](https://www.nexusmods.com/valheim/mods/3451) 1.0.4以上

依存ModはZIPに含まれていません。ホストと参加者それぞれに導入してください。

[最新版のZIP](https://github.com/suzutan/valheim-stamina-control/releases/latest) をダウンロードし、ゲームのインストール先に展開してください。
`BepInEx/plugins/StaminaControl/StaminaControl.dll` が配置されれば完了です。
ソースからビルドする場合は [ビルド手順](docs/building.md) を参照してください。

## 設定

[ConfigurationManager](https://thunderstore.io/c/valheim/p/shudnal/ConfigurationManager/) を使うと、ゲーム内で変更できます。
F1（初期設定）で開き、「Stamina Control」を選択してください。

設定ファイルは初回起動後に `BepInEx/config/jp.suzutan.valheim.staminacontrol.cfg` に作成されます。

| 設定 | 初期値 | 説明 |
| --- | --- | --- |
| Enabled | true | Modを有効にする |
| ConsumptionMultiplier | 1 | スタミナ消費倍率 |
| RecoveryMultiplier | 1 | スタミナ自然回復倍率 |

倍率は0～20で指定します。`0.5` で半分、`2` で2倍。`0` にすると消費／自然回復がなくなります。
変更はその場で反映されます。

回復倍率は休息などの効果と重なります。最大スタミナ、回復が始まるまでの待ち時間、ポーションの回復量は変わりません。

## マルチプレイ

ホスト／専用サーバーと参加者全員に、同じバージョンのStamina ControlとConditional Config Syncが必要です。
Modが未導入、またはバージョンが非互換の場合は接続できません。

設定を変更できるのはホストとサーバー管理者です。管理者は `adminlist.txt` で指定してください。
専用サーバーにConfigurationManagerを入れる必要はありません。

フレンドを招待するホスト形式で、接続と倍率設定の同期を確認済みです。専用サーバーは未確認です。

## 互換性・不具合報告

接続・設定同期を確認した構成：

| ゲーム／Mod | バージョン |
| --- | --- |
| Valheim | 1.0.12 |
| BepInExPack Valheim | 5.4.2350 |
| Stamina Control | 1.0.2 |
| Conditional Config Sync | 1.0.4 |
| ConfigurationManager | 1.1.16 |

Vortexで管理する場合は、依存ModもVortexから導入・有効化し、Deployしてください。
DLLだけを手動で差し替えると、Vortexの表示と実際に読み込まれるバージョンが食い違う場合があります。
実際の読み込みバージョンは `BepInEx/LogOutput.log` の `Loading [...]` で確認できます。

ConfigurationManagerのバージョン差でも接続が拒否される場合があります。上記構成を使う場合は、ホストと参加者で1.1.16に揃えてください。
1.1.18はCCS 1.0.5以上を要求するため、CCS 1.0.4との組み合わせでは読み込まれません。

スタミナを変更する他のModと併用すると、効果が重なる場合があります。

不具合は [Issues](https://github.com/suzutan/valheim-stamina-control/issues) へ。
ゲームとModのバージョン、再現手順、`BepInEx/LogOutput.log` の該当箇所を添えてください。

[Changelog](CHANGELOG.md) · [MIT License](LICENSE)
