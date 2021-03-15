namespace a3innuva.Importia.Validations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Serialization;

    public class ValidationBinder : ISerializationBinder
    {
        public IList<Type> KnownTypes = new List<Type>()
        {
            typeof(a3innuva.TAA.Migration.SDK.Implementations.Account),
            typeof(a3innuva.TAA.Migration.SDK.Implementations.InputInvoice),
            typeof(a3innuva.TAA.Migration.SDK.Implementations.InputInvoiceLine),
            typeof(a3innuva.TAA.Migration.SDK.Implementations.Journal),
            typeof(a3innuva.TAA.Migration.SDK.Implementations.JournalLine),
            typeof(a3innuva.TAA.Migration.SDK.Implementations.MigrationInfo),
            typeof(a3innuva.TAA.Migration.SDK.Implementations.MigrationSet),
            typeof(a3innuva.TAA.Migration.SDK.Implementations.OutputInvoice),
            typeof(a3innuva.TAA.Migration.SDK.Implementations.OutputInvoiceLine),

            typeof(a3innuva.TAA.Migration.SDK.Interfaces.IMigrationEntity[]),
            typeof(a3innuva.TAA.Migration.SDK.Interfaces.IInputInvoiceLine[]),
            typeof(a3innuva.TAA.Migration.SDK.Interfaces.IJournalLine[]),
            typeof(a3innuva.TAA.Migration.SDK.Interfaces.IOutputInvoiceLine[])
        };

        public Type BindToType(string assemblyName, string typeName)
        {
            var type =  KnownTypes.SingleOrDefault(t => t.FullName == typeName);
            if (type == null)
                throw new Exception($"{typeName} no es un tipo serializable valido");

            return type;
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }
    }
}
