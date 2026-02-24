using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Tests.Domain.ValueObjects;

[TestClass]
public class EmailTests
{
    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void Constructor_Null_Email_Throws()
    {
        new Email(null);
    }
    
    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void Constructor_Bad_Email_Throws()
    {
        new Email("abcd.abcd");
    }
    
    [TestMethod]
    public void Constructor_Valid_Email()
    {
        new Email("abcd@blo.abcd");
    }
}