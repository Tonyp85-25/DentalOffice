using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Tests.Domain.ValueObjects;

[TestClass]
public class EmailTests
{

    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void ConstructorBadEmailThrows()
    {
       Assert.ThrowsException<BusinessRuleException>(()=>
           new Email("abcd.abcd")
        ) ;
    }

    [TestMethod]
    public void ConstructorValidEmail()
    {
        var email=new Email("abcd@blo.abcd");
        Assert.IsInstanceOfType<Email>(email);
    }
}
