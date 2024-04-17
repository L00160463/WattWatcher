using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public class SmsService
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromNumber; 

    public SmsService(string accountSid, string authToken, string fromNumber)
    {
        _accountSid = accountSid;
        _authToken = authToken;
        _fromNumber = fromNumber;

        TwilioClient.Init(_accountSid, _authToken);
    }

    public void SendSms(string to, string message)
    {
        var toWhatsApp = $"whatsapp:{to}";  // Ensures the number is correctly formatted for WhatsApp usage
        var fromWhatsApp = $"whatsapp:{_fromNumber}";

        var messageOptions = new CreateMessageOptions(new PhoneNumber(toWhatsApp))
        {
            From = new PhoneNumber(fromWhatsApp),
            Body = message
        };

        MessageResource.Create(messageOptions);
    }
}
