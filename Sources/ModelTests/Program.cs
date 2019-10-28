using System;
using Xtensive.Orm;
using Xtensive.Orm.Configuration;
using System.Linq;

namespace DO4_4_Test03
{
    class Program
    {
        static void Main(string[] args)
        {
            // Loading configuration section for in-memory database. 
            // See other cases in App.config file.
            var config = DomainConfiguration.Load("Default");
            var domain = Domain.Build(config);

            using (var session = domain.OpenSession())
            {
                using (var transactionScope = session.OpenTransaction())
                {
                    // Creating new persistent object
                    new Entity1(session) { Name = "John" };
                    new Entity1(session) { Name = "JOHM" };
                    // Committing transaction
                    transactionScope.Complete();
                }
            }

            // Reading all persisted objects from another Session
            using (var session = domain.OpenSession())
            {
                using (var transactionScope = session.OpenTransaction())
                {
//                    foreach (var myEntity in session.Query.All<Entity1>())
//                    {
//                        Console.WriteLine(myEntity.Name);
//                    }

                    foreach (var entity1 in session.Query.All<Entity1>().Where(c => c.Name == "john"))
                    {
                        Console.WriteLine(entity1.Name);
                    }

                    transactionScope.Complete();
                }
            }
            Console.ReadKey();
        }
    }
}
