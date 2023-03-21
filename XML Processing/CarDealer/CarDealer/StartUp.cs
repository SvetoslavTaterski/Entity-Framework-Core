using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();

        //string inputXml = File.ReadAllText("../../../Datasets/sales.xml");

        //string result = ImportSales(context, inputXml);

        string result = GetSalesWithAppliedDiscount(context);

        Console.WriteLine(result);
    }

    public static string ImportSuppliers(CarDealerContext context, string inputXml)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var suppliersDto = xmlHelper.Deserialize<ImportSuppliersDto[]>(inputXml, "Suppliers");

        ICollection<Supplier> validSuppliers = new HashSet<Supplier>();

        foreach (var supplierDto in suppliersDto)
        {
            Supplier supplier = new Supplier()
            {
                Name = supplierDto.Name,
                IsImporter = supplierDto.IsImporter
            };

            validSuppliers.Add(supplier);
        }

        context.Suppliers.AddRange(validSuppliers);
        context.SaveChanges();

        return $"Successfully imported {validSuppliers.Count}";
    }

    public static string ImportParts(CarDealerContext context, string inputXml)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var parts = xmlHelper.Deserialize<ImportPartsDto[]>(inputXml, "Parts");

        ICollection<Part> validParts = new HashSet<Part>();

        int[] suppliersIds = context.Suppliers.Select(s => s.Id).ToArray();

        foreach (var partDto in parts)
        {
            if (!suppliersIds.Contains(partDto.SupplierId))
            {
                continue;
            }

            Part part = new Part()
            {
                Name = partDto.Name,
                Price = partDto.Price,
                Quantity = partDto.Quantity,
                SupplierId = partDto.SupplierId
            };

            validParts.Add(part);
        }

        context.Parts.AddRange(validParts);
        context.SaveChanges();

        return $"Successfully imported {validParts.Count}";
    }

    public static string ImportCars(CarDealerContext context, string inputXml)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var carsDtos = xmlHelper.Deserialize<ImportCarsDto[]>(inputXml, "Cars");

        List<Car> cars = new List<Car>();
        List<PartCar> partCars = new List<PartCar>();
        int[] allPartIds = context.Parts.Select(p => p.Id).ToArray();
        int carId = 1;

        foreach (var dto in carsDtos)
        {
            Car car = new Car()
            {
                Make = dto.Make,
                Model = dto.Model,
                TraveledDistance = dto.TraveledDistance
            };

            cars.Add(car);

            foreach (int partId in dto.Parts
                         .Where(p => allPartIds.Contains(p.PartId))
                         .Select(p => p.PartId)
                         .Distinct())
            {
                PartCar partCar = new PartCar()
                {
                    CarId = carId,
                    PartId = partId
                };
                partCars.Add(partCar);
            }
            carId++;
        }

        context.Cars.AddRange(cars);
        context.PartsCars.AddRange(partCars);
        context.SaveChanges();

        return $"Successfully imported {cars.Count}";
    }

    public static string ImportCustomers(CarDealerContext context, string inputXml)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var customerDtos = xmlHelper.Deserialize<ImportCustomersDto[]>(inputXml, "Customers");

        ICollection<Customer> customers = new HashSet<Customer>();

        foreach (var customerDto in customerDtos)
        {
            Customer customer = new Customer()
            {
                Name = customerDto.Name,
                BirthDate = customerDto.BirthDate,
                IsYoungDriver = customerDto.IsYoungDriver
            };

            customers.Add(customer);
        }

        context.Customers.AddRange(customers);
        context.SaveChanges();

        return $"Successfully imported {customers.Count}";
    }

    public static string ImportSales(CarDealerContext context, string inputXml)
    {
        XmlHelper xmlHelper = new XmlHelper();

        int[] carIds = context.Cars.Select(x => x.Id).ToArray();

        var salesDtos = xmlHelper.Deserialize<ImportSaleDto[]>(inputXml, "Sales");

        ICollection<Sale> sales = new HashSet<Sale>();

        foreach (var saleDto in salesDtos
                     .Where(s => carIds.Contains(s.CarId)))
        {
            Sale sale = new Sale()
            {
                Discount = saleDto.Discount,
                CarId = saleDto.CarId,
                CustomerId = saleDto.CustomerId
            };

            sales.Add(sale);
        }

        context.Sales.AddRange(sales);
        context.SaveChanges();

        return $"Successfully imported {sales.Count}";
    }

    public static string GetCarsWithDistance(CarDealerContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var cars = context.Cars
            .Where(c => c.TraveledDistance > 2_000_000)
            .OrderBy(c => c.Make)
            .ThenBy(c => c.Model)
            .Take(10)
            .Select(c => new ExportCarsWithDistanceDto()
            {
                Make = c.Make,
                Model = c.Model,
                TraveledDistance = c.TraveledDistance,
            })
            .ToArray();

        return xmlHelper.Serialize(cars, "cars");
    }

    public static string GetCarsFromMakeBmw(CarDealerContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var cars = context.Cars
            .Where(c => c.Make == "BMW")
            .OrderBy(c => c.Model)
            .ThenByDescending(c => c.TraveledDistance)
            .Select(c => new ExportCarsFromMakeBmwDto()
            {
                Id = c.Id,
                Model = c.Model,
                TraveledDistance = c.TraveledDistance
            })
            .ToArray();

        return xmlHelper.Serialize(cars, "cars");
    }

    public static string GetLocalSuppliers(CarDealerContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var suppliers = context.Suppliers
            .Where(s => s.IsImporter == false)
            .Select(s => new ExportLocalSuppliersDto()
            {
                Id = s.Id,
                Name = s.Name,
                PartsCount = s.Parts.Count
            })
            .ToArray();

        return xmlHelper.Serialize(suppliers, "suppliers");
    }

    public static string GetCarsWithTheirListOfParts(CarDealerContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var cars = context.Cars
            .Select(c => new ExportCarsWithTheirListOfPartsDto()
            {
                Make = c.Make,
                Model = c.Model,
                TraveledDistance = c.TraveledDistance,
                parts = c.PartsCars
                    .Select(p => new ExportPartsDto()
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price,
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
            })
            .OrderByDescending(c => c.TraveledDistance)
            .ThenBy(c => c.Model)
            .Take(5)
            .ToArray();

        return xmlHelper.Serialize(cars, "cars");
    }

    public static string GetTotalSalesByCustomer(CarDealerContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var customers = context.Customers
            .Where(c => c.Sales.Any())
            .Select(c => new
            {
                FullName = c.Name,
                BoughtCars = c.Sales.Count,
                SpentMoney = c.Sales
                    .Select(s => new
                    {
                        Prices = c.IsYoungDriver
                        ? s.Car.PartsCars.Sum(p => Math.Round((double)p.Part.Price * 0.95, 2))
                        : s.Car.PartsCars.Sum(p => (double)p.Part.Price)
                    })
                    .ToArray()
            })
            .ToArray();

        ExportTotalSalesByCustomerDto[] totalSalesDtos = customers
            .OrderByDescending(t => t.SpentMoney.Sum(s => s.Prices))
            .Select(t => new ExportTotalSalesByCustomerDto()
            {
                FullName = t.FullName,
                BoughtCars = t.BoughtCars,
                SpentMoney = t.SpentMoney.Sum(s => s.Prices).ToString("f2")
            })
            .ToArray();

        return xmlHelper.Serialize(totalSalesDtos, "customers");
    }

    public static string GetSalesWithAppliedDiscount(CarDealerContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var sales = context.Sales
            .Select(s => new ExportSalesWithAppliedDiscountDto()
            {
                SingleCar = new SingleCar()
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TraveledDistance = s.Car.TraveledDistance
                },
                Discount = (int)s.Discount,
                CustomerName = s.Customer.Name,
                Price = s.Car.PartsCars.Sum(p => p.Part.Price),
                PriceWithDiscount = Math.Round((double)(s.Car.PartsCars.Sum(p => p.Part.Price) * (1 - (s.Discount / 100))), 4)
            })
            .ToArray();

        return xmlHelper.Serialize(sales, "sales");
    }
}
