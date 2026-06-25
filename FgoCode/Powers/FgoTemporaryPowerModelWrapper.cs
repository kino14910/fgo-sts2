using BaseLib.Abstracts;
using BaseLib.Extensions;
using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Powers;

public abstract class FgoTemporaryPowerModelWrapper<TModel, TPower> : CustomTemporaryPowerModelWrapper<TModel, TPower>
    where TModel : AbstractModel
    where TPower : PowerModel
{
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}