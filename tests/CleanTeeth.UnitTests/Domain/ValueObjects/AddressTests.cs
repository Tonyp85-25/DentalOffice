using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Tests.Domain.ValueObjects;

[TestClass]
public class AddressTests
{
    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void AddressWithMissingValuesThrows()
    {
        Address.Create("8", null, null, null);
    }

    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void AddressWithBadZipcodeThrows()
    {
        Address.Create("8", "tututu", "123560", "abcd");
    }

    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void AddressWithBadCityThrows()
    {
        Address.Create("8", "tututu", "12356", "a");
    }

    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void AddressWithBadStreetThrows()
    {
        Address.Create("8", "tutu", "12356", "abcd");
    }

    [TestMethod]
    public void AddressWith0StartingZipcodeSucceed()
    {
        var address = Address.Create("8", "tututu", "01235", "abcd");
        Assert.IsInstanceOfType<Address>(address);
    }


}
