﻿using System;
using System.Collections.Generic;

namespace Spendo_Backend.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
