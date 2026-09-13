# Stamina Control

Valheimのスタミナ消費量と自然回復速度を変更するModです。
消費を半分にしたり、回復を2倍にしたり、それぞれ個別に設定できます。

サーバーと参加者全員にインストールしてください。設定はサーバー側から同期されます。

## インストール

必要なMod：

- [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/)
- [Conditional Config Sync](https://thunderstore.io/c/valheim/p/shudnal/ConditionalConfigSync/) 1.0.5以上

`StaminaControl.dll` を `BepInEx/plugins/StaminaControl/` に配置してください。
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

マルチプレイでの実機確認はまだ行っていません。

## 互換性・不具合報告

Valheim 1.0.12 / BepInExPack 5.4.2350 / ConfigurationManager 1.1.18で起動確認済みです。
スタミナを変更する他のModと併用すると、効果が重なる場合があります。

不具合は [Issues](https://github.com/suzutan/valheim-stamina-control/issues) へ。
ゲームとModのバージョン、再現手順、`BepInEx/LogOutput.log` の該当箇所を添えてください。

[Changelog](CHANGELOG.md) · [MIT License](LICENSE)
