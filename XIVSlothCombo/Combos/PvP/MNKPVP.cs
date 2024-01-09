using XIVSlothCombo.CustomComboNS;

namespace XIVSlothCombo.Combos.PvP
{
    internal static class MNKPvP
    {
        public const byte ClassID = 2;
        public const byte JobID = 20;

        public const uint
            PhantomRushCombo = 55,
            Bootshine = 29472,
            TrueStrike = 29473,
            SnapPunch = 29474,
            DragonKick = 29475,
            TwinSnakes = 29476,
            Demolish = 29477,
            PhantomRush = 29478,
            SixSidedStar = 29479,
            Enlightenment = 29480,
            RisingPhoenix = 29481,
            RiddleOfEarth = 29482,
            ThunderClap = 29484,
            EarthsReply = 29483,
            Meteodrive = 29485;

        public static class Buffs
        {
            public const ushort
                WindResonance = 2007,
                FireResonance = 3170,
                EarthResonance = 3171;
        }

        public static class Debuffs
        {
            public const ushort
                PressurePoint = 3172;
        }

        internal class MNKPvP_Burst : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MNKPvP_Burst;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Bootshine or TrueStrike or SnapPunch or DragonKick or TwinSnakes or Demolish or PhantomRush)
                {

                    if (TargetHasEffectAny(SAMPvP.Buffs.Chiten) || TargetHasEffectAny(PLDPvP.Buffs.HallowedGround) || TargetHasEffectAny(DRKPvP.Buffs.UndeadRedemption) ||TargetHasEffectAny(PLDPvP.Buffs.Covered))
                        return OriginalHook(11);

                    if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
                    {
                        if (IsEnabled(CustomComboPreset.MNKPvP_Burst_RiddleOfEarth) && IsOffCooldown(RiddleOfEarth) && PlayerHealthPercentageHp() <= 95)
                            return OriginalHook(RiddleOfEarth);

                        if (IsEnabled(CustomComboPreset.MNKPvP_Burst_Thunderclap) && !HasEffect(Buffs.WindResonance) && GetRemainingCharges(ThunderClap) > 0)
                            return OriginalHook(ThunderClap);

                        if (CanWeave(actionID))
                        {
                            if (IsOffCooldown(SixSidedStar))
                                return OriginalHook(SixSidedStar);

                            if (IsEnabled(CustomComboPreset.MNKPvP_Burst_RiddleOfEarth) && HasEffect(Buffs.EarthResonance) && GetBuffRemainingTime(Buffs.EarthResonance) < 6)
                                return OriginalHook(EarthsReply);

                            if (GetRemainingCharges(RisingPhoenix) > 0 && !HasEffect(Buffs.FireResonance) && (lastComboMove is Demolish || IsOffCooldown(Enlightenment)))
                                return OriginalHook(RisingPhoenix);
                        }

                        if (HasEffect(Buffs.FireResonance))
                        {
                            if (lastComboMove is Demolish)
                                return OriginalHook(PhantomRush);

                            if (IsOffCooldown(Enlightenment))
                                return OriginalHook(Enlightenment);
                        }
                        if ((EnemyHealthCurrentHp() < 30000 && EnemyHealthCurrentHp()>1) && GetLimitBreakValue()==100)
                            return OriginalHook(Meteodrive);
                        if ((EnemyHealthCurrentHp() < 36000 && EnemyHealthCurrentHp() > 1) && TargetHasEffectAny(Debuffs.PressurePoint) && GetLimitBreakValue() == 100)
                            return OriginalHook(Meteodrive);
                    }
                    if(GetLimitBreakValue() == 100 && TargetHasEffect(Debuffs.PressurePoint) && (EnemyHealthCurrentHp() < 24000 && EnemyHealthCurrentHp() > 1 )&& TargetHasEffectAny(PvPCommon.Buffs.Guard)) 
                        return OriginalHook(Meteodrive);

                    if(GetLimitBreakValue() == 100 && !TargetHasEffect(Debuffs.PressurePoint)&& (EnemyHealthCurrentHp() < 16000 && EnemyHealthCurrentHp() > 1) && TargetHasEffectAny(PvPCommon.Buffs.Guard))
                        return OriginalHook(Meteodrive);
                    

                }

                return actionID;
            }
        }
    }
}