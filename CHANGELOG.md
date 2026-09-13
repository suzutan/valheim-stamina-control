# 1.0.1

- Mod識別子を `jp.suzutan.valheim.staminacontrol` に変更。
- GitHub ActionsによるビルドとRelease ZIPの配布に対応。

旧 `jp.custom.valheim.staminacontrol.cfg` を使用していた場合は、ゲーム終了中に
`jp.suzutan.valheim.staminacontrol.cfg` へ設定を移してください。マルチでは全員を更新してください。

# 1.0.0

- ConfigurationManagerで消費・自然回復倍率を0～20倍に設定。
- Conditional Config Syncで必須Mod照合・サーバー共通設定・管理者編集に対応。
- 消費RPC受信側を補正して二重適用を回避。
- 行動開始判定と消費倍率を整合。0消費時に残量0でも行動可能。
- 自然回復の既存補正・待ち時間を維持。
