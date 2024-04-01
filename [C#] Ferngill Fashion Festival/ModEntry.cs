
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.SpecialOrders;

namespace FashionFestivalSMAPI
{
    internal sealed class ModEntry : Mod
    {
        private static Harmony harmony;
        public override void Entry(IModHelper helper)
        {
            ModEntry.harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(SpecialOrder), nameof(SpecialOrder.IsTimedQuest)),
                postfix: new HarmonyMethod(typeof(ModEntry), nameof(SpecialOrders_IsTimed_postfix))
            );
            Helper.Events.GameLoop.DayEnding += OnDayEnding;
        }

        private void OnDayEnding(object sender, DayEndingEventArgs e)
        {
            if (!Context.IsMainPlayer)
                return;

            foreach (SpecialOrder o in Game1.player.team.specialOrders)
            {
                if (o.questKey.Value.StartsWith("Emily_FashionShow"))
                {
                    o.dueDate.Value = Game1.Date.TotalDays + 100;
                }
            }
        }

        private static void SpecialOrders_IsTimed_postfix(ref SpecialOrder __instance, ref bool __result)
        {
            if (__instance.questKey.Value.Equals("Emily_FashionShow"))
            {
                __result = false;
            }
        }
    }
}