namespace Kwetterprise.ApiGateway.Business.Models
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class Service : IEquatable<Service>
    {
        public const string GuidPropertyName = "guid";
        public const string ServiceNamePropertyName = "serviceName";
        public const string BaseUrlPropertyName = "baseUrl";

        public Service()
        {

        }

        public Service(Service service)
        : this(service.Guid, service.Name, service.Url)
        {

        }

        public Service(Guid guid, string name, string url)
        {
            this.Guid = guid;
            this.Name = name;
            this.Url = url;
        }

        /// <summary>
        /// Gets the guid of the service instance.
        /// </summary>
        [JsonPropertyName(Service.GuidPropertyName)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        [JsonPropertyName(Service.ServiceNamePropertyName)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets the URI the service can be reached at.
        /// </summary>
        [JsonPropertyName(Service.BaseUrlPropertyName)]
        public string Url { get; set; } = null!;

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public bool Equals(Service? other)
        {
            if (object.ReferenceEquals(null, other)) return false;
            if (object.ReferenceEquals(this, other)) return true;
            return this.Guid.Equals(other.Guid) && this.Name == other.Name && this.Url == other.Url;
        }

        public override bool Equals(object? obj)
        {
            if (object.ReferenceEquals(null, obj)) return false;
            if (object.ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((Service)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Guid, this.Name, this.Url);
        }

        public static bool operator ==(Service left, Service right)
        {
            return object.Equals(left, right);
        }

        public static bool operator !=(Service left, Service right)
        {
            return !object.Equals(left, right);
        }

        public override string ToString()
        {
            return $"Name: {this.Name} | Guid: {this.Guid} | Url: {this.Url}";
        }
    }
}