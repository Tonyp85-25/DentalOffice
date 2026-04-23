using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Tests.Domain.ValueObjects;

[TestClass]
public class AddressTests
{
    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void Address_With_Missing_Values_Throws()
    {
        Address.Create("8", null, null, null);
    }
    
    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void Address_With_Bad_Zipcode_Throws()
    {
        Address.Create("8", "tututu", "123560", "abcd");
    }
    
    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void Address_With_Bad_City_Throws()
    {
        Address.Create("8", "tututu", "12356", "a");
    }
    
    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void Address_With_Bad_Street_Throws()
    {
        Address.Create("8", "tutu", "12356", "abcd");
    }
    
    [TestMethod]
    public void Address_With_0_Starting_Zipcode_Succeed()
    {
        Address.Create("8", "tututu", "01235", "abcd");
    }
    
    
}