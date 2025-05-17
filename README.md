# Password Vault Web Uygulaması

Bu proje, kullanıcıların şifrelerini güvenli bir şekilde saklayabilecekleri, yönetebilecekleri ve organize edebilecekleri modern bir web uygulamasıdır.

## Teknolojiler

### Backend
- ASP.NET Core MVC
- Entity Framework Core
- Microsoft SQL Server
- ASP.NET Core Identity (Kimlik Doğrulama)
- JWT (JSON Web Token) Authentication
- RESTful API

### Frontend
- HTML5
- CSS3
- JavaScript
- jQuery
- jQuery AJAX
- Bootstrap 5
- Bootstrap Icons

## Özellikler

### Kullanıcı Yönetimi
- JWT tabanlı kimlik doğrulama
- Kullanıcı kaydı ve girişi
- Güvenli oturum yönetimi
- Şifre sıfırlama

### Şifre Yönetimi
- Kullanıcı şifreleri, geri döndürülemez (tek yönlü) bir şifreleme algoritması ile hashlenerek veritabanında saklanmaktadır.
- Kullanıcı adları ve e-posta adresleri benzersizdir; aynı kullanıcı adı veya e-posta adresi ile birden fazla kayıt oluşturulamaz.
- Kullanıcıların üye oldukları sitelere ait hesap şifreleri, geri döndürülebilir bir şifreleme algoritması kullanılarak şifrelenmekte ve bu şekilde veritabanında saklanmaktadır.
- jQuery AJAX ile asenkron işlemler
  - Şifre ekleme
  - Şifre düzenleme
  - Şifre silme
  - Şifre listeleme
- Şifreleri güvenli bir şekilde saklama
- Şifre görüntüleme ve gizleme
- Tek tıkla şifre kopyalama
- Site adı ve kullanıcı adı ile organize etme

### Kullanıcı Arayüzü
- Modern ve responsive tasarım
- Bootstrap modal'ları ile kolay işlem yapma
- jQuery ile dinamik DOM manipülasyonu
- Kullanıcı dostu arayüz
- Dinamik tablo görünümü

### Özel Özellikler
- Rastgele şifre oluşturucu
  - Özelleştirilebilir şifre uzunluğu (8-32 karakter)
  - Büyük/küçük harf seçeneği
  - Sayı ve özel karakter seçenekleri
- jQuery ile anlık şifre görüntüleme/gizleme
- jQuery ile tek tıkla panoya kopyalama

## Mimari

Uygulama üç katmanlı mimari kullanılarak geliştirilmiştir:
1. **Web Katmanı**: 
   - MVC
   - jQuery ile asenkron işlemler
   - Frontend mantığı
2. **API Katmanı**: 
   - RESTful API endpoints
   - JWT authentication
   - İş mantığı ve servisler
3. **Data Katmanı**: 
   - Entity Framework Core
   - Repository pattern
   - Veritabanı işlemleri

## API Endpoints

- POST /api/auth/login - Kullanıcı girişi
- POST /api/auth/register - Kullanıcı kaydı
- GET /api/passwords - Şifreleri listele
- POST /api/passwords - Yeni şifre ekle
- PUT /api/passwords/{id} - Şifre güncelle
- DELETE /api/passwords/{id} - Şifre sil

## AJAX İşlemleri

Tüm veri işlemleri jQuery AJAX kullanılarak asenkron olarak gerçekleştirilir:
- GET isteği ile şifre listesi yükleme
- POST isteği ile yeni şifre ekleme
- PUT isteği ile şifre güncelleme
- DELETE isteği ile şifre silme

## Ekran Görüntüleri
![1](https://github.com/user-attachments/assets/9ac77568-dc79-4b26-ab1a-e8c2b1afa927)
![2](https://github.com/user-attachments/assets/5096f3c5-cbd7-4ecf-97c1-e9b277f09b21)
![3](https://github.com/user-attachments/assets/4e99493c-9051-4e94-941f-00a985bd84d0)
![4](https://github.com/user-attachments/assets/6a6a31aa-9f80-4ae9-9950-639b201d58e4)
![5](https://github.com/user-attachments/assets/3c6f35fa-3dff-4b22-95ef-756cfed56aff)
![Screenshot 2025-05-17 054038](https://github.com/user-attachments/assets/0aff44fb-19ba-4ad4-b739-4314abc64ce9)
![7](https://github.com/user-attachments/assets/0bce54b4-0a35-46c4-8d3e-ce51711cc1f3)
![8](https://github.com/user-attachments/assets/8848c56e-fe63-4a53-bdb9-f8d5f2d02ab7)
![9](https://github.com/user-attachments/assets/a49ff5ad-8c76-4167-8052-53b11ae834b0)


## Kurulum Gereksinimleri

- .NET 6.0 SDK veya üzeri
- Microsoft SQL Server
- Visual Studio 2022 (önerilen) veya VS Code
- Node.js ve npm (frontend bağımlılıkları için)

## Geliştirme Ortamı Kurulumu

1. Repoyu klonlayın
2. Veritabanı bağlantı dizesini `appsettings.json` dosyasında güncelleyin
3. JWT secret key'i `appsettings.json` dosyasında güncelleyin
4. Package Manager Console'da migration'ları uygulayın:
   ```
   Update-Database
   ```
5. Uygulamayı başlatın
