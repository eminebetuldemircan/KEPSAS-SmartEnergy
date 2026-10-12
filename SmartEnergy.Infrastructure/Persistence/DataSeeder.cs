using SmartEnergy.Domain.Entities;
using SmartEnergy.Domain.Enums;

namespace SmartEnergy.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(SmartEnergyDbContext context)
    {
        // Ensure Database is created if using local dev DB
        await context.Database.EnsureCreatedAsync();

        if (context.Customers.Any())
        {
            return; // Data already seeded
        }

        // 1. Create Main Customer: Betül Demircan
        var customer = new Customer(
            customerNumber: "K-784920",
            firstName: "Betül",
            lastName: "Demircan",
            tcknVkn: "12345678901",
            phone: "0532 555 12 34",
            email: "betul.demircan@kepsas.com.tr",
            customerType: "Bireysel"
        );

        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        // 2. Create User linked to Customer
        var user = new User(
            firstName: "Betül",
            lastName: "Demircan",
            email: "betul.demircan@kepsas.com.tr",
            tcknVkn: "12345678901",
            passwordHash: "123456", // Simple hash/plain for testing
            role: "Abone"
        );
        user.LinkCustomer(customer.Id);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        customer.LinkUser(user.Id);

        // 3. Create Addresses
        var homeAddress = new Address(
            userId: user.Id,
            city: "Kayseri",
            district: "Melikgazi",
            neighborhood: "Alpaslan Mah.",
            street: "Sivas Cad.",
            buildingNumber: "42",
            apartmentNumber: "5",
            postalCode: "38030"
        );

        var workAddress = new Address(
            userId: user.Id,
            city: "Kayseri",
            district: "Kocasinan",
            neighborhood: "Sahabiye Mah.",
            street: "İstasyon Cad.",
            buildingNumber: "12",
            apartmentNumber: "1",
            postalCode: "38010"
        );

        context.Addresses.AddRange(homeAddress, workAddress);
        await context.SaveChangesAsync();

        // 4. Create Meters
        var meter1 = new Meter(
            meterNumber: "MTR-2026-991",
            serialNumber: "SN-887410",
            type: MeterType.Electricity,
            installationDate: new DateTime(2022, 5, 15)
        );
        meter1.UpdateReading(4286.40m, DateTime.Now.AddDays(-2));

        var meter2 = new Meter(
            meterNumber: "MTR-2026-992",
            serialNumber: "SN-887411",
            type: MeterType.Electricity,
            installationDate: new DateTime(2023, 8, 10)
        );
        meter2.UpdateReading(1850.10m, DateTime.Now.AddDays(-5));

        context.Meters.AddRange(meter1, meter2);
        await context.SaveChangesAsync();

        // 5. Create Subscriptions
        var sub1 = new Subscription(
            userId: user.Id,
            addressId: homeAddress.Id,
            meterId: meter1.Id,
            subscriptionNumber: "SUB-380901",
            contractNumber: "SZ-2022-771",
            startDate: new DateTime(2022, 5, 15)
        );

        var sub2 = new Subscription(
            userId: user.Id,
            addressId: workAddress.Id,
            meterId: meter2.Id,
            subscriptionNumber: "SUB-380902",
            contractNumber: "SZ-2023-882",
            startDate: new DateTime(2023, 8, 10)
        );

        context.Subscriptions.AddRange(sub1, sub2);
        await context.SaveChangesAsync();

        // 6. Create Bills
        var bill1 = new Bill(
            subscriptionId: sub1.Id,
            billNumber: "FTR-2026-005",
            billingPeriodStart: new DateTime(2026, 4, 1),
            billingPeriodEnd: new DateTime(2026, 4, 30),
            issueDate: new DateTime(2026, 5, 2),
            dueDate: DateTime.Now.AddDays(7),
            previousIndex: 4000.00m,
            currentIndex: 4286.40m,
            energyAmount: 480.50m,
            distributionAmount: 162.00m,
            taxAmount: 100.00m
        ); // Total 742.50 ₺ - Unpaid

        var bill2 = new Bill(
            subscriptionId: sub1.Id,
            billNumber: "FTR-2026-004",
            billingPeriodStart: new DateTime(2026, 3, 1),
            billingPeriodEnd: new DateTime(2026, 3, 31),
            issueDate: new DateTime(2026, 4, 2),
            dueDate: new DateTime(2026, 4, 17),
            previousIndex: 3750.00m,
            currentIndex: 4000.00m,
            energyAmount: 430.00m,
            distributionAmount: 140.00m,
            taxAmount: 85.00m
        ); // Total 655.00 ₺ - Paid
        bill2.MarkAsPaid();

        var bill3 = new Bill(
            subscriptionId: sub2.Id,
            billNumber: "FTR-2026-003",
            billingPeriodStart: new DateTime(2026, 3, 1),
            billingPeriodEnd: new DateTime(2026, 3, 31),
            issueDate: new DateTime(2026, 4, 2),
            dueDate: new DateTime(2026, 4, 17),
            previousIndex: 1600.00m,
            currentIndex: 1850.10m,
            energyAmount: 380.00m,
            distributionAmount: 120.00m,
            taxAmount: 75.00m
        ); // Paid
        bill3.MarkAsPaid();

        context.Bills.AddRange(bill1, bill2, bill3);
        await context.SaveChangesAsync();

        // 7. Create Payments
        var pay1 = new Payment(bill2.Id, 655.00m, "Kredi Kartı", "TXN-99812");
        var pay2 = new Payment(bill3.Id, 575.00m, "Banka Havalesi", "TXN-99813");
        context.Payments.AddRange(pay1, pay2);

        // 8. Create Consumption Records
        var months = new[] { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs" };
        var kwhValues = new decimal[] { 240.5m, 265.0m, 250.0m, 286.4m, 290.0m };
        for (int i = 0; i < months.Length; i++)
        {
            var date = new DateTime(2026, i + 1, 15);
            var rec = new ConsumptionRecord(
                meter1.Id,
                date,
                kwhValues[i],
                4000m + (i * 250m),
                false
            );
            context.ConsumptionRecords.Add(rec);
        }

        // 9. Create Security Deposit & Compensation & RefundRequest
        var deposit = new SecurityDeposit("GVD-2022-441", 1450.00m, 1450.00m, customer.Id, sub1.Id);
        var comp = new Compensation("TZM-2026-001", 320.00m, "Onaylandı", DateTime.Now.AddDays(-15), "Kesinti Süre Limit Aşımı Tazminatı", customer.Id);
        var refund = new RefundRequest("IDE-2026-102", 450.00m, "IBAN", "TR38 0006 2000 0000 1234 5678 90", "Fatura mükerrer ödeme iadesi", customer.Id);

        context.SecurityDeposits.Add(deposit);
        context.Compensations.Add(comp);
        context.RefundRequests.Add(refund);

        // 10. Create Applications
        var app1 = new PortalApplication(
            applicationNumber: "BAS-2026-089",
            applicationType: "BilgiEdinme",
            title: "Tarife Bilgisi Talebi",
            description: "Mesken aboneliği için zamanlı tarife geçiş şartları hakkında bilgi talep ediyorum.",
            customerId: customer.Id,
            subscriptionId: sub1.Id
        );

        var app2 = new PortalApplication(
            applicationNumber: "BAS-2026-092",
            applicationType: "Randevu",
            title: "Müşteri Hizmetleri Randevusu",
            description: "Sayaç değişim talebi için randevu alındı.",
            customerId: customer.Id,
            subscriptionId: sub1.Id
        );

        context.PortalApplications.AddRange(app1, app2);

        // 11. Create Appointments
        var appointment = new Appointment(
            appointmentNumber: "RND-2026-551",
            serviceType: "Sayaç Kontrolü / Değişimi",
            location: "Melikgazi Müşteri Hizmetleri Merkezi",
            appointmentDate: DateTime.Now.AddDays(3),
            timeSlot: "10:30 - 11:00",
            customerId: customer.Id,
            notes: "Sayaç ekran kontrolü yapılacaktır."
        );
        context.Appointments.Add(appointment);

        // 12. Create Notifications
        var notif1 = new Notification(
            title: "Yeni Faturanız Oluşturuldu",
            message: "Nisan 2026 dönemine ait 742,50 ₺ tutarındaki faturanız oluşturulmuştur. Son ödeme tarihi: 14 Mayıs 2026.",
            type: "Fatura",
            customerId: customer.Id
        );

        var notif2 = new Notification(
            title: "Randevu Hatırlatması",
            message: "Melikgazi MHM randevunuza 3 gün kalmıştır. Lütfen kimlik belgenizi yanınızda bulundurunuz.",
            type: "Randevu",
            customerId: customer.Id
        );

        var notif3 = new Notification(
            title: "Ödemeniz Alındı",
            message: "FTR-2026-004 numaralı faturanıza ait 655,00 ₺ ödeme başarıyla sistemimize yansımıştır.",
            type: "Ödeme",
            customerId: customer.Id
        );

        var notif4 = new Notification(
            title: "Planlı Bakım ve Şebeke İyileştirmesi",
            message: "Alpaslan Mahallesi bölgesinde 12 Mayıs tarihinde saat 09:00 - 13:00 saatleri arasında planlı şebeke bakım çalışması gerçekleştirilecektir.",
            type: "Duyuru",
            customerId: customer.Id
        );

        var notif5 = new Notification(
            title: "Enerji Tasarrufu Önerisi",
            message: "Geçen aya göre tüketiminiz %12 azaldı. Tasarruflu kullanımınız için teşekkür ederiz!",
            type: "Tasarruf",
            customerId: customer.Id
        );

        var notif6 = new Notification(
            title: "Tarife Değişikliği Bilgilendirmesi",
            message: "Üç zamanlı akıllı sayaç tarifeniz başarıyla aktif edilmiştir.",
            type: "Sözleşme",
            customerId: customer.Id
        );

        context.Notifications.AddRange(notif1, notif2, notif3, notif4, notif5, notif6);

        await context.SaveChangesAsync();
    }
}
