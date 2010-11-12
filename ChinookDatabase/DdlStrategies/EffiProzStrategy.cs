﻿using System;
using System.Data.Metadata.Edm;

namespace ChinookDatabase.DdlStrategies
{
    public class EffiProzStrategy : AbstractDdlStrategy
    {
        public string SchemaName { get; set; }

        public EffiProzStrategy()
        {
            CanReCreateDatabase = true;
            SchemaName = "Chinook";
        }

        public override string Name
        {
            get { return "EffiProz"; }
        }

        public override string Identity
        {
            get { return "GENERATED BY DEFAULT AS IDENTITY(INCREMENT BY 1 START WITH 1)"; }
        }

        public override KeyDefinition PrimaryKeyDef
        {
            get { return KeyDefinition.OnAlterTable; }
        }

        public override string FormatName(string name)
        {
            return string.Format("\"{0}\"", name);
        }

        public override string FormatDateValue(string value)
        {
            var date = Convert.ToDateTime(value);
            return string.Format("'{0}-{1}-{2} 00:00:00'", date.Year, date.Month, date.Day);
        }

        public override string GetFullyQualifiedName(string schema, string name)
        {
            return base.GetFullyQualifiedName(SchemaName, name);
        }

        public override string WriteCreateDatabase(string databaseName)
        {
            return string.Format("CREATE SCHEMA {0} AUTHORIZATION DBA;", FormatName(SchemaName));
        }

        public override string WriteCreateColumn(EdmProperty property, Version targetVersion)
        {
            var notnull = (property.Nullable ? "" : "NOT NULL");
            var identity = GetIdentity(property, targetVersion);
            return string.Format("{0} {1} {2} {3}",
                                 FormatName(property.Name),
                                 GetStoreType(property),
                                 identity, notnull).Trim();
        }

        public override string WriteExecuteCommand()
        {
            return @"\";
        }

   }
}