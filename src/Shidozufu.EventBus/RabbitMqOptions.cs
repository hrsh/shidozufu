namespace Shidozufu.EventBus
{
    public class RabbitMqOptions
    {
        public string Host { get; set; }

        public string Port { get; set; }

        public string Protocol { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ConnectionString =>
            $"{Protocol}://{Username}:{Password}@{Host}:{Port}";

        public PublishOptions PublishOptions { get; set; }

        public SubscribOptions SubscribOptions { get; set; }
    }
}
