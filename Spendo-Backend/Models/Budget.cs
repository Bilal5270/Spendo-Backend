﻿using System;
using System.Collections.Generic;

namespace Spendo_Backend.Models;

public partial class Budget
{
    public int BudgetId { get; set; }

    public int? UserId { get; set; }

    public int? CategoryId { get; set; }

    public decimal Amount { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }
}
