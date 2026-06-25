using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Fgo.FgoCode.Extensions;

public static class FgoDynamicVarSetExtensions
{
    /// <summary>
    ///     Get the NP (Noble Phantasm) variable.
    /// </summary>
    public static DynamicVar Np(this DynamicVarSet vars)
    {
        return vars["NP"];
    }

    /// <summary>
    ///     Get the Star variable.
    /// </summary>
    public static DynamicVar Star(this DynamicVarSet vars)
    {
        return vars["Star"];
    }
}