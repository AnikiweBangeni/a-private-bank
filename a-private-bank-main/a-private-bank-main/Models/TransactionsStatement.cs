using System;
using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace a_private_bank_main.Models;

public partial class TransactionsStatement
{
    public int Id { get; set; }

    [Name ("Nr")]
    public int Nr { get; set; }

    public long Account { get; set; }

    [Name("Posting Date")]
    public DateTime PostingDate { get; set; }

    [Name("Transaction Date")]
    public DateTime TransactionDate { get; set; }
    
    public string Description { get; set; } = null!;

    [Name("Original Description")]
    public string OriginalDescription { get; set; } = null!;

    [Name("Parent Category")]
    public string ParentCategory { get; set; } = null!;

    public string Category { get; set; } = null!;

    [Name("Money In")]
    public decimal? MoneyIn { get; set; }

    [Name("Money Out")]
    public decimal? MoneyOut { get; set; }

    public decimal? Fee { get; set; }

    public decimal? Balance { get; set; }
}
