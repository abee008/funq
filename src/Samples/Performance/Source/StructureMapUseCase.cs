﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using Domain;

namespace Performance
{
	[System.ComponentModel.Description("StructureMap")]
	public class StructureMapUseCase : UseCase
	{
		Container container;

		public StructureMapUseCase()
		{
			container = new Container();
			container.Configure(
				x => x.ForRequestedType<IWebService>()
					.TheDefault.Is.ConstructedBy(
					c => new WebService(
						c.GetInstance<IAuthenticator>(),
						c.GetInstance<IStockQuote>())
					));

			container.Configure(
				x => x.ForRequestedType<IAuthenticator>()
					.TheDefault.Is.ConstructedBy(
					c => new Authenticator(
						c.GetInstance<ILogger>(), 
						c.GetInstance<IErrorHandler>(), 
						c.GetInstance<IDatabase>())
					));

			container.Configure(
				x => x.ForRequestedType<IStockQuote>()
					.TheDefault.Is.ConstructedBy(
					c => new StockQuote(
						c.GetInstance<ILogger>(),
						c.GetInstance<IErrorHandler>(),
						c.GetInstance<IDatabase>())
					));

			container.Configure(
				x => x.ForRequestedType<IDatabase>()
					.TheDefault.Is.ConstructedBy(
					c => new Database(
						c.GetInstance<ILogger>(), 
						c.GetInstance<IErrorHandler>())
					));

			container.Configure(
				x => x.ForRequestedType<IErrorHandler>()
					.TheDefault.Is.ConstructedBy(
					c => new ErrorHandler(c.GetInstance<ILogger>())
					));

			container.Configure(
				x => x.ForRequestedType<ILogger>()
					.TheDefault.IsThis(new Logger()));
		}

		public override void Run()
		{
			var webApp = container.GetInstance<IWebService>();
			webApp.Execute();
		}
	}
}
