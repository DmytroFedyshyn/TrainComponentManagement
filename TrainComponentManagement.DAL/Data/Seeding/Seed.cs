using Microsoft.EntityFrameworkCore;
using TrainComponentManagement.DAL.Models;

namespace TrainComponentManagement.DAL.Data.Seeding
{
    public static class ComponentDataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Component>().HasData(
                new Component { Id = 1, Name = "Engine", UniqueNumber = "ENG123", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 2, Name = "Passenger Car", UniqueNumber = "PAS456", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 3, Name = "Freight Car", UniqueNumber = "FRT789", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 4, Name = "Wheel", UniqueNumber = "WHL101", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 5, Name = "Seat", UniqueNumber = "STS234", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 6, Name = "Window", UniqueNumber = "WIN567", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 7, Name = "Door", UniqueNumber = "DR123", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 8, Name = "Control Panel", UniqueNumber = "CTL987", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 9, Name = "Light", UniqueNumber = "LGT456", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 10, Name = "Brake", UniqueNumber = "BRK789", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 11, Name = "Bolt", UniqueNumber = "BLT321", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 12, Name = "Nut", UniqueNumber = "NUT654", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 13, Name = "Engine Hood", UniqueNumber = "EH789", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 14, Name = "Axle", UniqueNumber = "AX456", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 15, Name = "Piston", UniqueNumber = "PST789", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 16, Name = "Handrail", UniqueNumber = "HND234", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 17, Name = "Step", UniqueNumber = "STP567", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 18, Name = "Roof", UniqueNumber = "RF123", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 19, Name = "Air Conditioner", UniqueNumber = "AC789", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 20, Name = "Flooring", UniqueNumber = "FLR456", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 21, Name = "Mirror", UniqueNumber = "MRR789", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 22, Name = "Horn", UniqueNumber = "HRN321", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 23, Name = "Coupler", UniqueNumber = "CPL654", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 24, Name = "Hinge", UniqueNumber = "HNG987", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 25, Name = "Ladder", UniqueNumber = "LDR456", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 26, Name = "Paint", UniqueNumber = "PNT789", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 27, Name = "Decal", UniqueNumber = "DCL321", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 28, Name = "Gauge", UniqueNumber = "GGS654", CanAssignQuantity = true, Quantity = 0 },
                new Component { Id = 29, Name = "Battery", UniqueNumber = "BTR987", CanAssignQuantity = false, Quantity = null },
                new Component { Id = 30, Name = "Radiator", UniqueNumber = "RDR456", CanAssignQuantity = false, Quantity = null }
            );
        }
    }
}