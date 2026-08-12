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
    private readonly TimeInterval _interval = new TimeInterval(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2));

    [TestMethod]
    public void ConstructorValidAppointment()
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
    public void ConstructorStartTimeInTHePastThrows()
    {
        var interval = new TimeInterval(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
        Assert.ThrowsException<BusinessRuleException>(() => new Appointment(_patientId, _dentistId, _dentalOfficeId, interval));


    }

    [TestMethod]
    public void CancelAppointmentChangeStatusToCancelled()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        appointment.Cancel();
        Assert.AreEqual(AppointmentStatus.Cancelled,appointment.Status);
    }

    [TestMethod]

    public void CancelAppointmentThrowsIfNotScheduled()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        appointment.Cancel();
        Assert.ThrowsException<BusinessRuleException>(appointment.Cancel);

    }

    [TestMethod]
    public void CompleteAppointmentChangeStatusToCompleted()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        appointment.Complete();
        Assert.AreEqual(AppointmentStatus.Completed,appointment.Status);
    }

    [TestMethod]

    public void CompleteAppointmentThrowsIfNotScheduled()
    {
        var appointment = new Appointment(_patientId, _dentistId, _dentalOfficeId, _interval);
        appointment.Complete();
        Assert.ThrowsException<BusinessRuleException>(appointment.Complete);

    }
}
