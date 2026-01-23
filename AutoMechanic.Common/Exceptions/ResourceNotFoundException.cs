namespace AutoMechanic.Common.Exceptions;

public class ResourceNotFoundException : InternalValidationException
{
    public ResourceNotFoundException(Type resourceType, string resourceId) : base(
        $"The requested resource {resourceType.Name} with ID {resourceId} was not found.")
    {
    }

    public ResourceNotFoundException(Type resourceType) : base(
        $"The requested resource {resourceType.Name} was not found.")
    {
    }
}
