namespace AutoMechanic.Common.Exceptions;

public class InternalValidationException : InternalLogicException
{
    public InternalValidationException(string message) : base(message)
    {
    }
}