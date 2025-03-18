using System;
using System.Collections.Generic;

namespace Spendo_Backend.Models;

public partial class Transaction
{
    public int Transactionid { get; set; }

    public int? Userid { get; set; }

    public int? Categoryid { get; set; }

    public decimal Amount { get; set; }

    public DateTime? Transactiondate { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }
}
