using Microsoft.OData.Client;
using ODataUtility.Microsoft.Dynamics.DataEntities;
using System;
using System.Linq;

namespace ODataConsoleApplication
{
    public static class QueryExamples
    {
        public static void ReadLegalEntities(Resources d365)
        {            
            foreach (var legalEntity in d365.LegalEntities.AsEnumerable())
            {
                Console.WriteLine("Name: {0}", legalEntity.Name);
            }
        }

        public static void GetInlineQueryCount(Resources d365)
        {
            var vendorsQuery = d365.Vendors.IncludeTotalCount();
            var vendors = vendorsQuery.Execute() as QueryOperationResponse<Vendor>;

            Console.WriteLine("Total vendors is {0}", vendors.TotalCount);
        }

        public static void GetTopRecords(Resources d365)
        {
            var vendorsQuery = d365.Vendors.AddQueryOption("$top", "10");
            var vendors = vendorsQuery.Execute() as QueryOperationResponse<Vendor>;

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }

        public static void ProductionOrderEntity(Resources d365)
        {
            var productionOrders = d365.ProductionOrderHeaders
                .Expand("ProdutionOrderBOMLines,ReleasedProductV2,ProductionOrderRouteJobs")
                //.AddQueryOption("cross-company", true)
                .Where(x => /*x.DataAreaId == "USMF" &&*/ x.ProductionOrderNumber == "P000173");

            foreach (var productionOrder in productionOrders)
            {
                Console.WriteLine("Production Order with ID {0} retrieved.", productionOrder.ProductionOrderNumber);
            }

            /*var productionOrders = d365.ProductionOrderHeaders
                .Where(x => x.ProductionOrderNumber == "P000210");

            var productionOrder = productionOrders.Single();

            Console.WriteLine("Production Order with ID {0} retrieved.", productionOrder.ProductionOrderNumber);

            var productionOrderUpdate = new DataServiceCollection<ProductionOrderHeader>(productionOrders)[0];

            productionOrderUpdate.ProductionOrderName += "1";
            d365.SaveChanges(SaveChangesOptions.PostOnlySetProperties);

            Console.WriteLine("Production Order with ID {0} updated.", productionOrderUpdate.ProductionOrderNumber);*/
        }

        public static void CustomerEntity(Resources d365)
        {
            var singleCustomer = d365.CustomersV3
                .Where(x => x.CustomerAccount == "US-001")
                .Single();

            Console.WriteLine($"Customer with ID {singleCustomer.CustomerAccount} retrieved.");

            foreach (var customer in d365.CustomersV3)
            {
                Console.WriteLine($"Customer with ID {customer.CustomerAccount} retrieved.");
            }
        }

        public static void ReleasedProductsEntity(Resources d365)
        {
            var product = d365.ReleasedProductsV2.Expand("ProductionOrderHeaders")
                .Where(x => x.ItemNumber == "D0001")
                .First();

            Console.WriteLine($"Released product {product.ItemNumber} retrieved.");
        }

        public static void SalesOrderEntity(Resources d365)
        {
            var salesOrder = d365.SalesOrderHeadersV2.Expand("SalesOrderLines")
                .Where(x => x.SalesOrderNumber == "000724")
                .Single();

            Console.WriteLine($"Sales order {salesOrder.SalesOrderNumber} retrieved.");
            
            var salesLineUpdate = new DataServiceCollection<SalesOrderLine>(
                d365.SalesOrderLines.Where(x => x.SalesOrderNumber == "000724"))[0];

            salesLineUpdate.RequestedReceiptDate = DateTimeOffset.UtcNow;
            d365.SaveChanges(SaveChangesOptions.PostOnlySetProperties);

            Console.WriteLine($"Sales line with order ID {salesLineUpdate.SalesOrderNumber} and Item ID {salesLineUpdate.ItemNumber} updated.");
        }

        public static void FilterSyntax(Resources d365)
        {
            var vendors = d365.Vendors.AddQueryOption("$filter", "VendorAccountNumber eq '1001'").Execute();

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }

        public static void SortSyntax(Resources d365)
        {
            var vendors = d365.Vendors.OrderBy(x => x.VendorAccountNumber);

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }


        public static void FilterByCompany(Resources d365)
        {
            var vendors = d365.Vendors.Where(x => x.DataAreaId == "USMF");

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }

        public static void ExpandNavigationalProperty(Resources d365Client)
        {            
            var salesOrdersWithLines = d365Client.SalesOrderHeaders.Expand("SalesOrderLine").Where(x => x.SalesOrderNumber == "012518" ).Take(5);

            foreach(var salesOrder in salesOrdersWithLines)
            {
                Console.WriteLine(string.Format("Sales order ID is {0}", salesOrder.SalesOrderNumber));

                foreach( var salesLine in salesOrder.SalesOrderLine)
                {
                    Console.WriteLine(string.Format("Sales order line with description {0} contains item id {1}", salesLine.LineDescription, salesLine.ItemNumber));
                }                
            }
        }

        public static void FilterOnNavigationalProperty(Resources d365Client)
        {
            var salesOrderLines = d365Client.SalesOrderLines.Where(x => x.SalesOrderHeader.SalesOrderStatus == SalesStatus.Invoiced);

            foreach (var salesOrderLine in salesOrderLines)
            {
                Console.WriteLine(salesOrderLine.ItemNumber);
            }          

        }
        
    }
}
