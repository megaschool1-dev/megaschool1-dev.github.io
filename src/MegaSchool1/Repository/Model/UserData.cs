﻿using System.Text.Json.Serialization;
using MegaSchool1.Model;

namespace MegaSchool1.Repository.Model;

public class UserData
{
    [JsonPropertyName("prospects")]
    public Dictionary<Strategy, List<Prospect>> Prospects { get; set; } = [];

    [JsonPropertyName("team_data")]
    public List<TeamMemberData> TeamData { get; set; } = [];

    [JsonPropertyName("last_page")]
    public string? LastPage { get; set; }
}