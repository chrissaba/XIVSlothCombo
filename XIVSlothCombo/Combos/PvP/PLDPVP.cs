using XIVSlothCombo.CustomComboNS;

namespace XIVSlothCombo.Combos.PvP
{
    internal static class PLDPvP
    {
        public const byte JobID = 19;

        public const uint
            FastBlade = 29058,
            RiotBlade = 29059,
            RoyalAuthority = 29060,
            ShieldBash = 29064,
            Confiteor = 29070,
            HolySheltron = 29067,
            Guardian = 29066,
            Phalanx = 29069;
            
        internal class Buffs
        {
            internal const ushort
                HallowedGround = 1302,
                Covered = 1301,
                BladeOfFaithReady = 3250;
                

        }
        internal class Debuffs
        {
            internal const ushort
                Stun = 1343;
        }

        internal class PLDPvP_Burst : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLDPvP_Burst;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is FastBlade or RiotBlade or RoyalAuthority)
                {

                    if (IsEnabled(CustomComboPreset.PLDPvP_ShieldBash) &&
                        InCombat() && IsOffCooldown(ShieldBash) && CanWeave(actionID) && !TargetHasEffectAny(PvPCommon.Buffs.Guard) && !TargetHasEffectAny(PvPCommon.Buffs.Resilience))
                        return ShieldBash;
                        
                    if (IsEnabled(CustomComboPreset.PLDPvP_Confiteor))
                    {
                       if (IsOffCooldown(Confiteor))
                        return Confiteor;
                    }
                    if (InCombat() && IsOffCooldown(HolySheltron) && CanWeave(actionID))
                        return HolySheltron;
                    if (HasEffect(Buffs.HallowedGround) && (OriginalHook(Phalanx) == 29071) || OriginalHook(Phalanx)==29072 || OriginalHook(Phalanx)==29073)
                        return OriginalHook(Phalanx);

                }

                return actionID;
            }
        }
    }
}