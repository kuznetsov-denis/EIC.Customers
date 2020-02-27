using System;

using EIC.Customers.Container;

namespace Process
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var task = (new CustomersProcess()).RunAsync(args);
				task.Wait();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Console.ReadLine();
			}
		}
	}
}
