# Multiple Layered API

## Proje Hakkında
Bu proje, .NET Core kullanılarak geliştirilmiş çok katmanlı bir API mimarisi örneğidir. Projede SOLID prensiplerine uygun olarak katmanlı mimari uygulanmıştır. Amaç, projeyi daha modüler, test edilebilir ve sürdürülebilir hale getirmektir.

---

## Kullanılan Teknolojiler
- **.NET 9**: Projenin temel çatısı.
- **Entity Framework Core**: Veritabanı işlemleri için.
- **Swagger**: API dokümantasyonu ve test amacıyla.
- **JWT Authentication**: Kimlik doğrulama ve yetkilendirme.
- **Middleware**: Özel ara katman çözümleri (örneğin hata yönetimi, zaman sınırlandırma).
- **Unit of Work**: Veritabanı işlemleri sırasında yönetilebilirlik ve performans için.
- **Dependency Injection**: Servislerin bağımlılıklarını çözmek için.

---

## Proje Yapısı

1. **Multiple-Layered.API**:
   - Controllers
   - Extensions (Middleware, Exception Handling, vb.)
   - appsettings.json
   - Program.cs

2. **Multiple-Layered-DataAccess.Library**:
   - Entity Configurations
   - Migrations
   - Models
   - Repositories
   - Seeds
   - Unit of Work

3. **Multiple-Layered-Service.Library**:
   - DTOs
   - Paginations
   - Services
   - Unit of Work

---

## Kurulum
1. **Bağımlılıkların yüklenmesi:**
   ```bash
   dotnet restore
   ```
2. **Veritabanı migrasyonlarının uygulanması:**
   ```bash
   dotnet ef database update
   ```
3. **Projenin çalıştırılması:**
   ```bash
   dotnet run --project Multiple-Layered.API
   ```

---

## Nasıl Çalıştırılır?
1. Proje dizinine gidin ve terminalden aşağıdaki komutları çalıştırın:
   - Proje bağımlılıklarını yükleyin: `dotnet restore`
   - Gerekli veritabanı tablolarını oluşturun: `dotnet ef database update`
   - API'yi başlatın: `dotnet run --project Multiple-Layered.API`

2. Tarayıcınızdan Swagger arayüzüne erişmek için şu adresi kullanın:
  ```bash
https://localhost:7002/swagger
```


## Lisans
Bu proje MIT lisansı ile lisanslanmıştır.
