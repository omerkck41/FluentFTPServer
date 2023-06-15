# FluentFTPServer
FluentFTP, .NET platformu için basit ve kullanıcı dostu bir FTP kütüphanesidir. Bu kütüphane, FTP sunucusuyla etkileşim kurmak için bir dizi kullanışlı işlev ve yöntem sağlar. FluentFTP, dosya ve dizin oluşturma, yükleme, indirme, silme, yeniden adlandırma gibi yaygın FTP işlemlerini gerçekleştirebilmenizi sağlar.

## Özellikler
* FTP sunucusuna bağlanma ve bağlantıyı kesme işlemleri
* Dosya ve dizin varlığını kontrol etme
* Dosya ve dizin listeleme
* Dosya yükleme ve indirme
* Dizin oluşturma, silme ve yeniden adlandırma
* Otomatik bağlantı yeniden deneme ve bağlantı havuzlama
* İşlem sürelerini ölçme ve performans iyileştirmeleri

### FTP kütüphanenizde kullanılan bazı programlama prensipleri ve kalıpları
**1- Tek Sorumluluk İlkesi (Single Responsibility Principle):** Her sınıf, yalnızca bir tek sorumluluğa sahip olmalıdır. IFtpClient arabirimi, FTP işlemlerini gerçekleştirmek için gerekli olan metotları içerir ve yalnızca FTP işlemleriyle ilgili görevleri yerine getirir.

**2- Bağımlılık Tersine Çevirme Prensibi (Dependency Inversion Principle):** IFtpClient arabirimi, FtpClientBuilder ve FtpClientFactory sınıfları tarafından kullanılır. Bu prensip, bağımlılıkların somut sınıflardan ziyade soyutlamalara yönlendirilmesini önerir. Bu sayede, bağımlılıklar daha esnek ve değiştirilebilir hale gelir.

**3- Fırsatçı Tekrar Kullanım Prensibi (Opportunistic Reuse Principle):** FluentFtpClient sınıfı, FluentFTP kütüphanesinin AsyncFtpClient sınıfını kullanarak FTP işlemlerini gerçekleştirir. Bu prensip, mevcut bir kütüphaneyi veya bileşeni kullanarak kodun tekrarını azaltır ve hazır çözümleri verimli bir şekilde kullanır.

**4- Yardımcı Sınıf (Helper Class):** FtpClientBuilder sınıfı, FluentFtpClient sınıfının oluşturulmasını kolaylaştırmak için bir yardımcı sınıf olarak kullanılır. Bu sınıf, FluentFTP kütüphanesinin yapısını gizler ve istemcilere kullanımı daha basit hale getiren bir arabirim sağlar.

**5- Birleştirme (Composition):** FluentFtpClient sınıfı, AsyncFtpClient sınıfını birleştirir ve FTP işlemlerini gerçekleştirmek için bu bileşeni kullanır. Birleştirme prensibi, daha küçük ve daha özelleştirilebilir bileşenleri bir araya getirerek daha karmaşık işlevselliği sağlar.

## Nasıl Kullanılır?
```cs
var ftpClient = new FtpClientBuilder()
    .WithServer("ftp.example.com")
    .WithPort(21)
    .WithCredentials("username", "password")
    .UseSsl()
    .Build();

bool exists = await ftpClient.FileExistsAsync("/httpdocs/favicon.ico");
bool directoryExists = await ftpClient.DirectoryExistsAsync("/httpdocs/Content");
bool createDirectory = await ftpClient.CreateDirectoryAsync("/httpdocs/omerkck");
await ftpClient.RenameDirectoryAsync("/httpdocs/omerkck", "/httpdocs/omerkck41");
await ftpClient.DeleteDirectoryAsync("/httpdocs/omerkck41");

var list = await ftpClient.ListDirectoryAsync("/httpdocs");
foreach (var item in list) { MessageBox.Show(item.ToString()); }

bool downFile = await ftpClient.DownloadFileAsync("/httpdocs/favicon.ico", "C:\\Users\\omerkck\\Desktop\\favicon.ico");
bool upFile = await ftpClient.UploadFileAsync("C:\\Users\\omerkck\\Desktop\\omerkck", "/httpdocs/omerkck");

```

## Kurulum
>FluentFTP'yi projenize eklemek için aşağıdaki NuGet komutunu kullanabilirsiniz:
```cs
Install-Package FluentFTP
```

