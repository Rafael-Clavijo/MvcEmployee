using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MvcEmployee.Models;

namespace MvcEmployee.Controllers
{
    public class EmployeesController : Controller
    {
        /// <summary>
        /// Shows the employees in the system, filtered by their ID.
        /// </summary>
        /// <param name="searchString">The filter</param>
        /// <returns></returns>
        public ActionResult Index(string searchString)
        {
            //Get all employees
            var employees = new WebEmployeesClient().GetAllEmployees();
            //Filter Employees
            if (!String.IsNullOrEmpty(searchString)) employees = employees.Where(s => s.Id.ToString().Contains(searchString)).ToList();
            //Fix calculated Employee properties
            employees.ForEach(hire => hire.AnnualSalary = hire.Salary * 12);
            //Display detailed result if there is a perfect match
            if (employees.Exists(x => x.Id.ToString().Equals(searchString)))
                return Redirect($"Employees/Details/{employees.First(x => x.Id.ToString().Equals(searchString)).Id}");
            //Display detailed result if there is only one option
            else if (employees.Count != 1)
                return View(employees);
            //Display filtered results
            else
                return Redirect($"Employees/Details/{employees.First().Id}");
        }

        /// <summary>
        /// Shows the details on the data of a particular employee.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            //Without id, throw an error message
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Employee employee = new WebEmployeesClient().GetEmployeesById((int)id);
            //If id not found, throw an error message
            if (employee == null)
                return HttpNotFound();
            //Calculate the annual salary field
            else
                employee.AnnualSalary = employee.Salary * 12;
            return View(employee);
        }
    }
}
