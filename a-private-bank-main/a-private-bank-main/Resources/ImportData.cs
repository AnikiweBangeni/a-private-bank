using a_private_bank_main.Contracts;
using a_private_bank_main.Models;
using a_private_bank_main.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_private_bank_main.Resources
{
    public class ImportData : IDataResourceContracts
    {
        private readonly AprivateBankContext _dbContext;

        public ImportData(AprivateBankContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TransactionsStatement>> ImportDataDocAsync( )
        {
            try
            {
                var folderPath = @"C:\Users\0209255666080\source\Projects 2026\Documents";

        var dataDocFile = new DirectoryInfo(folderPath)
            .GetFiles("*.csv")
            .OrderByDescending(x => x.LastWriteTime)
            .FirstOrDefault();

        if (dataDocFile == null)
        {
            throw new FileNotFoundException("No CSV files found in the folder.");
        }
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    PrepareHeaderForMatch = args => args.Header?.Trim(),
                    TrimOptions = TrimOptions.Trim
                };

                using (var readDoc = new StreamReader(dataDocFile.FullName))

                using (var docContent = new CsvReader(readDoc, config))
                {
                    var docRecords = docContent.GetRecords<TransactionsStatement>().Where(x => x.Balance >= 0).ToList();


                    if(docRecords == null || !docRecords.Any())
                    {
                        throw new InvalidOperationException("No valid records found in the CSV file.");
                    }

                    return docRecords;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        public async Task<bool> PostImportedDataToDbAsync( )
        {
            var data = await this.ImportDataDocAsync();

            if (data == null || !data.Any())
                throw new InvalidOperationException("No data to import. ImportDataDocAsync returned null or empty.");

            var existing = await _dbContext.TransactionsStatements
                .Select(x => x.Nr)
                .ToListAsync();

            var newRecords = data
                .Where(c => !existing.Contains(c.Nr))
                .Select(x => new TransactionsStatement
                {
                    Account = x.Account,
                    Balance = x.Balance,
                    Category = x.Category,
                    Description = x.Description,
                    Fee = x.Fee,
                    MoneyIn = x.MoneyIn,
                    MoneyOut = x.MoneyOut,
                    Nr = x.Nr,
                    OriginalDescription = x.OriginalDescription,
                    ParentCategory = x.ParentCategory,
                    PostingDate = x.PostingDate,
                    TransactionDate = x.TransactionDate
                })
                .ToList();

            await _dbContext.TransactionsStatements.AddRangeAsync(newRecords);

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
    
}
