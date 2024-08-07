﻿using Dalamud.Game.ClientState.Objects.SubKinds;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using XIVSlothCombo.Combos.PvE;
using XIVSlothCombo.Core;
using XIVSlothCombo.CustomComboNS;


namespace XIVSlothCombo.Combos.PvP
{
    internal static class SAMPvP
    {
        public const byte JobID = 34;

        public const uint
            KashakCombo = 58,
            Yukikaze = 29523,
            Gekko = 29524,
            Kasha = 29525,
            Hyosetsu = 29526,
            Mangetsu = 29527,
            Oka = 29528,
            OgiNamikiri = 29530,
            Soten = 29532,
            Chiten = 29533,
            Mineuchi = 29535,
            MeikyoShisui = 29536,
            Midare = 29529,
            Kaeshi = 29531,
            Zantetsuken = 29537;

        public static class Buffs
        {
            public const ushort
                Kaiten = 3201,
                Midare = 3203,
                Chiten = 1240;
        }

        public static class Debuffs
        {
            public const ushort
                Kuzushi = 3202;
        }

        public static class Config
        {
            public const string
                SAMPvP_SotenCharges = "SamSotenCharges",
                SAMPvP_SotenHP = "SamSotenHP";

        }

        internal class SAMPvP_BurstMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SAMPvP_BurstMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var sotenCharges = PluginConfiguration.GetCustomIntValue(Config.SAMPvP_SotenCharges);
                
                if ((IsNotEnabled(CustomComboPreset.SAMPvP_BurstMode_MainCombo) && actionID == MeikyoShisui) ||
                    (IsEnabled(CustomComboPreset.SAMPvP_BurstMode_MainCombo) && actionID is Yukikaze or Gekko or Kasha or Hyosetsu or Oka or Mangetsu))
                {
                    if (TargetHasEffectAny(SAMPvP.Buffs.Chiten) || TargetHasEffectAny(PLDPvP.Buffs.HallowedGround) || TargetHasEffectAny(DRKPvP.Buffs.UndeadRedemption) || TargetHasEffectAny(PLDPvP.Buffs.Covered))
                        return OriginalHook(11);

                    if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
                    {
                        if (IsEnabled(CustomComboPreset.SAMPvP_BurstMode_Stun) && IsOffCooldown(Mineuchi) && !TargetHasEffectAny(PvPCommon.Buffs.Resilience) && !(TargetHasEffectAny(PvPCommon.Debuffs.Stun) || TargetHasEffectAny(PvPCommon.Debuffs.Bind) || TargetHasEffectAny(PvPCommon.Debuffs.DeepFreeze)))
                            return OriginalHook(Mineuchi);

                        if (IsOffCooldown(MeikyoShisui) && OriginalHook(OgiNamikiri)!=Kaeshi)
                            return OriginalHook(MeikyoShisui);

                        if (IsEnabled(CustomComboPreset.SAMPvP_BurstMode_Chiten) && IsOffCooldown(Chiten) && InCombat() && PlayerHealthPercentageHp() <= 95)
                            return OriginalHook(Chiten);

                        if (GetCooldownRemainingTime(Soten) < 1 && CanWeave(Yukikaze))
                            return OriginalHook(Soten);

                        if (OriginalHook(MeikyoShisui) == Midare)
                            return OriginalHook(MeikyoShisui);

                        if (IsOffCooldown(OgiNamikiri) && OriginalHook(MeikyoShisui)!= Midare)
                            return OriginalHook(OgiNamikiri);

                        if (IsOnCooldown(OgiNamikiri)&&(GetRemainingCharges(Soten) > sotenCharges && CanWeave(Yukikaze)))
                            return OriginalHook(Soten);

                        if (OriginalHook(OgiNamikiri) == Kaeshi)
                            return OriginalHook(OgiNamikiri);
                    }
                    if (TargetHasEffectAny(PvPCommon.Buffs.Guard) && OriginalHook(OgiNamikiri) == Kaeshi)
                        return OriginalHook(11);
                }
                {
                    if (TargetHasEffectAny(SAMPvP.Debuffs.Kuzushi) && GetLimitBreakValue()==100 && !TargetHasEffectAny(PLDPvP.Buffs.HallowedGround) && !TargetHasEffectAny(DRKPvP.Buffs.UndeadRedemption))
                        return OriginalHook(Zantetsuken);
                }

                return actionID;
            }
        }

        internal class SAMPvP_KashaFeatures : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SAMPvP_KashaFeatures;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var SamSotenHP = PluginConfiguration.GetCustomIntValue(Config.SAMPvP_SotenHP);

                if (actionID is Yukikaze or Gekko or Kasha or Hyosetsu or Mangetsu or Oka)
                {
                    if (!InMeleeRange() && actionID != SAMPvP.OgiNamikiri)
                    {
                        if (IsEnabled(CustomComboPreset.SAMPvP_KashaFeatures_GapCloser) && GetRemainingCharges(Soten) > 0 && GetTargetHPPercent() <= SamSotenHP)
                            return OriginalHook(Soten);

                        if (IsEnabled(CustomComboPreset.SAMPvP_KashaFeatures_AoEMeleeProtection) && !IsOriginal(Yukikaze) && !HasEffect(Buffs.Midare) && IsOnCooldown(MeikyoShisui) && IsOnCooldown(OgiNamikiri) && OriginalHook(OgiNamikiri) != Kaeshi)
                            return SAM.Yukikaze;
                    }
                }

                return actionID;
            }
        }
    }
}