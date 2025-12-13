using IntroductionToWebAPIs.Services.IService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Security.Cryptography;
using System.Text;

namespace IntroductionToWebAPIs.Services.Service
{
    public class SmsService : ISmsService
    {
        private readonly IDictionary<string, string> _config;

        public SmsService(IConfiguration configuration)
        {
            _config = new Dictionary<string, string>
            {
                ["dlm"] = configuration["SmsSettings:Dlm"]!,
                ["t"] = configuration["SmsSettings:T"]!,
                ["login"] = configuration["SmsSettings:Login"]!,
                ["pass_hash"] = configuration["SmsSettings:PassHash"]!,
                ["sender"] = configuration["SmsSettings:Sender"]!
            };
        }

        public async Task<string> SendSmsAsync(string phoneNumber, string message)
        {
            var txn_id = new Random().Next(100000, 100000000).ToString();
            var str_hash = Sha256Hash(txn_id + _config["dlm"] + _config["login"] + _config["dlm"] + _config["sender"] + _config["dlm"] + phoneNumber + _config["dlm"] + _config["pass_hash"]);

            var client = new RestClient("https://api.osonsms.com/sendsms_v1.php");
            var request = new RestRequest();
            request.Method = Method.Get;

            request.AddParameter("from", _config["sender"]);
            request.AddParameter("login", _config["login"]);
            request.AddParameter("t", _config["t"]);
            request.AddParameter("phone_number", phoneNumber);
            request.AddParameter("msg", message);
            request.AddParameter("str_hash", str_hash);
            request.AddParameter("txn_id", txn_id);

            var response = await client.ExecuteAsync(request);
            var content = response.Content;

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new Exception("Ответ от SMS-сервиса пустой.");
            }

            try
            {
                JObject joResponse = JObject.Parse(content);

                if (joResponse["error"] != null)
                {
                    return $"Ошибка: {joResponse["error"]?["msg"]}";
                }
                else
                {
                    return $"Сообщение успешно отправлено. ID: {joResponse["msg_id"]}";
                }
            }
            catch (JsonReaderException)
            {
                throw new Exception($"Ответ от SMS-сервиса не является валидным JSON: {content}");
            }
        }

        private string Sha256Hash(string value)
        {
            var sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));
                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
