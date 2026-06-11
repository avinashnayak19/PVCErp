using PVCErp.Domain.Common;

namespace PVCErp.Domain.Entities;

public sealed class Supplier : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? GstNumber { get; set; }
    public string? Phone { get; set; }
}

public sealed class Customer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? GstNumber { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}

public sealed class RawMaterial : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = "Kg";
    public decimal ReorderLevel { get; set; }
}

public sealed class Machine : BaseEntity
{
    public string MachineNumber { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public sealed class ScrewBarrel : BaseEntity
{
    public string BarrelNumber { get; set; } = string.Empty;
    public string? Type { get; set; }
    public decimal TargetKgPerHour { get; set; }
}
