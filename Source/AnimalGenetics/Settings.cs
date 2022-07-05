using Verse;

namespace AnimalGenetics;

public static class Settings
{
    public static UISettings UI;
    public static IntegrationSettings Integration;
    public static CoreSettings InitialCore;

    public static CoreSettings Core => Find.World.GetComponent<AnimalGenetics>().Settings;
}