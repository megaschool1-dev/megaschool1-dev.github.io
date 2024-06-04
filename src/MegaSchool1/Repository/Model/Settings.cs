﻿using System.Text.Json.Serialization;
using MegaSchool1.Model;
using MegaSchool1.Model.Repository;

namespace MegaSchool1.Repository.Model;

public class Settings
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = default!;

    [JsonPropertyName("team_members")]
    public List<TeamMember> TeamMembers { get; set; } = [];

    [JsonPropertyName("fast_start_checklist")]
    public FastStartChecklist FastStartChecklist { get; set; } = new();

    [JsonPropertyName("givbuxCode")]
    public string? GivBuxCode { get; set; }

    [JsonPropertyName("livestream_platform_preference")]
    public Dictionary<string, LivestreamPlatform> LivestreamPlatformPreference { get; set; } = new()
    {
        { "72day-blitz", LivestreamPlatform.Facebook },
        { "72day-blitz-spanish", LivestreamPlatform.Facebook },
    };
}