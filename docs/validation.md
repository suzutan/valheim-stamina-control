# Validation — Stamina Control 1.0.0

2026-09-13 / Windows / Valheim 1.0.12 / Unity 6000.0.75.
BepInExPack 5.4.2350, Conditional Config Sync 1.0.5, shudnal ConfigurationManager 1.1.18.
.NET SDK 10.0.111 targeting net472: build passed with zero warnings/errors.

The initial implementation passed 30 runtime assertions in an isolated BepInEx directory,
running the installed game headlessly. Tests invoked real Harmony-patched game methods
on managed-only fixtures, checked patch registration, and exercised ConfigurationManager's
setting collection and editing APIs. No normal character or world was loaded by the tests.

Verified:

- Required remote mod registration and local source/admin state.
- Server-controlled registration of consumption and recovery multipliers.
- Consumption 0, 0.5, 1, 2, 20 applies once at the RPC receiving endpoint.
- Free actions at zero stamina without resetting the regeneration delay.
- Consumption clamps at zero; disabling restores vanilla cost.
- Natural regeneration preserves the incoming factor; non-player entities are unaffected.
- Invalid, negative and excessive multiplier normalization.
- Exactly three visible ConfigurationManager settings; its edit API updates effective config.
- All three Harmony patch registrations.

Not verified: two physical clients, dedicated server, actual client/server delivery,
administrator changes over the network, late join, disconnect restoration, crossplay,
on-screen UI rendering/interaction, and compatibility with a full third-party mod collection.
Synchronization, authorization and admission use the standalone CCS library.
These checks are not a completed multiplayer playtest.

## Multiplayer acceptance check

1. Install the same Stamina Control version and CCS on the host/server and all clients.
2. Set consumption to 0.5 and recovery to 2 on the host; verify matching values on a client.
3. Run and recover stamina on both clients; check the expected behavior.
4. Change the host values to 1/1 while connected; verify the other client updates.
5. Verify ordinary clients cannot edit the shared settings; repeat with a server administrator.
6. Check late joining, disconnect/local-value restoration, and reconnection.
7. Check that a client missing the required mod is rejected with an appropriate diagnostic.
