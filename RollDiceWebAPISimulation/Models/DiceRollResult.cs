﻿using System.Text.Json.Serialization;

namespace RollDiceWebAPISimulation.Models;

public class DiceRollResults
{
    public int Id { get; set; }
    public int DiceRollId { get; set;  }
    public int Result { get; init; }

    [JsonIgnore]
    public DiceRoll? DiceRoll { get; set; }
}