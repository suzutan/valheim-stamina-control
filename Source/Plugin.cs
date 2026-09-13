using System;
using BepInEx;
using BepInEx.Configuration;
using ConditionalConfigSync;
using HarmonyLib;

namespace StaminaControl
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency("_shudnal.ConditionalConfigSync", "1.0.5")]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const string Guid = "jp.suzutan.valheim.staminacontrol";
        public const string Name = "Stamina Control";
        public const string Version = "1.0.0";

        internal static ConfigEntry<bool> Enabled;
        internal static ConfigEntry<float> Consumption;
        internal static ConfigEntry<float> Recovery;
        private Harmony harmony;
        internal static ConfigSync Sync;

        private void Awake()
        {
            Sync = new ConfigSync(Guid)
            {
                DisplayName = Name,
                CurrentVersion = Version,
                MinimumRequiredVersion = Version,
                ModRequired = true
            };

            Enabled = Bind("Enabled", true,
                "有効／無効。ホストまたはサーバー管理者が変更すると全員に反映されます。", null, 30);
            Consumption = Bind("ConsumptionMultiplier", 1f,
                "スタミナ消費倍率。0 = 消費なし、0.5 = 半分、1 = 通常、2 = 2倍。全員で共通。",
                new FiniteMultiplierRange(), 20);
            Recovery = Bind("RecoveryMultiplier", 1f,
                "自然回復倍率。0 = 自然回復なし、1 = 通常、2 = 2倍。休息などの補正に乗算。回復開始待ち時間・ポーション・最大値は変更しません。",
                new FiniteMultiplierRange(), 10);

            // CCS validates writes on the server as well as exposing read-only UI metadata.
            // Unlocked non-admin publication remains disabled (the library default).
            ConfigEntry<bool> locked = Config.Bind("Server", "LockConfiguration", true,
                new ConfigDescription("サーバー管理者以外による設定変更を禁止します。",
                    null, new ConfigurationManagerAttributes { Browsable = false }));
            Sync.AddLockingConfigEntry(locked);

            harmony = new Harmony(Guid);
            try
            {
                harmony.PatchAll(typeof(Plugin).Assembly);
                Logger.LogInfo("Ready: stamina consumption, natural recovery, and cost checks patched. Server synchronization required.");
            }
            catch (Exception error)
            {
                harmony.UnpatchSelf();
                Logger.LogError("Stamina Control could not patch this game version. No partial gameplay patches remain. " + error);
                throw;
            }
        }

        private ConfigEntry<T> Bind<T>(string key, T value, string description,
            AcceptableValueBase range, int order)
        {
            ConfigEntry<T> entry = Config.Bind("Stamina", key, value,
                new ConfigDescription(description, range,
                    new ConfigurationManagerAttributes { Order = order }));
            Sync.AddConfigEntry(entry, ConfigSyncMode.AlwaysServerControlled);
            return entry;
        }

        internal static float CostMultiplier => Enabled.Value ? MultiplierMath.Normalize(Consumption.Value) : 1f;
        internal static float RecoveryMultiplier => Enabled.Value ? MultiplierMath.Normalize(Recovery.Value) : 1f;

        private void OnDestroy()
        {
            harmony?.UnpatchSelf();
        }
    }

    // ConfigurationManager discovers these fields by name; no UI DLL dependency on servers.
    internal sealed class ConfigurationManagerAttributes
    {
        public int? Order;
        public bool? Browsable;
        public bool? ShowRangeAsPercent = false;
    }

    internal sealed class FiniteMultiplierRange : AcceptableValueRange<float>
    {
        public FiniteMultiplierRange() : base(0f, 20f) { }
        public override object Clamp(object value) => MultiplierMath.Normalize((float)value);
        public override bool IsValid(object value)
        {
            float number = (float)value;
            return !float.IsNaN(number) && !float.IsInfinity(number) && number >= 0f && number <= 20f;
        }
    }

    // Patch the receiving endpoint, not both sender and receiver: RPC costs scale exactly once.
    [HarmonyPatch(typeof(Player), "RPC_UseStamina", new Type[] { typeof(long), typeof(float) })]
    internal static class ConsumptionPatch
    {
        [HarmonyPrefix]
        private static void Prefix(ref float __1)
        {
            __1 = MultiplierMath.ScalePositive(__1, Plugin.CostMultiplier);
        }
    }

    // Keep action affordability checks consistent with the modified consumption.
    [HarmonyPatch(typeof(Player), nameof(Player.HaveStamina), new Type[] { typeof(float) })]
    internal static class AffordabilityPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(ref float __0, ref bool __result)
        {
            float multiplier = Plugin.CostMultiplier;
            if (multiplier == 0f)
            {
                __result = true;
                return false;
            }
            __0 = MultiplierMath.ScalePositive(__0, multiplier);
            return true;
        }
    }

    // This method participates in the existing natural-regeneration formula. It preserves
    // rested/debuff multipliers, regeneration delay, maximum clamp and network state writes.
    [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyStaminaRegen))]
    internal static class RecoveryPatch
    {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(Character ___m_character, ref float __0)
        {
            if (___m_character is Player)
                __0 *= Plugin.RecoveryMultiplier;
        }
    }
}
