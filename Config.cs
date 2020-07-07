
class ConfigDef : Def.Def
{
    public string dataDir;

    public int validity_totalKarma;
    public int validity_bestKarma;
    public int validity_uniqueDates;
}

[Def.StaticReferences]
static class Config
{
    static Config() { Def.StaticReferencesAttribute.Initialized(); }

    public static ConfigDef Global;
}
