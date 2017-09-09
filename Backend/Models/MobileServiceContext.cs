using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Backend.DataObjects;
using Microsoft.Azure.Mobile.Server.Tables;

namespace Backend.Models
{
    /// <summary>
    /// Mobile service context.
    /// </summary>
    public class MobileServiceContext : DbContext
    {
        /// <summary>
        /// The name of the connection string.
        /// </summary>
        private const string connectionStringName = "Name=MS_TableConnectionString";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Backend.Models.MobileServiceContext"/> class.
        /// </summary>
        public MobileServiceContext() : base(connectionStringName)
        {
        }

        /// <summary>
        /// Gets or sets the todo items.
        /// </summary>
        /// <value>The todo items.</value>
        public DbSet<TodoItem> TodoItems { get; set; }

        /// <summary>
        /// Ons the model creating.
        /// </summary>
        /// <param name="modelBuilder">Model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
			modelBuilder.Conventions.Add(
				new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
					"ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
		}
    }
}
