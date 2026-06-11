namespace PVCErp.Domain.Common;

public enum QualityStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3
}

public enum ProductionStage
{
    PipeProduction = 1,
    Socketing = 2,
    Crushing = 3,
    PulverizerConversion = 4
}

public enum DispatchStatus
{
    Pending = 1,
    PartiallyDispatched = 2,
    Dispatched = 3,
    Invoiced = 4
}

public enum PaymentStatus
{
    Unpaid = 1,
    PartiallyPaid = 2,
    Paid = 3
}
