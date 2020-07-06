
class ConfigDef : Def.Def
{
    public string dataDir;
}

[Def.StaticReferencesAttribute]
static class Config
{
    static Config() { Def.StaticReferencesAttribute.Initialized(); }

    public static ConfigDef Global;
}
