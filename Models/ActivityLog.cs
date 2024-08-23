namespace Inventory_PLM.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }  // Unique identifier for the log entry
        public DateTime Timestamp { get; set; }  // When the action took place
        public string Action { get; set; }  // Description of the action performed
        public string? Details { get; set; }  // Additional details or context
        public int UserId { get; set; }  // Foreign key to the user who performed the action

        // Optional: Navigation property to link back to the user (if using EF Core)
        public virtual User User { get; set; }
    
    }
}
