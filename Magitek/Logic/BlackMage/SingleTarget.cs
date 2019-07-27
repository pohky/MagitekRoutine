﻿using System.Threading.Tasks;
using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
using Magitek.Models.BlackMage;
using Magitek.Utilities;

namespace Magitek.Logic.BlackMage
{
    internal static class SingleTarget
    {
        public static async Task<bool> Xenoglossy()
        {
            // Requires Polyglot
            if (!ActionResourceManager.BlackMage.PolyglotStatus)
                return false;

            // If we're moving in combat
            if (Combat.MovingInCombatTime.ElapsedMilliseconds > 100)
            {
                // If we don't have procs (while in movement), cast
                if (!Core.Me.HasAura(Auras.ThunderCloud) && !Core.Me.HasAura(Auras.FireStarter))
                    return await Spells.Xenoglossy.Cast(Core.Me.CurrentTarget);
            }
            //If while in Umbral 3 and, we didn't use Thunder in the Umbral window
            if (ActionResourceManager.BlackMage.UmbralStacks == 3 && Casting.LastSpell != Spells.Thunder3)
            {
                //We don't have max mana
                if(Core.Me.CurrentMana < 10000)
                    return await Spells.Xenoglossy.Cast(Core.Me.CurrentTarget);
            }
        return false;        
        }

        public static async Task<bool> Despair()
        {
            if (!Core.Me.HasEnochian())
                return false;

            // If we're not in astral, stop
            if (ActionResourceManager.BlackMage.AstralStacks != 3)
                return false;

            // If our mana is lower than 2400, cast
            if (Core.Me.CurrentMana < 2400)
                return await Spells.Despair.Cast(Core.Me.CurrentTarget);

            return false;
        }

        public static async Task<bool> Fire()
        {
            if (ActionResourceManager.BlackMage.AstralStacks != 3)
                return false;

            if (ActionResourceManager.BlackMage.StackTimer.TotalMilliseconds > 5000)
                return false;

            return await Spells.Fire.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> Fire4()
        {
            if (!Core.Me.HasEnochian())
                return false;

            // If we need to refresh stack timer, stop
            if (ActionResourceManager.BlackMage.StackTimer.TotalMilliseconds <= 5000)
                return false;

            // If we have 3 astral stacks and our mana is equal to or greater than 2400
            if (ActionResourceManager.BlackMage.AstralStacks == 3 && Core.Me.CurrentMana >= 2400)
                return await Spells.Fire4.Cast(Core.Me.CurrentTarget);

            return false;
        }

        public static async Task<bool> Fire3()
        {
            // Use if we're in Umbral and we have 3 hearts
            if (ActionResourceManager.BlackMage.UmbralHearts == 3 && ActionResourceManager.BlackMage.UmbralStacks == 3)
                return await Spells.Fire3.Cast(Core.Me.CurrentTarget);

            // If we're moving in combat
            if (Combat.MovingInCombatTime.ElapsedMilliseconds > 100)
            {
                // If we have the proc, cast
                if (Core.Me.HasAura(Auras.FireStarter))
                    return await Spells.Fire3.Cast(Core.Me.CurrentTarget);
            }

            // Use if we're in Astral and we have Firestarter, but Firestarter has 3 seconds or less left
            if (ActionResourceManager.BlackMage.AstralStacks > 0 && Core.Me.HasAura(Auras.FireStarter) && !Core.Me.HasAura(Auras.FireStarter, true, 3000))
                return await Spells.Fire3.Cast(Core.Me.CurrentTarget);

            //Use if we're at the end of Astral phase and we have a Fire3 proc
            if (ActionResourceManager.BlackMage.AstralStacks > 0 && Core.Me.HasAura(Auras.FireStarter) && Core.Me.CurrentMana <= 1200)
                return await Spells.Fire3.Cast(Core.Me.CurrentTarget);
                  
            return false;
        }

        public static async Task<bool> Thunder3()
        {
            // If we need to refresh stack timer, stop
            if (ActionResourceManager.BlackMage.StackTimer.TotalMilliseconds <= 5000)
                return false;

            // If the last spell we cast is triple cast, stop
            if (Casting.LastSpell == Spells.Triplecast)
                return false;
            // It takes a second for thunder dot to actually hit the boss...
            if (Casting.LastSpell == Spells.Thunder3)
                return false;

            // If we have the triplecast aura, stop
            if (Core.Me.HasAura(Auras.Triplecast))
                return false;

            // If we have thunder cloud, but we don't have at least 3 seconds of it left, use the proc
            if (Core.Me.HasAura(Auras.ThunderCloud) && !Core.Me.HasAura(Auras.ThunderCloud, true, 3000))
                return await Spells.Thunder3.Cast(Core.Me.CurrentTarget);

            // Refresh thunder if it's about to run out
            if (!Core.Me.CurrentTarget.HasAura(Auras.Thunder3, true, BlackMageSettings.Instance.ThunderRefreshSecondsLeft))
                return await Spells.Thunder3.Cast(Core.Me.CurrentTarget);

            return false;
        }

        public static async Task<bool> Blizzard4()
        {
            if (!Core.Me.HasEnochian())
                return false;

            // If we need to refresh stack timer, stop
            if (ActionResourceManager.BlackMage.StackTimer.TotalMilliseconds <= 5000)
                return false;

            // While in Umbral 
            if (ActionResourceManager.BlackMage.UmbralStacks > 0)
            {
                // If we have less than 3 hearts, cast
                if (ActionResourceManager.BlackMage.UmbralHearts != 3)
                    return await Spells.Blizzard4.Cast(Core.Me.CurrentTarget);
            }

            return false;
        }

        public static async Task<bool> Blizzard3()
        {
            // If we have no umbral or astral stacks, cast 
            if (ActionResourceManager.BlackMage.AstralStacks == 0 && ActionResourceManager.BlackMage.UmbralStacks == 0)
                return await Spells.Blizzard3.Cast(Core.Me.CurrentTarget);

            // If our mana is less than 800 while in astral
            if (ActionResourceManager.BlackMage.AstralStacks > 0 && Core.Me.CurrentMana < 800)
                return await Spells.Blizzard3.Cast(Core.Me.CurrentTarget);

            return false;
        }
    }
}
