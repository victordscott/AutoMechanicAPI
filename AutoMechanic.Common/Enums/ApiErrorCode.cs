using System.Text.Json.Serialization;

namespace AutoMechanic.Common.Enums
{
    public enum ApiErrorCode
    {
        InvalidEmailAddress = 1,
        RegisterEmailAddressExists = 2,
        SetUserRoleError = 3,
        CreateCustomerError = 4
    }
}
