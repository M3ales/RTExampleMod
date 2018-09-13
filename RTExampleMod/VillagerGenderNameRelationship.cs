using RelationshipTooltips.Relationships;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTExampleMod
{
    /// <summary>
    /// A Relationship which appends the NPC's gender to the end of their name.
    /// </summary>
    public class VillagerGenderNameRelationship : IRelationship
    {
        public Func<Character, Item, bool> ConditionsMet => CheckConditions;//Instead of a lambda we use a method to keep things clean.

        /// <summary>
        /// Checks if this text should be added to the Tooltip currently being displayed. This is a method implementation of Func<Character, Item, bool>. 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private bool CheckConditions(Character c, Item i)
        {
            //If its an NPC and Villager, then the header and display will be added to the tooltip.
            return c is NPC && ((NPC)c).isVillager();
        }

        public int Priority => -500;

        public bool BreakAfter => false;

        public string GetDisplayText<T>(string currentDisplay, T character, Item item = null) where T : Character
        {
            return "";
        }

        public string GetHeaderText<T>(string currentHeader, T character, Item item = null) where T : Character
        {
            NPC npc = character as NPC;
            if (npc == null)//just incase
                return "";
            return npc.Gender == NPC.male ? " - Male" : " - Female";
        }
    }
}
