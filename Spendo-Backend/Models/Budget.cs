using System;
using System.Collections.Generic;

namespace Spendo_Backend.Models;

public partial class Budget
{
    public int Budgetid { get; set; }

    public int? Userid { get; set; }

    public int? Categoryid { get; set; }

    public decimal Amount { get; set; }

    public DateTime Startdate { get; set; }

    public DateTime Enddate { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }
}
