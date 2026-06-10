namespace SampSharp
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("SampSharp.SourceGenerator", "1.0.0.0")]
    public static class Entrypoint
    {
        private static readonly global::CTF.Host.Startup _startup = new();
        private static SampSharp.OpenMp.Core.StartupContext _context;
        [global::System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute]
        public static void Initialize(SampSharp.OpenMp.Core.SampSharpInitParams inf)
        {
            _context = new SampSharp.OpenMp.Core.StartupContext(inf);
            _context.InitializeUsing(_startup);
        }

        public static void Main()
        {
            SampSharp.OpenMp.Core.StartupContext.MainInfoProvider();
        }
    }
}
