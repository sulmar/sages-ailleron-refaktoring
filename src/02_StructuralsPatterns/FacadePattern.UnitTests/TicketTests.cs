using FacadePattern.Models;
using FacadePattern.Repositories;
using FacadePattern.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FacadePattern.UnitTests
{
    [TestClass]
    public class TicketTests
    {
        [TestMethod]
        public void BuyTicket()
        {
            // Arrange
            string from = "Bydgoszcz";
            string to = "Warszawa";
            DateTime when = DateTime.Parse("2023-09-20");
            byte numberOfPlaces = 3;

            IRailwayConnectionRepository railwayConnectionRepository = new PkpRailwayConnectionRepository();
            TicketCalculator ticketCalculator = new TicketCalculator();
            ReservationService reservationService = new ReservationService();
            PaymentService paymentService = new PaymentService();
            EmailService emailService = new EmailService();

            ITicketService ticketService = new TicketService(railwayConnectionRepository,
                ticketCalculator, reservationService, paymentService, emailService);

            TicketParameters parameters = new TicketParameters(from, to, when, numberOfPlaces);

            // Act
            Ticket ticket = ticketService.Buy(parameters);

            // Assert
            Assert.AreEqual("Bydgoszcz", ticket.RailwayConnection.From);
            Assert.AreEqual("Warszawa", ticket.RailwayConnection.To);
            Assert.AreEqual(DateTime.Parse("2023-09-20"), ticket.RailwayConnection.When);
            Assert.AreEqual(3, ticket.NumberOfPlaces);
        }

        [TestMethod]
        public void CancelTicket()
        {
            // Arrange
            string from = "Bydgoszcz";
            string to = "Warszawa";
            DateTime when = DateTime.Parse("2022-07-15");
            byte numberOfPlaces = 3;

            IRailwayConnectionRepository railwayConnectionRepository = new PkpRailwayConnectionRepository();
            TicketCalculator ticketCalculator = new TicketCalculator();
            ReservationService reservationService = new ReservationService();
            PaymentService paymentService = new PaymentService();
            EmailService emailService = new EmailService();

            RailwayConnection railwayConnection = railwayConnectionRepository.Find(from, to, when);
            decimal price = ticketCalculator.Calculate(railwayConnection, numberOfPlaces);
            Reservation reservation = reservationService.MakeReservation(railwayConnection, numberOfPlaces);
            Ticket ticket = new Ticket { RailwayConnection = reservation.RailwayConnection, NumberOfPlaces = reservation.NumberOfPlaces, Price = price };
            Payment payment = paymentService.CreateActivePayment(ticket);

            // Act
            reservationService.CancelReservation(ticket.RailwayConnection, ticket.NumberOfPlaces);
            paymentService.RefundPayment(payment);



        }
    }
}
