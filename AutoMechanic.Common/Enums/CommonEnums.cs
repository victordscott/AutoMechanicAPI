using System.Text.Json.Serialization;

namespace AutoMechanic.Common.Enums
{
    public enum FileTypeEnum
    {
        Image = 1,
        Video = 2,
        PDF = 3,
        Text = 4
    }

    public enum ApiErrorCode
    {
        InvalidEmailAddress = 1,
        RegisterEmailAddressExists = 2,
        SetUserRoleError = 3,
        CreateCustomerError = 4
    }
}
