using System;
using MathUtils.Collections;

namespace MathUtils
{
    public interface IEntity : IGuid
    {
        string EntityName { get; }
        IEntity GetPart(Guid key);
    }
}
