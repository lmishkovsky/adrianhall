using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using Backend.DataObjects;
using Backend.Models;
using Microsoft.Azure.Mobile.Server.Config;
using Owin;

namespace Backend
{
    /// <summary>
    /// Startup.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configures the mobile app.
        /// </summary>
        /// <param name="app">App.</param>
        public static void ConfigureMobileApp(IAppBuilder app) 
        {
            var config = new HttpConfiguration();
            var mobileConfig = new MobileAppConfiguration();

            mobileConfig.AddTablesWithEntityFramework().ApplyTo(config);

            Database.SetInitializer(new MobileServiceInitializer());

            app.UseWebApi((config));
        }
    }

    /// <summary>
    /// Mobile service initializer.
    /// </summary>
	public class MobileServiceInitializer : CreateDatabaseIfNotExists<MobileServiceContext>
	{
        /// <summary>
        /// Seed the specified context.
        /// </summary>
        /// <returns>The seed.</returns>
        /// <param name="context">Context.</param>
		protected override void Seed(MobileServiceContext context)
		{
			List<TodoItem> todoItems = new List<TodoItem>
			{
				new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
				new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false }
			};

			foreach (TodoItem todoItem in todoItems)
			{
				context.Set<TodoItem>().Add(todoItem);
			}

			base.Seed(context);
		}
	}
}
