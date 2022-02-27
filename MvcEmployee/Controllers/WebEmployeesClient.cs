using System;
using System.Collections.Generic;
using System.Web.UI;
using MvcEmployee.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;

namespace MvcEmployee.Controllers
{
    /// <summary>
    /// Service client that consumes the remote data API
    /// </summary>
    public class WebEmployeesClient : Page
    {
        #region Properties
        /// <summary>
        /// Reference to the data source location
        /// </summary>
        private static Uri _baseAdress = new Uri("http://dummy.restapiexample.com/api/v1/");

        /// <summary>
        /// Connection client
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Response from the data source
        /// </summary>
        public HttpResponseMessage Response { get; set; }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public WebEmployeesClient()
        {
            _client = new HttpClient { Timeout = TimeSpan.FromMilliseconds(180000) };
        }
        #endregion Constructor

        #region GET
        /// <summary>
        /// Gets the data of all employees
        /// </summary>
        /// <returns>A list of all objects</returns>
        public List<Employee> GetAllEmployees()
        {
            //Set up the response object
            List<EmployeeJSON> employeesList;
            //Set up the headers
            if (_client.BaseAddress == null)
                _client.BaseAddress = _baseAdress;
            _client.DefaultRequestHeaders.Accept.Clear();
            //The response is called asynchronously
            Response = _client.GetAsync("employees").Result;
            //The data is interpreted and returned
            RemoteEmployees responseData;
            try { responseData = JsonConvert.DeserializeObject<RemoteEmployees>(Response.Content.ReadAsStringAsync().Result); }
            catch { responseData = new RemoteEmployees(); }
            if (responseData.Status != "success") return new List<Employee>();
            employeesList = responseData.Data;
            return employeesList.Select(responseObj => new Employee { Id = responseObj.Id, Age = responseObj.Employee_Age, Name = responseObj.Employee_Name, ProfileImage = responseObj.Profile_Image, Salary = responseObj.Employee_Salary }).ToList();
        }

        /// <summary>
        /// Gets the data of a employee by their id
        /// </summary>
        /// <returns></returns>
        public Employee GetEmployeesById(int id)
        {
            //Set up the response object
            EmployeeJSON responseObj;
            //Set up the headers
            if (_client.BaseAddress == null)
                _client.BaseAddress = _baseAdress;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //The response is called asynchronously
            Response = _client.GetAsync($"employee/{id}").Result;
            //The data is interpreted and returned
            RemoteEmployee responseData;
            try { responseData = JsonConvert.DeserializeObject<RemoteEmployee>(Response.Content.ReadAsStringAsync().Result); }
            catch { responseData = new RemoteEmployee(); }
            if (responseData.Status != "success") return null;
            responseObj = responseData.Data;
            return new Employee { Id = responseObj.Id, Age = responseObj.Employee_Age, Name = responseObj.Employee_Name, ProfileImage = responseObj.Profile_Image, Salary = responseObj.Employee_Salary };
        } 
        #endregion
    }

    /// <summary>
    /// Remote data container for a single employee
    /// </summary>
    public class RemoteEmployee { public string Status { get; set; } public EmployeeJSON Data { get; set; } public string Message { get; set; } }
    
    /// <summary>
    /// Remote data container for a list of employees
    /// </summary>
    public class RemoteEmployees { public string Status { get; set; } public List<EmployeeJSON> Data { get; set; } public string Message { get; set; } }
}