﻿using Bookcase.Events;
using RelationshipTooltips.API;
using StardewModdingAPI;

namespace RTExampleMod
{
    public class RTExampleMod : Mod
    {
        public override void Entry(IModHelper helper)
        {
            //Register with Bookcase's FirstGameTick event.
            //Priority defaults to Priority.Normal which is alright for our case.
            BookcaseEvents.FirstGameTick.Add(RegisterWithRT);
        }
        private void RegisterWithRT(Event e)
        {
            //Get the API
            IRelationshipAPI api = Helper.ModRegistry.GetApi<IRelationshipAPI>("M3ales.RelationshipTooltips");
            //Subscribe to the Register event (Auto Generated Subscription Method)
            //Lambda is probably easiest
            api.RegisterRelationships += Api_RegisterRelationships;
        }

        private void Api_RegisterRelationships(object sender, EventArgsRegisterRelationships e)
        {
            e.Relationships.Add(new MyNewRelationship());
        }
    }
}
