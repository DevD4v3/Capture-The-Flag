namespace SampSharp.Entities.SAMP.Commands;

/// <summary>
/// Temporary compatibility shim for legacy SampSharp command parameter aliases.
/// Remove once native support is restored.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)] 
public class CommandParameterAttribute : Attribute 
{ 
    public string Name { get; set; }  
    public Type Parser { get; set; } 
}
