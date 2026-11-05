namespace Trusted.Core.Enums
{
    public enum DealStatus
    {
        Pending = 0,     // Buyer initiated, waiting for mediator to accept
        Active = 1,      // Mediator accepted, deal in progress
        Completed = 2,   // Deal completed successfully
        Cancelled = 3,   // Deal was cancelled
        Disputed = 4     // Deal is under dispute
    }
}

