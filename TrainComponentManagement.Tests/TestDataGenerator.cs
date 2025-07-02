using Faker;
using TrainComponentManagement.BLL.DTOs;

namespace TrainComponentManagement.Tests
{
    public static class TestDataGenerator
    {
        public static CreateOrUpdateComponentDto NewCreateDto()
        {
            bool canAssign = RandomNumber.Next(2) == 1;

            return new CreateOrUpdateComponentDto
            {
                Name = Faker.Name.FullName(),

                UniqueNumber = RandomNumber.Next(10000, 99999).ToString(),

                CanAssignQuantity = canAssign,

                Quantity = canAssign
                                      ? RandomNumber.Next(0, 100)
                                      : (int?)null
            };
        }

        public static ComponentDto NewComponentDto()
        {
            var c = NewCreateDto();
            return new ComponentDto
            {
                Id = RandomNumber.Next(1, 1000),
                Name = c.Name,
                UniqueNumber = c.UniqueNumber,
                CanAssignQuantity = c.CanAssignQuantity,
                Quantity = c.Quantity
            };
        }
    }
}
