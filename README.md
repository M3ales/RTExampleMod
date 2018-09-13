# RTExampleMod
Example Mod for [SMAPI 2.7](https://smapi.io/), [Relationship Tooltips 2.0.0](https://github.com/M3ales/RelationshipTooltips/releases/) and [Bookcase 0.5.0](https://stardewvalley.curseforge.com/projects/bookcase/)

## API
As of RelationshipTooltips 2.0.0-beta.2 you can now add your own text to the relationship tooltips mod. It currently will search for *anything* inheriting from `StardewValley.Character` and allow you to provide conditions for different text displays. It's currently limited to text, but hopefully in future this will expand to Images.
### Installation
#### Requirements
* [SMAPI 2.6 or later](https://smapi.io/) (Preferably later, but it should still work in installs down to SMAPI 2.0)
* [RelationshipTooltips 2.0.0](https://github.com/M3ales/RelationshipTooltips/releases) or later installed in `StardewValley\Mods`
* [Bookcase 0.5.0](https://stardewvalley.curseforge.com/projects/bookcase/files) or later installed in `StardewValley\Mods`
* Stardew Valley 1.3 or later.
* Mod targetting .net 4.5.2 or higher. (4.5.2 recommended) [You can change this - details below]
#### Referencing the RT API
You'll need to add `RelationshipTooltips.dll` as a reference within your own mod's project. An example using Visual Studio is detailed below.

Ensure you are running .net 4.5.2 or higher for your project. If making a new project set the target framework to 4.5.2. 

![picture alt](https://i.imgur.com/vXx8XBO.png "Double click on Properties in Soltuion Explorer")

![picture alt](https://i.imgur.com/3DQO6IS.png "Set your Target Framework to 4.5.2")

Add Relationship Tooltips and Bookcase as References.

![picture alt](https://i.imgur.com/2uORNVY.png "Add Reference via Solution Explorer")

![picture alt](https://i.imgur.com/O6OzcqP.png "Click Browse, then Add Relationship Tooltips")

![picture alt](https://i.imgur.com/QMJFpVn.png "Click Browse, then Add Bookcase")

![picture alt](https://i.imgur.com/AvZ74rl.png "Your References should look something like this.")

Click OK to confirm, then check on in the solution explorer that the references have been imported.

![picture alt](https://i.imgur.com/Ld7gEeK.png "Your References should look something like this.")

Make sure copyToLocal is false for Bookcase and RT.dlls.

![picture alt](https://i.imgur.com/w55IC9F.png "Set CopyToLocal false")

#### Setting up API registration
Setup your Mod's Entry() method as you would with any other SMAPI mod.

Add a method registration with `BookcaseEvents.FirstGameUpdate` using `Priority.Low`, or Higher. (Init is performed at `Priority.Lowest` so to ensure your mod is recognised you need to make sure you are using a higher priority)
```csharp
using Bookcase.Events;
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
          //We'll register all the plugin relationships here later.
        }
    }
}
```
#### Creating and adding a Relationship
Next create your relationship (plugin) type. To be valid it must implement `RelationshipTooltips.Relationships.IRelationship`.
```csharp
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
```
Going back to your Mod's main entry file, we can add the new relationship we just made by adding it to the param's Relationship list for OnHover relationships:
```csharp
private void Api_RegisterRelationships(object sender, EventArgsRegisterRelationships e)
{
  e.RelationshipsOnHover.Add(new MyNewRelationship());
}
```
If we want the tooltip to always display we can do this instead:
```csharp
private void Api_RegisterRelationships(object sender, EventArgsRegisterRelationships e)
{
  e.RelationshipsOnScreen.Add(new MyNewRelationship());
}
```
Combining the two will provide behaviour where the tooltip will appear on screen, and be replaced by the hover tooltip when the player moves their mouse over the specified character. (Give it a test to see what I mean)
```csharp
private void Api_RegisterRelationships(object sender, EventArgsRegisterRelationships e)
{
  e.RelationshipsOnHover.Add(new MyNewRelationship());
  e.RelationshipsOnScreen.Add(new MyNewRelationship());
}
```

#### Manifest
Finally we can setup our mod's manifest.json file to include dependancies for RT so that you wont end up with SMAPI compatibility failures because your mod is crashing due to invalid load order.

You'll want to add a Dependency snippet to the end of your `manifest.json`.
```json
  "Dependencies": [
    {
      "UniqueID": "M3ales.RelationshipTooltips",
      "MinimumVersion": "2.0.0"
    }
```

The full `manifest.json` should look something like this.
```json
{
  "Name": "RelationshipTooltips Example Mod",
  "Author": "M3ales",
  "Version": "1.1.0",
  "Description": "Demonstrates a basic integration of the RT API.",
  "UniqueID": "M3ales.RTExampleMod",
  "EntryDll": "RTExampleMod.dll",
  "MinimumApiVersion": "2.0",
  "UpdateKeys": [ "GitHub:M3ales/RTExampleMod" ],
  "Dependencies": [
    {
      "UniqueID": "M3ales.RelationshipTooltips",
      "MinimumVersion": "2.0.0"
    }
  ]
}
```

To test that everything is working, build and run your Mod, RT will output a list of registered Relationships at the end of it's init, which hopefully is after your registration (Bookcase should ensure this).

We can test if it is working by checking SMAPI console after startup. RT will log all registered relationships post init, which means we can easily see if it's a loading/registration issue.

![picture alt](https://i.imgur.com/BUn4KGR.png "The mod shows up as you can see, with Priority 5000")

And ingame we see:

![picture alt](https://i.imgur.com/WEoTjD3.png "The mod stacking with NPCRelationship and VillagerBirthdayRelationship")

As you can see since we have `BreakAfter => false;` The mod has continued to add the other Relationships after ours, but if we'd used `BreakAfter => true;` we would only see the text of our mod (since it has the highest Priority at 5000).

Moving on to a more useful example, you can make a simple mod to append the NPC's gender to their name by:

```csharp
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

```
And we need to disable the first mod we made - we can just comment it out for now.
```csharp
private void Api_RegisterRelationships(object sender, EventArgsRegisterRelationships e)
{
            //Just commenting out this part so we can try other mods.
            //e.RelationshipsOnHover.Add(new MyNewRelationship());
            //e.RelationshipsOnScreen.Add(new MyNewRelationship());
            e.RelationshipsOnHover.Add(new VillagerGenderNameRelationship());
            e.RelationshipsOnHover.Add(new MonsterHealthRelationship(Monitor));
}
```
Building the solution and running it will yield:

![picture alt](https://i.imgur.com/7csqOGn.png "The mod shows up as you can see, with Priority 5000")

## Notes on API
* Don't use Priorities which are single increments of eachother unless you are **EXPLICITLY** intending them never to have anything run inbetween. (Someone who wants to put something between will just change your priorities anyways, so no point)
* Try stay away from Priorities which are the same, since their ordering is random dependant on the load order.
* You can see what other elements have been registered - and their priorities by iterating through `e.Relationships`.
