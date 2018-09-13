using RelationshipTooltips.Relationships;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTExampleMod
{
    public class MonsterHealthRelationship : IRelationship
    {
        private IMonitor Monitor;
        public MonsterHealthRelationship(IMonitor monitor)
        {
            Monitor = monitor;
        }
        public Func<Character, Item, bool> ConditionsMet => (c,i) => c is Monster;

        public int Priority => 10000;

        public bool BreakAfter => false;

        public string GetDisplayText<T>(string currentDisplay, T character, Item item = null) where T : Character
        {
            return "";
        }

        public string GetHeaderText<T>(string currentHeader, T character, Item item = null) where T : Character
        {
            Monster m = character as Monster;
            Monitor.Log(m.ToString());
            if (m != null)
            {
                int hp = m.Health;
                int maxHP = m.MaxHealth;
                return $"{hp}/{maxHP}";
            }
            return "";
        }
    }
}
