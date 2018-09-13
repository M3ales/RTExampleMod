using RelationshipTooltips.Relationships;
using StardewValley;
using System;

namespace RTExampleMod
{
    public class MyNewRelationship : IRelationship
    {
        public Func<Character, Item, bool> ConditionsMet => (c,i) => true; //always display

        public int Priority => 5000;//higher is placed first/higher on tooltip

        public bool BreakAfter => false; //If this is true then all other relationships of lower priority will be skipped.

        public string GetDisplayText<T>(string currentDisplay, T character, Item item = null) where T : Character
        {
            return "This is the body text and is small";
        }

        public string GetHeaderText<T>(string currentHeader, T character, Item item = null) where T : Character
        {
            return "This is the header text and is large";
        }
    }
}
