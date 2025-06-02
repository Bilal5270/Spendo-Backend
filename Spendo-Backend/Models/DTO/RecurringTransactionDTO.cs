namespace Spendo_Backend.Models.DTO
{
    public class RecurringTransactionDTO
    {
        public int RecurringId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal OriginalAmount { get; set; }
        public string RecurrenceInterval { get; set; }

        public decimal WeeklyAmount { get; set; }
        public decimal MonthlyAmount { get; set; }
        public decimal YearlyAmount { get; set; }
    }

}
