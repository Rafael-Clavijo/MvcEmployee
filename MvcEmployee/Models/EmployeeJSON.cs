using System;
using System.Runtime.Serialization;

namespace MvcEmployee.Models
{
    [DataContract]
    [Serializable]
    public class EmployeeJSON
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Employee_Name { get; set; }
        [DataMember]
        public int Employee_Salary { get; set; }
        [DataMember]
        public short Employee_Age { get; set; }
        [DataMember]
        public byte[] Profile_Image { get; set; }
    }
}