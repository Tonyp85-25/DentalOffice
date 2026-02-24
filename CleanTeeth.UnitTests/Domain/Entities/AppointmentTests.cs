using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Tests.Domain.Entities;

[TestClass]
public class AppointmentTests
{
    private Guid _patientId = Guid.NewGuid();
    private Guid _dentistId = Guid.NewGuid();
    private Guid _dentalOfficeId =Guid.NewGuid();
    private TimeInterval _interval = new TimeInterval(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2));
    
    [TestMethod]
    public void Constructor_Valid_Appointment()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        Assert.AreEqual(_patientId,appointment.PatientId);
        Assert.AreEqual(_dentistId,appointment.DentistId);
        Assert.AreEqual(_dentalOfficeId,appointment.DentalOfficeId);
        Assert.AreEqual(AppointmentStatus.Scheduled,appointment.Status);
        Assert.AreEqual(_interval,appointment.TimeInterval);
        Assert.AreNotEqual(Guid.Empty, appointment.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void Constructor_StartTimeInTHePast_Throws()
    {
        var interval = new TimeInterval(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
        new Appointment(_patientId, _dentistId, _dentalOfficeId, interval);

    }

    [TestMethod]
    public void CancelAppointment_ChangeStatusToCancelled()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        appointment.Cancel();
        Assert.AreEqual(AppointmentStatus.Cancelled,appointment.Status);
    }
    
    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void CancelAppointment_ThrowsIfNotScheduled()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        appointment.Cancel();
        appointment.Cancel();
    }
    
    [TestMethod]
    public void CompleteAppointment_ChangeStatusToCompleted()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        appointment.Complete();
        Assert.AreEqual(AppointmentStatus.Completed,appointment.Status);
    }
    
    [TestMethod]
    [ExpectedException(typeof(BusinessRuleException))]
    public void CompleteAppointment_ThrowsIfNotScheduled()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        appointment.Complete();
        appointment.Complete();
    }
}