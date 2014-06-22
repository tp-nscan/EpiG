using MathUtils.Collections;

namespace Workflows
{
    public interface IEntity : IGuid, IGuidParts
    {
        string EntityName { get; }
    }
}
