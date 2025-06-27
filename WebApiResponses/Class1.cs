using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApiResponses
{
    // Models/Steam/SteamGameResponse.cs
    public class SteamGameResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public SteamGameData Data { get; set; }
    }

    public class SteamGameData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("short_description")]
        public string ShortDescription { get; set; }

        [JsonPropertyName("header_image")]
        public string HeaderImage { get; set; }

        [JsonPropertyName("achievements")]
        public SteamAchievements Achievements { get; set; }
    }

    public class SteamAchievements
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("highlighted")]
        public List<SteamAchievement> Highlighted { get; set; }
    }

    public class SteamAchievement
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }
    }
}
