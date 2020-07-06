using System;

class Program
{
    static void Main(string[] args)
    {
        Def.Config.DefaultHandlerThrowExceptions = Def.Config.DefaultExceptionBehavior.Never;

        {
            var parser = new Def.Parser();
            parser.AddFile("xml/config.xml");
            parser.Finish();
        }
            
        Dbg.Inf(Config.Global.dataDir);
    }
}
